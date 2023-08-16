using UnityEngine;
using System.Collections;

/*
Description: Class used to display the leaderboard of the previous level the user played. This requires
             custom scene manager in order to get the level that was previously played.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, March 27th, 2017
*/
public class CLeaderboardDisplayer : MonoBehaviour
{
    [Tooltip("The gameobjects to display the desired leaderboard. Index 0 is the default, used if there is no scene manager. Index 1 is the beginner level. Index 2 is the advanced level.")]
    public GameObject[] m_leaderboardDisplay;
    /*
    Description: Show the corresponding leaderboard display, according to the data stored in the scene manager
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 27th, 2017
    */
    private void Start()
    {
        //If there is a scene manager
        if (CSceneManager.PInstanceSceneManager != null)
        {
            //Display the leaderboard for the previous level played
            DisplayLevelLeaderboard(CSceneManager.PInstanceSceneManager.PPreviousTypeSceneLoaded);
        }
        else//If there is no scene manager
        {
            //Display the default leaderboard display
            DisplayLevelLeaderboard(ELevelState.NoMotion);
        }
    }

    /*
    Description: Displays the leaderboard display object for the desired object
    Parameters: ELevelState aLevel - The corresponding level of the leaderboard display to show
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 27th, 2017
    */
    private void DisplayLevelLeaderboard(ELevelState aLevel)
    {
        //Display the corresponding leaderboard display, while hiding the others
        CUtilitySetters.SetActiveAndDeactivateOther(m_leaderboardDisplay, (int)aLevel);
    }
}
