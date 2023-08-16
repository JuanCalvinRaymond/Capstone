using UnityEngine;
using System.Collections;
using System;

/*
Description: Class to manage easing with a Vector2. This is done by handling
             2 CEase objects, which ease individual floats individually. All of
             the easing parameters of each vector element are the same
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public class CEaseVector2 : IEaseType<Vector2>
{
    private CEase m_easeXValue;
    private CEase m_easeYValue;

    /*
    Description: Get the desired easing function according to the ease type and mode
                 being passed.
    Parameters: EEaseType aEaseType - The ease type that will be used
                EEaseMode aEaseMode - The ease mode that will be used
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public CEaseVector2(EEaseType aEaseType, EEaseMode aEaseMode)
    {
        //Create an ease for the X value
        m_easeXValue = new CEase(aEaseType, aEaseMode);

        //Create an ease for the Y value
        m_easeYValue = new CEase(aEaseType, aEaseMode);
    }

    /*
    Description: Get the ease type
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public EEaseType GetEaseType()
    {
        //Check only one value
        return m_easeXValue.GetEaseType();
    }

    /*
    Description: Get the ease mode
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public EEaseMode GetEaseMode()
    {
        //Check only one value
        return m_easeXValue.GetEaseMode();
    }

    /*
    Description: Get the current easing time
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, March 09th, 2017
    */
    public float GetEasingTimer()
    {
        //Only check time for first value
        return m_easeXValue.GetEasingTimer();
    }

    /*
    Description: Get the ease speed
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public float GetEaseSpeed()
    {
        //Check only one value
        return m_easeXValue.GetEaseSpeed();
    }

    /*
    Description: Set the ease speed for both values of the vector2
    Parameters: float aSpeed -The ease speed that will be set.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void SetEaseSpeed(float aEaseSpeed)
    {
        m_easeXValue.SetEaseSpeed(aEaseSpeed);
        m_easeYValue.SetEaseSpeed(aEaseSpeed);
    }

    /*
    Description: Get the current value of the parameter being eased
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public Vector2 GetValue()
    {
        //Return the value in the X ease and in the Y ease
        return new Vector2(m_easeXValue.GetValue(), m_easeYValue.GetValue());
    }

    /*
    Description: Get if the current ease is running
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public bool IsEasing()
    {
        //Check if only one value is easing
        return m_easeXValue.IsEasing();
    }

    /*
    Description: Set the properties of the ease according to its current change in value
    Parameters: float aStartingValue - The value that the ease will have at start.
                float aChangeInValue - How much the starting value will change
                float aDuration - How long the ease will last
                MonoBehaviour aMonoBehaviour - The monobehavior that contains the value that will be eased
                float aExtraParameter - An extra parameters used for some easing functions like 
                                        elasticity and overshoot
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void SetEase(Vector2 aStartingValue, Vector2 aChangeInValue, float aDuration, MonoBehaviour aMonobehaviour, float aExtraParameter = 1.0f)
    {
        //Set the X ease
        m_easeXValue.SetEase(aStartingValue.x, aChangeInValue.x, aDuration, aMonobehaviour, aExtraParameter);

        //Set the Y ease
        m_easeYValue.SetEase(aStartingValue.y, aChangeInValue.y, aDuration, aMonobehaviour, aExtraParameter);
    }

    /*
    Description: Set the properties of the ease according ot the final desired value
    Parameters: float aStartingValue - The value that the ease will have at start.
                float aFinalValue - The final value the ease will have
                float aDuration - How long the ease will last
                MonoBehaviour aMonoBehaviour - The monobehavior that contains the value that will be eased
                float aExtraParameter - An extra parameters used for some easing functions like 
                                        elasticity and overshoot
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void SetEaseFinalValue(Vector2 aStartingValue, Vector2 aFinalValue, float aDuration, MonoBehaviour aMonobehaviour, float aExtraParameter = 1.0f)
    {
        //Set the ease final values
        SetEase(aStartingValue, aFinalValue - aStartingValue, aDuration, aMonobehaviour, aExtraParameter);
    }

    /*
    Description: Runs the desired ease
    Parameters: float aDelay - The amount of time the object will wait before starting the ease.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void Run(float aDelay)
    {
        //Run the  x ease
        m_easeXValue.Run(aDelay);

        //Run the y ease
        m_easeYValue.Run(aDelay);
    }

    /*
    Description: Stop the easing coroutines being ran
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void Stop()
    {
        //Stop all easing objects
        m_easeXValue.Stop();
        m_easeYValue.Stop();
    }
}
