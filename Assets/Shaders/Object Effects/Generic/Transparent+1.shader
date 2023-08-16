Shader "AAA/ObjectEffect/Generic/Transparent+1"
{
    //This shader is the same as Unity standard surface shader. The only difference is that its
    //Queue is being set to "Transparent+1". In that way objects with this shader can be rendered
    //on top of the target's glow that turns off Ztest.

    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
        {
            Tags
            { 
                "RenderType" = "Opaque"
                "Queue" = "Transparent+1"//Make this object render on "top" of transparent objects
            }

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Lambert

            uniform sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            fixed4 u_Color;

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
    //FallBack "Diffuse"
}
