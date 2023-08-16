using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class CControlLayout : MonoBehaviour
{
    public Texture m_imageHTCViveControls;
    public Texture m_imageMouseKeyboardControls;
    public Texture m_imageGamepadControls;

    private void Start()
    {
        //Because this function only gets called at start, the mesh renderer is not stored anywhere
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material material = meshRenderer.material;

        //If there is a setting storer
        if(CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //According to the input method
            switch (CSettingsStorer.PInstanceSettingsStorer.PInputMethod)
            {
                //If it is a vive controller set the texture for it
                case EControllerTypes.ViveController:
                    CUtilitySetters.SetMaterialTexture(ref material, m_imageHTCViveControls);
                    break;
                //If it is a gamepad controller set the texture for it
                case EControllerTypes.GamepadController:
                    CUtilitySetters.SetMaterialTexture(ref material, m_imageGamepadControls);
                    break;
                //If it is a mouse and keyboard controller set the texture for it
                case EControllerTypes.MouseAndKeyboardController:
                    CUtilitySetters.SetMaterialTexture(ref material, m_imageMouseKeyboardControls );
                    break;
                default:
                    break;
            }
        }
        else//If there is no setting storer
        {
            //Set the texture for the mouse and keyboard
            CUtilitySetters.SetMaterialTexture(ref material, m_imageMouseKeyboardControls);
        }
    }
}
