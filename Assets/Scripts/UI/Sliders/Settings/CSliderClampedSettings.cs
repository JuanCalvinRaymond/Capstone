using UnityEngine;
using System.Collections;
using System;

/*
Description: Slider functionality to set setting options that have a 0 to 1 (0 to 100 percent) range
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CSliderClampedSettings : ASliderFunctionality
{
    /*
    Description: Enum used to determine which clamped settings will be set
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public enum EClampedSettingOptionsSlider
    {
        MainVolumePercent,
        SoundEffectsVolumePercent,
        MenuSoundsVolumePercent,
        MusicVolumePercent,
        BrightnessPercent,
        GammaPercent
    }

    [Tooltip("The ASliderFunctionality slider min and max values are not used in this class.But they must not be changed from their 0 and 1 values")]
    public EClampedSettingOptionsSlider m_settingToSet;

    /*
    Description: Suscribe to the chosen setting of the setting storer.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void Awake()
    {
        //Call base awake, suscribe to slider events
        base.Awake();

        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Suscribe to the desire setting
            switch (m_settingToSet)
            {
                case EClampedSettingOptionsSlider.MainVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnVolumePercentChange += MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.SoundEffectsVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnSoundsEffectVolumeChange += MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.MenuSoundsVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnMenuSoundsVolumeChange += MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.MusicVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnMusicVolumeChange += MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.BrightnessPercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnBrightnessPercentChange += MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.GammaPercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnGammaPercentChange += MoveSlider;
                    break;
                default:
                    break;
            }
        }
        else//If there is no setting storer
        {
            //Disable this component
            enabled = false;
        }
    }

    /*
    Description: Unsuscribe from setting storer setting events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void OnDestroy()
    {
        //Call base OnDestroy, unsuscribe from slider events
        base.OnDestroy();

        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscrbie from the desire setting
            switch (m_settingToSet)
            {
                case EClampedSettingOptionsSlider.MainVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnVolumePercentChange -= MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.SoundEffectsVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnSoundsEffectVolumeChange -= MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.MenuSoundsVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnMenuSoundsVolumeChange -= MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.MusicVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnMusicVolumeChange -= MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.BrightnessPercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnBrightnessPercentChange -= MoveSlider;
                    break;
                case EClampedSettingOptionsSlider.GammaPercent:
                    CSettingsStorer.PInstanceSettingsStorer.OnGammaPercentChange -= MoveSlider;
                    break;
                default:
                    break;
            }
        }
    }

    /*
    Description: According to the setting to set, set the default initial value of the slider
                 using the value stored in the settings storer.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override float GetSliderInitialValue()
    {
        float valueToSet = 0.0f;

        //According to the setting we want to set, get the value currently stored
        switch (m_settingToSet)
        {

            case EClampedSettingOptionsSlider.MainVolumePercent://Main Volume
                valueToSet = CSettingsStorer.PInstanceSettingsStorer.PVolumePercent;
                break;
            case EClampedSettingOptionsSlider.SoundEffectsVolumePercent://Sound Effects
                valueToSet = CSettingsStorer.PInstanceSettingsStorer.PSoundEffectsVolume;
                break;
            case EClampedSettingOptionsSlider.MenuSoundsVolumePercent://Menu Sounds
                valueToSet = CSettingsStorer.PInstanceSettingsStorer.PMenuSoundsVolume;
                break;
            case EClampedSettingOptionsSlider.MusicVolumePercent://Music Volume
                valueToSet = CSettingsStorer.PInstanceSettingsStorer.PMusicVolume;
                break;
            case EClampedSettingOptionsSlider.BrightnessPercent://Brightness Percent
                valueToSet = CSettingsStorer.PInstanceSettingsStorer.PBrightnessPercent;
                break;
            case EClampedSettingOptionsSlider.GammaPercent://Gamma Percent
                valueToSet = CSettingsStorer.PInstanceSettingsStorer.PGammaPercent;
                break;
            default:
                break;
        }

        return valueToSet;
    }

    /*
    Description: Save the value of the slider in the setting storer.
    Parameters: float aSliderPercent - The current percent value of the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void OnSliderValueChange(float aSliderPercent)
    {
            //Ensure that the slider is within the 0 to 1 range
            aSliderPercent = Mathf.Clamp01(aSliderPercent);

            //According to which setting we want to set
            switch (m_settingToSet)
            {
                //Main Volume
                case EClampedSettingOptionsSlider.MainVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.PVolumePercent = aSliderPercent;
                    break;
                //Sound effect Volume
                case EClampedSettingOptionsSlider.SoundEffectsVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.PSoundEffectsVolume = aSliderPercent;
                    break;
                //Menu Sounds Volume
                case EClampedSettingOptionsSlider.MenuSoundsVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.PMenuSoundsVolume = aSliderPercent;
                    break;
                //Music Volume
                case EClampedSettingOptionsSlider.MusicVolumePercent:
                    CSettingsStorer.PInstanceSettingsStorer.PMusicVolume = aSliderPercent;
                    break;
                //Brightness
                case EClampedSettingOptionsSlider.BrightnessPercent:
                    CSettingsStorer.PInstanceSettingsStorer.PBrightnessPercent = aSliderPercent;
                    break;
                //Gamma
                case EClampedSettingOptionsSlider.GammaPercent:
                    CSettingsStorer.PInstanceSettingsStorer.PGammaPercent = aSliderPercent;
                    break;
                default:
                    break;
            }
    }
}
