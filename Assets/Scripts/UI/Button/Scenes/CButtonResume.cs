using UnityEngine;
using System.Collections;

using System;

/*
Description: Resume Button class inherit from AButtonFunctionality which will resume the game
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
public class CButtonResume : AButtonFunctionality
{
    /*
    Description: Call Play function on Game manager script
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    public override void OnButtonExecution()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the game is not currently on play state
            if (CGameManager.PInstanceGameManager.PGameState != EGameStates.Play)
            {
                //Set it  to play state
                CGameManager.PInstanceGameManager.PGameState = EGameStates.Play;
            }
        }
    }
}
