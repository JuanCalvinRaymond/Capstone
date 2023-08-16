using UnityEngine;
using System.Collections;
using System;

/*
Description: Child of ASlider that sets a Horizontal Slider. Calculations are done in local space. 
             So that the slider can be appropiately rotated.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CHorizontalSlider : ASlider
{
    /*
    Description: Calculate the slider value according to the hitposition (converted to local space) and
                 the limit game object boundaries.
    Paramters: Vector3 aWorldHitPosition - The world position where the slider was pressed
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void CalculateSliderPercent(Vector3 aWorldHitPosition)
    {
        // If there are limiting objects for the slider
        if (m_minGameObjectLimit != null && m_maxGameObjectLimit != null)
        {
            // Transform the world position to local space
            Vector3 localHitPosition = transform.InverseTransformPoint(aWorldHitPosition);

            // Get the percentage  of the slider , according to the position hit and
            // its distance between the min game object limit and the max game object limit
            m_sliderPercentValue = Mathf.InverseLerp(m_minGameObjectLimit.transform.localPosition.x,
                m_maxGameObjectLimit.transform.localPosition.x,
                localHitPosition.x);

            // Clamp the value to a 0 to 1 range.
            m_sliderPercentValue = Mathf.Clamp01(m_sliderPercentValue);
        }
    }

    /*
    Description: Move the slider object horizontally according to the local position of the 
                 limit objects.
    Paramters: float aPercent - The current percent value of the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public override void MoveSliderObject(float aPositionPercent)
    {
        // If there is a slider game object, and objects to limit the slider
        if (m_sliderGameObject != null && m_minGameObjectLimit != null && m_maxGameObjectLimit != null)
        {
            // Save the current position of the slider
            Vector3 resultingPosition = m_sliderGameObject.transform.localPosition;

            // Using the percent value of the slider, calculate the X position the slider
            // should have between the 2 objects in local space.
            resultingPosition.x = Mathf.Lerp(m_minGameObjectLimit.transform.localPosition.x,
                m_maxGameObjectLimit.transform.localPosition.x,
                aPositionPercent);

            // Set the new position of the slider
            m_sliderGameObject.transform.localPosition = resultingPosition;
        }
    }
}
