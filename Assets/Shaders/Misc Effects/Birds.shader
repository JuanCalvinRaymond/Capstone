// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
This shader is a combination of a compute shader, script, and geometry shader that work together to
simulate birds flocking through the sky within a loosely confined area, and then draw them.

While this is in a functioning state, there are still a few more things to complete:
- Reacting to the player shooting at the flocks
- Drawing a double-sided quad (two quads) instead of just one that isn't culled in order to handle
    a top and bottom texture.
- Support for rendering arbitrary meshes instead of quads.

Creator: Charlotte C. Brown
*/
Shader "AAA/MiscEffect/Birds"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

        // How large the birds should be, in meters.
		u_birdSize ("Bird Size", Float) = 0.25

        // The most birds should be able to bank while turning corners.
        u_maxBanking ("Max Banking", Float) = 75.0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
            // TODO: Create a second upside-down quad so disabling culling is not required.
			Cull Off
			ZWrite Off

			CGPROGRAM

			#pragma vertex VertShader
			#pragma fragment FragShader
			#pragma geometry GeomShader
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float u_birdSize;
            uniform float u_maxBanking;
            uniform float4 _LightColor0;

			            /*
            Takes a given world-space position and converts it to be represented by a bird's local XYZ axes instead.
            This can be thought of as a "look along the given axis" function.
            Params:
                aForward - The forward (z) axis of the local space to convert to.
                aPosition - The world-space coordinate to convert.
            Return:
                The given position represented in the axis defined by the forward direction.
            Creator: Charlotte C. Brown
            */
			float4 ToLocalCoord(float3 aForward, float4 aPosition)
			{
                // Calculate the bird's local XYZ axes via 
				float3 zAxis = aForward;
				float3 xAxis = cross(float3(0, 1, 0), zAxis);
				float3 yAxis = cross(zAxis, xAxis);

				float3 z = zAxis * aPosition.z;
				float3 x = xAxis * aPosition.x;
				float3 y = yAxis * aPosition.y;

                return float4(z + x + y, 0);
			}

			/*
            This is a standard rotation-around-an-arbitrary-axis function. Wikipedia was my reference though not
            directly copied from. Google this if you're confused! It simply rotates a given point around a given
            line by a given angle.
            Params:
                aAxis - The line to rotate around.
                aAngle - How many radians to rotate by.
                aPosition - The original position to rotate.
            Return:
                The position rotated around the axis.
            Creator: Charlotte C. Brown
            */
			float4 RotateAroundAxis(float3 aAxis, float aAngle, float4 aPosition)
			{
                float cosAngle = cos(aAngle);
                float sinAngle = sin(aAngle);

                // Build the rotation matrix to rotate a point around an axis.
				float4x4 rotationMatrix = float4x4(
					(1.0f - cosAngle) * aAxis.x * aAxis.x + cosAngle, (1.0f - cosAngle) * aAxis.x * aAxis.y - aAxis.z * sinAngle, (1.0f - cosAngle) * aAxis.z * aAxis.x + aAxis.y * sinAngle, 0.0f,
					(1.0f - cosAngle) * aAxis.x * aAxis.y + aAxis.z * sinAngle, (1.0f - cosAngle) * aAxis.y * aAxis.y + cosAngle, (1.0f - cosAngle) * aAxis.y * aAxis.z - aAxis.x * sinAngle, 0.0f,
					(1.0f - cosAngle) * aAxis.z * aAxis.x - aAxis.y * sinAngle, (1.0f - cosAngle) * aAxis.y * aAxis.z + aAxis.x * sinAngle, (1.0f - cosAngle) * aAxis.z * aAxis.z + cosAngle, 0.0f,
					0.0f, 0.0f, 0.0f, 1.0f
				);

                // Apply the rotation to the point.
				return mul(rotationMatrix, aPosition);
			}


            // This is a very specialized shader and as such the inputs may look a bit odd. See CBirds.cs for info.
			struct SVertexInput
			{
				float4 a_objectPosition : POSITION;
				float3 a_velocity : NORMAL;
				float4 a_previousVelocity : TANGENT;
			};

            // This is a very specialized shader and as such the inputs may look a bit odd. See CBirds.cs for info.
			struct SFragmentInput
			{
				float4 v_previousVelocity : TANGENT;
				float4 v_clipPosition : SV_POSITION;
				float4 v_objectPosition : TEXCOORD1;
				float3 v_velocity : NORMAL;
				float2 v_UV : TEXCOORD2;
			};
			
            /*
            Mostly passes the data through to the geometry and fragment shaders.
            Creator: Charlotte C. Brown
            */
			SFragmentInput VertShader (SVertexInput aInput)
			{
				SFragmentInput output;
                output.v_clipPosition = UnityObjectToClipPos(aInput.a_objectPosition);
                output.v_objectPosition = aInput.a_objectPosition;
                output.v_velocity = aInput.a_velocity;
                output.v_previousVelocity = aInput.a_previousVelocity;
                output.v_UV = float2(0, 0);
				return output;
			}

            /*
            Takes a point from the incoming mesh and converts it into a quad in order to draw a bird at
            the appropriate location. Also handles roll and pitch of the quad to simulate angles at which
            a bird would normally fly. Generates UV coordinates in order to map the texture to the quad.
            TODO: Arbitrary mesh handling.
            Creator: Charlotte C. Brown
            */
			[maxvertexcount(4)]
			void GeomShader(point SFragmentInput aInput[1], inout TriangleStream<SFragmentInput> aOutput)
			{
				SFragmentInput vertex;

				vertex.v_velocity = aInput[0].v_velocity;
				vertex.v_previousVelocity = aInput[0].v_previousVelocity;

				float3 normalizedVelocity = normalize(vertex.v_velocity);

				// Calculate banking.
                float3 previousForward = aInput[0].v_previousVelocity.xyz;
                previousForward.y = 0.0f;
                previousForward = normalize(previousForward);

                float3 currentForward = aInput[0].v_velocity;
                currentForward.y = 0.0f;
                currentForward = normalize(currentForward);

                float3 xAxis = cross(previousForward, float3(0, 1, 0));
                float angle = dot(xAxis, currentForward) * u_maxBanking;

				//Create the vertices to make a quad
				// Top-left.
				vertex.v_UV = float2(0, 1);
				vertex.v_objectPosition = aInput[0].v_objectPosition + RotateAroundAxis(normalizedVelocity, angle, ToLocalCoord(normalizedVelocity, float4(-u_birdSize, 0.0f, u_birdSize, 0)));
				vertex.v_clipPosition = UnityObjectToClipPos(vertex.v_objectPosition);
				aOutput.Append(vertex);

				// Bottom-left.
				vertex.v_UV = float2(0, 0);
				vertex.v_objectPosition = aInput[0].v_objectPosition + RotateAroundAxis(normalizedVelocity, angle, ToLocalCoord(normalizedVelocity, float4(-u_birdSize, 0.0f, -u_birdSize, 0)));
				vertex.v_clipPosition = UnityObjectToClipPos(vertex.v_objectPosition);
				aOutput.Append(vertex);

				// Top-right.
				vertex.v_UV = float2(1, 1);
				vertex.v_objectPosition = aInput[0].v_objectPosition + RotateAroundAxis(normalizedVelocity, angle, ToLocalCoord(normalizedVelocity, float4(u_birdSize, 0.0f, u_birdSize, 0)));
				vertex.v_clipPosition = UnityObjectToClipPos(vertex.v_objectPosition);
				aOutput.Append(vertex);

				// Bottom-Right.
				vertex.v_UV = float2(1, 0);
				vertex.v_objectPosition = aInput[0].v_objectPosition + RotateAroundAxis(normalizedVelocity, angle, ToLocalCoord(normalizedVelocity, float4(u_birdSize, 0.0f, -u_birdSize, 0)));
				vertex.v_clipPosition = UnityObjectToClipPos(vertex.v_objectPosition);
				aOutput.Append(vertex);
			}
			
            /*
            Very simple frag shader that just applies a texture affected by global lighting.
            Creator: Charlotte C. Brown
            */
			float4 FragShader (SFragmentInput aInput) : Color
			{
                float4 colour = tex2D(_MainTex, aInput.v_UV) * _LightColor0;
				return colour;
			}

			ENDCG
		}
	}
}
