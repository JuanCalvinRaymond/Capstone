using UnityEngine;
using System.Collections;


/*
Description: Script used to apply a post process effect that has a single float variable as intensity.
This script is also responsible for setting that value as an uniform.
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, December 27, 2016
Extra Notes: The shader file must have a uniform named "u_effectStrength" for it to work.
*/
public class CGenericPostProccess : MonoBehaviour
{
    /*
    Description: Enum to know if the post process is linked to any setting. If it is linked to a setting
    this would ensure that the script suscribe to the correct OnChangeEvent
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    public enum EPostProcessSettings
    {
        None,
        Brightness,
        Gamma
    }

    private const string M_UNIFORM_EFFECT_STRENGTH_NAME = "u_effectStrength";

    private Material m_postProcessMaterial;

    private float m_effectAmount = 0.0f;

    public Shader m_postProcessShader;

    public float m_minimumEffectStrength = 0.5f;
    public float m_maximumEffectStrength = 1.5f;

    //Used to know if we want to suscribe to any event
    public EPostProcessSettings m_postProcessSetting = EPostProcessSettings.None;

    /*
    Description: Create a material using the desired shader
    Parameters(Optional):
    Creator: Alvaro Chavez Mixco
    Extra Notes:
    */
    private void Awake()
    {
        //If there is a shader
        if (m_postProcessShader != null)
        {
            //Create a material for it
            m_postProcessMaterial = new Material(m_postProcessShader);
        }
        else//If there is no shader
        {
            enabled = false;//Disable the component
        }
    }

    /*
    Description: Suscribe to the settings change event from the setting storer, and
    update the intial value of the effect
    Creator: Alvaro Chavez Mixco
    */
    private void Start()
    {
        SuscribeToSettingChangeEvent(m_postProcessSetting);
    }

    /*
    Description: Unsuscribe to the settings change event from the setting storer
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    private void OnDestroy()
    {
        UnSuscribeToSettingChangeEvent(m_postProcessSetting);
    }

    /*
    Description: Suscribe to the settings change event from the setting storer, and update the current
    strength of the effect
    Parameters: EPostProcessSettings aSetting-The setting to which the script would suscribe in case it changes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    Extra Notes: This function will disable the component if the setting sotrer is null
    */
    private void SuscribeToSettingChangeEvent(EPostProcessSettings aSetting)
    {
        //If the setting storer is valid
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            switch (aSetting)
            {
                case EPostProcessSettings.None:
                    //Set the starting value
                    SetEffectStrength(m_effectAmount);
                    break;

                case EPostProcessSettings.Brightness:
                    //Suscribe to the brightness percent change event
                    CSettingsStorer.PInstanceSettingsStorer.OnBrightnessPercentChange += SetEffectStrength;
                    //Set the intial states
                    SetEffectStrength(CSettingsStorer.PInstanceSettingsStorer.PBrightnessPercent);
                    break;

                case EPostProcessSettings.Gamma:
                    //Suscribe to the gamma percent change event
                    CSettingsStorer.PInstanceSettingsStorer.OnGammaPercentChange += SetEffectStrength;
                    //Set the intial states
                    SetEffectStrength(CSettingsStorer.PInstanceSettingsStorer.PGammaPercent);
                    break;

                default:
                    break;
            }
        }
        else//If there is no setting storer
        {
            //Disable the component
            enabled = false;
        }
    }

    /*
    Description: Unsuscribe to the settings change event from the setting storer.
    Parameters: EPostProcessSettings aSetting-The setting to which the script would unsuscribe
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    */
    private void UnSuscribeToSettingChangeEvent(EPostProcessSettings aSetting)
    {
        //If the setting storer is valid
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            switch (aSetting)
            {
                case EPostProcessSettings.None:
                    break;

                case EPostProcessSettings.Brightness:
                    //Unsuscribe to the brightness percent change event
                    CSettingsStorer.PInstanceSettingsStorer.OnBrightnessPercentChange -= SetEffectStrength;
                    break;

                case EPostProcessSettings.Gamma:
                    //Unsuscribe to the gamma percent change event
                    CSettingsStorer.PInstanceSettingsStorer.OnGammaPercentChange -= SetEffectStrength;
                    break;

                default:
                    break;
            }
        }
    }

    /*
    Description: Apply a post process using the desired post process shader (through the material that is using it)
    Parameters(Optional):
    Creator: Alvaro Chavez Mixco
    Extra Notes:
    */
    private void OnRenderImage(RenderTexture aSource, RenderTexture aDestination)
    {
        //Apply the post process effect, no if check for material. Because in Awake,
        // if the material is null the component will be disabled
        Graphics.Blit(aSource, aDestination, m_postProcessMaterial);
    }

    /*
    Description: Set the desired effect strength percent as a uniform in the post process material
    Parameters(Optional): float aEffectStrength-The desired strength of the effect
    Creator: Alvaro Chavez Mixco
    Extra Notes: This function gets called because it suscribe to  an event on the ngs menu
    */
    public void SetEffectStrength(float aEffectStrength)
    {
        //Assuming the percent is from 0 to 1, convert it to actual  values between the min and max
        m_effectAmount = Mathf.Lerp(m_minimumEffectStrength, m_maximumEffectStrength, aEffectStrength);

        //If the material is valid
        if (m_postProcessMaterial != null)
        {
            //Set its values
            m_postProcessMaterial.SetFloat(M_UNIFORM_EFFECT_STRENGTH_NAME, m_effectAmount);
        }
    }
}
