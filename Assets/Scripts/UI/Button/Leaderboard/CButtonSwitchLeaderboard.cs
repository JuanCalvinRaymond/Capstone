using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

/*
Description: Class used to switch a leaderboard display to showing the local leaderboard to 
showing the online leaderboard, and viceversa.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, January 22, 2017
*/
public class CButtonSwitchLeaderboard : AButtonFunctionality
{
    public CLeaderboardDisplay m_leaderboardDisplay;
    public Text m_currentLeaderboardTitle;

    [Header("Label Settings")]
    public MeshFilter m_labelMeshFilter;
    public Mesh m_localMesh;
    public Mesh m_onlineMesh;

    /*
    Description: At start, update the text display to match whatever leaderboard
    the leaderboard display is showing
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    protected override void Start()
    {
        //Call the  base start method
        base.Start();

        //Update the text display according to which leaderboard the leaderbaord display is showing
        UpdateStatusDisplay();
    }

    /*
    Description: Updates the display to show whether the leaderboard display is showing
    the local or the online leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void UpdateStatusDisplay()
    {
        //If the leaderboard display is valid
        if (m_leaderboardDisplay != null)
        {
            //If the leaderbaord display is showing the local leaderboard
            if (m_leaderboardDisplay.PIsShowingLocalLeaderboard == true)
            {
                //Change the text
                CUtilitySetters.SetText2DText(ref m_currentLeaderboardTitle, CServerClientConstants.M_LABEL_OFFLINE_TEXT);

                //Change the mesh
                CUtilitySetters.SetMesh(ref m_labelMeshFilter, m_localMesh);
            }
            else//If the leaderboard display is showing the online leaderboard
            {
                //Change the text
                CUtilitySetters.SetText2DText(ref m_currentLeaderboardTitle, CServerClientConstants.M_LABEL_ONLINE_TEXT);

                //Change the mesh
                CUtilitySetters.SetMesh(ref m_labelMeshFilter, m_onlineMesh);
            }
        }
    }

    /*
    Description: Flips which leaderboard, local or online, the leaderboard display is currently showing.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    public override void OnButtonExecution()
    {
        //If the leaderboard display is valid
        if (m_leaderboardDisplay != null)
        {
            //Flip which leaderboard (local or online) it is currently showing
            m_leaderboardDisplay.PIsShowingLocalLeaderboard = !m_leaderboardDisplay.PIsShowingLocalLeaderboard;

            UpdateStatusDisplay();
        }
    }
}
