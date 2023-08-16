using UnityEngine;
using System.Collections;

/*
Description: The post-process script to be added in order to control a Stylize shader.
Creator: Charlotte C. Brown
*/
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CStylize : MonoBehaviour
{
    // The name of the inverse proj. matrix uniform.
    private const string M_UNIFORM_INVERSE_PROJECTION = "u_inverseProjectionMatrix";

    // The name of the inverse view matrix uniform.
    private const string M_UNIFORM_INVERSE_VIEW = "u_inverseViewMatrix";

	// Reference to the material that contains a stylize shader.
	public Material m_stylizeShaderMaterial;

    /*
    Description: Set up the main camera to capture a full depth texture.
    Creator: Charlotte C. Brown
    */
    private void Start()
	{
        Camera.main.depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
	}

    /*
    Description: Pass in the two matrices required to read from the skybox cubemap.
    Creator: Charlotte C. Brown
    */
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture aSource, RenderTexture aDestination)
	{
        // Get the inverse of the camera's projection matrix.
        Matrix4x4 inverseProjectionMatrix = Camera.main.projectionMatrix;
        inverseProjectionMatrix = inverseProjectionMatrix.inverse;

        // Set the inverse projection and view matrices so we can properly sample from the cubemap in the shader.
        m_stylizeShaderMaterial.SetMatrix("u_inverseProjectionMatrix", inverseProjectionMatrix);
        m_stylizeShaderMaterial.SetMatrix("u_inverseViewMatrix", Camera.main.cameraToWorldMatrix);

        // Apply the post effect.
        Graphics.Blit(aSource, aDestination, m_stylizeShaderMaterial);
	}
}
