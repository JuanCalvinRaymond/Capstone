using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
Description: A score modifier to check if player are shooting with one gun while the other gun is in the air
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickFreefall : ATrickScoreModifiers
{
    //Comparison variable
    private bool m_comparison;

    //Current index to delete
    private int m_indexToDelete;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Freefall";
    }

    /*
    Description: Check if the other hand is empty, return true if empty and false if the hand is grabbing weapon
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date:2 Dec 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_comparison = false;
        m_indexToDelete = 0;

        if(aWeaponDataToCheck.Count == aWeaponDataToCompare.Count)
        {
            //Iterate through all list to check
            for (int i = 0; i < aWeaponDataToCheck.Count; i++)
            {
                //If weapon is active
                if (aWeaponDataToCheck[i].m_active)
                {
                    //if time when shoot is the most recent one
                    if (aWeaponDataToCheck[i].m_timeRegisteredToTheList <= aTimeWhenShot)
                    {
                        //If the other weapon is not being held and the weapon is not the same as the other gun
                        if (aWeaponDataToCompare[i].m_holdingHand == EWeaponHand.None && aWeaponDataToCheck[i].m_holdingHand != aWeaponDataToCompare[i].m_holdingHand)
                        {
                            //Set comparison to true or false based on if the other weapon active or not
                            m_comparison = aWeaponDataToCompare[i].m_active;

                            //Set index to delete to current index
                            m_indexToDelete = i;
                        }
                        else
                        {
                            //Reset comparison back to false
                            m_comparison = false;
                        }
                    }
                }
            }
        }

        //If player performed the trick
        if(m_comparison)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
        }
        //Return comparison
        return m_comparison;
    }
}
