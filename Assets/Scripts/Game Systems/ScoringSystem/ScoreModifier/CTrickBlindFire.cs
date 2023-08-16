using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if the gun is facing away from player over a certain angle when shot
Creator: Juan Calvin Raymond
Creation Date:22 Jan 2016
*/
public class CTrickBlindFire : ATrickScoreModifiers
{
    //Comparison variable
    private float m_comparison;

    //Index to delete
    private int m_indexToDelete;

    //Variable to tweak in inspector
    public float m_minAngleFromPlayerHead = 105;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Blind Fire";
    }

    /*
    Description: Check if the gun is facing away from player over a certain of angle
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_comparison = 0;
        m_indexToDelete = 0;
        
        //Iterate through all list to check
        for (int i = 0; i < aWeaponDataToCheck.Count; i++)
        {
            //If the time when shoot is the most recent one
            if (aWeaponDataToCheck[i].m_timeRegisteredToTheList <= aTimeWhenShot && aWeaponDataToCheck[i].m_active)
            {
                //Check the angle
                m_comparison = Vector3.Angle(aWeaponDataToCheck[i].m_playerForwardDirection, aWeaponDataToCheck[i].m_weaponForwardDirection);
                

                //Set index to delete to current index
                m_indexToDelete = i;
            }
        }

        //Return true if the other gun is facing away from player in a certain angle
        if (m_comparison >= m_minAngleFromPlayerHead)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
            return true;
        }
        return false;

    }
}
