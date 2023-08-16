using UnityEngine;
using System.Collections;

/*
Description: Abstract class made to implement the functionality of a slider
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
[RequireComponent(typeof(ASlider))]
public abstract class ASliderFunctionality : MonoBehaviour
{
    protected ASlider m_slider;

    [Tooltip("The minimum value in the slider.")]
    public float m_sliderMinValue = 0.0f;
    [Tooltip("The maximum value in the slider.")]
    public float m_sliderMaxValue = 1.0f;

    /*
    Description: Get the slider component of the object, suscribe to its events, and set its initial
                 position/value.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected virtual void Awake()
    {
        //Get the slider
        m_slider = GetComponent<ASlider>();

        //Get the initial value/position of the slider
        m_slider.PSliderPercentValue = GetSliderInitialValue();

        //Set initial slider value and call event
        OnSliderValueChange(m_slider.PSliderPercentValue);

        //Suscribe to the button slider option
        m_slider.OnSliderValueChange += OnSliderValueChange;
    }

    /*
    Description: Unsuscrbie from slider events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected virtual void OnDestroy()
    {
        //Unsuscribe from the button slider option
        m_slider.OnSliderValueChange -= OnSliderValueChange;
    }

    /*
    Description: Convert the slider value to a 0 to 1 range, and move the actual slider object.
    Parameters: float aValue - The current value of the slider.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected virtual void MoveSlider(float aValue)
    {
        //Convert the slider 0 to 1 percent to a value, within the specified range
        float aValuePercent = Mathf.InverseLerp(m_sliderMinValue, m_sliderMaxValue, aValue);

        //Move the slider
        m_slider.MoveSliderObject(aValuePercent);
    }

    /*
    Description: Returns the 0 to 1 percent value that will be set for the slider at start.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected abstract float GetSliderInitialValue();


    /*
    Description: Function called when the slider value is changed.
    Parameters: float aSliderPercent - The percent value stored in the slider.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: This function is called when the Slider event OnSliderValueChange is called.
    */
    protected abstract void OnSliderValueChange(float aSliderPercent);
}
