// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
Description: Used to create a glow around all shootable objects within the game.
Creator: Charlotte C. Brown
*/
Shader "AAA/ObjectEffect/CutoutGlow"
{
    Properties
    {
        // Diffuse texture.
        u_objectTexture("Object Texture", 2D) = "white" {}

        // What colour the object should glow.
        u_glowColour("Glow Colour", Color) = (1.0, 0.5, 0, 0.95)

        // How large the outer glow should be.
        u_glowSize("Glow Size", Range(1, 20)) = 1.15

        // How much the glow should be blown up from the center vs extruded along normals.
        u_scaleToExtrude("Scale to Extrusion Ratio", Range(0, 1)) = 1

        // An arbitrary tint colour.
        u_objectColourMultiply("Object Colour Multiply", Color) = (0.9, 0.95, 1, 1)

        // An arbitrary additive colour.
        u_objectColourAdditive("Object Colour Additive", Color) = (0, 0, 0, 0)

        // The darkest the diffuse texture and lighting is allowed to be.
        u_objectMinimumBrightness("Object Min Brightness", Range(0, 1)) = 0.05

        // The farthest away, in meters, the camera can be and still show the outline.
        u_maxGlowDistance("Max Glow Distance", Float) = 100.0

        // How small/large the specular highlight is.
        u_specularExponent("Specular Exponent", Range(0, 100)) = 5

        // How bright the specular highlight is.
        u_specularBrightness("Specular Brightness", Range(0, 10)) = 2.5
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        // This pass creates the outer glow for the target.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            //ZTest Off

            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment FragShader

            #include "Assets/Shaders/Include/AAA_Common.cginc"

            uniform float4 u_glowColour;
            uniform float u_glowSize;
            uniform float u_maxGlowDistance;
            uniform float4 _LightColor0;
            uniform float u_specularExponent;
            uniform float u_specularBrightness;
            uniform float u_scaleToExtrude;

            struct SVertexInput_Custom
            {
                float4 a_objectPosition : POSITION;
                half2 a_uv : TEXCOORD0;
                float4 a_normal : NORMAL;
            };

            struct SFragmentInput_Custom
            {
                float4 v_clipPosition : SV_POSITION;
                half2 v_uv : TEXCOORD0;
                float4 v_normal : NORMAL;
                float3 v_worldPosition : TEXCOORD2;

                // How visible the glow should be.
                float v_visibilityPercent : TEXCOORD1;
            };

            SFragmentInput_Custom VertShader(SVertexInput_Custom aVertexInput)
            {
                SFragmentInput_Custom fragmentInput;
                float4 objectPosition = aVertexInput.a_objectPosition;

                // Scale up the object from its origin and extrude along the vertex normals, then lerp between the values for the final glow size.
                float3 glowPositionScale = objectPosition.xyz * u_glowSize;
                float3 glowPositionExtrude = objectPosition.xyz + aVertexInput.a_normal.xyz * (u_glowSize - 1.0);
                objectPosition.xyz = lerp(glowPositionScale, glowPositionExtrude, u_scaleToExtrude);

                // Get the object's world position.
                float4 worldPosition = ObjectToWorld(aVertexInput.a_objectPosition);

                // Calculate how visible the glow should be based on distance from the camera.
                float distanceFromCamera = distance(worldPosition.xyz, _WorldSpaceCameraPos.xyz);
                float visibilityPercent = step(distanceFromCamera, u_maxGlowDistance);
                fragmentInput.v_visibilityPercent = visibilityPercent;

                // Store the world-space coordinates.
                fragmentInput.v_worldPosition = worldPosition.xyz;

                // Calculate the world-space normal.
                fragmentInput.v_normal = float4(ObjectToWorld_Direction(aVertexInput.a_normal.xyz), 0.0);

                // Move the coordinate into clip space.
                fragmentInput.v_clipPosition = UnityObjectToClipPos(objectPosition);
                fragmentInput.v_uv = aVertexInput.a_uv;

                return fragmentInput;
            }

            float4 FragShader(SFragmentInput_Custom aFragmentInput) : COLOR
            {
                // Get the default glow colour.
                float4 colour = u_glowColour;

                // Calculate how bright the specular highlight should be.
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightDirectionReflect = reflect(-lightDirection, aFragmentInput.v_normal);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - aFragmentInput.v_worldPosition);
                float specularAmount = saturate(dot(viewDirection, lightDirectionReflect));

                // Shrink/grow the highlight size based on an exponent.
                specularAmount = pow(specularAmount, u_specularExponent);

                // Increase/decrease highlight brightness based on given value. Clamp 0-1 to avoid bleeding.
                specularAmount = saturate(specularAmount * u_specularBrightness);

                // Fade out the highlight based on distance from the camera.
                specularAmount *= aFragmentInput.v_visibilityPercent;

                // Add in specular highlight, clamp to avoid "blow outs"
                colour.rgb += _LightColor0.rgb * specularAmount;
                colour = saturate(colour);

                // Fade out the glow based on distance from the camera.
                colour.a *= aFragmentInput.v_visibilityPercent;
                return colour;
            }
            ENDCG
        }

        // This pass draws the target itself on top of the glow.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment FragShader

            #include "Assets\Shaders\Include\AAA_Common.cginc"

            uniform sampler2D u_objectTexture;
            uniform float4 u_objectTexture_ST;
            uniform float4 u_objectColourMultiply;
            uniform float4 u_objectColourAdditive;
            uniform float u_objectMinimumBrightness;
            uniform float4 _LightColor0;

            struct SVertexInput_Custom
            {
                float4 a_objectPosition : POSITION;
                half2 a_uv : TEXCOORD0;
                float3 a_normal : NORMAL;
            };

            struct SFragmentInput_Custom
            {
                float4 v_clipPosition : SV_POSITION;
                half2 v_uv : TEXCOORD0;
                float v_diffusePercent : TEXCOORD1;
            };

            SFragmentInput_Custom VertShader(SVertexInput_Custom aVertexInput)
            {
                SFragmentInput_Custom fragmentInput;

                // Calculate final clip position & uv.
                fragmentInput.v_clipPosition = UnityObjectToClipPos(aVertexInput.a_objectPosition);
                fragmentInput.v_uv = aVertexInput.a_uv;

                // Calculate how bright the diffuse lighting should be.
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 normalDirection = normalize(mul(float4(aVertexInput.a_normal.xyz, 0.0), unity_WorldToObject).xyz);
                //float diffusePct = clamp(dot(normalDirection, lightDirection), 0.0, 1.0);//FIX IT
				float diffusePct = 0.5f;//FIX IT
                fragmentInput.v_diffusePercent = diffusePct;

                return fragmentInput;
            }

            float4 FragShader(SFragmentInput_Custom aFragmentInput) : Color
            {
                // Get the base diffuse texture colour.
                float4 colour = tex2D(u_objectTexture, aFragmentInput.v_uv * u_objectTexture_ST.xy + u_objectTexture_ST.zw);

                // Apply an arbitrary customizable multiply colour, allows for tinting.
                colour *= u_objectColourMultiply;

                // Calculate final diffuse amount, this allows for a minimum diffuse amount to be set in the editor in order to avoid complete blackness.
                float diffusePercent = (1 - u_objectMinimumBrightness) * aFragmentInput.v_diffusePercent + u_objectMinimumBrightness;

                // Multiply the diffuse texture by the lighting colour and diffuse amount.
                colour.rgb *= _LightColor0 * diffusePercent;

                // Add in a flat additive colour.
                colour += u_objectColourAdditive;

                // Clamp colour to avoid bleeding.
                colour = saturate(colour);
                
                return colour;
            }
                ENDCG
        }
    }
}
