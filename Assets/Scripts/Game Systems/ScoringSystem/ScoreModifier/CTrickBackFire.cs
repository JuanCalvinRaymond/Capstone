using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A score modifier to check if weapon is shot behind player and outside Tinnitus box
Creator: Juan Calvin Raymond
Creation Date: 22 Jan 2016
*/
public class CTrickBackFire : ATrickScoreModifiers
{
    //Const to limit weapon facing angle
    private const float M_ANGLE_ACCEPTANCE = 70.0f;

    //Current index to the delete
    private int m_indexToDelete;

    //Player scale, it will set it to default value of 1,1,1 in Awake
    private Vector3 m_playerScale;

    //Matrix to inverse and calculate weapon position
    private Matrix4x4 m_playerMatrix;

    //Comparison variable
    private bool m_comparison;

    //Tinnitus box size
    public float m_boxSize = 0.5f;

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Back Fire";
        m_playerScale = new Vector3(1, 1, 1);
    }

    /*
    Description: Check if weapon shot behind player and outside Tinnitus box
    Parameters: aWeaponDataToCheck : Left or right weapon data to check
                aWeaponDataToCompare : Left or right weapon data to compare
                aWeaponScript : Weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        //Reset all variable
        m_comparison = false;
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
                    //Set player transform, rotation, and scale matrix
                    m_playerMatrix.SetTRS(aWeaponDataToCheck[i].m_playerPosition, aWeaponDataToCheck[i].m_playerQuaternion, m_playerScale);

                    //Calculate weapon position offset from player
                    Vector3 weaponOffset = aWeaponDataToCheck[i].m_weaponPosition - aWeaponDataToCheck[i].m_playerPosition;

                    //Rotate weapon position using player inverse matrix
                    weaponOffset = m_playerMatrix.inverse * weaponOffset;
                    

                    //If weapon is outside Tinnitus box
                    if ((weaponOffset.x < m_boxSize / 2 && weaponOffset.x > -m_boxSize / 2)
                        && (weaponOffset.y < -m_boxSize / 2)
                        && weaponOffset.z <= 0.1f)
                    {
                        //Set comparison to true
                        m_comparison = true;

                        //Set index to delete to current index
                        m_indexToDelete = i;
                    }
                    //If weapon is inside Tinnitus box
                    else
                    {
                        //Set comparison to false
                        m_comparison = false;
                    }

                }
            }
        }

        //Return comparison
        if (m_comparison)
        {
            //Add index to delete to the list
            m_checkIndexToDelete.Add(m_indexToDelete);
        }
        return m_comparison;
    }
}
