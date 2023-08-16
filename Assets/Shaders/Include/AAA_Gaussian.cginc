#ifndef __AAA_GAUSSIAN_CGINC
#define __AAA_GAUSSIAN_CGINC

// ===== Blurring Functions =====

/*
Description: Returns a colour based on a 9-tap gaussian blurring algorithm. This function assumes
the given texture is an raw depth texture (_CameraDepthTexture, etc.).
Parameters: The raw depth texture, UV coordinates, and how much to blur by.
Creator: Charlotte C. Brown
Creation Date: Oct. 9th 2016
*/
float4 Gaussian9TapHorizontal_Depth(sampler2D aDepthTex, float2 aUV, float aBlurAmount)
{
    float4 blurColour = float4(0.0, 0.0, 0.0, 0.0);

    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.4 * aBlurAmount, aUV.y) * 0.000229;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.3 * aBlurAmount, aUV.y) * 0.005977;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.2 * aBlurAmount, aUV.y) * 0.060598;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.1 * aBlurAmount, aUV.y) * 0.241732;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x, aUV.y) * 0.382928;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.1 * aBlurAmount, aUV.y) * 0.241732;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.2 * aBlurAmount, aUV.y) * 0.060598;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.3 * aBlurAmount, aUV.y) * 0.005977;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.4 * aBlurAmount, aUV.y) * 0.000229;

    blurColour = saturate(blurColour);

    return blurColour;
}

/*
Description: Returns a colour based on a 17-tap gaussian blurring algorithm. This function assumes
the given texture is an raw depth texture (_CameraDepthTexture, etc.).
Parameters: The raw depth texture, UV coordinates, and how much to blur by.
Creator: Charlotte C. Brown
Creation Date: Oct. 9th 2016
*/
float4 Gaussian17TapHorizontal_Depth(sampler2D aDepthTex, float2 aUV, float aBlurAmount)
{
    float4 blurColour = float4(0.0, 0.0, 0.0, 0.0);

    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.8 * aBlurAmount, aUV.y) * 0.000078;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.7 * aBlurAmount, aUV.y) * 0.000489;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.6 * aBlurAmount, aUV.y) * 0.002403;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.5 * aBlurAmount, aUV.y) * 0.009245;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.4 * aBlurAmount, aUV.y) * 0.027835;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.3 * aBlurAmount, aUV.y) * 0.065592;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.2 * aBlurAmount, aUV.y) * 0.12098;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x - 0.1 * aBlurAmount, aUV.y) * 0.17467;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x, aUV.y) * 0.197417;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.1 * aBlurAmount, aUV.y) * 0.17467;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.2 * aBlurAmount, aUV.y) * 0.12098;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.3 * aBlurAmount, aUV.y) * 0.065592;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.4 * aBlurAmount, aUV.y) * 0.027835;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.5 * aBlurAmount, aUV.y) * 0.009245;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.6 * aBlurAmount, aUV.y) * 0.002403;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.7 * aBlurAmount, aUV.y) * 0.000489;
    blurColour += ColourAtPoint_Depth(aDepthTex, aUV.x + 0.8 * aBlurAmount, aUV.y) * 0.000078;

    blurColour = saturate(blurColour);

    return blurColour;
}

/*
Description: Applies a 9-tap gaussian blur algorithm to a given texture.
Parameters: The texture to blur, UV coords, and how much to blur by.
Creator: Charlotte C. Brown
Creation Date: Oct. 9th 2016
Extra Notes:
*/
float4 Gaussian9TapVertical_Texture(sampler2D aTexture, float2 aUV, float aBlurAmount)
{
    float4 blurColour = float4(0.0, 0.0, 0.0, 0.0);

    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.4 * aBlurAmount) * 0.000229;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.3 * aBlurAmount) * 0.005977;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.2 * aBlurAmount) * 0.060598;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.1 * aBlurAmount) * 0.241732;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y) * 0.382928;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.1 * aBlurAmount) * 0.241732;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.2 * aBlurAmount) * 0.060598;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.3 * aBlurAmount) * 0.005977;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.4 * aBlurAmount) * 0.000229;

    blurColour = saturate(blurColour);

    return blurColour;
}

/*
Description: Returns a colour based on a 9-tap gaussian blurring algorithm. This function assumes
the given texture is a colour texture
Parameters: The colour texture, UV coordinates, and how much to blur by.
Creator: Charlotte C. Brown
Creation Date: Oct. 9th 2016
*/
float4 Gaussian9TapHorizontal_Texture(sampler2D aTexture, float2 aUV, float aBlurAmount)
{
    float4 blurColour = float4(0.0, 0.0, 0.0, 0.0);

    blurColour += ColourAtPoint_Texture(aTexture, aUV.x - 0.4 * aBlurAmount, aUV.y) * 0.000229;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x - 0.3 * aBlurAmount, aUV.y) * 0.005977;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x - 0.2 * aBlurAmount, aUV.y) * 0.060598;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x - 0.1 * aBlurAmount, aUV.y) * 0.241732;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y) * 0.382928;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x + 0.1 * aBlurAmount, aUV.y) * 0.241732;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x + 0.2 * aBlurAmount, aUV.y) * 0.060598;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x + 0.3 * aBlurAmount, aUV.y) * 0.005977;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x + 0.4 * aBlurAmount, aUV.y) * 0.000229;

    blurColour = saturate(blurColour);

    return blurColour;
}

/*
Description: Applies a 17-tap gaussian blur algorithm to a given texture.
Parameters: The texture to blur, UV coords, and how much to blur by.
Creator: Charlotte C. Brown
Creation Date: Oct. 9th 2016
Extra Notes:
*/
float4 Gaussian17TapVertical_Texture(sampler2D aTexture, float2 aUV, float aBlurAmount)
{
    float4 blurColour = float4(0.0, 0.0, 0.0, 0.0);

    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.8 * aBlurAmount) * 0.000078;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.7 * aBlurAmount) * 0.000489;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.6 * aBlurAmount) * 0.002403;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.5 * aBlurAmount) * 0.009245;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.4 * aBlurAmount) * 0.027835;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.3 * aBlurAmount) * 0.065592;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.2 * aBlurAmount) * 0.12098;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y - 0.1 * aBlurAmount) * 0.17467;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y) * 0.197417;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.1 * aBlurAmount) * 0.17467;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.2 * aBlurAmount) * 0.12098;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.3 * aBlurAmount) * 0.065592;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.4 * aBlurAmount) * 0.027835;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.5 * aBlurAmount) * 0.009245;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.6 * aBlurAmount) * 0.002403;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.7 * aBlurAmount) * 0.000489;
    blurColour += ColourAtPoint_Texture(aTexture, aUV.x, aUV.y + 0.8 * aBlurAmount) * 0.000078;

    blurColour = saturate(blurColour);

    return blurColour;
}

#endif