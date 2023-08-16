using UnityEngine;
using System.Collections;

/*
Description: Class uses to display the current values of the game settings. This includes
showingPlatform, showingAimingAids, etc.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, January 9, 2017
*/
public class CGameSettingsDisplay : CSettingsDisplayValues
{
    //3D Text objects where the values will be dispalyed
    public TextMesh m_showPlatformText;
    public TextMesh m_showAimingAidsText;
    public TextMesh m_inputSensitivityText;
    public TextMesh m_invertYAxisText;

    /*
    Description: Display the input show platform setting from the settings storer
    Parameters(Optional):bool aShowPlatform- Whether the platform will show up or not
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateShowPlatformText(bool aShowPlatform)
    {
        //Display if we want to show the platform
        SetBoolText(ref m_showPlatformText, aShowPlatform);
    }

    /*
    Description: Display the input show aiming aids setting from the settings storer
    Parameters(Optional):bool aShowAimingAids- Whether the aiming aids will show up
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateShowAimingAids(bool aShowAimingAids)
    {
        //Display if we want to show the laser pointer and the crosshair
        SetBoolText(ref m_showAimingAidsText, aShowAimingAids);
    }

    /*
Description: Display the input sensitivity setting from the settings storer
Parameters(Optional):float aInputSensitivity-Input sensitivity to display
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, Novemeber 24, 2016
Extra Notes: This function is generally called because it suscribe to a setting change event from
the settings storer
*/
    private void UpdateInputSensitivityText(float aInputSensitivity)
    {
        SetFloatText(ref m_inputSensitivityText,aInputSensitivity);
    }

    /*
    Description: Display the input invert Y axis setting from the settings storer
    Parameters(Optional):bool aInvertYAxis- Whether the input for the Y axis is be inverted
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateInvertYAxisText(bool aInvertYAxis)
    {
        SetBoolText(ref m_invertYAxisText, aInvertYAxis);
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
        //Suscribe to all its settings changed event
        CSettingsStorer.PInstanceSettingsStorer.OnIsShowingPlatformChange += UpdateShowPlatformText;
        CSettingsStorer.PInstanceSettingsStorer.OnIsShowingAimingAidsChange += UpdateShowAimingAids;
        CSettingsStorer.PInstanceSettingsStorer.OnInputSensitivityChange += UpdateInputSensitivityText;
        CSettingsStorer.PInstanceSettingsStorer.OnInvertedYAxisChange += UpdateInvertYAxisText;
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
        //Unsuscribe to all its settings changed event
        CSettingsStorer.PInstanceSettingsStorer.OnIsShowingPlatformChange -= UpdateShowPlatformText;
        CSettingsStorer.PInstanceSettingsStorer.OnIsShowingAimingAidsChange -= UpdateShowAimingAids;
        CSettingsStorer.PInstanceSettingsStorer.OnInputSensitivityChange -= UpdateInputSensitivityText;
        CSettingsStorer.PInstanceSettingsStorer.OnInvertedYAxisChange -= UpdateInvertYAxisText;
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
        UpdateShowPlatformText(CSettingsStorer.PInstanceSettingsStorer.PIsShowingPlatform);
        UpdateShowAimingAids(CSettingsStorer.PInstanceSettingsStorer.PIsShowingAimingAids);
        UpdateInputSensitivityText(CSettingsStorer.PInstanceSettingsStorer.PInputSensitivity);
        UpdateInvertYAxisText(CSettingsStorer.PInstanceSettingsStorer.PIsInvertedYAxis);
    }
}