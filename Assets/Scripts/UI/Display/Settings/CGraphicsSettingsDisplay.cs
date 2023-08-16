using UnityEngine;
using System.Collections;

/*
Description: Class uses to display the current values of the game settings. This includes
brightness, gamma, etc.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, January 9, 2017
*/
public class CGraphicsSettingsDisplay : CSettingsDisplayValues
{
    //3D Text objects where the values will be dispalyed
    public TextMesh m_qualityLevelText;
    public TextMesh m_brightnessText;
    public TextMesh m_gammaText;
    public TextMesh m_resolutionText;
    public TextMesh m_drawDistanceText;

    /*
    Description: Display the quality level setting from the settings storer
    Parameters:int aQualityLevel-Quality level to display to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 30, 2017
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateQualityLevelText(int aQualityLevel)
    {
        //Add +1 to quality, so that its index starts at 1 and not 0.
        SetIntText(ref m_qualityLevelText, aQualityLevel+1);
    }

    /*
    Description: Display the brightness setting from the settings storer
    Parameters:float aBrightnessPercent-Brightness 0-1 percentage to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateBrightnessText(float aBrightnessPercent)
    {
        SetPercentText(ref m_brightnessText, aBrightnessPercent);
    }

    /*
    Description: Display the gamma setting from the settings storer
    Parameters:float aGammaPercent-Gamma 0-1 percentage to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, Novemeber 24, 2016
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateGammaText(float aGammaPercent)
    {
        SetPercentText(ref m_gammaText, aGammaPercent);
    }

    /*
    Description: Display the quality level setting from the settings storer
    Parameters:int aQualityLevel-Quality level to display to display
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 30, 2017
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateScreenResolutionText(SScreenResolution aResolution)
    {
        SetScreenResolutionText(ref m_resolutionText, aResolution);
    }

    /*
    Description: Display the quality level setting from the settings storer
    Parameters:float aDrawDistance- A draw distance for the camera
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, February 1, 2017
    Extra Notes: This function is generally called because it suscribe to a setting change event from
    the settings storer
    */
    private void UpdateDrawDistanceText(float aDrawDistance)
    {
        //Display it as an int
        SetIntText(ref m_drawDistanceText, (int)aDrawDistance);
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
        CSettingsStorer.PInstanceSettingsStorer.OnQualityLevelChange += UpdateQualityLevelText;
        CSettingsStorer.PInstanceSettingsStorer.OnBrightnessPercentChange += UpdateBrightnessText;
        CSettingsStorer.PInstanceSettingsStorer.OnGammaPercentChange += UpdateGammaText;
        CSettingsStorer.PInstanceSettingsStorer.OnScreenResolutionChange += UpdateScreenResolutionText;
        CSettingsStorer.PInstanceSettingsStorer.OnDrawDistanceChange += UpdateDrawDistanceText;
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
        CSettingsStorer.PInstanceSettingsStorer.OnQualityLevelChange -= UpdateQualityLevelText;
        CSettingsStorer.PInstanceSettingsStorer.OnBrightnessPercentChange -= UpdateBrightnessText;
        CSettingsStorer.PInstanceSettingsStorer.OnGammaPercentChange -= UpdateGammaText;
        CSettingsStorer.PInstanceSettingsStorer.OnScreenResolutionChange -= UpdateScreenResolutionText;
        CSettingsStorer.PInstanceSettingsStorer.OnDrawDistanceChange -= UpdateDrawDistanceText;
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
        UpdateQualityLevelText(CSettingsStorer.PInstanceSettingsStorer.PQualityLevel);
        UpdateBrightnessText(CSettingsStorer.PInstanceSettingsStorer.PBrightnessPercent);
        UpdateGammaText(CSettingsStorer.PInstanceSettingsStorer.PGammaPercent);
        UpdateScreenResolutionText(CSettingsStorer.PInstanceSettingsStorer.PScreenResolution);
        UpdateDrawDistanceText(CSettingsStorer.PInstanceSettingsStorer.PDrawDistance);
    }
}
