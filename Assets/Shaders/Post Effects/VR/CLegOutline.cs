using UnityEngine;
using System.Collections;

/*
Description: Creates a holographic looking effect on an object. This offsets vertices along the object's local Z axis
axis, so make sure that's aligned properly and that there are enough slices along the local Y axis to handle the effect.
Creator: Charlotte C. Brown
*/
public class CLegOutline : MonoBehaviour
{
    // Uniform string name definitions.
    private const string M_UNIFORM_CAMERA_TEXTURE = "u_cameraTexture";
    private const string M_UNIFORM_VIVE_CAMERA_ASPECT_RATIO = "u_viveCameraAspectRatio";
    private const string M_UNIFORM_CAMERA_DIRECTION = "u_cameraDirection";
    private const string M_UNIFORM_PLAYER_DOWN_VECTOR = "u_playerDownVector";
    private GameObject m_playerObject = null;

    // Reference to the material that contains a leg outline shader.
    public Material m_legOutlineShaderMaterial;

    /*
    Description: Grab the Vive's camera on start.
    Creator: Charlotte C. Brown
    */
    private void Start()
    {
        m_playerObject = CGameManager.PInstanceGameManager.PPlayerObject;
    }
        
    private void OnEnable()
    {
        // Claim the Vive camera.
        AcquireCamera();
    }

    private void OnDisable()
    {
        ReleaseCamera();
    }

    /*
    Description: Release the Vive's camera on shutdown.
    Creator: Charlotte C. Brown
    */
    private void OnDestroy()
    {
        // Tell SteamVR that we're done with the Vive camera.
        ReleaseCamera();
    }

    private void OnApplicationQuit()
    {
        ReleaseCamera();
    }

    /*
    Description: Send the Vive's current camera info and last frame to the leg shader along with the game's rendered texture.
    Creator: Charlotte C. Brown
    */
    private void OnRenderImage(RenderTexture aSource, RenderTexture aDestination)
    {
        // Get the source of the camera texture (the Vive camera itself).
        var source = SteamVR_TrackedCamera.Source(true);

        // Get the Vive camera's texture.
        Texture2D texture = source.texture;

        // If the texture doesn't exist, that means the camera wasn't detected (like due to not being in VR, but perhaps running on USB 2.0).
        if (texture == null && m_legOutlineShaderMaterial != null)
        {
            return;
        }

        // Get the UV bounds of the non-distorted portion of the Vive's camera feed (see SteamVR docs for info on this).
        var bounds = source.frameBounds;
        float scaleU = bounds.uMax - bounds.uMin;
        float scaleV = bounds.vMax - bounds.vMin;

        // Calculate the aspect ratio of the Vive's camera in order to keep square texels in the shader (see SteamVR docs for info on this).
        float aspect = (float)texture.width / texture.height;
        aspect *= Mathf.Abs(scaleU / scaleV);

        // Set the leg shader's uniforms (see LegOutline.shader for info).
        m_legOutlineShaderMaterial.SetTexture(M_UNIFORM_CAMERA_TEXTURE, texture);
        m_legOutlineShaderMaterial.SetTextureOffset(M_UNIFORM_CAMERA_TEXTURE, new Vector2(bounds.uMin, bounds.vMin));
        m_legOutlineShaderMaterial.SetTextureScale(M_UNIFORM_CAMERA_TEXTURE, new Vector2(scaleU, scaleV));
        m_legOutlineShaderMaterial.SetFloat(M_UNIFORM_VIVE_CAMERA_ASPECT_RATIO, aspect);
        m_legOutlineShaderMaterial.SetVector(M_UNIFORM_CAMERA_DIRECTION, transform.forward);
        m_legOutlineShaderMaterial.SetVector(M_UNIFORM_PLAYER_DOWN_VECTOR, -m_playerObject.transform.up);

        // Apply the post effect.
        Graphics.Blit(aSource, aDestination, m_legOutlineShaderMaterial);
    }

    /*
    Description: Grabs the Vive's camera from SteamVR and disables this script if it wasn't found.
    Creator: Charlotte C. Brown
    */
    private void AcquireCamera()
    {
        // The video stream must be symmetrically acquired and released in order to properly disable the stream once there are no consumers.
        SteamVR_TrackedCamera.VideoStreamTexture source = SteamVR_TrackedCamera.Source(true);
        source.Acquire();
        
        // Auto-disable if no camera is present.
        if (!source.hasCamera)
        {
            enabled = false;
        }
    }

    /*
    Description: Releases control of the Vive's camera - normally called during shutdown.
    Creator: Charlotte C. Brown
    */
    private void ReleaseCamera()
    {
        // The video stream must be symmetrically acquired and released in order to properly disable the stream once there are no consumers.
        SteamVR_TrackedCamera.VideoStreamTexture source = SteamVR_TrackedCamera.Source(true);
        source.Release();
    }
}
