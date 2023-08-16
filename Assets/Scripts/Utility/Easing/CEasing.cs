using UnityEngine;
using System.Collections;

using System;

/*
Description: Enum to store the easing modes supported
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public enum EEaseMode
{
    In,
    Out,
    InOut
};

/*
Description: Enum to store the easing types supported
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public enum EEaseType
{
    Linear,
    Quadratic,
    Cubic,
    Quartic,
    Quintic,
    Sin,
    Exponential,
    Circular,
    Bounce,
    Elastic,
    Back
}

//Delegate containing the signature of how an easing function looks like
public delegate float delegEaseFunction(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParamter = 1.0f);

/*
Description: Class used for general easing functions. Like getting easing functions according
             to ease type and mode. This class is the one that communicates mostly with the
             easing library.
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public class CEasing
{
    /*
    Description: Get an easing function according to a type, of mode In
    Parameters: EEaseType aEaseType - The type of ease desired.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    private static delegEaseFunction GetEasingInFunction(EEaseType aEaseType)
    {
        //The desired easing type
        switch (aEaseType)
        {
            case EEaseType.Linear:
                return Lib_Easing.CEasingFunctions.Linear;
            case EEaseType.Quadratic:
                return Lib_Easing.CEasingFunctions.QuadraticIn;
            case EEaseType.Cubic:
                return Lib_Easing.CEasingFunctions.CubicIn;
            case EEaseType.Quartic:
                return Lib_Easing.CEasingFunctions.QuarticIn;
            case EEaseType.Quintic:
                return Lib_Easing.CEasingFunctions.QuinticIn;
            case EEaseType.Sin:
                return Lib_Easing.CEasingFunctions.SinIn;
            case EEaseType.Exponential:
                return Lib_Easing.CEasingFunctions.ExponentialIn;
            case EEaseType.Circular:
                return Lib_Easing.CEasingFunctions.CircularIn;
            case EEaseType.Bounce:
                return Lib_Easing.CEasingFunctions.BounceIn;
            case EEaseType.Elastic:
                return Lib_Easing.CEasingFunctions.ElasticIn;
            case EEaseType.Back:
                return Lib_Easing.CEasingFunctions.BackIn;
            default:
                return Lib_Easing.CEasingFunctions.Linear;
        }
    }

    /*
    Description: Get an easing function according to a type, of mode Out
    Parameters: EEaseType aEaseType - The type of ease desired.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    private static delegEaseFunction GetEasingOutFunction(EEaseType aEaseType)
    {
        //The desired easing type
        switch (aEaseType)
        {
            case EEaseType.Linear:
                return Lib_Easing.CEasingFunctions.Linear;
            case EEaseType.Quadratic:
                return Lib_Easing.CEasingFunctions.QuadraticOut;
            case EEaseType.Cubic:
                return Lib_Easing.CEasingFunctions.CubicOut;
            case EEaseType.Quartic:
                return Lib_Easing.CEasingFunctions.QuarticOut;
            case EEaseType.Quintic:
                return Lib_Easing.CEasingFunctions.QuinticOut;
            case EEaseType.Sin:
                return Lib_Easing.CEasingFunctions.SinOut;
            case EEaseType.Exponential:
                return Lib_Easing.CEasingFunctions.ExponentialOut;
            case EEaseType.Circular:
                return Lib_Easing.CEasingFunctions.CircularOut;
            case EEaseType.Bounce:
                return Lib_Easing.CEasingFunctions.BounceOut;
            case EEaseType.Elastic:
                return Lib_Easing.CEasingFunctions.ElasticOut;
            case EEaseType.Back:
                return Lib_Easing.CEasingFunctions.BackOut;
            default:
                return Lib_Easing.CEasingFunctions.Linear;
        }
    }

    /*
    Description: Get an easing function according to a type, of mode InOut
    Parameters: EEaseType aEaseType - The type of ease desired.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    private static delegEaseFunction GetEasingInOutFunction(EEaseType aEaseType)
    {
        //The desired easing type
        switch (aEaseType)
        {
            case EEaseType.Linear:
                return Lib_Easing.CEasingFunctions.Linear;
            case EEaseType.Quadratic:
                return Lib_Easing.CEasingFunctions.QuadraticInOut;
            case EEaseType.Cubic:
                return Lib_Easing.CEasingFunctions.CubicInOut;
            case EEaseType.Quartic:
                return Lib_Easing.CEasingFunctions.QuarticInOut;
            case EEaseType.Quintic:
                return Lib_Easing.CEasingFunctions.QuinticInOut;
            case EEaseType.Sin:
                return Lib_Easing.CEasingFunctions.SinInOut;
            case EEaseType.Exponential:
                return Lib_Easing.CEasingFunctions.ExponentialInOut;
            case EEaseType.Circular:
                return Lib_Easing.CEasingFunctions.CircularInOut;
            case EEaseType.Bounce:
                return Lib_Easing.CEasingFunctions.BounceInOut;
            case EEaseType.Elastic:
                return Lib_Easing.CEasingFunctions.ElasticInOut;
            case EEaseType.Back:
                return Lib_Easing.CEasingFunctions.BackInOut;
            default:
                return Lib_Easing.CEasingFunctions.Linear;
        }
    }

    /*
    Description: Get an easing function according to a type and mode
    Parameters: EEaseType aEaseType - The type of ease desired.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public static delegEaseFunction GetEasingFunction(EEaseType aEaseType, EEaseMode aEaseMode)
    {
        //According to the ease mode
        switch (aEaseMode)
        {
            case EEaseMode.In:
                return GetEasingInFunction(aEaseType);
            case EEaseMode.Out:
                return GetEasingOutFunction(aEaseType);
            case EEaseMode.InOut:
                return GetEasingInOutFunction(aEaseType);
            default:
                break;
        }

        return null;
    }
}