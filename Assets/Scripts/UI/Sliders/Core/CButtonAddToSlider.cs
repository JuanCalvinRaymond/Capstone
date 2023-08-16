using UnityEngine;
using System.Collections;
using System;

/*
Description: Button used to add a determined amount to a slider.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CButtonAddToSlider : AButtonFunctionality
{
    [Header("Add To Slider Settings")]
    [Tooltip("The slider that will be modified.")]
    public ASlider m_sliderSystem;

    [Tooltip("Value from -1.0 to 1.0. Amount to add to current slider percent value.")]
    [Range(-1.0f, 1.0f)]
    public float m_valueToAdd = 0.01f;

    /*
    Description: Adds a  determined amount to a slider value.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function called when the Button event OnExecution is called.
    */
    public override void OnButtonExecution()
    {
        //If the slider system is valid
        if(m_sliderSystem!=null)
        {
            //Add the amount to the current slider value
            m_sliderSystem.PSliderPercentValue += m_valueToAdd; ;
        }
    }
}
