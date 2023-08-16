using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib_Easing
{
    /*
    Description: Class that contains a series of different static easing functions
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public static class CEasingFunctions
    {
        //Easing Formulas
        //http://gizma.com/easing/#l
        //http://wiki.unity3d.com/index.php?title=Tween
        //https://github.com/jesusgollonet/ofpennereasing/tree/master/PennerEasing
        //ElasticEase Gameplay Programming Winter 2016 Project

        /*
        Description: Private function used by different easing funcstion to ease a value according to a certain power
        Parameters: float aPower - The power that will be used for the ease
                    float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
        Creator: Alvaro Chavez Mixco
        Creation Date : Wednesday, March 01st, 2017
        */
        private static float PowerEaseIn(float aPower, float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Tween the value to the desired power
            return aChangeInValue * (float)Math.Pow(timePercent, aPower) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value linearly (power of 1)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float Linear(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //Do an ease to the power of 1, linear
            return PowerEaseIn(1.0f, aStartingValue, aChangeInValue, aCurrentTime, aDuration);
        }

        /*
        Description: Easing function to tween a value quadratic (power of 2) In (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuadraticIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //Do an ease to the power of 2, quadratic
            return PowerEaseIn(2.0f, aStartingValue, aChangeInValue, aCurrentTime, aDuration);
        }

        /*
        Description: Easing function to tween a value quadratic (power of 2) out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuadraticOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Tween the value, applying the quadratic easing on the second half of the curve
            return -aChangeInValue * (timePercent * (timePercent - 2.0f)) + aStartingValue;
        }

        /*
    Description: Easing function to tween a value quadratic (power of 2) in out ( start and end of curve)
    Parameters: float aStartingValue - The current value of the float being tweened
                float aChangeInValue - The amount that the value will change
                float aCurrentTime - The current time of the tween
                float aDuration - The total time duration of the tween
                float aExtraParameter - Not used in this function
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
        public static float QuadraticInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get the half total time
            float halfTime = aCurrentTime / (aDuration / 2.0f);

            //If the curve is in the first half
            if (halfTime < 1.0f)
            {
                //Ease quadratic in
                return aChangeInValue / 2.0f * (float)Math.Pow(halfTime, 2.0f) + aStartingValue;
            }

            //If the curve is in the second half, revert value back to 0 to 1 range
            halfTime--;

            //Ease quadratic out
            return -aChangeInValue / 2.0f * (halfTime * (halfTime - 2.0f) - 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value cubic (power of 3) In (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float CubicIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //Do an ease to the power of 3, cubic
            return PowerEaseIn(3.0f, aStartingValue, aChangeInValue, aCurrentTime, aDuration);
        }

        /*
        Description: Easing function to tween a value cubic (power of 3) Out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float CubicOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Make the time percent  the inverse negative, since it currently is a 0 to 1 value
            timePercent--;

            //Ease cubic out out
            return aChangeInValue * ((float)Math.Pow(timePercent, 3.0f) + 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value cubic (power of 3)  In Out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float CubicInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get the half total time
            float halfTime = aCurrentTime / (aDuration / 2.0f);

            //If the curve is in the first half
            if (halfTime < 1.0f)
            {
                //Ease cubic in
                return aChangeInValue / 2.0f * (float)Math.Pow(halfTime, 3.0f) + aStartingValue;
            }

            //If the curve is in the second half, revert value back to 0 to 1 range, since it currently is in a 0 to 2 range.
            halfTime -= 2;

            //Ease cubic out
            return aChangeInValue / 2.0f * ((float)Math.Pow(halfTime, 3.0f) + 2.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value quartic (power of 4)  In  (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuarticIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //Do an ease to the power of 4, quartic
            return PowerEaseIn(4.0f, aStartingValue, aChangeInValue, aCurrentTime, aDuration);
        }

        /*
        Description: Easing function to tween a value quartic (power of 4)  Out  (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuarticOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Make the time percent  the inverse negative, since it currently is a 0 to 1 value
            timePercent--;

            //Ease quartic out
            return -aChangeInValue * ((float)Math.Pow(timePercent, 4.0f) - 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value quartic (power of 4)  In Out  (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
         */
        public static float QuarticInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get the half total time
            float halfTime = aCurrentTime / (aDuration / 2.0f);

            //If the curve is in the first half
            if (halfTime < 1.0f)
            {
                //Ease quartic in
                return aChangeInValue / 2.0f * (float)Math.Pow(halfTime, 4.0f) + aStartingValue;
            }

            //If the curve is in the second half, revert value back to 0 to 1 range, since it currently is in a 0 to 2 range, and make it negative.
            halfTime -= 2;

            //Ease quartic out
            return -aChangeInValue / 2.0f * ((float)Math.Pow(halfTime, 4.0f) - 2.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value quntic (power of 5)  In  (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuinticIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //Do an ease to the power of 5, quintic
            return PowerEaseIn(5.0f, aStartingValue, aChangeInValue, aCurrentTime, aDuration);
        }

        /*
        Description: Easing function to tween a value quntic (power of 5)  Out  (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuinticOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Make the time percent  the inverse negative, since it currently is a 0 to 1 value
            timePercent--;

            //Ease quintic out
            return aChangeInValue * ((float)Math.Pow(timePercent, 5.0f) + 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value quntic (power of 5) In Out  (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float QuinticInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get the half total time
            float halfTime = aCurrentTime / (aDuration / 2.0f);

            //If the curve is in the first half
            if (halfTime < 1.0f)
            {
                //Ease quintic in
                return aChangeInValue / 2.0f * (float)Math.Pow(halfTime, 5.0f) + aStartingValue;
            }

            //If the curve is in the second half, revert value back to 0 to 1 range, since it currently is in a 0 to 2 range, and make it negative
            halfTime -= 2;

            //Ease quintic out
            return aChangeInValue / 2.0f * ((float)Math.Pow(halfTime, 5.0f) + 2.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using Sin wave In (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float SinIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Ease Sin In
            return -aChangeInValue * (float)Math.Cos(timePercent * (Math.PI / 2.0f)) + aChangeInValue + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using Sin wave Out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float SinOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            return aChangeInValue * (float)Math.Sin(timePercent * (Math.PI / 2.0f)) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using Sin wave In Out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float SinInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Ease  Sin in out
            return -aChangeInValue / 2.0f * ((float)Math.Cos(Math.PI * timePercent) - 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a exponential (power of 10) In (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float ExponentialIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Ease exponential in
            return aChangeInValue * (float)Math.Pow(2.0f, 10.0f * (timePercent - 1)) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a exponential (power of 10) Out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float ExponentialOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Ease exponential out
            return aChangeInValue * (-(float)Math.Pow(2.0f, -10.0f * timePercent) + 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a exponential (power of 10) In Out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float ExponentialInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get the half total time
            float halfTime = aCurrentTime / (aDuration / 2.0f);

            //If the curve is in the first half
            if (halfTime < 1.0f)
            {
                //Ease exponential in
                return aChangeInValue / 2.0f * (float)Math.Pow(2.0f, 10.0f * (halfTime - 1.0f)) + aStartingValue;
            }

            //If the curve is in the second half, revert value back to 0 to 1 range, since it currently is in a 0 to 2 range.
            aCurrentTime--;

            //Ease exponential in out
            return aChangeInValue / 2.0f * (-(float)Math.Pow(2.0f, -10.0f * halfTime) + 2.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a circular ease In (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float CircularIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Ease circular in
            return -aChangeInValue * ((float)Math.Sqrt(1.0f - (timePercent * timePercent)) - 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a circular ease Out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float CircularOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the current time percent according to total duration
            float timePercent = aCurrentTime / aDuration;

            //Make the time percent  the inverse negative, since it currently is a 0 to 1 value
            timePercent--;

            //Ease circular out
            return aChangeInValue * (float)Math.Sqrt(1.0f - (timePercent * timePercent)) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a circular ease In Out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float CircularInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get the half total time
            float halfTime = aCurrentTime / (aDuration / 2.0f);

            //If the curve is in the first half
            if (halfTime < 1.0f)
            {
                //Ease circular in
                return -aChangeInValue / 2.0f * ((float)Math.Sqrt(1.0f - (halfTime * halfTime)) - 1.0f) + aStartingValue;
            }

            //If the curve is in the second half, revert value back to 0 to 1 range, since it currently is in a 0 to 2 range, and make it negative
            halfTime -= 2;

            //Ease circular out
            return aChangeInValue / 2.0f * ((float)Math.Sqrt(1.0f - (halfTime * halfTime)) + 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a bounce ease In (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float BounceIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Ease bounce in
            return aChangeInValue - BounceOut(0, aChangeInValue, aDuration - aCurrentTime, aDuration) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a bounce ease Out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float BounceOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }
            else if ((aCurrentTime /= aDuration) < (1.0f / 2.75f))//If curve is in first 36%
            {
                //Ease value
                return (aChangeInValue * (7.5625f * aCurrentTime * aCurrentTime)) + aStartingValue;
            }
            else if (aCurrentTime < (2.0f / 2.75f))//If curve is in first 72%
            {
                //Ease value
                float postFix = aCurrentTime -= (1.5f / 2.75f);
                return (aChangeInValue * (7.5625f * (postFix) * aCurrentTime + 0.75f) + aStartingValue);
            }
            else if (aCurrentTime < (2.5f / 2.75f))//If curve is in first 90%
            {
                //Ease value
                float postFix = aCurrentTime -= (2.25f / 2.75f);
                return (aChangeInValue * (7.5625f * (postFix) * aCurrentTime + 0.9375f) + aStartingValue);
            }
            else//If curve is above 90 percent completion
            {
                //Ease value
                float postFix = aCurrentTime -= (2.625f / 2.75f);
                return (aChangeInValue * (7.5625f * (postFix) * aCurrentTime + 0.984375f) + aStartingValue);
            }
        }

        /*
        Description: Easing function to tween a value using a bounce ease In Out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aExtraParameter - Not used in this function
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float BounceInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aExtraParameter = 1.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }
            else if (aCurrentTime < aDuration / 2.0f)//If curve is in first half
            {
                //Ease bounce in
                return BounceIn(0, aChangeInValue, aCurrentTime * 2.0f, aDuration) * 0.5f + aStartingValue;
            }
            else//If curve is in second half
            {
                //Ease bounce out
                return BounceOut(0, aChangeInValue, aCurrentTime * 2.0f - aDuration, aDuration)
                    * 0.5f + aChangeInValue * 0.5f + aStartingValue;
            }
        }

        /*
        Description: Easing function to tween a value using a elastic ease In (Start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aElasticity - How elastic the bounce caused by the ease will be.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float ElasticIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aElasticity = 3.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0.0f)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate values for elastic wave
            float period = aDuration * 0.3f;
            float amplitude = aChangeInValue;
            float quarterPeriod = period / 4;

            //Make time negative
            aCurrentTime -= 1;

            //Calculate the power of the "elactic bounce according to the elasticity
            float pow = aElasticity * aCurrentTime;

            //Ease elastic in
            float postFix = amplitude * (float)Math.Pow(10, pow);
            float result = postFix * (float)Math.Sin(((aCurrentTime * aDuration - quarterPeriod) * (2.0f * (float)Math.PI) / period) + aStartingValue);

            //Inverse value sign, from negative to positive
            return result * -1;
        }

        /*
        Description: Easing function to tween a value using a elastic ease out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aElasticity - How elastic the bounce caused by the ease will be.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float ElasticOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aElasticity = 3.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate values for elastic wave
            float period = aDuration * 0.3f;
            float amplitude = aChangeInValue;
            float quarterPeriod = period / 4;

            //Ease elastic out
            return (amplitude * (float)Math.Pow(10, -aElasticity * aCurrentTime) * (float)Math.Sin((aCurrentTime * aDuration - quarterPeriod)
                * (2 * (float)Math.PI) / period) + aChangeInValue + aStartingValue);
        }

        /*
        Description: Easing function to tween a value using a elastic ease in out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aElasticity - How elastic the bounce caused by the ease will be.
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float ElasticInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aElasticity = 3.0f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the time percent according to half duration of ease
            float timePercent = aCurrentTime / (aDuration / 2.0f);

            //Calculate values for elastic wave
            float period = aDuration * (0.3f * 1.5f);
            float amplitude = aChangeInValue;
            float quarterPeriod = period / 4;
            float postFix;

            //If curve in first half
            if (timePercent < 1.0f)
            {
                //Ease elastic in
                postFix = amplitude * (float)Math.Pow(10, aElasticity * (aCurrentTime -= 1));
                float calculatedValue = (postFix * (float)Math.Sin((timePercent * aDuration - quarterPeriod) * (2 * (float)Math.PI) / period));
                calculatedValue *= -0.5f;
                calculatedValue += aStartingValue;

                return calculatedValue;
            }
            else//If curve is in second half
            {
                //Ease elastic out
                postFix = (amplitude * (float)Math.Pow(10, -aElasticity * (aCurrentTime -= 1)));
                return postFix * (float)Math.Sin((aCurrentTime * aDuration - quarterPeriod) * (2 * (float)Math.PI) / period) * 0.5f + aChangeInValue + aStartingValue;
            }
        }

        /*
        Description: Easing function to tween a value using a back ease in (start of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aOvershoot - higher means greater overshoot ("bounce" back) (0 produces cubic easing with no overshoot, and 
                                       the default value of 1.70158 produces an overshoot of 10 percent).
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float BackIn(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aOvershoot = 1.70158f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get time percent according to ease duration
            float timePercent = aCurrentTime / aDuration;

            //Ease back in
            return aChangeInValue * timePercent * timePercent * ((aOvershoot + 1.0f) * timePercent - aOvershoot) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a back ease out (end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aOvershoot - higher means greater overshoot ("bounce" back) (0 produces cubic easing with no overshoot, and 
                                       the default value of 1.70158 produces an overshoot of 10 percent).
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float BackOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aOvershoot = 1.70158f)
        {
            //If current time is 0 or less
            if (aCurrentTime <= 0)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Calculate the time percent, and convert it to negative
            float inverseTimePercent = (aCurrentTime / aDuration) - 1.0f;

            //Ease back out
            return aChangeInValue * (inverseTimePercent * inverseTimePercent * ((aOvershoot + 1.0f) * inverseTimePercent + aOvershoot) + 1.0f) + aStartingValue;
        }

        /*
        Description: Easing function to tween a value using a back ease in out (start and end of curve)
        Parameters: float aStartingValue - The current value of the float being tweened
                    float aChangeInValue - The amount that the value will change
                    float aCurrentTime - The current time of the tween
                    float aDuration - The total time duration of the tween
                    float aOvershoot - higher means greater overshoot ("bounce" back) (0 produces cubic easing with no overshoot, and 
                                       the default value of 1.70158 produces an overshoot of 10 percent).
        Creator: Alvaro Chavez Mixco
        Creation Date : Thursday, February 23rd, 2017
        */
        public static float BackInOut(float aStartingValue, float aChangeInValue, float aCurrentTime, float aDuration, float aOvershoot = 1.70158f)
        {
            //If the current time is over limit
            //If current time is 0 or less
            if (aCurrentTime <= 0)
            {
                //Just return the starting value
                return aStartingValue;
            }
            else if (aCurrentTime >= aDuration)//If the current time is over limit
            {
                //Simply return the starting value plus the change it has to have
                return aStartingValue + aChangeInValue;
            }

            //Get half the change in value
            float halfChangeInValue = aChangeInValue / 2.0f;

            //Get double the current time
            float doubleCurrentTime = aCurrentTime * 2.0f;

            //If curve is in first half
            if (aCurrentTime < aDuration / 2.0f)
            {
                //Ease back in
                return BackIn(aStartingValue, halfChangeInValue, doubleCurrentTime, aDuration, aOvershoot);
            }
            else//If curve is in second half
            {
                //Ease back out
                return BackOut(aStartingValue + halfChangeInValue, halfChangeInValue, doubleCurrentTime - aDuration, aDuration, aOvershoot);
            }
        }
    }
}