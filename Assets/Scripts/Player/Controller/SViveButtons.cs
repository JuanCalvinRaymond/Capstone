using UnityEngine;
using System.Collections;

/*
Description: Structs to easily organize with understandable name the Vive controller input buttons
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 24, 2016
*/
[SerializeField]
public struct SViveButtons
{
    private Valve.VR.EVRButtonId m_triggerButton;
    private Valve.VR.EVRButtonId m_gripButton;
    private Valve.VR.EVRButtonId m_dPadDown;
    private Valve.VR.EVRButtonId m_dPadRight;
    private Valve.VR.EVRButtonId m_dPadLeft;
    private Valve.VR.EVRButtonId m_dPadUp;
    private Valve.VR.EVRButtonId m_aButton;
    private Valve.VR.EVRButtonId m_touchPadButton;
    private Valve.VR.EVRButtonId m_systemButton;
    private Valve.VR.EVRButtonId m_menuButton;
    //May not be needed, you may only need to use the touchpad and trigger buttons.Because the GetAxis function internally does the operaion
    // axisId = (uint)buttonId - (uint)EVRButtonId.k_EButton_Axis0; and a switchcase from 0 to 4.
    private Valve.VR.EVRButtonId m_touchPadAxis;//Not sure, maybe?
    private Valve.VR.EVRButtonId m_triggerAxis;

    SViveIndividualWeaponKeys m_weaponKeys;

    public Valve.VR.EVRButtonId PTriggerButton { get { return m_triggerButton; } }
    public Valve.VR.EVRButtonId PGripButton { get { return m_gripButton; } }
    public Valve.VR.EVRButtonId PDPadDown { get { return m_dPadDown; } }
    public Valve.VR.EVRButtonId PDPadRight { get { return m_dPadRight; } }
    public Valve.VR.EVRButtonId PDPadLeft { get { return m_dPadLeft; } }
    public Valve.VR.EVRButtonId PDPadUp { get { return m_dPadUp; } }
    public Valve.VR.EVRButtonId PAButton { get { return m_aButton; } }
    public Valve.VR.EVRButtonId PTouchPadButton { get { return m_touchPadButton; } }
    public Valve.VR.EVRButtonId PSystemButton { get { return m_systemButton; } }
    public Valve.VR.EVRButtonId PMenuButton { get { return m_menuButton; } }
    public Valve.VR.EVRButtonId PTouchPadAxis { get { return m_touchPadAxis; } }//only has X value
    public Valve.VR.EVRButtonId PTriggerAxis { get { return m_triggerAxis; } }

    public SViveIndividualWeaponKeys PWeaponKeys { get { return m_weaponKeys; } }

    /*
    Description: Saves all the Vive controller buttons in an easier, more accessible way
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public void SetViveKeyBinding()
    {
        m_triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        m_gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        m_dPadDown = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
        m_dPadRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
        m_dPadLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
        m_dPadUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
        m_aButton = Valve.VR.EVRButtonId.k_EButton_A;
        m_touchPadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
        m_systemButton = Valve.VR.EVRButtonId.k_EButton_System;
        m_menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

        m_touchPadAxis = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
        m_triggerAxis = Valve.VR.EVRButtonId.k_EButton_Axis1;//Only has X value

        SetWeaponKeyBinding();
    }

    /*
     Description: Function to save which buttons will be used by the CWeaponViveControlInput
     Creator: Alvaro Chavez Mixco
     Creation Date: Thursday, November 10th, 2016
     */
    public void SetWeaponKeyBinding()
    {
        m_weaponKeys.m_fireButtons = m_triggerButton;
        m_weaponKeys.m_fireAxis = m_triggerAxis;
        m_weaponKeys.m_reloadButtons = m_touchPadButton;
        m_weaponKeys.m_grabButton = m_gripButton;
    }
};

/*
Description: Struct to merely organize the strings that will be passed to the CWeaponControlInput class
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, October 10th, 2016
*/
public struct SViveIndividualWeaponKeys
{
    public Valve.VR.EVRButtonId m_fireButtons;
    public Valve.VR.EVRButtonId m_fireAxis;
    public Valve.VR.EVRButtonId m_reloadButtons;
    public Valve.VR.EVRButtonId m_grabButton;
}