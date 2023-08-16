using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//PENDING
public class CWeaponDataTracker : MonoBehaviour
{
    //Weapon Script
    private AWeapon m_weapon;

    //List of weapon data
    private List<SWeaponData> m_listOfWeaponData;

    //Weapon data registration timer
    private float m_registerDataTimer;
    
    //Variable to tweak in inspector
    public int m_maxAmountOfWeaponData;
    public float m_intervalToRegisterWeaponData;
    
    public List<SWeaponData> PListOfWeaponData
    {
        get
        {
            return m_listOfWeaponData;
        }
    }

    private void Awake()
    {
        m_weapon = GetComponent<AWeapon>();
        m_listOfWeaponData = new List<SWeaponData>();
        m_registerDataTimer = m_intervalToRegisterWeaponData;
    }

    // Update is called once per frame
    void Update()
    {
        if(CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
        {
            //Decrease register timer
            m_registerDataTimer -= CGameManager.PInstanceGameManager.GetScaledDeltaTime();

            //If timer is finished
            if (m_registerDataTimer < 0)
            {
                //Track data from both weapon
                TrackWeaponData();

                //Reset timer
                m_registerDataTimer = m_intervalToRegisterWeaponData;
            }
        }
    }

    /*
    Description: Tracking the weapon data, if the data is more than maximum delete the oldest one and shift every index back by 1
    Parameters: aWeapon : Weapon script
                aListOfWeaponData : list of weapon data to modify
    Creator: Juan Calvin Raymond
    Creation Date:2 Des 2016
    */
    private void TrackWeaponData()
    {
            //Make a temp variable
            SWeaponData tempWeaponData = m_weapon.PWeaponData;

            //Set time registered to list variable
            tempWeaponData.m_timeRegisteredToTheList = Time.time;

            //Add weapon data to the list
            m_listOfWeaponData.Add(tempWeaponData);

            //If the list is more than maximum
            if (m_listOfWeaponData.Count > m_maxAmountOfWeaponData)
            {
                //Remove data at index 0
                m_listOfWeaponData.RemoveAt(0);
            }
    }

    /*
   Description: Removing data from right weapon data list
   Parameters: aIndex : Index to delete
   Creator: Juan Calvin Raymond
   Creation Date: 6 Feb 2017
   */
    public void RemovingDataAtIndex(int aIndex)
    {
        m_listOfWeaponData.RemoveAt(aIndex);
    }
}
