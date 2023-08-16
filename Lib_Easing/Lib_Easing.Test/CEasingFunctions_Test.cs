using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using NUnit.Framework;
using Lib_Easing;

namespace Lib_Easing.Test
{
    /*
    Description: Class used to unit test the CEasingFunctions test class
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    [TestFixture]
    public class CEasingFunctions_Test
    {
        /*
        Description: Enum used for testing purposes only. It contains all the different ease types implemented 
                     by the CEasingFunctions class
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        private enum EEaseTypes
        {
            Linear,
            QuadraticIn,
            QuadraticOut,
            QuadraticInOut,
            CubicIn,
            CubicOut,
            CubicInOut,
            QuarticIn,
            QuarticOut,
            QuarticInOut,
            QuinticIn,
            QuinticOut,
            QuinticInOut,
            SinIn,
            SinOut,
            SinInOut,
            ExponentialIn,
            ExponentialOut,
            ExponentialInOut,
            CircularIn,
            CircularOut,
            CircularInOut,
            BounceIn,
            BounceOut,
            BounceInOut,
            ElasticIn,
            ElasticOut,
            ElasticInOut,
            BackIn,
            BackOut,
            BackInOut
        }

        //A percent between 0 and 1. This was made into a variabel so that it can easily be modified to
        //test the easing functions at multiple points through their curves. Ideally this would
        //be left at 0.5f (half 
        public const float M_TIME_MIDDLE_VALUE_RANGE_TEST = 0.5f;


        public const double M_ERROR_MARGIN = 0.01;
        public const float M_RANGE_TEST_EASE_DURATION = 10.0f;

        public const float M_RANGE_TEST_STARTING_VALUE = 0.0f;
        public const float M_RANGE_TEST_CHANGE_IN_VALUE = 35.0f;

        public delegate float delegEasingFloatFunction(
            float aStartValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1);

        #region GET_EASING_FUNCTIONS

        /*
        Description: Classs to get the easing function of the desired type as a delegate.
        Parameters: EEaseTypes aEaseType - The type of easing that will be obtained
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        private static delegEasingFloatFunction GetEasingFloatFunction(EEaseTypes aEaseType)
        {
            //According to the easing type desired
            switch (aEaseType)
            {
                //Get the corresponding function from the CEasing function class
                case EEaseTypes.Linear:
                    return CEasingFunctions.Linear;
                case EEaseTypes.QuadraticIn:
                    return CEasingFunctions.QuadraticIn;
                case EEaseTypes.QuadraticOut:
                    return CEasingFunctions.QuadraticOut;
                case EEaseTypes.QuadraticInOut:
                    return CEasingFunctions.QuadraticInOut;
                case EEaseTypes.CubicIn:
                    return CEasingFunctions.CubicIn;
                case EEaseTypes.CubicOut:
                    return CEasingFunctions.CubicOut;
                case EEaseTypes.CubicInOut:
                    return CEasingFunctions.CubicInOut;
                case EEaseTypes.QuarticIn:
                    return CEasingFunctions.QuarticIn;
                case EEaseTypes.QuarticOut:
                    return CEasingFunctions.QuarticOut;
                case EEaseTypes.QuarticInOut:
                    return CEasingFunctions.QuarticInOut;
                case EEaseTypes.QuinticIn:
                    return CEasingFunctions.QuinticIn;
                case EEaseTypes.QuinticOut:
                    return CEasingFunctions.QuinticOut;
                case EEaseTypes.QuinticInOut:
                    return CEasingFunctions.QuinticInOut;
                case EEaseTypes.SinIn:
                    return CEasingFunctions.SinIn;
                case EEaseTypes.SinOut:
                    return CEasingFunctions.SinOut;
                case EEaseTypes.SinInOut:
                    return CEasingFunctions.SinInOut;
                case EEaseTypes.ExponentialIn:
                    return CEasingFunctions.ExponentialIn;
                case EEaseTypes.ExponentialOut:
                    return CEasingFunctions.ExponentialOut;
                case EEaseTypes.ExponentialInOut:
                    return CEasingFunctions.ExponentialInOut;
                case EEaseTypes.CircularIn:
                    return CEasingFunctions.CircularIn;
                case EEaseTypes.CircularOut:
                    return CEasingFunctions.CircularOut;
                case EEaseTypes.CircularInOut:
                    return CEasingFunctions.CircularInOut;
                case EEaseTypes.BounceIn:
                    return CEasingFunctions.BounceIn;
                case EEaseTypes.BounceOut:
                    return CEasingFunctions.BounceOut;
                case EEaseTypes.BounceInOut:
                    return CEasingFunctions.BounceInOut;
                case EEaseTypes.ElasticIn:
                    return CEasingFunctions.ElasticIn;
                case EEaseTypes.ElasticOut:
                    return CEasingFunctions.ElasticOut;
                case EEaseTypes.ElasticInOut:
                    return CEasingFunctions.ElasticInOut;
                case EEaseTypes.BackIn:
                    return CEasingFunctions.BackIn;
                case EEaseTypes.BackOut:
                    return CEasingFunctions.BackOut;
                case EEaseTypes.BackInOut:
                    return CEasingFunctions.BackInOut;
            }

            return null;
        }
        #endregion

        #region CHECK_VALUES_FUNCTIONS

        /*
         Description: Function to test that the values at one quarter time, half time, and
                      three quarter, being calculated by an easing function match the expected values
                      being passsed in.
         Parameters: delegEasingFloatFunction aEasingFunction - The easing function that will be tested
                     float aExpectedResultAtOneQuarter - The expected value for the easing function at one quarter time
                     float aExpectedResultAtHalf - The expected value for the easing function at half time
                     float aExpectedResultAtThreeQuarters - The expected value for the easing function at three quarters time
         Creator: Alvaro Chavez Mixco
         Creation Date : Thursday, February 23rd, 2017
         */
        public static void TestFunctionValues(delegEasingFloatFunction aEasingFunction,
            float aExpectedResultAtOneQuarter, 
            float aExpectedResultAtHalf,
            float aExpectedResultAtThreeQuarters)
        {
            //Test results at certain points
            float startValue = 0.0f;
            float changeInValue = 10.0f;
            float duration = 1.0f;

            //Calculate the values using the actual function being tested
            float actualResultAtStart = aEasingFunction(startValue, changeInValue, 0.0f, duration);
            float actualResultAtOneQuarter = aEasingFunction(startValue, changeInValue, duration / 4.0f, duration);
            float actualResultAtHalf = aEasingFunction(startValue, changeInValue, duration / 2.0f, duration);
            float actualResultAtThreeQuarters = aEasingFunction(startValue, changeInValue, (duration / 4.0f) * 3.0f, duration);
            float actualResultAtEnd = aEasingFunction(startValue, changeInValue, duration, duration);

            //Compare the actual results with the expected results
            Assert.AreEqual(aExpectedResultAtOneQuarter, actualResultAtOneQuarter, M_ERROR_MARGIN);
            Assert.AreEqual(aExpectedResultAtHalf, actualResultAtHalf, M_ERROR_MARGIN);
            Assert.AreEqual(aExpectedResultAtThreeQuarters, actualResultAtThreeQuarters, M_ERROR_MARGIN);
        }

        /*
        Description: Tests that the value at the start time, and end time are correct. Also check that
                     values with time below zero or time above the duration of ease are the starting value 
                     and the ending value, corresnpondingly. Finally it also checks that some value within the start and end of the curve, 
                     is limited to those values
        Parameters: delegEasingFloatFunction aEasingFunction - The easing function that will be tested
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static void TestRangeStartEndHalfValues(delegEasingFloatFunction aEasingFunction)
        {
            //Initialize variables
            float resultBeforeZero;
            float resultAtStart;
            float resultAtHalf;
            float resultAtEnd;
            float resultOverDuration;

            ///Calculate the values of each result at that time, using the desried easing fucntion
            CalculateValues(aEasingFunction, out resultBeforeZero, out resultAtStart, out resultAtHalf, out resultAtEnd, out resultOverDuration);

            //Check that the result at the time between start and duration, is limited to a value between start value and end value
            CheckValueWithinStartEnd(resultAtStart, resultAtEnd, resultAtHalf);

            //Check that the vaue at start time is the same as the start value, and that the value at the 
            //end time is the same as the starting value + changeInValue
            CheckValueAtStartAndEnd(resultAtStart, resultAtEnd);

            //Check that the result at a time below 0 is the starting value, and that a result at a time over duration is the ending value
            CheckValueOverDuration(resultBeforeZero, resultOverDuration);
        }

        /*
        Description: Tests that the value at the start time, and end time are correct. Also check that
                     values with time below zero or time above the duration of ease are the starting value 
                     and the ending value, corresnpondingly.
        Parameters: delegEasingFloatFunction aEasingFunction - The easing function that will be tested
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static void TestRangeStartEndValues(delegEasingFloatFunction aEasingFunction)
        {
            //Initialize variables
            float resultBeforeZero;
            float resultAtStart;
            float resultAtHalf;
            float resultAtEnd;
            float resultOverDuration;

            ///Calculate the values of each result at that time, using the desried easing fucntion
            CalculateValues(aEasingFunction, out resultBeforeZero, out resultAtStart, out resultAtHalf, out resultAtEnd, out resultOverDuration);

            //Check that the vaue at start time is the same as the start value, and that the value at the 
            //end time is the same as the starting value + changeInValue
            CheckValueAtStartAndEnd(resultAtStart, resultAtEnd);

            //Check that the result at a time below 0 is the starting value, and that a result at a time over duration is the ending value
            CheckValueOverDuration(resultBeforeZero, resultOverDuration);
        }

        /*
        Description: Tests that the value at the start time, and end time are correct. Also check that
                     values with time below zero or time above the duration of ease are the starting value 
                     and the ending value, corresnpondingly.
        Parameters: delegEasingFloatFunction aEasingFunction - The easing function that will be tested
                    out float aResultBeforeZero - The float that will store the value of the easing function at a time below 0
                    out float aResultAtStart - The float that will store the value of the easing function at time 0
                    out float aResultAtHalf - The float that will store the value of the easing function at a time between 0 and duration time
                    out float aResultAtEnd - The float that will store the value of the easing function at duration time
                    out float aResultOverEnd - The float that will store the value of the easing function at a time above the duration time
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static void CalculateValues(delegEasingFloatFunction aEasingFunction, out float aResultBeforeZero,
            out float aResultAtStart, out float aResultAtHalf, out float aResultAtEnd, out float aResultOverEnd)
        {
            //Calculate the result at a time below 0
            aResultBeforeZero = aEasingFunction(M_RANGE_TEST_STARTING_VALUE, M_RANGE_TEST_CHANGE_IN_VALUE, M_RANGE_TEST_EASE_DURATION * -0.5f, M_RANGE_TEST_EASE_DURATION);

            //Calculate the result at time 0
            aResultAtStart = aEasingFunction(M_RANGE_TEST_STARTING_VALUE, M_RANGE_TEST_CHANGE_IN_VALUE, 0.0f, M_RANGE_TEST_EASE_DURATION);

            //Calculate the result at a time between 0 and duration time
            aResultAtHalf = aEasingFunction(M_RANGE_TEST_STARTING_VALUE, M_RANGE_TEST_CHANGE_IN_VALUE, M_RANGE_TEST_EASE_DURATION * M_TIME_MIDDLE_VALUE_RANGE_TEST, M_RANGE_TEST_EASE_DURATION);

            //Calculate the result at complete duration time
            aResultAtEnd = aEasingFunction(M_RANGE_TEST_STARTING_VALUE, M_RANGE_TEST_CHANGE_IN_VALUE, M_RANGE_TEST_EASE_DURATION * 1.0f, M_RANGE_TEST_EASE_DURATION);

            //Calculate the result at a time above the duration time
            aResultOverEnd = aEasingFunction(M_RANGE_TEST_STARTING_VALUE, M_RANGE_TEST_CHANGE_IN_VALUE, M_RANGE_TEST_EASE_DURATION * 1.5f, M_RANGE_TEST_EASE_DURATION);
        }

        /*
        Description: Tests that the result of the easing function at time 0 is the starting value; and that
                     the result of the easing function at duration time is the starting value + the change in value
        Parameters: float aResultAtStart - The value of the easing function at time 0
                    float aResultAtEnd - The value of the easing function at duration time
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static void CheckValueAtStartAndEnd(float aResultAtStart, float aResultAtEnd)
        {
            //Test if the result of the easing function at time 0 is the starting value
            Assert.AreEqual(M_RANGE_TEST_STARTING_VALUE, aResultAtStart, M_ERROR_MARGIN);

            //if the result of the easing function at duration time is the end value
            Assert.AreEqual(M_RANGE_TEST_STARTING_VALUE + M_RANGE_TEST_CHANGE_IN_VALUE * 1.0f, aResultAtEnd, M_ERROR_MARGIN);
        }

        /*
        Description: Tests that a result of the easing function at a time between 0 and duration gives
                     a value between the result at start and the result at end
        Parameters: float aResultAtStart - The value of the easing function at time 0
                    float aResultAtEnd - The value of the easing function at duration time
                    float aResultBetween - The value of the easing function at a time between 0 and duration
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static void CheckValueWithinStartEnd(float aResultAtStart, float aResultAtEnd, float aResultBetween)
        {
            //If the value between is equals or bigger than the start result
            Assert.GreaterOrEqual(aResultBetween, aResultAtStart);

            //If the value between is equals or smaller than the end result
            Assert.LessOrEqual(aResultBetween, aResultAtEnd);
        }

        /*
        Description: Tests that a result of the easing function at a negative time result in 
                     the value of the easing function at time 0; and that the result of the easing function,
                     at a time bigger than the duration results in the value at duration time
        Parameters: float aValueBeforeZeroTime - The value of the easing function at a time smaller than 0
                    float aValueOverDuration - The value of the easing function at a time bigger than duration
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static void CheckValueOverDuration(float aValueBeforeZeroTime, float aValueOverDuration)
        {
            //If the value at a time smaller than 0 is not the same as the starting value
            Assert.AreEqual(M_RANGE_TEST_STARTING_VALUE, aValueBeforeZeroTime, M_ERROR_MARGIN);

            //If the value at a time bigger than the duration is not the same as the end value
            Assert.AreEqual(M_RANGE_TEST_STARTING_VALUE + M_RANGE_TEST_CHANGE_IN_VALUE, aValueOverDuration, M_ERROR_MARGIN);
        }

        #endregion

        #region FLOAT_EASE_TESTS


        /*
        Description: Function to unit test CEasingFunctions, LinearFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void LinearFloat_Test()
        {
            //Get the linear ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.Linear);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                2.5f,
                5.0f,
                7.5f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuadraticInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuadraticInFloat_Test()
        {
            //Get the QuadraticIn ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuadraticIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.625f,
                2.5f,
                5.625f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuadraticOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuadraticOutFloat_Test()
        {
            //Get the QuadraticOut ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuadraticOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                4.375f,
                7.5f,
                9.375f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuadraticInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuadraticInOutFloat_Test()
        {
            //Get the QuadraticInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuadraticInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                1.25f,
                5.0f,
                8.75f);
        }

        /*
        Description: Function to unit test CEasingFunctions, CubicInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void CubicInFloat_Test()
        {
            //Get the CubicInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.CubicIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.15625f,
                1.25f,
                4.21875f);
        }

        /*
        Description: Function to unit test CEasingFunctions, CubicOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void CubicOutFloat_Test()
        {
            //Get the CubicOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.CubicOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                5.78125f,
                8.75f,
                9.84375f);
        }

        /*
        Description: Function to unit test CEasingFunctions, CubicInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void CubicInOutFloat_Test()
        {
            //Get the CubicInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.CubicInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.625f,
                5.0f,
                9.375f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuarticInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuarticInFloat_Test()
        {
            //Get the QuarticInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuarticIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.03906f,
                0.625f,
                3.16406f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuarticOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuarticOutFloat_Test()
        {
            //Get the QuarticOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuarticOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                6.83594f,
                9.375f,
                9.96094f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuarticInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuarticInOutFloat_Test()
        {
            //Get the QuarticInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuarticInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.3125f,
                5.0f,
                9.6875f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuinticInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuinticInFloat_Test()
        {
            //Get the QuinticInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuinticIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.009765f,
                0.3125f,
                2.37305f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuinticOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuinticOutFloat_Test()
        {
            //Get the QuinticOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuinticOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                7.62695f,
                9.6875f,
                9.99023f);
        }

        /*
        Description: Function to unit test CEasingFunctions, QuinticInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void QuinticInOutFloat_Test()
        {
            //Get the QuinticInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.QuinticInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.15625f,
                5.0f,
                9.84375f);
        }

        /*
        Description: Function to unit test CEasingFunctions, SinInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void SinInFloat_Test()
        {
            //Get the SinInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.SinIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.761205f,
                2.92893f,
                6.17317f);
        }

        /*
        Description: Function to unit test CEasingFunctions, SinOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void SinOutFloat_Test()
        {
            //Get the SinOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.SinOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                3.82683f,
                7.07107f,
                9.2388f);
        }

        /*
        Description: Function to unit test CEasingFunctions, SinInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void SinInOutFloat_Test()
        {
            //Get the SinInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.SinInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                1.4644f,
                5.0f,
                8.5355f);
        }

        /*
        Description: Function to unit test CEasingFunctions, ExponentialInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void ExponentialInFloat_Test()
        {
            //Get the ExponentialInFloat function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.ExponentialIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.055242f,
                0.3125f,
                1.76777f);
        }

        /*
        Description: Function to unit test CEasingFunctions, ExponentialOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void ExponentialOutFloat_Test()
        {
            //Get the ExponentialOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.ExponentialOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                8.23223f,
                9.6875f,
                9.94476f);
        }

        /*
        Description: Function to unit test CEasingFunctions, ExponentialInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void ExponentialInOutFloat_Test()
        {
            //Get the ExponentialInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.ExponentialInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.15625f,
                9.9951f,
                9.99984f);
        }

        /*
        Description: Function to unit test CEasingFunctions, CircularInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void CircularInFloat_Test()
        {
            //Get the CircularInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.CircularIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.317542f,
                1.33975f,
                3.38562f);
        }

        /*
        Description: Function to unit test CEasingFunctions, CircularOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void CircularOutFloat_Test()
        {
            //Get the CircularOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.CircularOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                6.61438f,
                8.66025f,
                9.68246f);
        }

        /*
        Description: Function to unit test CEasingFunctions, CircularInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void CircularInOutFloat_Test()
        {
            //Get the CircularInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.CircularInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.66987f,
                5.0f,
                9.3301f);
        }

        /*
        Description: Function to unit test CEasingFunctions, BounceInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void BounceInFloat_Test()
        {
            //Get the BounceInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.BounceIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.2734375f,
                2.34375f,
                5.2734f);
        }

        /*
        Description: Function to unit test CEasingFunctions, BounceOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void BounceOutFloat_Test()
        {
            //Get the BounceOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.BounceOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                4.7265625f,
                7.65625f,
                9.7265f);
        }

        /*
        Description: Function to unit test CEasingFunctions, BounceInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void BounceInOutFloat_Test()
        {
            //Get the BounceInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.BounceInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            //Finally it also checks that some value withing the start and end of the curve, is limited to those values
            TestRangeStartEndHalfValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                1.171875f,
                5.0f,
                8.828125f);
        }

        /*
        Description: Function to unit test CEasingFunctions, ElasticInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void ElasticInFloat_Test()
        {
            //Get the ElasticInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.ElasticIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            TestRangeStartEndValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                -1.7782f,
                -1.58f,
                2.8117f);
        }

        /*
        Description: Function to unit test CEasingFunctions, ElasticOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void ElasticOutFloat_Test()
        {
            //Get the ElasticOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.ElasticOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            TestRangeStartEndValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                7.18f,
                11.5811f,
                11.7782f);
        }

        /*
        Description: Function to unit test CEasingFunctions, ElasticInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void ElasticInOutFloat_Test()
        {
            //Get the ElasticInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.ElasticInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            TestRangeStartEndValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.68f,
                -2.1122f,
                18.35f);
        }

        /*
        Description: Function to unit test CEasingFunctions, BackInFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void BackInFloat_Test()
        {
            //Get the BackInFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.BackIn);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            TestRangeStartEndValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                -0.3125f,
                0.0f,
                2.8125f);
        }

        /*
        Description: Function to unit test CEasingFunctions, BackOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void BackOutFloat_Test()
        {
            //Get the BackOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.BackOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            TestRangeStartEndValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                7.1875f,
                10.0f,
                10.3125f);
        }

        /*
        Description: Function to unit test CEasingFunctions, BackInOutFloat function.
                     The function will check that the result values of the function are within the correct value
                     range, and that they provide the correct result at multiple times.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        [Test]
        public static void BackInOutFloat_Test()
        {
            //Get the BackInOutFloat ease function
            delegEasingFloatFunction easingFunction = GetEasingFloatFunction(EEaseTypes.BackInOut);

            //Test that the value at the start, and end are correct.
            //Also check times less than 0 or above the duration of ease
            TestRangeStartEndValues(easingFunction);

            //Test the value of the function at certain points with the values you expect it to produce
            TestFunctionValues(easingFunction,
                0.0f,
                5.0f,
                10.0f);
        }

        #endregion
    }
}