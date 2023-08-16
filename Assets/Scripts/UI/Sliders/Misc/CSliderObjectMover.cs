using UnityEngine;
using System.Collections;
using System;

/*
Description: Slider functionality to move an object according between 2 positions, according to the
             value of the slider.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CSliderObjectMover : ASliderFunctionality
{
    [Tooltip("The object that will be moved according to the slider")]
    public GameObject m_objectToMove;

    [Tooltip("The local position that the object will have if the slider has a value of 0")]
    public Vector3 m_startPosition;
    [Tooltip("The local position that the object will have if the slider has a value of 1")]
    public Vector3 m_endPosition;

    [Tooltip("The starting value the slider will have")]
    public float m_initialSliderValue = 0.0f;

    public delegate void delegSliderMoverObject(float aSliderPercent, Vector3 aSliderWorldPosition, Vector3 aObjectMovedPosition);
    public event delegSliderMoverObject OnSliderMovedObject;

    /*
    Description: Start the slider at its initial value
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override float GetSliderInitialValue()
    {
        return m_initialSliderValue;
    }

    /*
    Description: Move the object local position according to the slider value, and its start and end positions.
    Parameters: float aPercent - The current percent value of the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void OnSliderValueChange(float aSliderPercent)
    {
        //If the object to move is valid
        if (m_objectToMove != null)
        {
            //Lerp the objects
            m_objectToMove.transform.localPosition = Vector3.Lerp(m_startPosition, m_endPosition, aSliderPercent);

            //If event is valid
            if(OnSliderMovedObject!=null)
            {
                OnSliderMovedObject(aSliderPercent, transform.position, m_objectToMove.transform.position);
            }
        }
    }
}
