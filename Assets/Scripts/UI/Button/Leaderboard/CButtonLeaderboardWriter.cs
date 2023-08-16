using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System;

/*
Description: Class used to get the current data from the player playthrough, and write it to the leaderboard
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, Novemeber 15, 2016
*/
public class CButtonLeaderboardWriter : AButtonFunctionality
{
    private SPlayerEntry m_playerData;//Data that will be written to the leaderboard
    private bool m_hasWrittenToLocalLeaderboard = false;
    private bool m_hasWrittenToOnlineLeaderboard = false;

    [Tooltip("The text from which the player name will be read")]
    public Text m_playerName;
    public bool m_allowMultipleWrites = false;

    [Header("Writing Destination")]
    public bool m_writeToLocalLeaderboard = true;
    public bool m_writeToOnlineLeaderboard = true;

    [Space(20)]
    [Tooltip("If the writer should ask the game manager if the current weapons are valid for the leaderboard")]
    public bool m_limitEntriesAccordingToWeapon = true;

    [Header("Default Name")]
    [Tooltip("Name that will be set if the text is empty")]
    public string m_defaultName = "Kevin";
    [Tooltip("Number the default name according to the number of entries in leaderboard")]
    public bool m_numberName = true;

    /*
    Description: The function will get the player name from the desired text object, and will fill the rest of the SPlayerEntry data by quering the 
    scoring system from the game manager. Finally, once it has the data it will write to the leaderbaord.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, Novemeber 15, 2016
    */
    private void WriteToLocalLeaderboard()
    {
        //If the user has never written to leaderboard, or he can write as many times as he wants
        if (m_hasWrittenToLocalLeaderboard == false || m_allowMultipleWrites == true)
        {
            //Get the leaderboard for the current level
            CLeaderboard leaderboardCurrentLevel =
                CGameManager.PInstanceGameManager.GetLeaderboard(CGameManager.PInstanceGameManager.PLevelState);

            //If the game manager has a leaderboard
            if (leaderboardCurrentLevel != null)
            {
                //Add the data to the leaderboard
                leaderboardCurrentLevel.AddToLeaderboard(m_playerData);

                //Write ALL the leaderboard data to a file
                leaderboardCurrentLevel.WriteToLeaderboard(CUtilityFiles.GetSavePath());

                //Set that it has written to leaderboard
                m_hasWrittenToLocalLeaderboard = true;
            }
        }

    }

    /*
    Description: Using the online manager, send the entry to the leaderboard so that it can be, if applicable, added to the online
    leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 17, 2017
    */
    private void WriteToOnlineLeaderboard()
    {
        //Check if the user is online
        //If there is no online manager or game manager
        if (COnlineManager.s_instanceOnlineManager == null && CGameManager.PInstanceGameManager == null)
        {
            //Exit the function
            return;
        }
        //If the online manager is not online
        else if (COnlineManager.s_instanceOnlineManager.GetIsConnectedToLeaderboardServer() == false)
        {
            //Exit the function
            return;
        }


        //If the user has never written to leaderboard, or he can write as many times as he wants
        if (m_hasWrittenToOnlineLeaderboard == false || m_allowMultipleWrites == true)
        {
            //Send this entry to the online leaderboard server so that it can be added
            COnlineManager.s_instanceOnlineManager.SendEntryToLeaderboard(CGameManager.PInstanceGameManager.PLevelState,
                m_playerData);

            //Set that it has written to leaderboard
            m_hasWrittenToOnlineLeaderboard = true;
        }
    }

    /*
    Description: This will fill out a player entry with data from the playerName text and the scoring system,
    and then, if applicable, it will write the data to the local and/or online leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 17, 2017
    */
    public void WriteToLeaderboard()
    {
        //If we only want to write valid entries to leaderboard
        if (m_limitEntriesAccordingToWeapon == true)
        {
            //If game manager is valid
            if (CGameManager.PInstanceGameManager != null)
            {
                //If the weapons valid with the game manager dont match the starting weapons from the setting storer
                if (CGameManager.PInstanceGameManager.CheckWeaponsValidForLeaderboard() == false)
                {
                    //Exit the function without writing to leaderboard
                    return;
                }
            }
        }

        //If there is a player name text
        if (string.IsNullOrEmpty(m_playerName.text) == false)
        {
            //Set its value
            m_playerData.m_playerName = m_playerName.text;
        }
        else//If there is no player name
        {
            //Get the leaderboard for the current level
            CLeaderboard leaderboardCurrentLevel =
                CGameManager.PInstanceGameManager.GetLeaderboard(CGameManager.PInstanceGameManager.PLevelState);

            int numberIndex = 0;

            //If the leaderboard is valid
            if (leaderboardCurrentLevel != null)
            {
                //Set the number according to the number of entries in leaderboard
                numberIndex = leaderboardCurrentLevel.PCurrentNumberEntries;
            }

            //Set default name plus desired index
            m_playerData.m_playerName = m_defaultName + numberIndex.ToString();

        }

        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If there is a scoring system
            if (CGameManager.PInstanceGameManager.PScoringSystem != null)
            {
                //Get the player data from the scoring system//Get the player data from the scoring system
                CGameManager.PInstanceGameManager.PScoringSystem.GetPlayerScoringData(ref m_playerData);
            }
        }

        //If we want to write to the local leaderboard
        if (m_writeToLocalLeaderboard == true)
        {
            //Write to the local leaderboard
            WriteToLocalLeaderboard();
        }

        //If we want to write to the online leaderboard
        if (m_writeToOnlineLeaderboard == true)
        {
            //Write to the online leaderboard
            WriteToOnlineLeaderboard();
        }
    }

    //Done in button click instead of on execution to ensure it has time to finish writing
    public override void OnButtonClick()
    {
        base.OnButtonClick();

        m_playerData = new SPlayerEntry();

        //Write the entry to the leaderboard
        WriteToLeaderboard();
    }

    public override void OnButtonExecution()
    {
    }
}