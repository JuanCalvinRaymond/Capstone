using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if player and weapon spun in a certain degree within a certain time window before shot
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickDiiizzzy : ATrickScoreModifiers
{
    //Comparison variable
    private float m_weaponComparison;
    private float m_playerComparison;
    private float m_timeComparison;

    //Previous value to compare
    private Vector3 m_previousWeaponForwardDirection;
    private Vector3 m_previousPlayerForwardDirection;
    private float m_previousRegisteredTime;

    //Variable to tweak in inspector
    public float m_minRotation = 90.0f;
    public float m_timeWindow = 1.0f;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Diiizzzy";
    }

    /*
    Description: Check if player and weapon spun over a certain angle in a certain time
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_previousWeaponForwardDirection = Vector3.zero;
        m_previousPlayerForwardDirection = Vector3.zero;
        m_previousRegisteredTime = 0.0f;
        m_timeComparison = 0;
        m_weaponComparison = 0;
        m_playerComparison = 0;

        //Iterate through all list to check
        for (int i = aWeaponDataToCheck.Count - 1; i >= 0; i--)
        {
            //If the weapon is active and being held
            if (aWeaponDataToCheck[i].m_active && aWeaponDataToCheck[i].m_physicState == EWeaponPhysicsState.Grabbed)
            {
                //If previous time is not set yet
                if (m_previousRegisteredTime == 0.0f)
                {
                    //Set previous time to a new time
                    m_previousRegisteredTime = aWeaponDataToCheck[i].m_timeRegisteredToTheList;

                    //Add current index to the list
                    m_checkIndexToDelete.Add(i);
                }
                //If previous time is already set
                else
                {
                    //Add the time difference to timeDuration
                    m_timeComparison += m_previousRegisteredTime - aWeaponDataToCheck[i].m_timeRegisteredToTheList;

                    //Set previous time to a new time
                    m_previousRegisteredTime = aWeaponDataToCheck[i].m_timeRegisteredToTheList;

                    //If the time duration is within time window to check valid trick
                    if (m_timeComparison < m_timeWindow)
                    {
                        //Calculate player and weapon angle
                        m_weaponComparison += CUtilityMath.CalculatingAngleDifference(aWeaponDataToCheck[i].m_weaponForwardDirection, ref m_previousWeaponForwardDirection);
                        m_playerComparison += CUtilityMath.CalculatingAngleDifference(aWeaponDataToCheck[i].m_playerForwardDirection, ref m_previousPlayerForwardDirection);

                        //Add current index to the list
                        m_checkIndexToDelete.Add(i);
                    }
                }

            }
        }

        m_checkIndexToDelete.Reverse();
        //Return true if player and weapon spun over a certain angle
        return m_weaponComparison > m_minRotation && m_playerComparison > m_minRotation ? true : false;
    }
}
