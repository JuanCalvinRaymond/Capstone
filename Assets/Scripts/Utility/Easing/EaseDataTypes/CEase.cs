using UnityEngine;
using System.Collections;
using System;

/*
Description: Class to manage easing with a float
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public class CEase : IEaseType<float>
{
    private EEaseType m_easeType;
    private EEaseMode m_easeMode;
    private float m_startingValue;
    private float m_changeInValue;
    private float m_duration;
    private MonoBehaviour m_monoBehaviour;
    private float m_extraParameter;

    private float m_delay;

    private float m_easeSpeed = 1.0f;

    private float m_currentTime;
    private float m_value;

    public delegEaseFunction m_easingFunction;

    /*
    Description: Get the desired easing function according to the ease type and mode
                 being passed.
    Parameters: EEaseType aEaseType - The ease type that will be used
                EEaseMode aEaseMode - The ease mode that will be used
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public CEase(EEaseType aEaseType, EEaseMode aEaseMode)
    {
        //Set the ease type and ease mode variable
        m_easeType = aEaseType;
        m_easeMode = aEaseMode;

        //Get the desired easing function
        m_easingFunction = CEasing.GetEasingFunction(aEaseType, aEaseMode);
    }

    /*
    Description: Get the ease type
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public EEaseType GetEaseType()
    {
        return m_easeType;
    }

    /*
    Description: Get the ease mode
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public EEaseMode GetEaseMode()
    {
        return m_easeMode;
    }

    /*
    Description: Get the current easing time
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, March 09th, 2017
    */
    public float GetEasingTimer()
    {
        return m_currentTime;
    }

    /*
    Description: Get the ease speed
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public float GetEaseSpeed()
    {
        return m_easeSpeed;
    }

    /*
    Description: Set the ease speed
    Parameters: float aSpeed -The ease speed that will be set.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void SetEaseSpeed(float aSpeed)
    {
        m_easeSpeed = aSpeed;
    }

    /*
    Description: Get the current value of the parameter being eased
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public float GetValue()
    {
        return m_value;
    }

    /*
    Description: Get if the current ease is running
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public bool IsEasing()
    {
        //If the current time is 0 or less
        if (m_currentTime <= 0.0f)
        {
            //Return that is not easing
            return false;
        }

        //Return the easing is being run
        return true;
    }

    /*
    Description: Set the properties of the ease according to its current change in value
    Parameters: float aStartingValue - The value that the ease will have at start.
                float aChangeInValue - How much the starting value will change
                float aDuration - How long the ease will last
                MonoBehaviour aMonoBehaviour - The monobehavior that contains that runs 
                                                the coroutine to ease the value
                float aExtraParameter - An extra parameters used for some easing functions like 
                                        elasticity and overshoot
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void SetEase(float aStartingValue, float aChangeInValue, float aDuration, MonoBehaviour aMonoBehaviour, float aExtraParameter = 1.0f)
    {
        //Set parameters
        m_startingValue = aStartingValue;
        m_changeInValue = aChangeInValue;
        m_duration = aDuration;
        m_monoBehaviour = aMonoBehaviour;
        m_extraParameter = aExtraParameter;

        //Set current value as starting value
        m_value = aStartingValue;
    }

    /*
    Description: Set the properties of the ease according ot the final desired value
    Parameters: float aStartingValue - The value that the ease will have at start.
                float aFinalValue - The final value the ease will have
                float aDuration - How long the ease will last
                                MonoBehaviour aMonoBehaviour - The monobehavior that contains that runs 
                                                               the coroutine to ease the value
                float aExtraParameter - An extra parameters used for some easing functions like 
                                        elasticity and overshoot
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void SetEaseFinalValue(float aStartingValue, float aFinalValue, float aDuration, MonoBehaviour aMonoBehaviour, float aExtraParameter = 1.0f)
    {
        //Set the ease, by calculating the change in value from the 
        //difference between the starting value and final value
        SetEase(aStartingValue, aFinalValue - aStartingValue, aDuration, aMonoBehaviour, aExtraParameter);
    }

    /*
    Description: Runs the desired ease
    Parameters: float aDelay - The amount of time the object will wait before starting the ease.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void Run(float aDelay = 0.0f)
    {
        //Set the delay time
        m_delay = aDelay;

        //If the object is not currently easing
        if (IsEasing() == false)
        {
            //Start the easing coroutine
            m_monoBehaviour.StartCoroutine(EaseCoroutine());
        }
    }

    /*
    Description: Stop the easing coroutine being ran
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void Stop()
    {
        //Stop the coroutine being ran
        m_monoBehaviour.StopCoroutine(EaseCoroutine());

        //Set the current time of the coroutine to 0
        m_currentTime = 0.0f;
    }

    /*
    Description: Coroutine to ease a value with the set easing function, until  
                 the desired value is reached.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    IEnumerator EaseCoroutine()
    {
        //Wait until the delay time is over
        yield return new WaitForSeconds(m_delay);

        //While the current time is less than the duration of the ease
        while (m_currentTime < m_duration)
        {
            //Ease the current value
            m_value = m_easingFunction(m_startingValue, m_changeInValue, m_currentTime, m_duration, m_extraParameter);

            //Increase the time according to delta time and 
            m_currentTime += Time.deltaTime * m_easeSpeed;

            //Wait for next frame
            yield return new WaitForEndOfFrame();
        }

        //If the duration time is over, ensure the correct final value is displayed
        //Starting value + change in value
        m_value = m_startingValue + m_changeInValue;

        //Wait for next frame
        yield return new WaitForEndOfFrame();

        //Set the current time to 0 to signal ease is over
        m_currentTime = 0.0f;
    }
}
