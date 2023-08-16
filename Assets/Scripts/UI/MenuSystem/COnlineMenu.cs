using UnityEngine;
using System.Collections;

/*
Description: Class use to control when the Online menu object will be shown.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, January 18, 2017
*/
public class COnlineMenu : MonoBehaviour
{
    [Tooltip("The parent object of anything that will be hidden during play game state")]
    public GameObject m_hider;

    /*
    Description: On Awake, suscribe to the game manager on game state change event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 18, 2017
    */
    private void Awake()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //Suscrbie to the game state change event
            CGameManager.PInstanceGameManager.OnGameStateChange += ShowMenu;

            //Set the initial value
            ShowMenu(CGameManager.PInstanceGameManager.PGameState);
        }
    }

    /*
    Description: On Awake, unsuscribe to the game manager on game state change event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 18, 2017
    */
    private void OnDestroy()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //Unsuscribe from its game state change event
            CGameManager.PInstanceGameManager.OnGameStateChange -= ShowMenu;
        }
    }

    /*
    Description: If the user is in play or pause state, hide the menu
    Parameters: EGameStates aGameState - The current game state of the game.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 18, 2017
    */
    private void ShowMenu(EGameStates aGameState)
    {
        //If the hider object is valid
        if (m_hider != null)
        {
            //According to the current game state
            switch (aGameState)
            {
                //If the user is playing
                case EGameStates.Play:
                    //Hide the menu
                    m_hider.SetActive(false);
                    break;
                //If the user is paused
                case EGameStates.Paused:
                    //Hide the menu
                    m_hider.SetActive(true);
                    break;
                    //For all the other states
                default:
                    //Show the mnu
                    m_hider.SetActive(true);
                    break;
            }
        }
    }
}
