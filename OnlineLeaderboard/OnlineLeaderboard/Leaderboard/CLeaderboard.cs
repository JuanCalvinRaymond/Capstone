/*UPDATED AS OF: TUESDAY, FEBRUARY 7, 2017*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
Description: Class used to manage the game leaderboard. The class will write the leaderbaord as a binary file, and it will also be able to read
it from the binary file. The class also stores the actual values that written into the file for use in game.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CLeaderboard : ICloneable
{
    private const float M_CURRENT_VERSION_NUMBER = 3.0f;

    //Leaderboard file settings
    private const string M_LEADERBOARD_FILE_NAME_GENERIC = "Leaderboard.bin";//Generic is mostly for testing purposes and prevent crashes/accidentally not saving data
    private const string M_LEADERBOARD_FILE_NAME_BEGINNER = "Leaderboard-Beginner.bin";
    private const string M_LEADERBOARD_FILE_NAME_ADVANCED = "Leaderboard-Advanced.bin";

    private const int M_MAX_NUMBER_OF_ENTRIES = 100;

    private string m_leaderboardFileName;
    private List<SPlayerEntry> m_currentLeaderboard;//The current values stored in the leaderboard for use in game
    private ELevelState m_leaderboardLevel = ELevelState.NoMotion;

    //Variables for how the leaderboard will be sorted
    private ELeaderboardSortingMethods m_sortingMethod;
    private ELeaderboardSortingMethods m_writeSortingMethod;//The sorting method that will be used before writing the leaderboard

    //Variables for score validation -TEMPORARY
    private int m_validationScoreMaxValue = int.MaxValue;
    private int m_validationStreakMaxValue = int.MaxValue;
    private int m_validationNumberOfTricksMaxValue = int.MaxValue;
    private int m_validationNumberOfCombosMaxValue = int.MaxValue;
    private float m_validationCompletionTimeMinValue = 0.0f;
    private float m_validationAccuracyMaxValue = 100.0f;

    //Variables for the event when the leaderboard changes
    private delegate int delegateSorting(SPlayerEntry entryA, SPlayerEntry entryB);
    private delegateSorting m_delegSorting;
    public delegate void delegateLeaderboardChange(List<SPlayerEntry> aListOfEntries);
    public event delegateLeaderboardChange OnLeaderboardChange;

    public List<SPlayerEntry> PCurrentLeaderboard
    {
        get
        {
            return m_currentLeaderboard;
        }
    }

    public ELevelState PLeaderboardLevel
    {
        get
        {
            return m_leaderboardLevel;
        }
    }

    public int PMaxNumberOfEntries
    {
        get
        {
            return M_MAX_NUMBER_OF_ENTRIES;
        }
    }

    public ELeaderboardSortingMethods PLeaderboardSortingMethod
    {
        set
        {
            m_sortingMethod = value;

            SetSortingDelegate(m_sortingMethod);
        }

        get
        {
            return m_sortingMethod;
        }
    }

    /*
    Description: When created, read the current leaderboard file
    Parameters: ELevelState aLevel - To which level the leaderboard belongs
                string aLeaderboardFilePath- The file path, not including file name, where the leaderboard is stored.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    */
    public CLeaderboard(ELevelState aLevel)
    {
        //Save the leaderboard level
        m_leaderboardLevel = aLevel;

        //Default sorting method
        m_sortingMethod = ELeaderboardSortingMethods.HighestScore;
        m_delegSorting = SortByHighestScore;

        //Set The sorting method that will be used before writing the leaderboard
        m_writeSortingMethod = ELeaderboardSortingMethods.HighestScore;

        //Create the list of player entries
        m_currentLeaderboard = new List<SPlayerEntry>();

        //Choose which file name the leaderboard will have
        SetLeaderboardFileName(aLevel);
    }

    /*
    Description: Function to decide, according to the current level selected, which leaderboard to read adn write to
    Parameters: ELevelState aLevel - To which level the leaderboard belongs
    Creator: Alvaro Chavez Mixco
    */
    private void SetLeaderboardFileName(ELevelState aLevel)
    {
        switch (aLevel)
        {
            case ELevelState.NoMotion://Other levels
                m_leaderboardFileName = M_LEADERBOARD_FILE_NAME_GENERIC;
                break;
            case ELevelState.Beginner://Beginner level
                m_leaderboardFileName = M_LEADERBOARD_FILE_NAME_BEGINNER;
                break;
            case ELevelState.Advanced://Advanced level
                m_leaderboardFileName = M_LEADERBOARD_FILE_NAME_ADVANCED;
                break;
            default:
                m_leaderboardFileName = M_LEADERBOARD_FILE_NAME_GENERIC;
                break;
        }
    }

    /*
    Description: Function to assign the corresponding sorting function to the leaderboard sorting delegate according to the
    enum being passed as an argument.
    Parameters: ELeaderboardSortingMethods aMethod- Which leaderbaord sorting method we want to use
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function set the sorting method we want to use, doesn't sort the leaderboard. This function
    also DOESN'T save the ELeaderboardSortingMethod being used. 
    */
    private void SetSortingDelegate(ELeaderboardSortingMethods aMethod)
    {
        switch (aMethod)
        {
            case ELeaderboardSortingMethods.Alphabetically://Alphatetically (A-Z)
                m_delegSorting = SortByAlphabetically;
                break;
            case ELeaderboardSortingMethods.InverseAlphabetically://Inverse Alphabetically (Z-A)
                m_delegSorting = SortByInverseAlphabetically;
                break;
            case ELeaderboardSortingMethods.HighestScore://Highest score
                m_delegSorting = SortByHighestScore;
                break;
            case ELeaderboardSortingMethods.LowestScore://Lowest score
                m_delegSorting = SortByLowestScore;
                break;
            case ELeaderboardSortingMethods.HighestStreak://Highest streak
                m_delegSorting = SortByHighestStreak;
                break;
            case ELeaderboardSortingMethods.LowestStreak://Lowest streak
                m_delegSorting = SortByLowestStreak;
                break;
            case ELeaderboardSortingMethods.HighestTime://Highest time
                m_delegSorting = SortByHighestTime;
                break;
            case ELeaderboardSortingMethods.LowestTime://Lowest time
                m_delegSorting = SortByLowestTime;
                break;
            case ELeaderboardSortingMethods.HighestShotsFired://Highest shots fired
                m_delegSorting = SortByHighestShotsFired;
                break;
            case ELeaderboardSortingMethods.LowestShotsFired://Lowest shots fired
                m_delegSorting = SortByLowestShotsFired;
                break;
            case ELeaderboardSortingMethods.HighestShotsHit://Highest shots hit
                m_delegSorting = SortByHighestShotsHit;
                break;
            case ELeaderboardSortingMethods.LowestShotsHit://Lowest shots hit
                m_delegSorting = SortByLowestShotsHit;
                break;
            case ELeaderboardSortingMethods.HighestAccuracy://Highest accuracy
                m_delegSorting = SortByHighestAccuracy;
                break;
            case ELeaderboardSortingMethods.LowestAccuracy://Lowest accuracy
                m_delegSorting = SortByLowestAccuracy;
                break;
            case ELeaderboardSortingMethods.HighestNumberOfTricks://Highest number of tricks
                m_delegSorting = SortByHighestNumberOfTricks;
                break;
            case ELeaderboardSortingMethods.LowestNumberOfTricks://Lowest number of tricks
                m_delegSorting = SortByLowestNumberOfTricks;
                break;
            default:
                m_delegSorting = SortByHighestScore;//By default set to the highest score
                break;
        }
    }

    /*
    Description: Function intented to be assigned to a delegate. Function to sort the leaderboard alphabetically according to the player name
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard 
    */
    private int SortByAlphabetically(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_playerName.CompareTo(entryA.m_playerName);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard inverse alphabetically (From 'Z' to 'A')  according to the palyer name
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByInverseAlphabetically(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByAlphabetically(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player score
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestScore(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_score.CompareTo(entryA.m_score);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player score
    Parameters(Optional): SPlayerEntry entryA - A player entry
                          SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestScore(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestScore(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player streak.
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestStreak(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_longestStreak.CompareTo(entryA.m_longestStreak);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player streak
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestStreak(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestStreak(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player time
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestTime(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_completionTime.CompareTo(entryA.m_completionTime);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player time
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestTime(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestTime(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player shots fired
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestShotsFired(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_shotsFired.CompareTo(entryA.m_shotsFired);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player shots fired
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestShotsFired(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestShotsFired(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player shots hit
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestShotsHit(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_shotsHit.CompareTo(entryA.m_shotsHit);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player shots hit
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestShotsHit(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestShotsHit(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player accuracy
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestAccuracy(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_accuracy.CompareTo(entryA.m_accuracy);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player accuracy
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestAccuracy(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestAccuracy(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player number of tricks
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestNumberOfTricks(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_accuracy.CompareTo(entryA.m_accuracy);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player number of tricks
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestNumberOfTricks(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestNumberOfTricks(entryB, entryA);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the highest player number of combos
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByHighestNumberOfCombos(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return entryB.m_numberOfCombos.CompareTo(entryA.m_numberOfCombos);
    }

    /*
    Description:Function intented to be assigned to a delegate. Function to sort the leaderboard according to the lowest player number of combos
    Parameters: SPlayerEntry entryA - A player entry
                SPlayerEntry entryB - The player entry that will be compared to entryA 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function modifies how the leaderboard will be sorted, it doesn't actually sorts the leaderboard
    */
    private int SortByLowestNumberOfCombos(SPlayerEntry entryA, SPlayerEntry entryB)
    {
        return SortByHighestNumberOfTricks(entryB, entryA);
    }

    /*
    Description: Validate a leaderboard entry to check if its valid. It will return true if the entry is valid, false if the entry is invalid.
    Parameters: SPlayerEntry aEntry - The entry that will be validated.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, January 19, 2017
    */
    private bool ValidateLeaderboardEntry(SPlayerEntry aEntry)
    {
        //TEMPORARY-Right now this is a simple general check. Later on it may be a level specific check using a
        //switch statement with m_leaderboardLevel

        //If the score is a negative value or beyond max value
        if (aEntry.m_score < 0 || aEntry.m_score > m_validationScoreMaxValue)
        {
            //Return the entry is invalid
            return false;
        }

        //If the streak is a negative value or beyond max value
        if (aEntry.m_longestStreak < 0 || aEntry.m_longestStreak > m_validationStreakMaxValue)
        {
            //Return the entry is invalid
            return false;
        }

        //If the completion time is negative, or is below the min completion time
        if (aEntry.m_completionTime < 0.0f || aEntry.m_completionTime < m_validationCompletionTimeMinValue)
        {
            //Return the entry is invalid
            return false;
        }

        //If the shots fire or shot hits are negative numbers
        if (aEntry.m_shotsHit < 0 || aEntry.m_shotsFired < 0)
        {
            //Return the entry is invalid
            return false;
        }

        //If the shots fired is less than the shots hit
        if (aEntry.m_shotsFired < aEntry.m_shotsHit)
        {
            //Return the entry is invalid
            return false;
        }

        //If the accuracy is not within the 0 to 1 range
        if (aEntry.m_accuracy < 0.0f || aEntry.m_accuracy > m_validationAccuracyMaxValue)
        {
            //Return the entry is invalid
            return false;
        }

        //If the number of tricks is a negative value or beyond max value
        if (aEntry.m_numberOfTricks < 0 || aEntry.m_numberOfTricks > m_validationNumberOfTricksMaxValue)
        {
            //Return the entry is invalid
            return false;
        }

        //If the number of combos is a negative value or beyond max value
        if (aEntry.m_numberOfCombos < 0 || aEntry.m_numberOfCombos > m_validationNumberOfCombosMaxValue)
        {
            //Return the entry is invalid
            return false;
        }

        //Return the entry is valid
        return true;
    }

    /*
    Description: Validate the entry, and if its valid add it to the leaderboard. This function return true if the entry was successfully
    added to the leaderboard, false if the entry wasn't valid and therefore not added to the leaderboard.
    Parameters: SPlayerEntry aEntry - The entry that will be validated and added to the leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, January 19, 2017
    */
    private bool AddValidateSingleEntry(SPlayerEntry aEntry)
    {
        //If the leaderboard entry is valid
        if (ValidateLeaderboardEntry(aEntry) == true)
        {
            //Add the entry to the list
            m_currentLeaderboard.Add(aEntry);

            //Return that the entry was successfully validated and added to leaderboard
            return true;
        }

        //Return that the entry was not added to the leaderboard
        return false;
    }

    /*
    Description: Add the entry to the current leaderboard, and then sort the leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function only adds the entry to the ingame leaderboard, it doesn't actually write to a file at this point. 
    */
    public void AddToLeaderboard(SPlayerEntry aEntry)
    {
        //If the entry was successfully validated and added to leaderboard
        if (AddValidateSingleEntry(aEntry) == true)
        {
            //Sort the list
            SortLeaderboard();
        }
    }

    /*
    Description: Add a list of entries to the leaderboard. This adds and validate each entry individually. After it has gone
    through all the entries it sorts the list.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, January 19, 2017
    Extra Notes: This function only adds the entry to the ingame leaderboard, it doesn't actually write to a file at this point. 
    */
    public void AddToLeaderboard(List<SPlayerEntry> aListOfEntries)
    {
        //If the list of entries is not null
        if (aListOfEntries != null)
        {
            //Variable to know if an entry was actually added to the leaderboard
            bool leaderboardChanged = false;

            //Go through all the entries in the list
            foreach (SPlayerEntry entry in aListOfEntries)
            {
                //If the entry was successfully validated and added to leaderboard
                if (AddValidateSingleEntry(entry) == true)
                {
                    //Mark that the leaderboard changed
                    leaderboardChanged = true;
                }
            }

            //If the leaderboard actually chagned
            if (leaderboardChanged == true)
            {
                //Sort the leaderboard
                SortLeaderboard();
            }
        }
    }

    /*
    Description: Returns a list of player entries, according to the number requested and the index
    being passed.
    Parameters: int aNumberOfEntries - The number of entries that will be returned
                int aStartingIndex - The starting index from where the entries will be get
    Creator: Alvaro Chavez Mixco
    Creation Date:  Saturday, January 20, 2017
    */
    public List<SPlayerEntry> GetLeaderboardEntries(int aNumberOfEntries, int aStartingIndex = 0)
    {
        List<SPlayerEntry> entriesFound = new List<SPlayerEntry>();

        //If the current leaderboard is valid and starting index is not negative
        if (m_currentLeaderboard != null && aStartingIndex >= 0)
        {
            //Loop for the desired number of entries you want
            for (int i = 0; i < aNumberOfEntries; i++)
            {
                //If the index we want to access is within the limit of the leaderboard
                if (aStartingIndex + i < m_currentLeaderboard.Count)
                {
                    //Add the desired entrie to the list htat will be returned
                    entriesFound.Add(m_currentLeaderboard[aStartingIndex + i]);
                }
                else//If the index we want to access is outside the limit of the leaderboard
                {
                    //Break out of the loop
                    break;
                }
            }
        }

        return entriesFound;
    }

    /*
    Description: The function will read the desired leaderboard file, anf fill the list of player entries so that it can be used in game
    Parameters: string aFilePath-Filepath where to read leaderboard. This is only the file path, and not the file name.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: TEMPORARY - PENDING CHANGE SO THAT IT ONLY READS/STORE THE DESIRED VALUES AND NOT THE WHOLE LEADERBOARD
    */
    public void ReadLeaderboard(string aFilePath)
    {
        //Get the path where the leaderboard is/will be
        string filePath = GetFinalSavePath(aFilePath);

        //Formatter to serialize and deserialize data
        BinaryFormatter formatter = new BinaryFormatter();

        //Open or create the leaderboard file and read stream
        BinaryReader fileStream = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate));

        //If there is somehting written in the file
        if (fileStream.BaseStream.Length > 0)
        {
            //Get the version number of the save path
            float savedVersionNumber = (float)formatter.Deserialize(fileStream.BaseStream);

            //If it is the correct version number
            if (savedVersionNumber == M_CURRENT_VERSION_NUMBER)
            {
                //While the leaderboard stream is not at the end
                while (fileStream.BaseStream.Position != fileStream.BaseStream.Length)
                {
                    //Create temporary entry
                    SPlayerEntry aEntry = new SPlayerEntry();

                    //Read all the values of the leaderboard and place it on the temporary entry
                    aEntry = (SPlayerEntry)formatter.Deserialize(fileStream.BaseStream);

                    m_currentLeaderboard.Add(aEntry);//Add the entry to the leaderboard

                    //Ensure the leaderboard doesn't go over the limit
                    if (m_currentLeaderboard.Count >= M_MAX_NUMBER_OF_ENTRIES)
                    {
                        break;
                    }
                }
            }
            //Sort the leaderboard
            SortLeaderboard();
        }

        //Close the file stream
        fileStream.Close();
    }

    /*
    Description: The function will take whatever is stored in the current ingame leaderboard (list of SPlayerEntry) and write it to a binary file
    Parameters: string aFilePath-Filepath where to write leaderboard. This is only the file path, and not the file name.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: 
    */
    public void WriteToLeaderboard(string aFilePath)
    {
        //Get the path where the leaderboard is/will be
        string filePath = GetFinalSavePath(aFilePath);

        //Formatter to serialize and deserialize data
        BinaryFormatter formatter = new BinaryFormatter();

        //Create the leaderboard file and write stream, it will overwrite any existing one
        BinaryWriter fileStream = new BinaryWriter(File.Open(filePath, FileMode.Create));

        //Ensure the leaderboard is sorted before writing it         
        //Make sure the leaderboard is sorted with the desired method
        PLeaderboardSortingMethod = m_writeSortingMethod;

        //Sort the leaderboard
        SortLeaderboard();

        //Write the version number first
        formatter.Serialize(fileStream.BaseStream, M_CURRENT_VERSION_NUMBER);

        //Go through each element in the leaderboard
        for (int i = 0; i < m_currentLeaderboard.Count; i++)
        {
            //Write all the properties of each player entry in the leaderboard
            formatter.Serialize(fileStream.BaseStream, m_currentLeaderboard[i]);

            //Ensure the leaderboard doesn't go over the limit
            if (i >= M_MAX_NUMBER_OF_ENTRIES)
            {
                break;
            }
        }

        //Ensure all the data was written correctly
        fileStream.Flush();

        //Close the write stream
        fileStream.Close();
    }

    /*
    Description: This function will sort the current leaderboard according to the desired sorting method. The function will also ensure the current leaderboard is
    withing the desired limit of entries. Besides that, the function will also call the OnLeaderboardChange event.
    Parameters(Optional): 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function modifies the ingame leaderboard, not the leaderboard file. 
    */
    public void SortLeaderboard()
    {
        //Sort the leaderbaord using the designated delegate
        m_currentLeaderboard.Sort(new Comparison<SPlayerEntry>(m_delegSorting));

        //If the leaderboard is full
        if (m_currentLeaderboard.Count > M_MAX_NUMBER_OF_ENTRIES)
        {
            //Remove the last elements
            m_currentLeaderboard.RemoveRange(M_MAX_NUMBER_OF_ENTRIES, m_currentLeaderboard.Count - M_MAX_NUMBER_OF_ENTRIES);
        }

        //If the event is valid
        if (OnLeaderboardChange != null)
        {
            //Call the event
            OnLeaderboardChange(m_currentLeaderboard);
        }
    }

    /*
    Description: Helper function to get the final data path of the leaderboard. This combines the "pure" file path, with 
    the name of the leaderboard file.
    Parameters: string aFilePath - The path , not including leaderboard file name, where the leaderboard will be stored 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 10, 2017                                                                                      
    */
    public string GetFinalSavePath(string aFilePath)
    {
        //Return the file path, combined with the filename
        return aFilePath + m_leaderboardFileName;
    }

    /*
    Description: Clear the leaderboard stored in memory, it doesn't delete the leaderboard file.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Saturday, January 20, 2017
    */
    public void ClearLeaderboardInMemory()
    {
        //Note: This clears the leaderboard stored in memory, it doesn't delete the leaderboard file.

        //If the current leaderboard is valid
        if (m_currentLeaderboard != null)
        {
            //Clear the leaderboard list of player entries, it doesn't affect the file.
            m_currentLeaderboard.Clear();
        }
    }


    /*
    Description: Function used to make a deep copy of the current leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Saturday, January 20, 2017
    Extra Note: Implementation of the ICloneable interface.
    */
    public object Clone()
    {
        //Create a new leaderboard with the same name as this one
        CLeaderboard clonedLeaderboard = new CLeaderboard(m_leaderboardLevel);

        //Copy each of the entries in the leaderboard
        clonedLeaderboard.AddToLeaderboard(m_currentLeaderboard);

        //Set the same sorting method (this also sets the sorting delegate)
        clonedLeaderboard.PLeaderboardSortingMethod = m_sortingMethod;

        //Return the cloned leaderboard
        return clonedLeaderboard;
    }
}
