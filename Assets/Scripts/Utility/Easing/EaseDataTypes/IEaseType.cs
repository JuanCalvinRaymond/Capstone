using UnityEngine;
using System.Collections;

/*
Description: Interface to make a type T able to ease
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public interface IEaseType<T>
{    
    /*
    Description: Get the ease type
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    EEaseType GetEaseType();

    /*
    Description: Get the ease mode
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    EEaseMode GetEaseMode();

    /*
    Description: Get the current easing time
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, March 09th, 2017
    */
    float GetEasingTimer();

    /*
    Description: Get the ease speed
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    float GetEaseSpeed();

    /*
    Description: Set the ease speed
    Parameters: float aSpeed -The ease speed that will be set.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    void SetEaseSpeed(float aEaseSpeed);

    /*
    Description: Get the current value of the parameter being eased
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    T GetValue();

    /*
    Description: Get if the current ease is running
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    bool IsEasing();

    /*
    Description: Prepare the ease to run from starting value to starting value plus a change in value
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    void SetEase(T aStartingValue, T aChangeInValue, float aDuration, MonoBehaviour aMonobehaviour, float aExtraParameter);

    /*
    Description: Prepare the ease to run from the starting value toward the end value
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    void SetEaseFinalValue(T aStartingValue, T aFinalValue, float aDuration, MonoBehaviour aMonobehaviour, float aExtraParameter);

    /*
    Description: Start the easing coroutine
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    void Run(float aDelay);

    /*
    Description: Stop the easing coroutine
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    void Stop();

}
