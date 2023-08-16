/*
Description: Reads a given image and runs edge detection math on it in order to highlight specific portions
of it, then overlays that on top of the main game render. Camera angle is taken into account to highlight
only the player's legs, as this shader is intended for use in VR to allow the player to know where they are
physically positioned in the world.
Creator: Charlotte C. Brown
*/
Shader "AAA/PostEffect/VR/LegOutline"
{
	Properties
	{
        // Automatically set main game rendered texture.
		_MainTex ("Texture", 2D) = "white" {}

        // Automatically set Vive camera texture.
       //u_cameraTexture("Camera Texture", 2D) = "white" {}

        // Automatically set Vive camera height to width ratio.
        //u_viveCameraAspectRatio("Aspect Ratio", Float) = 1.0

        // Automatically set Vive headset facing direction in world-space.
        //u_cameraDirection("Camera Direction", Vector) = (0, 0, 1, 1)

         //Controls how large the visibility radius of the camera should be. Higher values = smaller radius.
        u_cameraSizeExponent("Camera Size Exponent", Range(0, 5)) = 2.5

        // How sensitive the shader should be to detecting edges.
        u_edgeTolerance("Edge Tolerance", Range(0, 1)) = 0.75

        // How wide edges should be as a percentage of texel size.
        u_edgeSize("Edge Size", Float) = 0.25

        // What colour the edges should be.
        u_edgeColour("Edge Colour", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex VertShader_PositionUV
			#pragma fragment FragShader_DiffuseEdgeDetection
			
			#include "Assets/Shaders/Include/AAA_Common.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D u_cameraTexture;
            uniform float4 u_cameraTexture_ST;
            uniform float4 u_cameraTexture_TexelSize;
            uniform float u_viveCameraAspectRatio;
            uniform float4 u_cameraDirection;
            uniform float u_cameraSizeExponent;
            uniform float u_edgeTolerance;
            uniform float u_edgeSize;
            uniform float4 u_edgeColour;	
            uniform float4 u_playerDownVector;
            
            /*
            Description: Reads the hue, saturation, and brightness of a given texture in order to detect where edges
            lie on the image within a given tolerance range. This also takes into account the player's looking
            direction in order to only show edges around the player's legs.
            Creator: Charlotte C. Brown
            */
			float4 FragShader_DiffuseEdgeDetection(SFragmentInput_PositionUV aFragmentInput) : Color
			{
                float4 mainColour = tex2D(_MainTex, aFragmentInput.v_uv);

                // Scale the UV vertically based on the Vive camera's aspect ratio.
                float2 uv = aFragmentInput.v_uv;
                uv.y *= u_viveCameraAspectRatio;

                float2 uvTapOffset = u_edgeSize * u_cameraTexture_TexelSize.xy;

                // Sample the camera texture around the current pixel to find edges.
                float4 cameraColourCenter = tex2D(u_cameraTexture, uv * u_cameraTexture_ST.xy + u_cameraTexture_ST.zw);
                float4 cameraColourRight = tex2D(u_cameraTexture, uv * u_cameraTexture_ST.xy + u_cameraTexture_ST.zw + float2(uvTapOffset.x, 0.0));
                float4 cameraColourLeft = tex2D(u_cameraTexture, uv * u_cameraTexture_ST.xy + u_cameraTexture_ST.zw + float2(-uvTapOffset.x, 0.0));
                float4 cameraColourUp = tex2D(u_cameraTexture, uv * u_cameraTexture_ST.xy + u_cameraTexture_ST.zw + float2(0.0, uvTapOffset.y));
                float4 cameraColourDown = tex2D(u_cameraTexture, uv * u_cameraTexture_ST.xy + u_cameraTexture_ST.zw + float2(0.0, -uvTapOffset.y));

                // Calculate if this pixel is on an edge. We have to invert after as we're actually checking.
                float edgeStrength = 0.0;
                edgeStrength += Colour_IsSimilar_HSB(cameraColourCenter.xyz, cameraColourRight.xyz, 1.0 - u_edgeTolerance);
                edgeStrength += Colour_IsSimilar_HSB(cameraColourCenter.xyz, cameraColourLeft.xyz, 1.0 - u_edgeTolerance);
                edgeStrength += Colour_IsSimilar_HSB(cameraColourCenter.xyz, cameraColourUp.xyz, 1.0 - u_edgeTolerance);
                edgeStrength += Colour_IsSimilar_HSB(cameraColourCenter.xyz, cameraColourDown.xyz, 1.0 - u_edgeTolerance);

                // Get the average of the edge strength sums and invert it (else everything except edges will glow).
                edgeStrength /= 4.0;
                edgeStrength = 1.0 - edgeStrength;

                // The percentage value of "down-looking-ness". TODO: Make sure u_playerDownVector is normalized!
                float lookDownRatio = pow(saturate(dot(u_cameraDirection.xyz, u_playerDownVector)), u_cameraSizeExponent);

                // Create the circle around the legs - larger the further down the player is looking.
                float cameraAmount = 1.0 - saturate(distance(aFragmentInput.v_uv, 
                    float2(0.5, 1.0 - 0.5 * u_viveCameraAspectRatio)) * u_cameraSizeExponent / lookDownRatio);
                
                // Fade in the camera as the player looks down.
                return lerp(mainColour, u_edgeColour, cameraAmount * lookDownRatio * edgeStrength);
			}

			ENDCG
		}
	}
}
