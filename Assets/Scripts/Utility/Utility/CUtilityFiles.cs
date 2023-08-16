using UnityEngine;

/*
Description: Utility class to handle file related paths/strings.
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, January 10, 2017
*/
public class CUtilityFiles
{
    /*
    Description: Function to get a valid data path where to save the leaderboard file.
    Creator: Alvaro Chavez Mixco
    Extra Notes: This method uses a Unity function.
    */
    static public string GetSavePath()
    {
        //For how often this function gets called, is beter to create a string for the 
        //forward slash rather than storing as a const.
        return Application.persistentDataPath + "/";
    }

    /*
    Description: Helper function to read a bool from the playerPrefs. This is done by reading an int from
    the playerPrefs, and converting it into a bool.
    Parameters: string aKeyName- The name of the key in the PlayerPrefs file
                bool aDefaultValue- The value that will be written to the key if doesn't exist.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday,December 30, 2016
    */
    public static bool ReadPlayerPrefsBool(string aKeyName, bool aDefaultValue = false)
    {
        return CUtilityMath.IntToBool(PlayerPrefs.GetInt(aKeyName, CUtilityMath.BoolToInt(aDefaultValue)));
    }

    /*
    Description: Helper function to read an int from the playerPrefs
    Parameters: string aKeyName- The name of the key in the PlayerPrefs file
                int aDefaultValue- The value that will be written to the key if doesn't exist.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    */
    public static int ReadPlayerPrefsInt(string aKeyName, int aDefaultValue = 0)
    {
        return PlayerPrefs.GetInt(aKeyName, aDefaultValue);
    }

    /*
    Description: Helper function to read a float from the playerPrefs
    Parameters: string aKeyName- The name of the key in the PlayerPrefs file
                float aDefaultValue- The value that will be written to the key if doesn't exist.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday,December 30, 2016
    */
    public static float ReadPlayerPrefsFloat(string aKeyName, float aDefaultValue = 0.0f)
    {
        return PlayerPrefs.GetFloat(aKeyName, aDefaultValue);
    }

    /*
    Description: Helper function to write a bool to the playerPrefs file. This is done
    by converting the bool to an int.
    Parameters: string aKeyName- The name of the key in the PlayerPrefs file
                bool aValue- The value that will be written to the playerPrefs key
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday,December 30, 2016
    */
    public static void WritePlayerPrefsBool(string aKeyName, bool aValue)
    {
        PlayerPrefs.SetInt(aKeyName, CUtilityMath.BoolToInt(aValue));
    }

    /*
    Description: Helper function to write a float to the playerPrefs file. 
    Parameters: string aKeyName- The name of the key in the PlayerPrefs file
               float aValue- The value that will be written to the playerPrefs key
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday,December 30, 2016
    */
    public static void WritePlayerPrefsFloat(string aKeyName, float aValue)
    {
        PlayerPrefs.SetFloat(aKeyName, aValue);
    }

    /*
    Description: Helper function to write an int to the playerPrefs file. 
    Parameters: string aKeyName- The name of the key in the PlayerPrefs file
                int aValue- The value that will be written to the playerPrefs key
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    */
    public static void WritePlayerPrefsInt(string aKeyName, int aValue)
    {
        PlayerPrefs.SetInt(aKeyName, aValue);
    }
}
