using UnityEngine;
using System.Collections;


/*
Description: Class used to get the current weapons the player is using, and save them as the "starting" weapons
in the setting storer
Creator: Alvaro Chavez Mixco
Creation Date:  Wednesday, January 25, 2017
*/
public class CCurrentWeaponsSaver : MonoBehaviour
{
    private CButton m_saveButton;//The button that will activate the when clicked, will activate teh settings storer

    public EWeaponHand m_handToSave = EWeaponHand.BothHands;

    /*
    Description: At start this function will get the button compoent that is in this same game object, and suscribe to itOnClick event
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, January 25, 2017
    */
    public void Start()
    {

        //Get the button in this object
        m_saveButton = GetComponent<CButton>();

        //If there is a button
        if (m_saveButton != null)
        {
            //Suscribe to the click event
            m_saveButton.OnClickEvent += SaveCurrentPlayerWeapons;

        }
    }

    /*
    Description: When this object is disabled, ensure that it unsuscribe from the OnClickEvent
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, January 25, 2017
    */
    private void OnDestroy()
    {
        //If there is a button
        if (m_saveButton != null)
        {
            //Unsuscribe from event
            m_saveButton.OnClickEvent -= SaveCurrentPlayerWeapons;
        }
    }


    /*
    Description: The function will get the weapons the player is currently holding, and save
    them in the settings storer.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, January 25, 2017
    Extra Note: This function normally gets called when the m_saveButton (the CButton component in this
    game object) call it OnClickEvent.
    */
    private void SaveCurrentPlayerWeapons()
    {
        //If there is a setting storer and a game manager (used to get the weapons the player is holding)
        if (CSettingsStorer.PInstanceSettingsStorer != null && CGameManager.PInstanceGameManager != null)
        {
            //If the player weapon handler script is valid
            if(CGameManager.PInstanceGameManager.PPlayerWeaponHandler!=null)
            {
                //Accordng to the settings, save the corresponding weapon
                switch (m_handToSave)
                {
                    //Don't save any hands weapon
                    case EWeaponHand.None:
                        break;
                    //Save right hand weapon
                    case EWeaponHand.RightHand:
                        //Get the current right weapon from the game manager and save it in the settings storer
                        CSettingsStorer.PInstanceSettingsStorer.PStartingRightWeapon =
                            CGameManager.PInstanceGameManager.PPlayerWeaponHandler.PCurrentRightWeapon;
                        break;
                    //Save left hand weapon
                    case EWeaponHand.LeftHand:
                        //Get the current left weapon from the game manager and save it in the settings storer
                        CSettingsStorer.PInstanceSettingsStorer.PStartingLeftWeapon =
                            CGameManager.PInstanceGameManager.PPlayerWeaponHandler.PCurrentLeftWeapon;
                        break;
                        //Save both hands weapons
                    case EWeaponHand.BothHands:
                        //Get the current right weapon from the game manager and save it in the settings storer
                        CSettingsStorer.PInstanceSettingsStorer.PStartingRightWeapon =
                            CGameManager.PInstanceGameManager.PPlayerWeaponHandler.PCurrentRightWeapon;

                        //Get the current left weapon from the game manager and save it in the settings storer
                        CSettingsStorer.PInstanceSettingsStorer.PStartingLeftWeapon =
                            CGameManager.PInstanceGameManager.PPlayerWeaponHandler.PCurrentLeftWeapon;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}