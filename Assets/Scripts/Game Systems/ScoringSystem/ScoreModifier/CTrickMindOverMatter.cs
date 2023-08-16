using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if weapon traveled back over a certain distance
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickMindOverMatter : ATrickScoreModifiers
{
    //Comparison variable
    private float m_comparison;

    //Longest distance weapon travelled
    private float m_longestTravelDistance;

    //Current index to delete
    private int m_indexToDelete;

    //Variable to tweak in inspector
    public float m_distanceToAcceptTrick = 10.0f;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Mind Over Matter";
    }

    /*
    Description: Check if weapon traveled ack over a certain distance
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_longestTravelDistance = 0;
        m_comparison = 0.0f;
        m_indexToDelete = 0;

        //Iterate through all list to check
        for (int i = 0; i < aWeaponDataToCheck.Count; i++)
        {
            //If weapon is traveling back
            if (aWeaponDataToCheck[i].m_physicState == EWeaponPhysicsState.TravelingBack)
            {
                //Calculate weapon distance from player
                float distance = Vector3.Distance(aWeaponDataToCheck[i].m_weaponPosition, aWeaponDataToCheck[i].m_playerPosition);

                //If distance is longer than longest distance
                if (distance > m_longestTravelDistance)
                {
                    //Set longest distance to weapon distance
                    m_comparison = distance;

                    m_longestTravelDistance = distance;

                    //Set index to delete to current index
                    m_indexToDelete = i;
                }
            }
        }

        //Return true if weapon traveled back over a certain distance
        if (m_comparison > m_distanceToAcceptTrick)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
            return true;
        }
        return false;
    }
}
