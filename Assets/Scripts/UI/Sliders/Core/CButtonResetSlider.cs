using UnityEngine;
using System.Collections;
using System;

/*
Description: Button functionality to reset a slider to a default value.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CButtonResetSlider : AButtonFunctionality
{
    [Header("Reset Slider Settings")]
    [Tooltip("The slider that will be modified.")]
    public ASlider m_sliderSystem;

    [Tooltip("The value that will be set in the slider when this button is pressed")]
    public float m_defaultValue = 0.5f;

    /*
    Description: Button functionality to reset a slider to a default value.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function called when the Button event OnExecution is called.
    */
    public override void OnButtonExecution()
    {
        //If the slider system is valid
        if (m_sliderSystem != null)
        {
            //Set the default value of the slide
            m_sliderSystem.PSliderPercentValue = m_defaultValue;
        }
    }
}
