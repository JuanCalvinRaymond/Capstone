// Post effect used to make it more obvious when the game is paused. Desaturates around
// the edges of the screen.
Shader "AAA/PostEffect/Paused"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM

			#pragma vertex VertShader_PositionUV
			#pragma fragment FragShader_Paused
			
			#include "Assets/Shaders/Include/AAA_Common.cginc"
			
			uniform sampler2D _MainTex;
			uniform float u_effectStrength;

			/*
			Desaturates based on distance from center of screen and a hashing function.
			*/
			float4 FragShader_Paused (SFragmentInput_PositionUV aFragmentInput) : Color
			{
				// Sample original scene colour.
				float4 colour = tex2D(_MainTex, aFragmentInput.v_uv);

				// Fully desaturate the colour.
				float4 finalColour = colour;
				finalColour.rgb = Colour_Desaturate(finalColour.rgb);

				// Get a hashed value based on time and screen position.
				float fuzziness = HashCoord(aFragmentInput.v_uv + float2(0, _Time.y)) * 0.5;

				// Calculate distance from the center of the screen as a percentage (1 = edge, 0 = center).
				float pctDistFromCenter = distance(aFragmentInput.v_uv, float2(0.5, 0.5)) / 0.7071;
				
				// Calculate the total amount to desaturate the scene. Pow is an arbitrary value to narrow the effect.
				float desaturationPct = pctDistFromCenter + fuzziness;
				desaturationPct = saturate(pow(desaturationPct, 10));

				// Account for effect strength settings from editor.
				desaturationPct *= u_effectStrength;

				// Apply the final effect as a lerp between full colour and desaturated colour.
				finalColour = lerp(colour, finalColour, desaturationPct);
				return finalColour;
			}

			ENDCG
		}
	}
}
