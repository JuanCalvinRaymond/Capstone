using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if weapon is rolled between a certain angle when shot
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickGangstaStyle : ATrickScoreModifiers
{
    //Comparison variable
    private float m_comparison;

    //Current index to delete
    private int m_indexToDelete;

    //Variable to tweak in inspector
    public float m_minRollValue = 80.0f;
    public float m_maxRollValue = 280.0f;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Gangsta Style";
    }

    /*
    Description: Check if weapon is rolled between a certain angle
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
                    //Set comparison to weapon z rotation
                    m_comparison = aWeaponDataToCheck[i].m_weaponRotation.z;
                    
                    //Set index to delete to current index
                    m_indexToDelete = i;
                }
            }
        }

        //Return true if weapon is rolled in a certain angle
        if( m_comparison > m_minRollValue && m_comparison < m_maxRollValue)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
            return true;
        }
        return false;
    }
}
