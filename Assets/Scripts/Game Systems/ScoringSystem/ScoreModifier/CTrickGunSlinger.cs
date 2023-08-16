using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if the gun is facing away from each other above certain angle when shot
Creator: Juan Calvin Raymond
Creation Date:2 Dec 2016
*/
public class CTrickGunSlinger : ATrickScoreModifiers
{
    //Comparison variable
    private float m_comparison;

    //Current index to delete
    private int m_indexToDelete;

    //Variable to tweak in inspector
    public float m_angleToAcceptTrick = 135;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Gunslinger";
    }

    /*
    Description: Check if the gun is facing away over a certain of angle
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 2 Des 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_comparison = 0;
        m_indexToDelete = 0;

        if (aWeaponDataToCheck.Count == aWeaponDataToCompare.Count)
        {
            //Iterate through all list to check
            for (int i = 0; i < aWeaponDataToCheck.Count; i++)
            {
                if (aWeaponDataToCheck[i].m_active)
                {
                    //If the time when shoot is the most recent one
                    if (aWeaponDataToCheck[i].m_timeRegisteredToTheList <= aTimeWhenShot)
                    {
                        if (aWeaponDataToCompare[i].m_holdingHand != EWeaponHand.None && aWeaponDataToCompare[i].m_active)
                        {
                            //Get the comparison data from the other list
                            m_comparison = Vector3.Angle(aWeaponDataToCheck[i].m_weaponForwardDirection, aWeaponDataToCompare[i].m_weaponForwardDirection);
                            
                            //Set index to delete to current index
                            m_indexToDelete = i;
                        }
                    }
                }
            }
        }

        //Return true if weapon facing away over a certain angle
        if (m_comparison >= m_angleToAcceptTrick)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
            m_compareIndexToDelete.Add(m_indexToDelete);
            return true;
        }
        return false;
    }

}