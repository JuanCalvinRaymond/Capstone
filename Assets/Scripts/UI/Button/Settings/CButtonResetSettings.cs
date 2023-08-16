using UnityEngine;
using System.Collections;
using System;

/*
Description: Class that inherits from AButtonFunctionality. This class provides the ability
             to reset to their default values different setting options
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CButtonResetSettings : AButtonFunctionality
{
    /*
    Description: Enum to determine which settings will be reset.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public enum ESetttingsToReset
    {
        Gameplay,
        Audio,
        Graphics,
        AllSettingss
    };

    public ESetttingsToReset m_settingToReset;

    /*
    Description: Reset the settings to their default values using the setting storer.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function called whe the button OnExecution event is called.
    */
    public override void OnButtonExecution()
    {
        //If there is a setting storer.
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //According to which settings the user wants to reset.
            switch (m_settingToReset)
            {
                //Reset gameplay settings
                case ESetttingsToReset.Gameplay:
                    CSettingsStorer.PInstanceSettingsStorer.ResetGameplaySetttings();
                    break;
                //Reset audio settings
                case ESetttingsToReset.Audio:
                    CSettingsStorer.PInstanceSettingsStorer.ResetAudioSettings();
                    break;
                //Reset graphics settings
                case ESetttingsToReset.Graphics:
                    CSettingsStorer.PInstanceSettingsStorer.ResetGraphicsSettings();
                    break;
                //Reset all the settings
                case ESetttingsToReset.AllSettingss:
                    CSettingsStorer.PInstanceSettingsStorer.ResetAllSettings();
                    break;
                default:
                    break;
            }
        }
    }
}
