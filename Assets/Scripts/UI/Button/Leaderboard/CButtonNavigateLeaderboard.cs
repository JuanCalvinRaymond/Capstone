using UnityEngine;
using System.Collections;
using System;


/*
Description: Button used to navigate through the entries of a leaderboard display.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, January 22, 2017
*/
public class CButtonNavigateLeaderboard : AButtonFunctionality
{
    /*
    Description:  Enum representing on which way the leaderboard entries will be traversed
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    public enum ELeaderboardNavigation
    {
        PreviousPage,
        NextPage
    }

    public CLeaderboardDisplay m_leaderboardDisplayToNavigate;
    public ELeaderboardNavigation m_navigationMethod;


    /*
    Description: Override of the AButtonFunctionality OnExecution function. This function
    will traverse to the leaderboard according to the respective navigation method.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    public override void OnButtonExecution()
    {
        //If the leaderboard display is valid
        if (m_leaderboardDisplayToNavigate != null)
        {
            //According to the navigation method
            switch (m_navigationMethod)
            {
                //Go to the previous leaderboard display page
                case ELeaderboardNavigation.PreviousPage:
                    m_leaderboardDisplayToNavigate.GoToPreviousPage();
                    break;
                //Go to the next leaderboard display page
                case ELeaderboardNavigation.NextPage:
                    m_leaderboardDisplayToNavigate.GoToNextPage();
                    break;
                default:
                    break;
            }
        }
    }
}
