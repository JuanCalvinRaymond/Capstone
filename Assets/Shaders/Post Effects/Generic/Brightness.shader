/*
Description: Post process shader to apply brightness to the current screen.
Parameters(Optional):
Creator:
Extra Notes:
*/
Shader "AAA/PostEffect/Generic/Brightness"
{
    Properties
	{
        _MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
        /*
        Description: The pass in the vert shaders merely converts from object to clip space, and pass uvs.
        While the fragment shader applies brightness to the screen
        Parameters(Optional):
        Creator:
        Extra Notes:The vertex shader function was made in the cginclude files
        */
        Pass
		{
            CGPROGRAM

			#pragma vertex VertShader_PositionUV
			#pragma fragment FragShader_Brightness

            #include "Assets/Shaders/Include/AAA_Common.cginc"

			uniform sampler2D _MainTex;
			uniform float u_effectStrength;

            /*
            Description: The shader merely samples the post process texture, and then multiplies
            it rgb colors by a brightness amount (tint). This makes the screen brighter and darker
            Parameters(Optional):
            Creator:
            Extra Notes:
            */
			float4 FragShader_Brightness(SFragmentInput_PositionUV aFragmentInput) : Color
			{
                //Sample the texture
                float4 color = tex2D(_MainTex, aFragmentInput.v_uv);

				//Multiply the desired brightness to the sampled color
				color.rgb *= u_effectStrength;

                //Ensure the final color is within a 0 to 1 range
				return saturate(color);
			}
			ENDCG
		}
	}
}
