using UnityEngine;
using System.Collections;

/*
Description: Class used to store and handle a CLeaderboard object. This class doesn't provide access to the leaderboard
actual functionality.
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, January 10, 2017
Extra Notes: This class was mainly created to allow modifications through UnityEditor.
*/
public class CLeaderboardHandler : MonoBehaviour
{
    private CLeaderboard m_leaderboard;

    public ELevelState m_leaderboardLevel;

    public CLeaderboard PLeaderboard
    {
        get
        {
            return m_leaderboard;
        }
    }

    public ELevelState PLeaderboardLevel
    {
        get
        {
            return m_leaderboardLevel;
        }
    }

    /*
    Description: At Awake, create the leaderboard  object at the desired data path, for the specified level.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 10, 2017
    */
    private void Awake()
    {
        //Create the CLeaderboard object
        m_leaderboard = new CLeaderboard(m_leaderboardLevel);

        //Read the existing leaderboard
        m_leaderboard.ReadLeaderboard(CUtilityFiles.GetSavePath());
        //Note: Reading the leaderboard on awake, rather than when it is needed means that the WHOLE leaderboard will be stored
        // in memory until it is used.This is done to make sorting/accessing leaderboard functions faster. If this is taking
        // too much memory the leaderboard should be read, before it is used, and cleared after it is used.
    }
}
