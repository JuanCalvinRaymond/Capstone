// Applies a vignette to the edges of the screen
Shader "AAA/PostEffect/Generic/Vignette"
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
			#pragma fragment FragShader_Vignette

			#include "Assets/Shaders/Include/AAA_Common.cginc"

			uniform sampler2D _MainTex;
			uniform float u_effectStrength;

			fixed4 FragShader_Vignette(SFragmentInput_PositionUV aFragmentInput) : SV_Target
			{
				fixed4 colour = tex2D(_MainTex, aFragmentInput.v_uv);
				
				// Calculate the amount to darken the edges of the screen.
				float vignetteCurve = clamp(0.001, 1, u_effectStrength);
				vignetteCurve = pow(vignetteCurve, 1 - vignetteCurve);

				// Calculate how far from the center of the screen this frag is, as a percent.
				// 0.7071 is the max diagonal distance from the center of the screen.
				float percentTowardsCenter = distance(aFragmentInput.v_uv, float2(0.5, 0.5));
				percentTowardsCenter = 1 - saturate(percentTowardsCenter / 0.7071);

				// How much this frag should be affected by the vignette.
				float vignetteMultiplier = lerp(1, percentTowardsCenter, vignetteCurve);

				// Apply the vignette.
				colour.rgb *= vignetteMultiplier;

				return colour;
			}

			ENDCG
		}
	}
}
