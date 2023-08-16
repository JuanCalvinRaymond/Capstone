using UnityEngine;
using System.Collections;


/*
Description: This is adapted from CGenericPostProcess to not use the settings storer.
Creator: Charlotte C. Brown
*/
public class CSimplePostProcess : MonoBehaviour
{
    private const string M_UNIFORM_EFFECT_STRENGTH_NAME = "u_effectStrength";
    private Material m_postProcessMaterial;

	[Range(0.0f, 1.0f)]
    public float m_effectAmount = 0.25f;
    public Shader m_postProcessShader;

    // Description: See CGenericPostProcess for info
    private void Awake()
    {
        //If there is a shader
        if (m_postProcessShader != null)
        {
            //Create a material for it
            m_postProcessMaterial = new Material(m_postProcessShader);

			//Set its values
			m_postProcessMaterial.SetFloat(M_UNIFORM_EFFECT_STRENGTH_NAME, m_effectAmount);
		}
        else//If there is no shader
        {
            enabled = false;//Disable the component
        }
	}
	
    // Description: See CGenericPostProcess for info
    private void OnRenderImage(RenderTexture aSource, RenderTexture aDestination)
    {
		//Apply the post process effect, no if check for material. Because in Awake,
		// if the material is null the component will be disabled
		Graphics.Blit(aSource, aDestination, m_postProcessMaterial);
    }
}
