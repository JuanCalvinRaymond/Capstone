using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System;


#if UNITY_EDITOR
public class CEasing_Tests
{
    [Test]
    public static void GetEasingFunction_Test()
    {
        delegEaseFunction[] expectedResults = new delegEaseFunction[33];

        expectedResults[0] = Lib_Easing.CEasingFunctions.Linear;
        expectedResults[1] = Lib_Easing.CEasingFunctions.Linear;
        expectedResults[2] = Lib_Easing.CEasingFunctions.Linear;
        expectedResults[3] = Lib_Easing.CEasingFunctions.QuadraticIn;
        expectedResults[4] = Lib_Easing.CEasingFunctions.QuadraticOut;
        expectedResults[5] = Lib_Easing.CEasingFunctions.QuadraticInOut;
        expectedResults[6] = Lib_Easing.CEasingFunctions.CubicIn;
        expectedResults[7] = Lib_Easing.CEasingFunctions.CubicOut;
        expectedResults[8] = Lib_Easing.CEasingFunctions.CubicInOut;
        expectedResults[9] = Lib_Easing.CEasingFunctions.QuarticIn;
        expectedResults[10] = Lib_Easing.CEasingFunctions.QuarticOut;
        expectedResults[11] = Lib_Easing.CEasingFunctions.QuarticInOut;
        expectedResults[12] = Lib_Easing.CEasingFunctions.QuinticIn;
        expectedResults[13] = Lib_Easing.CEasingFunctions.QuinticOut;
        expectedResults[14] = Lib_Easing.CEasingFunctions.QuinticInOut;
        expectedResults[15] = Lib_Easing.CEasingFunctions.SinIn;
        expectedResults[16] = Lib_Easing.CEasingFunctions.SinOut;
        expectedResults[17] = Lib_Easing.CEasingFunctions.SinInOut;
        expectedResults[18] = Lib_Easing.CEasingFunctions.ExponentialIn;
        expectedResults[19] = Lib_Easing.CEasingFunctions.ExponentialOut;
        expectedResults[20] = Lib_Easing.CEasingFunctions.ExponentialInOut;
        expectedResults[21] = Lib_Easing.CEasingFunctions.CircularIn;
        expectedResults[22] = Lib_Easing.CEasingFunctions.CircularOut;
        expectedResults[23] = Lib_Easing.CEasingFunctions.CircularInOut;
        expectedResults[24] = Lib_Easing.CEasingFunctions.BounceIn;
        expectedResults[25] = Lib_Easing.CEasingFunctions.BounceOut;
        expectedResults[26] = Lib_Easing.CEasingFunctions.BounceInOut;
        expectedResults[27] = Lib_Easing.CEasingFunctions.ElasticIn;
        expectedResults[28] = Lib_Easing.CEasingFunctions.ElasticOut;
        expectedResults[29] = Lib_Easing.CEasingFunctions.ElasticInOut;
        expectedResults[30] = Lib_Easing.CEasingFunctions.BackIn;
        expectedResults[31] = Lib_Easing.CEasingFunctions.BackOut;
        expectedResults[32] = Lib_Easing.CEasingFunctions.BackInOut;

        EEaseType[] possibleTypes = (EEaseType[])(Enum.GetValues(typeof(EEaseType)));
        EEaseMode[] possibleModes = (EEaseMode[])(Enum.GetValues(typeof(EEaseMode)));

        for (int i = 0; i < possibleTypes.Length; i++)
        {
            for (int j = 0; j < possibleModes.Length; j++)
            {
                delegEaseFunction functionObtained = CEasing.GetEasingFunction(possibleTypes[i], possibleModes[j]);

                int totalIndex = i * possibleModes.Length + j;

                Assert.AreEqual(expectedResults[totalIndex], functionObtained);
            }
        }
    }

}
#endif