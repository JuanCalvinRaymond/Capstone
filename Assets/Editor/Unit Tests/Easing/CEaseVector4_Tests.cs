using UnityEngine;
using UnityEditor;
using NUnit.Framework;

#if UNITY_EDITOR
class CEaseVector4_Tests
{
    private const EEaseType M_EXPECTED_EASE_TYPE = EEaseType.Bounce;
    private const EEaseMode M_EXPECTED_EASE_MODE = EEaseMode.In;

    private const float M_STARTING_VALUE_X = 25.0f;
    private const float M_STARTING_VALUE_Y = 63.0f;
    private const float M_STARTING_VALUE_Z = 48.0f;
    private const float M_STARTING_VALUE_W = 22.0f;

    public static void CheckEaseValues(EEaseType aResultEaseType, EEaseMode aResultEaseMode, Vector4 aStartingValue, bool aIsEasing)
    {
        Assert.AreEqual(M_EXPECTED_EASE_TYPE, aResultEaseType);
        Assert.AreEqual(M_EXPECTED_EASE_MODE, aResultEaseMode);

        Assert.AreEqual(new Vector4(M_STARTING_VALUE_X, M_STARTING_VALUE_Y, M_STARTING_VALUE_Z, M_STARTING_VALUE_W), aStartingValue);

        Assert.False(aIsEasing);
    }

    [Test]
    public static void SetEase_Test()
    {
        CEaseVector4 easeVector4 = new CEaseVector4(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

        easeVector4.SetEase(new Vector4(M_STARTING_VALUE_X, M_STARTING_VALUE_Y, M_STARTING_VALUE_Z, M_STARTING_VALUE_W), new Vector4(20.0f, 193.0f, 57.0f,300.0f), 10.0f, new MonoBehaviour());

        CheckEaseValues(easeVector4.GetEaseType(), easeVector4.GetEaseMode(), easeVector4.GetValue(), easeVector4.IsEasing());
    }


    [Test]
    public static void SetEaseFinalValue_Test()
    {
        {
            CEaseVector4 easeVector4 = new CEaseVector4(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

            easeVector4.SetEaseFinalValue(new Vector4(M_STARTING_VALUE_X, M_STARTING_VALUE_Y,M_STARTING_VALUE_Z,M_STARTING_VALUE_W), new Vector4(20.0f, 393.0f, 23.0f,2017.0f), 10.0f, new MonoBehaviour());

            CheckEaseValues(easeVector4.GetEaseType(), easeVector4.GetEaseMode(), easeVector4.GetValue(), easeVector4.IsEasing());
        }
    }
}
#endif
