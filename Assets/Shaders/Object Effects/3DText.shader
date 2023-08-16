// Taken from the Unity wiki. This simply draws 3D text behind other objects instead of through them.
Shader "AAA/ObjectEffect/3D Text"
{
    Properties
    {
        _MainTex("Font Texture", 2D) = "white" {}
        _Color("Text Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Lighting Off
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Color[_Color]
            SetTexture[_MainTex]
            {
                combine primary, texture * primary
            }
        }  
    }
}