/*
Description: Post process shader to apply gamma correction to the current screen.
Parameters(Optional):
Creator: 
Extra Notes:
*/
Shader "AAA/PostEffect/Generic/Gamma"
{
    Properties
	{
        _MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
        /*
        Description: The pass in the vert shaders merely converts from object to clip space, and pass uvs.
        While the fragment shader applies the gamma correction
        Parameters(Optional):
        Creator:
        Extra Notes:The vertex shader function was made in the cginclude files
        */
        Pass
		{
            CGPROGRAM

			#pragma vertex VertShader_PositionUV
			#pragma fragment FragShader_GammaCorrection

            #include "Assets/Shaders/Include/AAA_Common.cginc"

			uniform sampler2D _MainTex;
			uniform float u_effectStrength;
            
            /*
            Description: The shader merely samples the post process texture, and then
            applies gamma correction to the pixel that was sampled.
            Parameters(Optional):
            Creator:
            Extra Notes:
            */
			float4 FragShader_GammaCorrection(SFragmentInput_PositionUV aFragmentInput) : Color
			{
                //Sample the texture
                float4 color = tex2D(_MainTex, aFragmentInput.v_uv);


                //Invert the values of the current color
                float4 invertedColor = 1.0 - color;

                //Apply gamma correction to the inverted color
                invertedColor = pow(invertedColor, 1.0 / u_effectStrength);

                //Invert the color again, making it back to "normal"
                color = 1.0 - invertedColor;

                //Ensure the object is visible
                color.a = 1.0;

                //Ensure the final color is within a 0 to 1 range
                return saturate(color);
			}
			ENDCG
		}
	}
}
