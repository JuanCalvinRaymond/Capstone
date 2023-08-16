using UnityEngine;
using System.Collections;
using System;

/*
Description: Class used to get the current weapons the player is using, and save them as the "starting" weapons
1            in the setting storer
Creator: Alvaro Chavez Mixco
Creation Date:  Wednesday, January 25, 2017
*/
public class CButtontWeaponsSaver : AButtonFunctionality
{
    public EWeaponHand m_handToSave = EWeaponHand.BothHands;

    public override void OnButtonExecution()
    {
        //Save the current weapons the player is holding
        SaveCurrentPlayerWeapons();
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
            if (CGameManager.PInstanceGameManager.PPlayerWeaponHandler != null)
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