// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AAA/ObjectEffect/Generic/UnlitTransparent+1"
{
    //This shader is the same as Unity unlit shader. The only difference is that its
    //Queue is being set to "Transparent+1". In that way objects with this shader can be rendered
    //on top of the target's glow that turns off Ztest. It is also being set as transparent so that it uses an alpha

    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags 
    {
            "RenderType" = "Transparent"//Make
            "Queue" = "Transparent+1"//Make this object render on "top" of transparent objects
            }
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }
}
