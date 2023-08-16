using UnityEngine;
using System.Collections;

/*
Description: Class used to read input from a single weapon
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, October 10th, 2016
*/
public class CWeaponControlInput
{
    protected SIndividualWeaponKeys m_weaponKeys;
    protected EWeaponHand m_weaponHand;

    public EWeaponHand PWeaponHand
    {
        get
        {
            return m_weaponHand;
        }

        set
        {
            PWeaponHand = value;
        }
    }

    /*
    Description: Constructor to save on which hand the weapon is, and which keys are used to control it.
    Parameters: EWeaponHand aWeaponHand-In which hand the weapon is, used for input keys.
                SIndividualWeaponKeys aWeaponKeys-The key binding used to control the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public CWeaponControlInput(EWeaponHand aWeaponHand, SIndividualWeaponKeys aWeaponKeys)
    {
        m_weaponKeys = aWeaponKeys;
        m_weaponHand = aWeaponHand;
    }

    /*
    Description: Checks if the corresponding firing key is being pressed
    Parameters: bool aWeaponAutomaticFire- Whether the weapon is automatic or not, it will determine if
    the function checks if the button is pressed, or just once the button is pressed down.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    virtual public bool GetIsFiringWeapon(bool aWeaponAutomaticFire = false)
    {
        //If the weapon is not automatic
        if (aWeaponAutomaticFire == false)
        {
            //Check whether the button was pressed down
            return Input.GetButtonDown(m_weaponKeys.m_fireWeaponKey);
        }
        else//If the weapon is automatic
        {
            //Check if the button is being pressed
            return Input.GetButton(m_weaponKeys.m_fireWeaponKey);
        }
    }

    /*
    Description: Manually called update function.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2017
    */
    virtual public void Update()
    {
    }

    /*
    Description: Checks if the corresponding reloading key is being pressed
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    virtual public bool GetIsReloading()
    {
        return Input.GetButtonDown(m_weaponKeys.m_reloadWeaponKey);
    }

    /*
    Description: Returns the axis of the shooting, or the trigger press.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    virtual public float GetTriggerPressedAxis()
    {
        return Input.GetAxis(m_weaponKeys.m_fireWeaponKey);
    }

    /*
    Description:Returns if the user has pressed the key to grab a weapon
    Creator: Alvaro Chavez Mixco
    */
    virtual public bool GetIsGrabbing()
    {
        return Input.GetButtonDown(m_weaponKeys.m_grabWeaponKey);
    }

}
