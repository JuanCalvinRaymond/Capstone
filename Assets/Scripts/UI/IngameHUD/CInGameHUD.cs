using UnityEngine;
using System.Collections;

using UnityEngine.UI;

/*
Description: Class that goes in a Canvas that uses screen space-Camera. This HUD is used to display ammo the player has.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, October 31st, 2016
Extra Notes: 
*/
public class CInGameHUD : MonoBehaviour
{
    //NOTE : FIX THIS
    private CPlayerWeaponHandler m_weaponHandler;

    private Vector3 m_initialBarScale;

    //Stats displayed in the hud
    private float m_percentRightWeaponAmmo;
    private float m_percentLeftWeaponAmmo;
    
    //The right ammo bars
    public GameObject m_rightWeaponAmmoHighBar;
    public GameObject m_rightWeaponAmmoLowBar;

    //The left ammo bars
    public GameObject m_leftWeaponAmmoHighBar;
    public GameObject m_leftWeaponAmmoLowBar;

    public GameObject m_hider;//Object parent of all the hud used to easily hide or unhide the data

    [Range(0, 1)]
    public float m_lowAmmoPercent = 0.5f;//percent before the bar changes color

    /*
    Description: It saves the playwe weapon handler, and it also saves the intitial scale of the bars.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    Extra Notes: 
    */
    private void Start()
    {
        //Get the player and scoring system, since that will be used to update the HUD
        if (CGameManager.PInstanceGameManager != null)
        {
            m_weaponHandler = CGameManager.PInstanceGameManager.PPlayerWeaponHandler;
        }

        //Get the scale of only 1 bar to know its starting value so that we can reduce the scale to decrease it.
        if (m_rightWeaponAmmoHighBar != null)
        {
            m_initialBarScale = m_rightWeaponAmmoHighBar.transform.localScale;
        }
    }

    /*
    Description: It updates the HUD info and display it. It also checks that the HUD is only shown when the game is actually being played.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    Extra Notes: 
    */
    private void Update()
    {
        if (CGameManager.PInstanceGameManager != null && m_hider != null)
        {
            //If player is not playing
            if (CGameManager.PInstanceGameManager.PGameState != EGameStates.Play)
            {
                m_hider.SetActive(false);//Deactivate the hider, parent object of all the HUD elements
            }
            else
            {
                m_hider.SetActive(true);//If the game is playing activate the hider
            }
        }

        GetData();//Gather the HUD data to display

        //Changed the information being displayed
        UpdateDisplay(m_rightWeaponAmmoHighBar, m_rightWeaponAmmoLowBar, m_percentRightWeaponAmmo);
        UpdateDisplay(m_leftWeaponAmmoHighBar, m_leftWeaponAmmoLowBar, m_percentLeftWeaponAmmo);
    }

    /*
    Description: It gets the ammo from the player weapons.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    Extra Notes: 
    */
    private void GetData()
    {
       //Get the player ammo
       if (m_weaponHandler != null)
       {
           //Calculate the percent of the ammo according to min ammo and max ammo

           //Avoid divides by 0
           if(m_weaponHandler.PCurrentRightWeaponScript != null)
            {
                if (m_weaponHandler.PCurrentRightWeaponScript.PMaxAmmo > 0)
                {
                    m_percentRightWeaponAmmo = m_weaponHandler.PCurrentRightWeaponScript.PCurrentAmmo / 
                        (float)m_weaponHandler.PCurrentRightWeaponScript.PMaxAmmo;
                }
            }


            //Avoid divides by 0
            if (m_weaponHandler.PCurrentLeftWeaponScript != null)
            {
                if (m_weaponHandler.PCurrentLeftWeaponScript.PMaxAmmo > 0)
                {
                    m_percentLeftWeaponAmmo = m_weaponHandler.PCurrentLeftWeaponScript.PCurrentAmmo /
                        (float)m_weaponHandler.PCurrentLeftWeaponScript.PMaxAmmo;
                }
            }
        }
    }

    /*
    Description: Changes the scale of the ammmo bar to make them increase and decrease in size.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    Extra Notes: 
    */
    private void UpdateDisplay(GameObject aWeaponAmmoHighBar, GameObject aWeaponAmmoLowBar, float aPercentWeaponAmmo)
    {
        //Update the right weapon ammo bar
        if (aWeaponAmmoHighBar != null && aWeaponAmmoLowBar != null)
        {
            //Set the scale for both of them
            aWeaponAmmoHighBar.transform.localScale = new Vector3(m_initialBarScale.x, m_initialBarScale.y * aPercentWeaponAmmo, m_initialBarScale.z);
            aWeaponAmmoLowBar.transform.localScale = new Vector3(m_initialBarScale.x, m_initialBarScale.y * aPercentWeaponAmmo, m_initialBarScale.z);

            //Decide which bar to show
            if (aPercentWeaponAmmo > m_lowAmmoPercent)//If the user has high ammo percent
            {
                aWeaponAmmoHighBar.SetActive(true);
                aWeaponAmmoLowBar.SetActive(false);
            }
            else//if the user has low ammo
            {
                aWeaponAmmoHighBar.SetActive(false);
                aWeaponAmmoLowBar.SetActive(true);
            }
        }
    }
}
