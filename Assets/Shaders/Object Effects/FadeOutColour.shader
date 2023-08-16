// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AAA/ObjectEffect/FadeOutColour"
{
	Properties
	{
		u_colour("Colour", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform float4 u_colour;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				return u_colour;
			}
			ENDCG
		}
	}
}