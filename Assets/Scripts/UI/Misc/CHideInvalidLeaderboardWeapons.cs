using UnityEngine;
using System.Collections;

/*
Description: Class to hide a gameobject if the weapons currently being used are not valid for the leadeboard.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, March 21th, 2017
*/
public class CHideInvalidLeaderboardWeapons : MonoBehaviour
{
    /*
    Description: When enabled,  if the weapon is not valid for leaderboards, disable it again.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 21th, 2017
    */
    private void OnEnable()
    {
        //If the weapon is not valid for leaderboard
        if (CGameManager.PInstanceGameManager.CheckWeaponsValidForLeaderboard() == false)
        {
            //Disable the object again
            gameObject.SetActive(false);
        }
    }
}
