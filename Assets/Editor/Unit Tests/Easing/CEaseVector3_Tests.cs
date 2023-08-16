using UnityEngine;
using UnityEditor;
using NUnit.Framework;

#if UNITY_EDITOR
class CEaseVector3_Tests
{
    private const EEaseType M_EXPECTED_EASE_TYPE = EEaseType.Bounce;
    private const EEaseMode M_EXPECTED_EASE_MODE = EEaseMode.In;


    private const float M_STARTING_VALUE_X = 25.0f;
    private const float M_STARTING_VALUE_Y = 63.0f;
    private const float M_STARTING_VALUE_Z = 48.0f;

    public static void CheckEaseValues(EEaseType aResultEaseType, EEaseMode aResultEaseMode, Vector3 aStartingValue, bool aIsEasing)
    {
        Assert.AreEqual(M_EXPECTED_EASE_TYPE, aResultEaseType);
        Assert.AreEqual(M_EXPECTED_EASE_MODE, aResultEaseMode);

        Assert.AreEqual(new Vector3(M_STARTING_VALUE_X, M_STARTING_VALUE_Y, M_STARTING_VALUE_Z), aStartingValue);

        Assert.False(aIsEasing);
    }

    [Test]
    public static void SetEase_Test()
    {
        CEaseVector3 easeVector3 = new CEaseVector3(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

        easeVector3.SetEase(new Vector3(M_STARTING_VALUE_X, M_STARTING_VALUE_Y, M_STARTING_VALUE_Z), new Vector3(20.0f, 193.0f,57.0f), 10.0f, new MonoBehaviour());

        CheckEaseValues(easeVector3.GetEaseType(), easeVector3.GetEaseMode(), easeVector3.GetValue(), easeVector3.IsEasing());
    }


    [Test]
    public static void SetEaseFinalValue_Test()
    {
        {
            CEaseVector3 easeVector3 = new CEaseVector3(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

            easeVector3.SetEaseFinalValue(new Vector3(M_STARTING_VALUE_X, M_STARTING_VALUE_Y, M_STARTING_VALUE_Z), new Vector3(20.0f, 393.0f,23.0f), 10.0f, new MonoBehaviour());

            CheckEaseValues(easeVector3.GetEaseType(), easeVector3.GetEaseMode(), easeVector3.GetValue(), easeVector3.IsEasing());
        }
    }
}
#endif
