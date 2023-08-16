using UnityEngine;
using System.Collections;

using System;

/*
Description: Structs to easily organize the inputs and axis strings (according to Unity input manager) into variables.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
Extra Notes: 
*/
[Serializable]
public struct SControlKeys
{
    private string m_fireRightWeaponKey;
    private string m_fireLeftWeaponKey;

    private string m_reloadRightWeaponKey;
    private string m_reloadLeftWeaponKey;

    private string m_horizontalMovementAxis;
    private string m_verticalMovementAxis;

    private string m_horizontalLookAxis;
    private string m_verticalLooKAxis;

    private string m_pauseKey;

    private string m_grabRightWeaponKey;
    private string m_grabLeftWeaponKey;

    private SIndividualWeaponKeys m_rightWeaponKeys;
    private SIndividualWeaponKeys m_leftWeaponKeys;

    public string PRightWeaponFireKey { get { return m_fireRightWeaponKey; } }
    public string PLeftWeaponFireKey { get { return m_fireLeftWeaponKey; } }

    public string PRightWeaponReloadKey { get { return m_reloadRightWeaponKey; } }
    public string PLeftWeaponReloadKey { get { return m_reloadLeftWeaponKey; } }

    public string PHorizontalMovementAxis { get { return m_horizontalMovementAxis; } }
    public string PVerticalMovementAxis { get { return m_verticalMovementAxis; } }

    public string PHorizontalLookAxis { get { return m_horizontalLookAxis; } }
    public string PVerticalLookAxis { get { return m_verticalLooKAxis; } }

    public string PPause { get { return m_pauseKey; } }

    public string PGrabRightWeaponKey { get { return m_grabRightWeaponKey; } }
    public string PGrabLeftWeaponKey { get { return m_grabLeftWeaponKey; } }

    public SIndividualWeaponKeys PRightWeaponKeys { get { return m_rightWeaponKeys; } }
    public SIndividualWeaponKeys PLeftWeaponKeys { get { return m_leftWeaponKeys; } }

    /*
    Description:Set the default names for the inputs for Mouse and Keyboard
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    public void SetDefaultMouseAndKeyboardInputStrings()
    {
        m_fireRightWeaponKey = "MK_FireRightWeapon";
        m_fireLeftWeaponKey = "MK_FireLeftWeapon";

        m_reloadRightWeaponKey = "MK_ReloadRightWeapon";
        m_reloadLeftWeaponKey = "MK_ReloadLeftWeapon";

        m_horizontalMovementAxis = "MK_Horizontal";
        m_verticalMovementAxis = "MK_Vertical";

        m_horizontalLookAxis = "MK_Mouse X";
        m_verticalLooKAxis = "MK_Mouse Y";

        m_pauseKey = "MK_Pause";

        m_grabRightWeaponKey = "MK_GrabRightWeapon";
        m_grabLeftWeaponKey = "MK_GrabLeftWeapon";

        SaveWeaponKeys();//Save the weapon struct variables
    }

    /*
    Description:Set the default names for the inputs for Gamepad
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    public void SetDefaultGamepadInputStrings()
    {
        m_fireRightWeaponKey = "GP_FireRightWeapon";
        m_fireLeftWeaponKey = "GP_FireLeftWeapon";

        m_reloadRightWeaponKey = "GP_ReloadRightWeapon";
        m_reloadLeftWeaponKey = "GP_ReloadLeftWeapon";

        m_horizontalMovementAxis = "GP_Horizontal";
        m_verticalMovementAxis = "GP_Vertical";

        m_horizontalLookAxis = "GP_HorizontalLook";
        m_verticalLooKAxis = "GP_VerticalLook";

        m_pauseKey = "GP_Pause";

        m_grabRightWeaponKey = "GP_GrabRightWeapon";
        m_grabLeftWeaponKey = "GP_GrabLeftWeapon";

        SaveWeaponKeys();//Save the weapon struct variables
    }

    /*
    Description: Saves the keys for firing and reloading, for both weapons, in a single to use struct.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public void SaveWeaponKeys()
    {
        //Save the weapon struct variables
        //Right weapon
        m_rightWeaponKeys = new SIndividualWeaponKeys();
        m_rightWeaponKeys.m_fireWeaponKey = m_fireRightWeaponKey;
        m_rightWeaponKeys.m_reloadWeaponKey = m_reloadRightWeaponKey;
        m_rightWeaponKeys.m_grabWeaponKey = m_grabRightWeaponKey;

        //Left weapon
        m_leftWeaponKeys = new SIndividualWeaponKeys();
        m_leftWeaponKeys.m_fireWeaponKey = m_fireLeftWeaponKey;
        m_leftWeaponKeys.m_reloadWeaponKey = m_reloadLeftWeaponKey;
        m_leftWeaponKeys.m_grabWeaponKey = m_grabLeftWeaponKey;
    }
}

/*
Description: Struct to merely organize the strings that will be passed to the CWeaponControlInput class
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, October 10th, 2016
*/
public struct SIndividualWeaponKeys
{
    public string m_fireWeaponKey;
    public string m_reloadWeaponKey;
    public string m_grabWeaponKey;
}

