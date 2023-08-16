using UnityEngine;
using System.Collections;
using System;

/*
Description: Slider functionality to set setting options that are not limited to  a 0 to 1 (0 to 100 percent) range.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
[RequireComponent(typeof(ASlider))]
public class CSliderUnclampedSettings : ASliderFunctionality
{
    /*
    Description: Enum used to determine which unclamped settings will be set
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public enum EUnclampedSettingOptionsSlider
    {
        InputSensitivity,
        DrawDistance
    }

    public EUnclampedSettingOptionsSlider m_settingToSet;

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
            //In case the setting changes outside of our control, update the slider accordingly
            //According to which setting we want to set
            switch (m_settingToSet)
            {
                //Input sensitivity
                case EUnclampedSettingOptionsSlider.InputSensitivity:
                    CSettingsStorer.PInstanceSettingsStorer.OnInputSensitivityChange += MoveSlider;
                    break;
                //Draw distance
                case EUnclampedSettingOptionsSlider.DrawDistance:
                    CSettingsStorer.PInstanceSettingsStorer.OnDrawDistanceChange += MoveSlider;
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
            //Unsuscribe from events
            //According to which setting we want to set
            switch (m_settingToSet)
            {
                //Input sensitivity
                case EUnclampedSettingOptionsSlider.InputSensitivity:
                    CSettingsStorer.PInstanceSettingsStorer.OnInputSensitivityChange -= MoveSlider;
                    break;
                //Draw distance
                case EUnclampedSettingOptionsSlider.DrawDistance:
                    CSettingsStorer.PInstanceSettingsStorer.OnDrawDistanceChange -= MoveSlider;
                    break;
                default:
                    break;
            }
        }
    }

    /*
    Description: According to the setting to set, set the default initial value of the slider
                 using the value stored in the settings storer. Because it is unclamped, the 
                 value stored in the Setting Storer is converted to a percent according to the 
                 slider min and max values.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override float GetSliderInitialValue()
    {
        float sliderValue = 0.0f;

        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //According to which setting we want to set,get the initial value
            switch (m_settingToSet)
            {
                //Input sensitivity
                case EUnclampedSettingOptionsSlider.InputSensitivity:
                    sliderValue = CSettingsStorer.PInstanceSettingsStorer.PInputSensitivity;
                    break;
                //Draw distance
                case EUnclampedSettingOptionsSlider.DrawDistance:
                    sliderValue = CSettingsStorer.PInstanceSettingsStorer.PDrawDistance;
                    break;
                default:
                    break;
            }
        }

        //Convert the slider 0 to 1 percent to a value, within the specified range
        return Mathf.InverseLerp(m_sliderMinValue, m_sliderMaxValue, sliderValue);
    }

    /*
    Description: Save the value of the slider in the setting storer. Because the value is unclamped
                 the percent being passed is lerped according to the slider min and max values.
    Parameters: float aSliderPercent - The current percent value of the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void OnSliderValueChange(float aSliderPercent)
    {
        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Convert the slider 0 to 1 percent to a value, within the specified range
            float sliderValue = Mathf.Lerp(m_sliderMinValue, m_sliderMaxValue, aSliderPercent);

            //According to which setting we want to set
            switch (m_settingToSet)
            {
                //Input sensitivity
                case EUnclampedSettingOptionsSlider.InputSensitivity:
                    CSettingsStorer.PInstanceSettingsStorer.PInputSensitivity = sliderValue;
                    break;
                //Draw distance
                case EUnclampedSettingOptionsSlider.DrawDistance:
                    CSettingsStorer.PInstanceSettingsStorer.PDrawDistance = sliderValue;
                    break;
                default:
                    break;
            }
        }
    }
}
