/*
Description: Creates a holographic looking effect on an object. This offsets vertices along the object's local Z axis
axis, so make sure that's aligned properly and that there are enough slices along the local Y axis to handle the effect.
Creator: Charlotte C. Brown
*/
Shader "AAA/ObjectEffect/Holograph"
{
    Properties
    {
        // Diffuse texture.
        _MainTex("Texture", 2D) = "white" {}

        // Flat colour to add to the texture.
        u_additiveColour("Additive Colour", Color) = (0, 0, 0, 0)

        // Flat colour to multiple with the texture.
        u_multiplicativeColour("Multiplicative Colour", Color) = (0.8, 0.9, 1, 0.7)

        // From how far the object should be seen in meters.
        u_maxViewDistance("Max View Distance", Float) = 500

        // Used to simulate long camera exposures.
        u_exposure("Exposure", Float) = 2.5

        // What colour the scanline should be.
        u_scanlineColour("Scanline Colour", Color) = (0.4, 0.8, 1, 1)

		// How fuzzy the image should be.
		u_fuzziness("Fuzziness", Range(0, 1)) = 0.2

        // An exponent that effects the thickness of the scanline.
        u_scanlineWidth("Scanline Width", Float) = 8

        // How many scanlines there should be.
        u_scanlineFrequency("Scanline Frequency", Float) = 100

        // How large the scanline offset should be.
        u_scanlineAmplitude("Scanline Amplitude", Float) = 0.015

        // How fast the scanlines should move.
        u_scanlineSpeed("Scanline Speed", Float) = 2

        // In which object space direction the scanlines should be offset along.
        u_scanlineForwardDirection("Scanline Object-space Axis", Vector) = (0, 1, 0, 0)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment FragShader

            #include "Assets/Shaders/Include/AAA_Common.cginc"

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 u_additiveColour;
            uniform float4 u_multiplicativeColour;
            uniform float u_maxViewDistance;
            uniform float u_exposure;
            uniform float4 u_scanlineColour;
            uniform float u_scanlineWidth;
            uniform float u_scanlineFrequency;
            uniform float u_scanlineAmplitude;
            uniform float u_scanlineSpeed;
            uniform float4 u_scanlineForwardDirection;
			uniform float u_fuzziness;

            struct SFragmentInput_Custom
            {
                float2 v_uv : TEXCOORD0;
                float4 v_clipPosition : SV_POSITION;

                // X = How visible the frag should be based on distance from player in world space
                // Y = How visible the frag should be based on the current scanline wave offset
                float2 v_visibilityPercent : TEXCOORD1;
            };

            SFragmentInput_Custom VertShader(SVertexInput_PositionUV aVertexInput)
            {
                SFragmentInput_Custom fragmentInput;

                // Create the horizontal waves in the holograph.
                float4 vertexObjectPosition = aVertexInput.a_objectPosition;
                float phase = _Time.y * u_scanlineSpeed;
                float phaseOffset = aVertexInput.a_uv.y * u_scanlineFrequency;
                float scanlineWave = sin(phase + phaseOffset);
                vertexObjectPosition.xyz += normalize(u_scanlineForwardDirection.xyz) * scanlineWave * u_scanlineAmplitude;
                fragmentInput.v_visibilityPercent.y = 1.0 - (scanlineWave + 1.0) / 2.0;

                // Calculate how visible the holograph should be based on distance from the player in world space.
                float4 vertexWorldPosition = ObjectToWorld(vertexObjectPosition);
                float3 offsetFromPlayer = vertexWorldPosition.xyz - _WorldSpaceCameraPos.xyz;
                float distanceFromPlayer = length(offsetFromPlayer);
                fragmentInput.v_visibilityPercent.x = 1.0 - saturate(distanceFromPlayer / u_maxViewDistance);

                // Calculate final position in clip space.
                fragmentInput.v_clipPosition = WorldToClip(vertexWorldPosition);
                fragmentInput.v_uv = aVertexInput.a_uv;

                return fragmentInput;
            }

            float4 FragShader(SFragmentInput_Custom aFragmentInput) : Color
            {
                // Get the base colour for the billboard.
                float4 colour = tex2D(_MainTex, aFragmentInput.v_uv * _MainTex_ST.xy + _MainTex_ST.zw);

                // Add the extra colouring
                colour += u_additiveColour;
                colour = saturate(colour);
                colour *= u_multiplicativeColour;

                // Add a scanline effect based on the strength provided from the vertex shader and the scanline's colour.
                colour.rgb += saturate((u_scanlineColour.rgb * pow(aFragmentInput.v_visibilityPercent.y, u_scanlineWidth)) * u_scanlineColour.a);
                colour.a -= saturate(pow(aFragmentInput.v_visibilityPercent.y, u_scanlineWidth) * (u_scanlineColour.a));

                // Apply transparency based on distance from player.
                colour.a *= aFragmentInput.v_visibilityPercent.x;

				// Add a bit of fuzziness to make it seem more projected.
				colour.a *= lerp(colour.a, HashCoord(aFragmentInput.v_uv), u_fuzziness);

                // Make sure colour is within 0-1 range.
                colour = saturate(colour);

                // Apply an exposure colour boost.
                float4 invertedColour = 1 - colour;
                invertedColour = pow(invertedColour, u_exposure);
                colour = 1 - invertedColour;

                return colour;
            }
            ENDCG
        }
    }
}
