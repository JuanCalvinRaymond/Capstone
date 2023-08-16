using UnityEngine;
using UnityEditor;
using NUnit.Framework;

#if UNITY_EDITOR
class CEaseVector2_Tests
{
    private const EEaseType M_EXPECTED_EASE_TYPE = EEaseType.Bounce;
    private const EEaseMode M_EXPECTED_EASE_MODE = EEaseMode.In;


    private const float M_STARTING_VALUE_X = 25.0f;
    private const float M_STARTING_VALUE_Y = 63.0f;

    public static void CheckEaseValues(EEaseType aResultEaseType, EEaseMode aResultEaseMode, Vector2 aStartingValue, bool aIsEasing)
    {
        Assert.AreEqual(M_EXPECTED_EASE_TYPE, aResultEaseType);
        Assert.AreEqual(M_EXPECTED_EASE_MODE, aResultEaseMode);

        Assert.AreEqual(new Vector2(M_STARTING_VALUE_X, M_STARTING_VALUE_Y), aStartingValue);

        Assert.False(aIsEasing);
    }

    [Test]
    public static void SetEase_Test()
    {
        CEaseVector2 easeVector2 = new CEaseVector2(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

        easeVector2.SetEase(new Vector2(M_STARTING_VALUE_X, M_STARTING_VALUE_Y), new Vector2(20.0f, 193.0f), 10.0f, new MonoBehaviour());

        CheckEaseValues(easeVector2.GetEaseType(), easeVector2.GetEaseMode(), easeVector2.GetValue(), easeVector2.IsEasing());
    }


    [Test]
    public static void SetEaseFinalValue_Test()
    {
        {
            CEaseVector2 easeVector2 = new CEaseVector2(M_EXPECTED_EASE_TYPE, M_EXPECTED_EASE_MODE);

            easeVector2.SetEaseFinalValue(new Vector2(M_STARTING_VALUE_X, M_STARTING_VALUE_Y), new Vector2(20.0f, 393.0f), 10.0f, new MonoBehaviour());

            CheckEaseValues(easeVector2.GetEaseType(), easeVector2.GetEaseMode(), easeVector2.GetValue(), easeVector2.IsEasing());
        }
    }
}
#endif
