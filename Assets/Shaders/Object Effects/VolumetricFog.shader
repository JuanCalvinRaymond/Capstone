Shader "AAA/ObjectEffect/Volumetric Fog"
{
	Properties
	{
		u_noiseTexture("Noise", 2D) = "white"{}
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}
		
		CGPROGRAM

		#pragma surface SurfShader_VolumetricFog Unlit alpha:fade
		#include "Assets/Shaders/Include/AAA_Common.cginc"

		uniform float u_thickness;
		uniform int u_numberOfSamples;
		uniform float u_densityPerMeter;
		uniform float u_minDensityPerSample;
		uniform float u_maxDensityPerSample;
		uniform sampler2D u_noiseTexture;
		uniform float u_uvScale;
		uniform float u_moveSpeedX;
		uniform float u_moveSpeedZ;
        uniform float u_noiseExponentTop;
        uniform float u_noiseExponentBottom;
        uniform float u_fogFadeOutStartPercent;
        uniform float4 u_colour;
		
		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
		};

		// REPLACE u_numberOfSamples with 15 IN BUILD OR BOOM!!!!!
		void SurfShader_VolumetricFog(Input aInput, inout SurfaceOutput aOutput)
		{
			// Calculate how many meters of fog this line of sight passes through.
			float metersOfFog = u_thickness / max(0.00001, aInput.viewDir.y);

			// Calculate how much distance there should be between each fog sample.
			float sampleStepSize = metersOfFog / (15 - 1);

			// The total amount of fog for this frag.
			float totalFogAmount = 0.0f;

			// For each fog sample...
			for (int i = 0; i < 15; i++)
			{
				// Get the base UVW coordinates - ignoring Y so we have a base of 0 for calculating viewing angles.
				float3 fogUVW = aInput.worldPos * float3(1, 0, 1);

				// Ray march along the view direction step by step and add the result to the UVW coordinate.
				fogUVW -= (aInput.viewDir * float3(1, 0, 1)) * sampleStepSize * i;

				// Apply the arbitrary UVW scale.
				fogUVW.xz /= u_uvScale;

                // The current sample percent - 0 = top, 1 = bottom.
                float samplePercent = (float)i / (15 - 1);

				// Apply the arbitrary movement.
				fogUVW.x += _Time.x * u_moveSpeedX;
				fogUVW.z += _Time.x * u_moveSpeedZ;

				// Sample the noise texture based on created UVW coordinates.
                float fogAmountXZ = tex2D(u_noiseTexture, fogUVW.xz).r;
                float fogAmount = fogAmountXZ * samplePercent;

                // Apply an arbitrary exponent to the fog that gets larger towards the surface.
                fogAmount = pow(fogAmount, lerp(u_noiseExponentTop, u_noiseExponentBottom, samplePercent));

                // Keep each step of fog to be within the allotted range.
                fogAmount = clamp(fogAmount, u_minDensityPerSample, u_maxDensityPerSample);

                // Apply density based on meters of fog.
                fogAmount *= samplePercent * u_densityPerMeter * sampleStepSize;

                totalFogAmount += fogAmount;
			}

			// Output final colour and alpha based on fog amount.
			aOutput.Albedo = u_colour.rgb;
			aOutput.Alpha = u_colour.a * totalFogAmount;

            // Fade out fog at the edge of the clipping plane.
            float maxDistance = _ProjectionParams.z - _ProjectionParams.y;
            float coordDistance = distance(_WorldSpaceCameraPos, aInput.worldPos);
            aOutput.Alpha *= 1.0 - saturate(smoothstep(maxDistance * u_fogFadeOutStartPercent, maxDistance, coordDistance));
		}

        float4 LightingUnlit(SurfaceOutput aOutput, half3 aLightDir, half3 aViewDir, float aAttenuation)
        {
            return float4(aOutput.Albedo, aOutput.Alpha);
        }

		ENDCG
	}
}
