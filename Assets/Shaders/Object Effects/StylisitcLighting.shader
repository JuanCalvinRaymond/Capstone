Shader "AAA/ObjectEffect/StylisticLighting"
{
	Properties
    {
        // ========== Lighting Effects ==========

		// The main texture of the surface.
		_MainTex ("Diffuse Texture", 2D) = "white" {}

		// Texture containing additional misc data. Currently red channel = spec map. GBA are not in use.
		u_auxMap("Auxiliary Map", 2D) = "black" {}

		// Normal map.
		u_normalMap("Normal Map", 2D) = "bump" {}

		// The map to reflect off of the object.
		u_reflectionCubemap("Reflection Cubemap", Cube) = "white" {}

        [Space(20)]
        // How much of the AO map to use.
        u_AOLevel("AO Strength", Range(0, 10)) = 1.5

        // Normal map strength.
        u_normalMapLevel("Normal Map Level", Range(0, 10)) = 0.5

        // How much reflection the object should use.
        u_reflectionLevel("Reflection Level", Range(0, 1)) = 1.0

        // How much to darken all reflective areas.
        u_reflectionDarkenAmount("Reflection Darken Amount", Range(0, 1)) = 0.025

        [Space(20)]
		// The base colour of the object.
		u_baseColour("Base Colour", Color) = (0, 0, 0, 0)

		// How much of the base colour the object should use.
		u_baseColourLevel("Base Colour Level", Range(0, 1)) = 0.0

        [Space(20)]
		// How much of the diffuse texture the object should use.
		u_diffuseLevel("Diffuse Level", Range(0, 1)) = 1.0

		// The minimum brightness of the lighting.
		u_minimumLighting("Lighting Minimum", Range(0, 1)) = 0.25

        // Slight glow around the edges of the object that face away from the camera, adds "pop".
        u_rimExponent("Rim Exponent", Range(1, 50)) = 10

        // See above.
        u_rimLevel("Rim Level", Range(0, 1)) = 0.05

        [Space(20)]
		// How sharp the specular highlight should be.
		u_specularExponent("Specular Exponent", Range(1, 50)) = 30.0

		// How much of the specular highlight the object should use.
		u_specularLevel("Specular Level", Range(0, 1)) = 0.5

		// 1 minus the specular highlight, makes it look like light is defracting/bouncing off of other objects.
		u_invSpecularExponent("Inverse Specular Exponent", Range(1, 50)) = 5.0

		// See above.
		u_invSpecularLevel("Inverse Specular Level", Range(0, 1)) = 0.05


        // ========== Stylistic Effects ==========

        [Space(20)]
        // What colour the edge highlight should be.
        u_edgeColour("Edge Colour", Color) = (0, 0, 0, 1)

        // How sensitive the edge detection is to colour change.
        u_edgeSensitivity("Edge Sensitivity", Range(0.001, 1)) = 0.03

        // How thick the edges are.
        u_edgeSize("Edge Size", Range(0, 10)) = 0.15

        // How pronounced the edge highlighting is.
        u_edgeWeight("Edge Weight", Range(0, 10)) = 1.0

        [Space(20)]
        // The highest value saturation can reach. (>1 = blow out colours, 0 = greyscale)
        u_saturationMaximum("Saturation Max", Range(1, 5)) = 1.5

        // The lowest value saturation can reach. (0 = greyscale, 1 = saturation max)
        u_saturationMinimum("Saturation Min", Range(0, 1)) = 0.0

        // At what percentage across the frustrum's Z axis desaturation should start (before this value = saturation max).
        u_desaturationStart("Desaturation Start", Range(0, 1)) = 0.3

        // At what percentage across the frustrum's Z axis desaturation should finish (after this value = saturation min).
        u_desaturationEnd("Desaturation End", Range(0, 1)) = 0.5

        // How quickly the desaturation should ramp up (0 = edge, 1 = linear, >1 = exponential curve)
        u_desaturationExponent("Desaturation Curve", Range(0, 10)) = 1

        [Space(20)]
        // How many meters at the bottom of buildings should dissolve.
        u_dissolveBottomDistance("Dissolve Bottom Distance", Range(0, 100)) = 25

		// The height of the "ground plane" in meters. (y = this)
		//u_groundHeight("Ground Height", Float) = 0.0

        // How quickly the fade out occurs as a percentage towards the near clipping plane from the far plane.
        u_dissolveFarDistance("Dissolve Far Ratio", Range(0, 1)) = 0.2

		// How quickly the fade out occurs in meters away from the near clipping plane.
		u_dissolveNearDistance("Dissolve Near Distance", Range(0, 2)) = 0.01
	}

	SubShader
    {
		Tags
		{ 
			"RenderType"="Opaque"
            //"Queue"="Geometry"
		}
		
		CGPROGRAM

		#pragma surface surf Stylize
		#pragma target 3.0

        #include "Assets/Shaders/Include/AAA_Common.cginc"

		uniform sampler2D _MainTex;
        uniform float4 _MainTex_TexelSize;
		uniform float4 u_baseColour;
		uniform float u_baseColourLevel;
		uniform float u_diffuseLevel;
		uniform float u_minimumLighting;
		uniform float u_specularExponent;
		uniform float u_specularLevel;
		uniform float u_invSpecularExponent;
		uniform float u_invSpecularLevel;
		uniform float u_rimExponent;
		uniform float u_rimLevel;
		uniform samplerCUBE u_reflectionCubemap;
		uniform float u_reflectionLevel;
		uniform sampler2D u_auxMap;
		uniform sampler2D u_normalMap;
		uniform float u_normalMapLevel;
        uniform float u_AOLevel;

        uniform float4 u_edgeColour;
        uniform float u_edgeSensitivity;
        uniform float u_edgeSize;
        uniform float u_edgeWeight;
        uniform float u_desaturationExponent;
        uniform float u_desaturationStart;
        uniform float u_desaturationEnd;
        uniform float u_saturationMinimum;
        uniform float u_saturationMaximum;
        uniform float u_dissolveBottomDistance;
		uniform float u_groundHeight;
        uniform float u_dissolveFarDistance;
		uniform float u_dissolveNearDistance;
		uniform float u_reflectionDarkenAmount;

		struct Input
        {
			float2 uv_MainTex;
            float3 worldPos;
			float4 screenPos;
		};

		void surf (Input input, inout SurfaceOutput output)
        {
            // Calculate distance from the camera and convert to a 0-1 range to determine the strength of the stylize effects.
            float distanceRatio = saturate(input.screenPos.z / _ProjectionParams.z);

            // The spacing for the blur detection sampling.
            float2 blurTapOffset = _MainTex_TexelSize.xy * u_edgeSize;

            // Sample the colour of scene around the current frag to later blur similar colours together.
            float4 sceneColourCenter = tex2D(_MainTex, input.uv_MainTex);
            float3 sceneColourRight = tex2D(_MainTex, input.uv_MainTex + float2(blurTapOffset.x, 0.0)).rgb;
            float3 sceneColourLeft = tex2D(_MainTex, input.uv_MainTex - float2(blurTapOffset.x, 0.0)).rgb;
            float3 sceneColourUp = tex2D(_MainTex, input.uv_MainTex + float2(0.0, blurTapOffset.y)).rgb;
            float3 sceneColourDown = tex2D(_MainTex, input.uv_MainTex - float2(0.0, blurTapOffset.y)).rgb;

            // Get the average of each sampled scene colour. This will be used in place of the non-blurred scene colour whenever similar colours are detected.
            float4 blurredSceneColour = sceneColourCenter;
            blurredSceneColour += float4(sceneColourRight, 1);
            blurredSceneColour += float4(sceneColourLeft, 1);
            blurredSceneColour += float4(sceneColourUp, 1);
            blurredSceneColour += float4(sceneColourDown, 1);
            blurredSceneColour /= 5;

            // Get the similarity of each sample compared to the center and average the total.
            float colourSimilarAmount = Colour_IsSimilar_HSB(sceneColourCenter.rgb, sceneColourRight, u_edgeSensitivity);
            colourSimilarAmount += Colour_IsSimilar_HSB(sceneColourCenter.rgb, sceneColourLeft, u_edgeSensitivity);
            colourSimilarAmount += Colour_IsSimilar_HSB(sceneColourCenter.rgb, sceneColourUp, u_edgeSensitivity);
            colourSimilarAmount += Colour_IsSimilar_HSB(sceneColourCenter.rgb, sceneColourDown, u_edgeSensitivity);
            colourSimilarAmount /= 4;

            // Amplify the weight of colour similarity.
            colourSimilarAmount = pow(colourSimilarAmount, u_edgeWeight);

            // If we're on an edge, use the sharp scene colour, otherwise use the blurred scene colour.
            float4 finalSceneColour = lerp(sceneColourCenter, blurredSceneColour, colourSimilarAmount);

            // Increase the final scene's saturation by a bit.
            finalSceneColour = float4(Colour_Saturate(finalSceneColour.rgb, u_saturationMaximum), 1.0);

            // Combine the edge detection with the original scene colour, edges will only appear closer to the camera.
            float4 colourWithOutline = lerp(finalSceneColour, u_edgeColour, (1 - colourSimilarAmount) * (1 - distanceRatio));

            // Desaturate the world the farther away it gets from the camera. Desaturation starts at Start% into depth buffer and ends at End% into depth buffer.
            float desaturationFactor = saturate(smoothstep(u_desaturationStart, u_desaturationEnd, distanceRatio));

            // Amplify the rate of desaturation and clamp the max desaturation to the set value.
            desaturationFactor = pow(desaturationFactor, u_desaturationExponent);
            desaturationFactor = min(1 - u_saturationMinimum, desaturationFactor);
			
            // Get the colour after desaturation.
            float3 desaturatedColourOverDistance = lerp(colourWithOutline.rgb, Colour_Desaturate(sceneColourCenter.rgb), desaturationFactor);

            // Sample the aux map to get specular and AO amounts. Apply the AO level now. Spec level applied later
            // because each lighting component applies its own multiplier.
            // R = Specular
            // G = AO
            // B = Unused
            // A = Unused
            float4 auxSample = tex2D(u_auxMap, input.uv_MainTex);
            auxSample.g = saturate(lerp(0.0, auxSample.g, u_AOLevel));

			// Lerp between the main texture and the base colour.
			output.Albedo = lerp(desaturatedColourOverDistance, u_baseColour.rgb, u_baseColourLevel);
			output.Albedo *= u_diffuseLevel;

            // Apply AO darkening.
            output.Albedo *= (1.0 - auxSample.g);
			
            // This is done to simulate fading out at the bottom of each building.
            if (HashCoord(input.worldPos.xz) > saturate((input.worldPos.y - u_groundHeight) / u_dissolveBottomDistance))
            {
                discard;
            }

			// Fuzziness for distant objects.
            if (HashCoord(input.worldPos.xz) > saturate((_ProjectionParams.z - input.screenPos.z) / (u_dissolveFarDistance * _ProjectionParams.z)))
            {
                discard;
            }

			// Fuzziness for near objects.
			if (HashCoord(input.worldPos.xz) > saturate(input.screenPos.z / u_dissolveNearDistance))
			{
				discard;
			}

			// Pass through the specular amount.
			output.Specular = auxSample.r;
            
            // Pass the desaturation amount to the lighting shader as the gloss value.
            output.Gloss = desaturationFactor;

			// Sample the normal map.
            output.Normal = UnpackScaleNormal(tex2D(u_normalMap, input.uv_MainTex), u_normalMapLevel)*float3(1, 1, 1);
		}

		float4 LightingStylize(SurfaceOutput output, half3 aLightDir, half3 aViewDir, float aAttenuation)
		{
			// Normalize incoming directional data.
			aLightDir = normalize(aLightDir);
			aViewDir = normalize(aViewDir);

			// Initialize various colours that will define what the object looks like.
			float4 diffuseColour = float4(output.Albedo, 1.0) * _LightColor0;
			float4 specularColour = _LightColor0;
			float4 invSpecularColour = specularColour;
			float4 rimColour = _LightColor0;
			float4 reflectionColour = float4(0, 0, 0, 0);
			float4 finalColour = float4(0, 0, 0, 0);
			float clampedAttenuation = clamp(aAttenuation, u_minimumLighting, 1.0);

			// Calculate the total diffuse colour.
			float diffusePct = saturate(dot(aLightDir, output.Normal));
			diffusePct *= clampedAttenuation;
			diffusePct = max(u_minimumLighting, diffusePct);
			diffuseColour.rgb *= diffusePct;

			// Calculate the total specular colour.
			float3 lightViewDirHalf = normalize(aLightDir + aViewDir);
			float specularPct = saturate(dot(lightViewDirHalf, output.Normal));
			float invSpecularPct = 1.0 - specularPct;
			specularPct = pow(specularPct, u_specularExponent);
			specularPct *= u_specularLevel;
			specularPct *= output.Specular;
			specularPct *= clampedAttenuation;
			specularColour.rgb *= specularPct;

			// Calculate the total inverse specular colour (makes it look like there is ambient refracting light).
			invSpecularPct = pow(invSpecularPct, u_invSpecularExponent);
			invSpecularPct *= u_invSpecularLevel;
			invSpecularPct *= output.Specular;
			invSpecularColour.rgb *= invSpecularPct;

			// Calculate the rim lighting colour (gives objects a bit of "pop").
			float rimPct = saturate(1.0 - dot(aViewDir, output.Normal));
			rimPct = pow(rimPct, u_rimExponent);
			rimPct *= u_rimLevel;
			rimColour *= rimPct;

			// Calculate the reflection colour (makes windows shiny and stuff).
			reflectionColour = texCUBE(u_reflectionCubemap, reflect(-aViewDir,output.Normal));
			//reflectionColour *= saturate(diffusePct + invSpecularPct + rimPct);
			reflectionColour *= output.Specular;
			reflectionColour *= u_reflectionLevel;
            reflectionColour *= clampedAttenuation;
			
			// Add all lighting effects.
			finalColour = diffuseColour + specularColour + invSpecularColour + rimColour + reflectionColour;
			finalColour.a = 1.0;

			// How much to darken lighting by.
			float darkenAmount = u_reflectionDarkenAmount * output.Specular;

			// Make sure the colour is in a 0-1 range.
            finalColour.rgb = saturate(lerp(finalColour.rgb, Colour_Desaturate(finalColour.rgb), output.Gloss) - darkenAmount);
            return finalColour;
		}

		ENDCG
	}

	Fallback "VertexLit"
}
