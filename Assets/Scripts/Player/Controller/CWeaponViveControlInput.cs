using UnityEngine;
using System.Collections;

using Valve.VR;

/*
Description: Child class of CWeaponControlInput, this class inherits from it because it does most of its checking input
functions. However, because of SteamVR, the keys for checking input in this child class are ints (Valve.VR.EVRButtonId), while
normally buttons are saved as strings 
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, October 10th, 2016
*/
public class CWeaponViveControlInput : CWeaponControlInput
{
    private SteamVR_Controller.Device m_controllerDevice;
    private SViveIndividualWeaponKeys m_viveWeaponKeys;

    public SteamVR_Controller.Device PControllerDevice
    {
        set
        {
            m_controllerDevice = value;
        }
    }

    /*
    Description: Constructor used to call the parentt CWeaponControlInput constructor, and to set some SteamVR specific 
    input methods
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public CWeaponViveControlInput(EWeaponHand aWeaponHand, SteamVR_Controller.Device aDevice, SViveIndividualWeaponKeys aWeaponKeys) : base(aWeaponHand, new SIndividualWeaponKeys())
    {
        m_controllerDevice = aDevice;
        m_viveWeaponKeys = aWeaponKeys;
    }

    /*
    Description: Overrides the check is firing function (steamVR uses buttons indices and not strings), this function is used  
    to determined if the fire button is pressed or not 
    Parameters: bool aWeaponAutomaticFire- Whether the weapon is automatic or not, it will determine if
    the function checks if the button is pressed, or just once the button is pressed down.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public override bool GetIsFiringWeapon(bool aWeaponAutomaticFire = false)
    {
        //If there is a controller device
        if (m_controllerDevice != null)
        {
            //If there is no automatic fire
            if (aWeaponAutomaticFire == false)
            {
                //Just check if the button was pushed down
                return m_controllerDevice.GetPressDown(m_viveWeaponKeys.m_fireButtons);
            }
            else//If there is automatic fire
            {
                //Check whenever the button is pressed
                return m_controllerDevice.GetPress(m_viveWeaponKeys.m_fireButtons);
            }
        }

        return false;
    }

    /*
    Description: Overrides the check is reloading function (steamVR uses buttons indices and not strings), this function is used  
    to determined if the reload button is pressed or not 
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public override bool GetIsReloading()
    {
        //If there is a controller device
        if (m_controllerDevice != null)
        {
            return m_controllerDevice.GetPressDown(m_viveWeaponKeys.m_reloadButtons);
        }

        return false;
    }

    /*
    Description: Overrides the check is trigger pressed axis function (steamVR uses buttons indices and not strings), this function is used  
    to determined how much is the player pushing the axis of the shooting button
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public override float GetTriggerPressedAxis()
    {
        //If there is a controller device
        if (m_controllerDevice != null)
        {
            return m_controllerDevice.GetAxis(m_viveWeaponKeys.m_fireAxis).x;//Axis for trigger is only X value
        }

        return 0.0f;
    }

    /*
     Description: Overrides the GetIsGrabbing function (steamVR uses buttons indices and not strings), this function is used  
     to determined if the grab button was pressed or not
     Parameters(Optional):
     Creator: Alvaro Chavez Mixco
     Extra Notes: 
     */
    public override bool GetIsGrabbing()
    {
        //If there is a controller device
        if (m_controllerDevice != null)
        {
            //Get if the grab button is has been pressed
            return m_controllerDevice.GetPressDown(m_viveWeaponKeys.m_grabButton);
        }

        return false;
    }
}