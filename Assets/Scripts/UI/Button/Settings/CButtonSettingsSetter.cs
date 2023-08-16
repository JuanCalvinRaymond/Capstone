using UnityEngine;
using System.Collections;

/*
Description: This class inherits from AButtonFunctionality is used to set all the different  properties there may
be in the settings
Creator: Alvaro Chavez Mixco
Creation Date:  Wednesday, Novemeber 23, 2016
*/
public class CButtonSettingsSetter : AButtonFunctionality
{
    public ESettingsOptions m_settingToSet;

    public float m_floatValueToSet;//Variable that will be assigned when the setting is a float
    public bool m_boolValueToSet;//Variable that will be assigned when the setting is a bool

    /*
    Description: Set the main volume percent
    Parameters: float aValue- value percent (0-1) to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    Extra Notes: 
    */
    private void SetMainVolumePercent(float aValue)
    {
        //Set volume
        CSettingsStorer.PInstanceSettingsStorer.PVolumePercent = aValue;
    }

    /*
    Description: Set the sound effect volume percent
    Parameters: float aValue- value percent (0-1) to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 10, 2017
    Extra Notes: 
    */
    private void SetSoundsEffectsVolumePercent(float aValue)
    {
        //Set volume
        CSettingsStorer.PInstanceSettingsStorer.PSoundEffectsVolume = aValue;
    }

    /*
    Description: Set the menu sounds volume percent
    Parameters: float aValue- value percent (0-1) to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 10, 2017
    */
    private void SetMenuSoundsVolumePercent(float aValue)
    {
        //Set volume
        CSettingsStorer.PInstanceSettingsStorer.PMenuSoundsVolume = aValue;
    }

    /*
    Description: Set the sound effect volume percent
    Parameters: float aValue- value percent (0-1) to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 10, 2017
    */
    private void SetMusicVolumePercent(float aValue)
    {
        //Set volume
        CSettingsStorer.PInstanceSettingsStorer.PMusicVolume = aValue;
    }

    /*
    Description: Set the brightness percent
    Parameters: float aValue- value percent (0-1) to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    */
    private void SetBrightnessPercent(float aValue)
    {
        //Set brightness
        CSettingsStorer.PInstanceSettingsStorer.PBrightnessPercent = aValue;
    }

    /*
    Description: Set the gamma percent
    Parameters: float aValue- value percent (0-1) to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016 
    */
    private void SetGammaPercent(float aValue)
    {
        //Set gamma
        CSettingsStorer.PInstanceSettingsStorer.PGammaPercent = aValue;
    }

    /*
    Description: Set the input sensitivity
    Parameters: float aValue- value to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    */
    private void SetInputSensitivity(float aValue)
    {
        //Set input sensitivity
        CSettingsStorer.PInstanceSettingsStorer.PInputSensitivity = aValue;
    }

    /*
    Description: Set the invert Y axis
    Parameters: bool aValue- bool value to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    */
    private void SetInvertYAxis(bool aValue)
    {
        //Set invert Y axis
        CSettingsStorer.PInstanceSettingsStorer.PIsInvertedYAxis = aValue;
    }

    /*
    Description: Set the show platform
    Parameters: bool aValue- bool value to set  
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    */
    private void SetShowPlatform(bool aValue)
    {
        //Set if we want to show the platform
        CSettingsStorer.PInstanceSettingsStorer.PIsShowingPlatform = aValue;
    }

    /*
    Description: Set the show aiming aids
    Parameters: bool aValue- bool value to set 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    */
    private void SetShowAimingAids(bool aValue)
    {
        //Set if we want to show the aiming aids (crosshair, laser pointers)
        CSettingsStorer.PInstanceSettingsStorer.PIsShowingAimingAids = aValue;
    }

    /*
    Description: Override of CButton OnExecution. When the button is pressed
    the script will assign to the setting storer whatever setting and value
    we have currently in this script.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, Novemeber 23, 2016
    */
    public override void OnButtonExecution()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            switch (m_settingToSet)
            {
                case ESettingsOptions.MainVolumePercent://Main Volume
                    SetMainVolumePercent(m_floatValueToSet);
                    break;
                case ESettingsOptions.SoundEffectsVolumePercent://Sound effects volume
                    SetSoundsEffectsVolumePercent(m_floatValueToSet);
                    break;
                case ESettingsOptions.MenuSoundsVolumePercent://Menu sounds volume
                    SetMenuSoundsVolumePercent(m_floatValueToSet);
                    break;
                case ESettingsOptions.MusicVolumePercent://Music volume
                    SetMusicVolumePercent(m_floatValueToSet);
                    break;
                case ESettingsOptions.BrightnessPercent://Brightness
                    SetBrightnessPercent(m_floatValueToSet);
                    break;
                case ESettingsOptions.GammaPercent://Gamma
                    SetGammaPercent(m_floatValueToSet);
                    break;
                case ESettingsOptions.InputSensitivity://Input
                    SetInputSensitivity(m_floatValueToSet);
                    break;
                case ESettingsOptions.InvertYAxis://Invert Y axis
                    SetInvertYAxis(m_boolValueToSet);
                    break;
                case ESettingsOptions.ShowAimingAids://Show aiming aids
                    SetShowAimingAids(m_boolValueToSet);
                    break;
                case ESettingsOptions.ShowPlatform://Show platform
                    SetShowPlatform(m_boolValueToSet);
                    break;
                default:
                    break;
            }
        }
    }
}