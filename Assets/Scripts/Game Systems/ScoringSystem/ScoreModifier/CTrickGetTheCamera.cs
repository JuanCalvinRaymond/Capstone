using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if player spun over a certain angle while weapon is in the air
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickGetTheCamera : ATrickScoreModifiers
{
    //Const variable
    private const float M_ANGLE_TO_REVERSE_THE_CALCULATION = 225.0f;
    private const float M_FULL_CIRCLE = 360.0f;

    //Comparison variable
    private float m_comparison;

    //Previous player Y rotation
    private float m_previousYRotation;

    //Angle difference
    private float m_angleDifference;

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
        m_trickName = "Get The Camera";
    }

    /*
    Description: Check if player spun over certain angle while weapon is in the air
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_previousYRotation = 0.0f;
        m_angleDifference = 0;
        m_comparison = 0;

        //Iterate through all list to check
        for (int i = 0; i < aWeaponDataToCheck.Count; i++)
        {
            //If weapon is active
            if (aWeaponDataToCheck[i].m_active)
            {
                //If weapon is in the air
                if (aWeaponDataToCheck[i].m_holdingHand == EWeaponHand.None)
                {

                    //Store player y rotation in a variable
                    float playerYRotation = aWeaponDataToCheck[i].m_playerRotation.y;

                    //If player rotation below 0
                    if (playerYRotation < 0)
                    {
                        //Add 360 degree to it
                        playerYRotation += M_FULL_CIRCLE;
                    }

                    //If previous rotation is not set yet
                    if (m_previousYRotation == 0.0f)
                    {
                        //Set previous rotation to player y rotation
                        m_previousYRotation = playerYRotation;
                        m_checkIndexToDelete.Add(i);
                    }
                    //If previous rotation is already set
                    else
                    {
                        //Calculate the angle difference
                        float angleDifference = playerYRotation - m_previousYRotation;

                        //Make sure it's always positives
                        angleDifference *= angleDifference < 0.0f ? -1 : 1;

                        //Check if the difference is too high, if yes then that means player jump between 1 to 359.
                        //Calculate by having full circle minus the angle difference.
                        angleDifference = angleDifference > M_ANGLE_TO_REVERSE_THE_CALCULATION ? M_FULL_CIRCLE - angleDifference : angleDifference;

                        //Increase the angle difference
                        m_angleDifference += angleDifference;

                        //Set previous rotation to player y rotation
                        m_previousYRotation = playerYRotation;

                        //Add current index to the list
                        m_checkIndexToDelete.Add(i);

                    }
                }
                //If weapon is being grabbed
                else if (aWeaponDataToCheck[i].m_physicState == EWeaponPhysicsState.Grabbed)
                {
                    //Set comparison to angle difference
                    m_comparison = m_angleDifference;

                    //Reset angle difference back to 0
                    m_angleDifference = 0;
                }

            }
        }

        //Return true if player spun over a certain angle
        return m_comparison >= m_minAngleToAcceptTrick ? true : false;
    }
}
