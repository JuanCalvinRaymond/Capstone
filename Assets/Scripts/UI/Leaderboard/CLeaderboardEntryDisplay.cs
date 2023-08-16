using UnityEngine;
using System.Collections;
   
using UnityEngine.UI;

/*
Description: This class is merely used to easily organize and access all the text objects that make up 
an entry in the leaderboard. In that way they can be easily modified.
Parameters(Optional): 
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
Extra Notes: 
*/
public class CLeaderboardEntryDisplay:MonoBehaviour
{
    //Simply organizes all the texts that will be displayed in a single entry on the leaderboard. Since this is a component,
    //this is a class and not a struct.
    public Text m_textName;    
    public Text m_textAccuracy;
    public Text m_textLongestStreak;
    public Text m_textNumberTricks;
    public Text m_textNumberCombos;
    public Text m_textTime;
    public Text m_textScore;
}
