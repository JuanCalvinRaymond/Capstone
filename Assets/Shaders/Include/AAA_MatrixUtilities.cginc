#ifndef __AAA_MATRIXUTILITIES_CGINC
#define __AAA_MATRIXUTILITIES_CGINC

// ===== Matrix Functions =====

/*
Description: All of the following functions simply convert a coordinate from the given space
into the destination space.
Parameters: The coordinate to convert.
Creator: Charlotte C. Brown
*/

float4 ObjectToWorld(float4 aObjectPosition)
{
    return mul(UNITY_MATRIX_M, aObjectPosition);
}

float4 WorldToClip(float4 aWorldPosition)
{
    return mul(UNITY_MATRIX_VP, aWorldPosition);
}

float3 ObjectToWorld_Direction(float3 aObjectDirection)
{
    return normalize(mul(float4(aObjectDirection, 0.0), unity_WorldToObject).xyz);
}

#endif