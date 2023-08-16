using UnityEngine;
using System.Collections;

/*
Description: End Game Menu class which inherit from CMenuSystem, will show all the children object when the game ends
Creator: Juan Calvin Raymond--
Creation Date: 11-1-2016
*/
public class CEndGameMenuSystem : CMenuSystem
{
    //EndGameStats script
    public CEndGameStats m_endGameStats;

    /*
    Description: Subscribe to Game Manager's OnEndGameState event
    Creator: Juan Calvin Raymond--
    Creation Date: 19 Dec 2016
    */
    protected override void Start()
    {
        base.Start();

        if (CGameManager.PInstanceGameManager != null)
        {
            CGameManager.PInstanceGameManager.OnEndGameState += OnEndGameState;
        }
    }

    /*
    Description: Unubscribe to Game Manager's OnEndGameState event
    Creator: Juan Calvin Raymond
    Creation Date: 19 Dec 2016
    */
    private void OnDestroy()
    {
        if (CGameManager.PInstanceGameManager != null)
        {
            CGameManager.PInstanceGameManager.OnEndGameState -= OnEndGameState;
        }
    }

    /*
    Description: Update EndGameStats's text and show all the children object
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    protected void OnEndGameState()
    {
        //If there are end game stats
        if (m_endGameStats != null)
        {
            //Update them
            m_endGameStats.UpdateText();
        }

        //Place the parent menu object in front of the player
        PlaceMenuObjectInFrontOfPlayer();

        //Activate the menu
        Activate();
    }
}
