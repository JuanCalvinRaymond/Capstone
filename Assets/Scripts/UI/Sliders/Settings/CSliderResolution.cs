using UnityEngine;
using System.Collections;
using System;

/*
Description: Slider functionality to set the screen resolution, for NonVR only. This works by
             "dividing" the slider background bar into sections according to the nnumber of supported
             resolutions. According to in which section the slider is, that resolution will be set.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CSliderResolution : ASliderFunctionality
{
    //Static read only so that the values can be computed  at compile time 
    private static readonly int[] S_WIDTH_RESOLUTIONS = { 1280, 1280, 1366, 1920, 1920 };
    private static readonly int[] S_HEIGHT_RESOLUTIONS = { 720, 1024, 768, 1080, 1200 };

    /*
    Description: Set the initial value of the slider according to the value stored in the setting storer.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override float GetSliderInitialValue()
    {
        float sliderPercent = 0;
        EScreenResolutions currentResolution = (EScreenResolutions) Enum.GetNames(typeof(EScreenResolutions)).Length-1;

        // If the setting storer is valid
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            // Get the store values from the setting storer
            int storedResWidth = CSettingsStorer.PInstanceSettingsStorer.PScreenResolution.m_width;
            int storedResHeight = CSettingsStorer.PInstanceSettingsStorer.PScreenResolution.m_height;

            // If the arrays of resolutions are not empty
            if (S_WIDTH_RESOLUTIONS.Length > 0 && S_HEIGHT_RESOLUTIONS.Length > 0 &&
                S_HEIGHT_RESOLUTIONS.Length == S_WIDTH_RESOLUTIONS.Length)//If the array of widths and heights have the same length
            {
                // Go through all the resolution widths
                for (int i = 0; i < S_WIDTH_RESOLUTIONS.Length; i++)
                {
                    // If the index in the array is bigger than the bigger number of resolutions
                    if (i > Enum.GetNames(typeof(EScreenResolutions)).Length)
                    {
                        //Exit the loop
                        break;
                    }

                    // If the value stored matches one of them
                    if (storedResWidth == S_WIDTH_RESOLUTIONS[i])
                    {
                        // If the height stored matches the same one that matched the height
                        if (storedResHeight == S_HEIGHT_RESOLUTIONS[i])
                        {
                            // Get the current value as screen resolution
                            currentResolution = (EScreenResolutions)i;
                        }
                    }
                }
            }
        }

        // If no valid resolution was found
        if (currentResolution == (EScreenResolutions)Enum.GetNames(typeof(EScreenResolutions)).Length)
        {
            //Set it to the lowest resolution
            currentResolution = (int)0;
        }

        // Get the slider percent by divinding the current resolution by the total,
        // offseting it to account for its index starting at 0 and max count being 1 value higher than the ones actually use
        sliderPercent = ((int)currentResolution + 1) / (Enum.GetNames(typeof(EScreenResolutions)).Length - 1);

        return sliderPercent;
    }

    /*
    Description: When  the slider value is changed, determine on which "section" the slider is
                 and according to that set the respective resolution.
    Parameters: float aSliderPercent - A 0 to 1 percent value of the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void OnSliderValueChange(float aSliderPercent)
    {
        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Ensure that the slider is within the 0 to 1 range
            aSliderPercent = Mathf.Clamp01(aSliderPercent);

            //Get the index of the resolution to set according to the slider percent and the current resolution values
            int resolutionIndexToSet = (int)Mathf.Lerp(0, S_WIDTH_RESOLUTIONS.Length, aSliderPercent);

            //If the resolution index is within the valid array range
            if (resolutionIndexToSet < S_WIDTH_RESOLUTIONS.Length && resolutionIndexToSet < S_HEIGHT_RESOLUTIONS.Length)
            {
                //Get the current resolution
                SScreenResolution currentResolution = CSettingsStorer.PInstanceSettingsStorer.PScreenResolution;

                //Get the respective resolution values according to the index
                currentResolution.m_width = S_WIDTH_RESOLUTIONS[resolutionIndexToSet];
                currentResolution.m_height = S_HEIGHT_RESOLUTIONS[resolutionIndexToSet];

                //Set the new resolution value in the settings storer
                CSettingsStorer.PInstanceSettingsStorer.PScreenResolution = currentResolution;
            }
        }
    }
}