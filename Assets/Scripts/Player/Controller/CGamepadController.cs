using UnityEngine;
using System.Collections;
using System;

/*
Description:Class to get the input from a gamepad controller
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
Extra Notes:
*/
public class CGamepadController : IController
{
    private CWeaponControlInput m_rightWeaponControl;
    private CWeaponControlInput m_leftWeaponControl;

    private SControlKeys m_gamepadControlKeys;

    private float m_inputSensitivity = 1.0f;
    private float m_invertYAxisValue = 1.0f;//A simple float that will have a value of 1 or -1 depending
                                            //on whether or not the YAxis is inverted or not. This is stored as a different variable in
                                            //order to avoid "if" statements. Since all the Y input will be multiplied by this value.

    public CWeaponControlInput PRightWeaponControl
    {
        get
        {
            return m_rightWeaponControl;
        }
    }

    public CWeaponControlInput PLeftWeaponControl
    {
        get
        {
            return m_leftWeaponControl;
        }
    }

    /*
    Description:Create a Scontrol keys struct and set it to the default gamepad input strings
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    public CGamepadController()
    {
        //Set the default keys that will be used for the game
        m_gamepadControlKeys = new SControlKeys();
        m_gamepadControlKeys.SetDefaultGamepadInputStrings();

        //Create the weapon controsl for right and left weapons
        m_rightWeaponControl = new CWeaponAxisControlInput(EWeaponHand.RightHand, m_gamepadControlKeys.PRightWeaponKeys);
        m_leftWeaponControl = new CWeaponAxisControlInput(EWeaponHand.LeftHand, m_gamepadControlKeys.PLeftWeaponKeys);

        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Suscribe to the input sensitivty change event
            CSettingsStorer.PInstanceSettingsStorer.OnInputSensitivityChange += SetInputSensitivity;

            //Suscribe to the inverted Y axis change event
            CSettingsStorer.PInstanceSettingsStorer.OnInvertedYAxisChange += SetInvertedYAxis;

            //Set the initial values according to settings
            SetInputSensitivity(CSettingsStorer.PInstanceSettingsStorer.PInputSensitivity);
            SetInvertedYAxis(CSettingsStorer.PInstanceSettingsStorer.PIsInvertedYAxis);
        }
    }

    /*
    Description: Unsuscribe from the setting events
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, November 22, 2016
    */
    private void OnDestroy()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe to the input sensitivty change event
            CSettingsStorer.PInstanceSettingsStorer.OnInputSensitivityChange -= SetInputSensitivity;

            //Unsuscribe to the inverted Y axis change event
            CSettingsStorer.PInstanceSettingsStorer.OnInvertedYAxisChange -= SetInvertedYAxis;
        }
    }

    /*
    Description: Update the weapon controls.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2017
    */
    public void UpdateController()
    {
        m_rightWeaponControl.Update();
        m_leftWeaponControl.Update();
    }

    /*
    Description:Return the move input from the gamepad axis string
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    public Vector3 GetMoveInput()
    {
        return new Vector3(Input.GetAxis(m_gamepadControlKeys.PHorizontalMovementAxis),
        0.0f,
        Input.GetAxis(m_gamepadControlKeys.PVerticalMovementAxis));
    }

    /*
    Description:Return the look input from the gamepad axis string
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    public Vector3 GetLookInput()
    {
        return new Vector3(Input.GetAxis(m_gamepadControlKeys.PHorizontalLookAxis),
            Input.GetAxis(m_gamepadControlKeys.PVerticalLookAxis) * m_invertYAxisValue,//Invert the Y value if necessary
            0.0f)
            * m_inputSensitivity;//Consider the input sensitivity
    }

    /*
    Description:Returns if the user pressed the pause button
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 27th, 2016
    */
    public bool GetPause()
    {
        return Input.GetButtonDown(m_gamepadControlKeys.PPause);
    }

    /*
    Description:Returns if the user pressed any key
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 18th, 2018
    */
    public bool GetAnyKeyPressed()
    {
        return Input.anyKeyDown;
    }

    /*
    Description:Returns if the user want to skip any actions, such as credits, etc. This check if the use
                pressing the escape key or any of the firing weapons.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 30th, 2017
    */
    public bool GetInterruptKeyPressed()
    {
        return GetPause() || m_leftWeaponControl.GetIsFiringWeapon(true) || m_rightWeaponControl.GetIsFiringWeapon(true);
    }

    /*
    Description: Empty function, no rumble in mouse and keyboard
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 10th, 2016
    */
    public void SetRumbleController(float aDuration, ushort aStrength, EWeaponHand aHand = EWeaponHand.RightHand)
    {
    }

    /*
    Description: Function to set the input sensitivty variable
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, November 21, 2016
    Extra Notes: This function is suscribed to the OnInputSensitivity event from the setting storer
    */
    public void SetInputSensitivity(float aInputSensitivity)
    {
        //Set the value
        m_inputSensitivity = aInputSensitivity;
    }

    /*
    Description: Function to set the  inverted axis variable, it will be 1 if it nos inverted
    and -1 if it is inverted
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, November 21, 2016
    Extra Notes: This function is suscribed to the OnInvertedYAxis event from the setting storer
    */
    public void SetInvertedYAxis(bool aInvertedYAxis)
    {
        //If the axis is not inverted
        if (aInvertedYAxis == false)
        {
            m_invertYAxisValue = 1.0f;//Set the invert value as 1
        }
        else //If the axis is inverted
        {
            m_invertYAxisValue = -1.0f;//Set the value to -1
        }
    }
}
