using UnityEngine;
using System.Collections;

/*
Description: Class used to read input from a single weapon . This class inherits from CWeaponControlInput,
but is distinguished because it considers the fire "button" an axis.
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, October 11, 2016
*/
public class CWeaponAxisControlInput : CWeaponControlInput
{
    private bool m_wasFiring = false;
    private float m_triggerAxis = 0.0f;

    //Variable to ensure that the value is only read once per frame, and not changed
    private bool m_previousWasFiring = false;

    /*
    Description: Constructor to merely call the parent class constructor
    Parameters: EWeaponHand aWeaponHand-In which hand the weapon is, used for input keys.
                SIndividualWeaponKeys aWeaponKeys-The key binding used to control the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 11, 2016
    */
    public CWeaponAxisControlInput(EWeaponHand aWeaponHand, SIndividualWeaponKeys aWeaponKeys) : base(aWeaponHand, aWeaponKeys)
    {
    }

    /*
    Description: Manually called update function. This function merely resets the set 
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2017
    */
    override public void Update()
    {
        m_triggerAxis = 0.0f;
        m_previousWasFiring = m_wasFiring;
    }

    /*
    Description: Checks if the corresponding firing key axis is being pressed. This function has to be called every frame to work properly.
    Parameters: bool aWeaponAutomaticFire- Whether the weapon is automatic or not, it will determine if
    the function checks if the button is pressed, or just once the button is pressed down.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 11, 2016
    */
    override public bool GetIsFiringWeapon(bool aWeaponAutomaticFire = false)
    {
        m_triggerAxis = Input.GetAxis(m_weaponKeys.m_fireWeaponKey);

        //If the weapon is not automatic
        if (aWeaponAutomaticFire == false)
        {
            //If the trigger is being pressed
            if (m_triggerAxis > 0.0f && m_previousWasFiring == false)
            {
                m_wasFiring = true;
                return true;
            }

            //Check that the trigger is no longer being pressed, and that is not locked
            if (m_triggerAxis <= 0.0f && m_previousWasFiring == true)
            {
                m_wasFiring = false;
            }
        }
        else//If the weapon is automatic
        {
            //Only check if the trigger is being pressed
            if (m_triggerAxis > 0.0f)
            {
                return true;
            }
        }

        //If the trigger is not beigng pressed
        return false;
    }
}
