/*
Description: The primary goal of the stylistic shader is simply to compliment Collateral’s target art style.
It applies a variety of subtle effects over the final rendering of the scene in order to make closer colours
stand out against a desaturated background. It also blends together similar colours that are nearby to one
another, while increasing the definition of all hard edges. Finally, it creates an intentionally non-realistic
but heavily stylized fog effect for the city to fade into as it gets further away.
Creator: Charlotte C. Brown
*/
Shader "AAA/PostEffect/Stylize"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

        // What colour the edge highlight should be.
		u_edgeColour("Edge Colour", Color) = (0, 0, 0, 1)

        // How sensitive the edge detection is to colour change.
		u_edgeSensitivity("Edge Sensitivity", Range(0.001, 1)) = 0.02

        // How thick the edges are.
		u_edgeSize("Edge Size", Range(0, 10)) = 0.75

        // How pronounced the edge highlighting is.
		u_edgeWeight("Edge Weight", Range(0, 10)) = 1.5

        // Skybox to sample the fog colours from.
        u_fogSkybox("Fog Skybox", Cube) = "white" {}

        // A tint to apply to the entire sky.
		u_fogColour("Fog Colour", Color) = (1, 1, 1, 1)

        // How quickly the fog ramps up over the frustrum.
		u_fogExponent("Fog Curve", Range(0, 10)) = 4.0

        // The highest value saturation can reach. (>1 = blow out colours, 0 = greyscale)
        u_saturationMaximum("Saturation Max", Range(1, 5)) = 1.5

        // The lowest value saturation can reach. (0 = greyscale, 1 = saturation max)
        u_saturationMinimum("Saturation Min", Range(0, 1)) = 0.9

        // At what percentage across the frustrum's Z axis desaturation should start (before this value = saturation max).
        u_desaturationStart("Desaturation Start", Range(0, 1)) = 0.15

        // At what percentage across the frustrum's Z axis desaturation should finish (after this value = saturation min).
        u_desaturationEnd("Desaturation End", Range(0, 1)) = 0.45

        // How quickly the desaturation should ramp up (0 = edge, 1 = linear, >1 = exponential curve)
        u_desaturationExponent("Desaturation Curve", Range(0, 10)) = 1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex VertShader_PositionUV
			#pragma fragment FragShader_Stylize

			#include "Assets/Shaders/Include/AAA_Common.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float4 _MainTex_ST;

			uniform sampler2D _CameraDepthNormalsTexture;

			uniform float4 u_edgeColour;
			uniform float u_edgeSensitivity;
			uniform float u_edgeSize;
			uniform float u_edgeWeight;

            uniform samplerCUBE u_fogSkybox;
			uniform float4 u_fogColour;
			uniform float u_fogExponent;

			uniform float u_desaturationExponent;
            uniform float u_desaturationStart;
            uniform float u_desaturationEnd;
            uniform float u_saturationMinimum;
			uniform float u_saturationMaximum;

            uniform float4x4 u_inverseProjectionMatrix;
            uniform float4x4 u_inverseViewMatrix;

            /*
            Description: Stylizes an image to make it less defined with brighter colours in a few ways:
                - Runs edge detection over the colours in the scene and highlights edges.
                - Slightly blurs all colours that are considered similar to one another, leaving edges alone.
                - Increases saturation of all colours near the camera.
                - Decreases saturation of all colours far from the camera.
                - Fades far objects into a cubemap texture as if it were fog. Not realistic, but fits the style.
            Parameters: Standard fragment input with clip pos and uvs.
            Creator: Charlotte C. Brown
            */
			float4 FragShader_Stylize (SFragmentInput_PositionUV aFragmentInput) : Color
			{
				// Store depth texture UVs separately as they may need to be flipped.
				float2 depthUV = aFragmentInput.v_uv;

#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
				{
					// Flip the depth texture if needed.
					depthUV.y = 1 - depthUV.y;
				}
#endif

				// Sample the normal and depth textures around the given frag to later determine if we're on an edge or not.
				float3 normalCenter;
				float depthCenter;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, depthUV), depthCenter, normalCenter);

                /* ==========
                // NOTE: This is being left here for now in case I change my mind on edge detection - don't want to forget how it was originally done.
				
                // The spacing for the edge detection sampling.
				float2 tapOffset = _MainTex_TexelSize.xy * u_edgeSize;

                float3 normalLeft;
				float depthLeft;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, depthUV - float2(tapOffset.x, 0.0)), depthLeft, normalLeft);

				float3 normalRight;
				float depthRight;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, depthUV + float2(tapOffset.x, 0.0)), depthRight, normalRight);

				float3 normalUp;
				float depthUp;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, depthUV + float2(0.0, tapOffset.y)), depthUp, normalUp);

				float3 normalDown;
				float depthDown;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, depthUV - float2(0.0, tapOffset.y)), depthDown, normalDown);

				// Get the average of how much each frag is considered an edge based on the previous samples.
				float edgeAmount = Colour_IsSimilar_Hue(normalCenter, normalLeft, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Brightness(depthCenter, depthLeft, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Hue(normalCenter, normalRight, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Brightness(depthCenter, depthRight, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Hue(normalCenter, normalUp, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Brightness(depthCenter, depthUp, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Hue(normalCenter, normalDown, u_edgeSensitivity);
				edgeAmount += Colour_IsSimilar_Brightness(depthCenter, depthDown, u_edgeSensitivity);
				edgeAmount /= 8;
				edgeAmount = pow(edgeAmount, u_edgeWeight);

				// Flip the edge amount as we currently have everything that is not an edge, which isn't what we want.
				edgeAmount = 1 - edgeAmount;
                // ========== */

                // The spacing for the blur detection sampling.
                float2 blurTapOffset = _MainTex_TexelSize.xy * u_edgeSize;

				// Sample the colour of scene around the current frag to later blur similar colours together.
				float4 sceneColourCenter = tex2D(_MainTex, aFragmentInput.v_uv);
                float3 sceneColourRight = tex2D(_MainTex, aFragmentInput.v_uv + float2(blurTapOffset.x, 0.0)).rgb;
                float3 sceneColourLeft = tex2D(_MainTex, aFragmentInput.v_uv - float2(blurTapOffset.x, 0.0)).rgb;
                float3 sceneColourUp = tex2D(_MainTex, aFragmentInput.v_uv + float2(0.0, blurTapOffset.y)).rgb;
                float3 sceneColourDown = tex2D(_MainTex, aFragmentInput.v_uv - float2(0.0, blurTapOffset.y)).rgb;

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
				float4 colourWithOutline = lerp(finalSceneColour, u_edgeColour, (1 - colourSimilarAmount) * (1 - depthCenter));

				// Desaturate the world the farther away it gets from the camera. Desaturation starts at Start% into depth buffer and ends at End% into depth buffer.
                float desaturationFactor = saturate(smoothstep(u_desaturationStart, u_desaturationEnd, depthCenter));

                // Amplify the rate of desaturation and clamp the max desaturation to the set value.
                desaturationFactor = pow(desaturationFactor, u_desaturationExponent);
                desaturationFactor *= u_saturationMinimum;

                // Get the colour after desaturation.
				float3 desaturatedColourOverDistance = lerp(colourWithOutline.rgb, Colour_Desaturate(sceneColourCenter.rgb), desaturationFactor);

				// Remove all of the translation components from the view matrix.
                float4x4 inverseViewMatrixNoTranslate = u_inverseViewMatrix;
                inverseViewMatrixNoTranslate[0].w = 0;
                inverseViewMatrixNoTranslate[1].w = 0;
                inverseViewMatrixNoTranslate[2].w = 0;

                // Get the current clip-space coordinate, then convert to view space, then rotate into world space.
                float4 frustumDirection = normalize(float4(depthUV * 2 - 1, 1, 1));
                frustumDirection = mul(u_inverseProjectionMatrix, frustumDirection);
                frustumDirection = mul(inverseViewMatrixNoTranslate, frustumDirection);

                // Sample the skybox based on the current frag's frustum direction.
                float4 cubeFogColour = texCUBE(u_fogSkybox, frustumDirection);

                // Add fog to the scene based on cubemap sample and fog tint.
				float3 totalColourWithFog = lerp(desaturatedColourOverDistance, cubeFogColour * u_fogColour, pow(depthCenter, u_fogExponent));
                
				// Convert back to a float4 and set the colour.
				float4 finalColour = float4(totalColourWithFog, 1);
                //return float4(desaturatedColourOverDistance, 1);
                return finalColour;
			}

			ENDCG
		}
	}
}
