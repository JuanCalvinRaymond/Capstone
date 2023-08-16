/*UPDATED AS OF: THURSDAY, FEBRUARY 2, 2017*/

using System;

/*
Description: Struct used to organize all the data that will written to the leaderboard when the player
finishes a level.
Parameters(Optional): 
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
Extra Notes: 
*/
[Serializable]
public struct SPlayerEntry
{
    public string m_playerName;
    public int m_score;
    public int m_longestStreak;
    public float m_completionTime;

    public int m_shotsFired;
    public int m_shotsHit;
    public float m_accuracy;

    public int m_numberOfTricks;
    public int m_numberOfCombos;

    //public string[] m_tricksDone;//COMMENTED FOR FUTURE USE
}
