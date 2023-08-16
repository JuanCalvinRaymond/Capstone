using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if weapon is faceing away between a certain angle from player
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickPeripheral : ATrickScoreModifiers
{
    //Comparison variable
    private float m_comparison;

    //Current index to delete
    private int m_indexToDelete;

    //Variable to tweak in inspector
    public float m_minAngleFromPlayerHead = 75;
    public float m_maxAngleFromPlayerHead = 105;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Peripheral";
    }

    /*
    Description: Check if the weapon is facing away between certain angle from player
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
            //If weapon is active
            if (aWeaponDataToCheck[i].m_active)
            {
                //If the time when shoot is the most recent one
                if (aWeaponDataToCheck[i].m_timeRegisteredToTheList <= aTimeWhenShot)
                {
                    //Set comparison to angle between weapon and player
                    m_comparison = Vector3.Angle(aWeaponDataToCheck[i].m_playerForwardDirection, aWeaponDataToCheck[i].m_weaponForwardDirection);

                    //Set index to delete to current index
                    m_indexToDelete = i;
                }
            }
        }
        //Return true if weapon is facing away between a certain angle
        if (m_comparison >= m_minAngleFromPlayerHead && m_comparison <= m_maxAngleFromPlayerHead)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
            return true;
        }
        return false;
    }
}
