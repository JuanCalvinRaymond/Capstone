#ifndef __AAA_INPUTSTRUCTFORMATS_CGINC
#define __AAA_INPUTSTRUCTFORMATS_CGINC

// ===== Vertex and Fragment Structs =====

/*
Description: Standard vertex struct.
Creator: Charlotte C. Brown
Creation Date: Oct. 7th 2016
*/
struct SVertexInput_PositionUV
{
    float4 a_objectPosition : POSITION;
    half2 a_uv : TEXCOORD0;
};

/*
Description: Standard fragment struct.
Creator: Charlotte C. Brown
Creation Date: Oct. 7th 2016
*/
struct SFragmentInput_PositionUV
{
    float4 v_clipPosition : SV_POSITION;
    half2 v_uv : TEXCOORD0;
};

#endif