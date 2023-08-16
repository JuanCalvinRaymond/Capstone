// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef __AAA_ENTRYPOINTS_CGINC
#define __AAA_ENTRYPOINTS_CGINC

// ===== Vertex and Fragment Entry Points =====

/*
Description: Standard object to clip space vertex function that outputs position and UV coordinates.
Parameters: The object space vertex definition.
Creator: Charlotte C. Brown
*/
SFragmentInput_PositionUV VertShader_PositionUV(SVertexInput_PositionUV aVertexInput)
{
    SFragmentInput_PositionUV fragmentInput;
    fragmentInput.v_clipPosition = UnityObjectToClipPos(aVertexInput.a_objectPosition);
    fragmentInput.v_uv = aVertexInput.a_uv;
    return fragmentInput;
}

/*
Description: Vertex function that simply passes all values straight to the fragment shader with no changes.
Parameters: The object space vertex definition.
Creator: Charlotte C. Brown
*/
SFragmentInput_PositionUV VertShader_PositionUV_Passthrough(SVertexInput_PositionUV aVertexInput)
{
    SFragmentInput_PositionUV fragmentInput;
    fragmentInput.v_clipPosition = aVertexInput.a_objectPosition;
    fragmentInput.v_uv = aVertexInput.a_uv;
    return fragmentInput;
}

#endif