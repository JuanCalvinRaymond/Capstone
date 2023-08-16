using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if weapon spun over a certain degree before shot
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickFlipShot : ATrickScoreModifiers
{
    //Comparison variable
    private float m_comparison;

    //Angle Difference
    private float m_angleDifference;

    //Previous value
    private Vector3 m_previousForwardDirection;

    //Variable to tweak in inspector
    public float m_minAngleToAcceptTrick = 300;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Flip Shot";
    }

    /*
    Description: Check if the weapon was tossed into the air and flipped 
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 2 Dec 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_previousForwardDirection = Vector3.zero;
        m_angleDifference = 0;

        //Iterate through all list to check
        for (int i = 0; i < aWeaponDataToCheck.Count; i++)
        {
            //If weapon is active
            if (aWeaponDataToCheck[i].m_active)
            {
                //If weapon is not being held
                if (aWeaponDataToCheck[i].m_holdingHand == EWeaponHand.None)
                {
                    //Calculate angle difference
                    m_angleDifference += CUtilityMath.CalculatingAngleDifference(aWeaponDataToCheck[i].m_weaponForwardDirection, ref m_previousForwardDirection);

                    //Add current index to the list
                    m_checkIndexToDelete.Add(i);
                }
            }
        }
        
        //Return true if weapon spun over a certain angle
        return m_angleDifference > m_minAngleToAcceptTrick ? true : false;
    }
}
