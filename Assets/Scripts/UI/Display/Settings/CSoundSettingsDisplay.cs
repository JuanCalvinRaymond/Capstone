using UnityEngine;
using System.Collections;
using System;

/*
Description: Class uses to display the current values of the game settings. This includes
mainVolume,SoundEffectsVolume, MenuSoundsVolume, MusicVolume, etc.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, January 9, 2017
*/
public class CSoundSettingsDisplay : CSettingsDisplayValues
{
    //3D Text objects where the values will be dispalyed
    public TextMesh m_mainVolumeText;
    public TextMesh m_soundsEffectVolumeText;
    public TextMesh m_menuSoundsVolumeText;
    public TextMesh m_musicVolumeText;

    /*
    Description: Display the volume setting from the settings storer
    Parameters(Optional):float aVolumePercent-Volume 0-1 percentage to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateMainVolumeText(float aVolumePercent)
    {
        SetPercentText(ref m_mainVolumeText, aVolumePercent);
    }

    /*
    Description: Display the volume setting from the settings storer
    Parameters(Optional):float aVolumePercent-Volume 0-1 percentage to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateSoundEffectVolumeText(float aVolumePercent)
    {
        SetPercentText(ref m_soundsEffectVolumeText, aVolumePercent);
    }

    /*
    Description: Display the volume setting from the settings storer
    Parameters(Optional):float aVolumePercent-Volume 0-1 percentage to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateMenuSoundsVolumeText(float aVolumePercent)
    {
        SetPercentText(ref m_menuSoundsVolumeText, aVolumePercent);
    }

    /*
    Description: Display the volume setting from the settings storer
    Parameters(Optional):float aVolumePercent-Volume 0-1 percentage to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateMusicVolumeText(float aVolumePercent)
    {
        SetPercentText(ref m_musicVolumeText, aVolumePercent);
    }


    /*
    Description: Implementation of abstract function that will be called at Awake. This function is
    where the class will suscribe to all the events it cares about.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    Extra Notes: This function doesn't check that the CSettingStorer instance is valid. This
    should be done before calling the function.
    */
    protected override void SuscribeToEvents()
    {
        CSettingsStorer.PInstanceSettingsStorer.OnVolumePercentChange += UpdateMainVolumeText;
        CSettingsStorer.PInstanceSettingsStorer.OnSoundsEffectVolumeChange += UpdateSoundEffectVolumeText;
        CSettingsStorer.PInstanceSettingsStorer.OnMenuSoundsVolumeChange += UpdateMenuSoundsVolumeText;
        CSettingsStorer.PInstanceSettingsStorer.OnMusicVolumeChange += UpdateMusicVolumeText;
    }

    /*
    Description: Implementation of abstract function that will be called OnDestroy. This function is
    where the class will unsuscrbie from all the events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    Extra Notes: This function doesn't check that the CSettingStorer instance is valid. This
    should be done before calling the function.
    */
    protected override void UnsuscribeToEvents()
    {
        CSettingsStorer.PInstanceSettingsStorer.OnVolumePercentChange -= UpdateMainVolumeText;
        CSettingsStorer.PInstanceSettingsStorer.OnSoundsEffectVolumeChange -= UpdateSoundEffectVolumeText;
        CSettingsStorer.PInstanceSettingsStorer.OnMenuSoundsVolumeChange -= UpdateMenuSoundsVolumeText;
        CSettingsStorer.PInstanceSettingsStorer.OnMusicVolumeChange -= UpdateMusicVolumeText;
    }

    /*
    Description: Implementation of abstract function used to update the values displayed in all the texts.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    Extra Notes: This function doesn't check that the CSettingStorer instance is valid. This
    should be done before calling the function.
    */
    protected override void SetDisplayValues()
    {
        UpdateMainVolumeText(CSettingsStorer.PInstanceSettingsStorer.PVolumePercent);
        UpdateSoundEffectVolumeText(CSettingsStorer.PInstanceSettingsStorer.PSoundEffectsVolume);
        UpdateMenuSoundsVolumeText(CSettingsStorer.PInstanceSettingsStorer.PMenuSoundsVolume);
        UpdateMusicVolumeText(CSettingsStorer.PInstanceSettingsStorer.PMusicVolume);
    }
}
