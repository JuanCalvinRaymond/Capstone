using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if weapon switch hand
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickSwitcheroo : ATrickScoreModifiers
{
    //Comparison variable
    private bool m_comparison;

    //Current index to delete
    private int m_indexToDelete;

    //Time when player held the last hand
    public float m_previousTimeRegistered;
    
    //Time window to perform trick
    public float m_timeWindow = 0.3f;

    //Variable to store previous hand that held the weapon
    private EWeaponHand m_previousHand;
    private EWeaponHand m_currentHand;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Switcheroo";
    }

    /*
    Description: Check if weapon switch hand
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                          aWeaponDataToCompare : Left or right weapon data to compare
                          aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_previousHand = EWeaponHand.None;
        m_comparison = false;
        m_indexToDelete = 0;
        m_previousTimeRegistered = 0;

        //Iterate through all list to check
        for (int i = 0; i < aWeaponDataToCheck.Count; i++)
        {
            //If weapon is active
            if (aWeaponDataToCheck[i].m_active)
            {
                //If weapon is being held
                if (aWeaponDataToCheck[i].m_holdingHand != EWeaponHand.None)
                {
                    //If previous hand is not set yet
                    if (m_previousHand == EWeaponHand.None)
                    {
                        //Set previous hand to weapon's hand
                        m_previousHand = aWeaponDataToCheck[i].m_holdingHand;

                        m_previousTimeRegistered = aWeaponDataToCheck[i].m_timeRegisteredToTheList;
                    }
                    //If weapon is not the same as previous
                    if (aWeaponDataToCheck[i].m_holdingHand != m_previousHand)
                    {
                        //Set comparison to true
                        m_comparison = true;

                        //Set index to delete to current index
                        m_indexToDelete = i;

                        //Set previous hand to weapon's hand
                        m_previousHand = aWeaponDataToCheck[i].m_holdingHand;

                        m_previousTimeRegistered = aWeaponDataToCheck[i].m_timeRegisteredToTheList;
                    }
                    //If weapon is the same as previous
                    else if (aWeaponDataToCheck[i].m_holdingHand == m_previousHand)
                    {
                        if(aWeaponDataToCheck[i].m_timeRegisteredToTheList - m_previousTimeRegistered > m_timeWindow)
                        {
                            //Set comparison to false
                            m_comparison = false;

                        }
                    }

                }
            }
        }

        //If m_comparison is true
        if (m_comparison)
        {
            //Add the previous index and current index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete - 1);
            m_checkIndexToDelete.Add(m_indexToDelete);

        }
        //Return comparison
        return m_comparison;
    }
}
