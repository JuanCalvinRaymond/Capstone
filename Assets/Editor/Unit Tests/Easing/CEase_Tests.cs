using UnityEngine;
using UnityEditor;
using NUnit.Framework;

#if UNITY_EDITOR
class CEase_Tests
{
    private const EEaseType M_EXPECTED_EASE_TYPE = EEaseType.Bounce;
    private const EEaseMode M_EXPECTED_EASE_MODE = EEaseMode.In;

    private const float M_STARTING_VALUE_X = 25.0f;

    public static void CheckEaseValues(EEaseType aResultEaseType, EEaseMode aResultEaseMode, float aStartingValue, bool aIsEasing)
    {
        Assert.AreEqual(M_EXPECTED_EASE_TYPE, aResultEaseType);
        Assert.AreEqual(M_EXPECTED_EASE_MODE, aResultEaseMode);

        Assert.AreEqual(M_STARTING_VALUE_X, aStartingValue);

        Assert.False(aIsEasing);
    }

    [Test]
    public static void SetEase_Test()
    {
        CEase easeFloat = new CEase(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

        easeFloat.SetEase(M_STARTING_VALUE_X, 50.0f, 10.0f, new MonoBehaviour());

        CheckEaseValues(easeFloat.GetEaseType(), easeFloat.GetEaseMode(), easeFloat.GetValue(), easeFloat.IsEasing());
    }


    [Test]
    public static void SetEaseFinalValue_Test()
    {
        CEase easeFloat = new CEase(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

        easeFloat.SetEaseFinalValue(M_STARTING_VALUE_X, 50.0f, 10.0f, new MonoBehaviour());

        CheckEaseValues(easeFloat.GetEaseType(), easeFloat.GetEaseMode(), easeFloat.GetValue(), easeFloat.IsEasing());
    }


}
#endif
