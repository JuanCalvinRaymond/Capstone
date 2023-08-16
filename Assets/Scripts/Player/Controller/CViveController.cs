using UnityEngine;
using System.Collections;

using Valve.VR;
using System;

/*
Description:Controller class to support both vive controllers and the headset
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
Extra Notes: 
*/
public class CViveController : IController
{
    private const float M_CONTROLLER_TOUCHPAD_DEADZONE = 0.1f;

    private CWeaponViveControlInput m_rightWeaponControlInput;
    private CWeaponViveControlInput m_leftWeaponControlInput;

    private SteamVR_ControllerManager m_steamControllerManager;

    private SViveButtons m_viveButtons;

    private GameObject m_viveHeadsetEyeObject;
    private GameObject m_rightControllerObject;
    private GameObject m_leftControllerObject;
    private SteamVR_TrackedObject m_viveHeadsetTrackedObject;//COMMENTED FOR FUTURE USE
    private SteamVR_TrackedObject m_rightControllerTrackedObject;
    private SteamVR_TrackedObject m_leftControllerTrackedObject;

    private float m_timerLeftRumble = 0.0f;
    private ushort m_rumbleLeftStrength = 0;

    private float m_timerRightRumble = 0.0f;
    private ushort m_rumbleRightStrength = 0;

    public CWeaponControlInput PRightWeaponControl
    {
        get
        {
            return m_rightWeaponControlInput;
        }

    }

    public CWeaponControlInput PLeftWeaponControl
    {
        get
        {
            return m_leftWeaponControlInput;
        }

    }

    /*
    Description:Function to check that the vr controller index is valid
    Parameters: int aIndex-The index for the current HTC vive controller
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, November 3, 2016
    Extra Notes: 
    */
    private bool IsValidControllerIndex(int aIndex)
    {
        if (aIndex > 0 && aIndex < OpenVR.k_unMaxTrackedDeviceCount)
        {
            return true;
        }

        return false;
    }

    private SteamVR_Controller.Device PRightControllerDevice
    {
        get
        {
            if (m_rightControllerTrackedObject != null)
            {
                int controllerIndex = (int)m_rightControllerTrackedObject.index;
                if (IsValidControllerIndex(controllerIndex) == true)
                {
                    return SteamVR_Controller.Input(controllerIndex);
                }
            }

            return null;
        }
    }

    private SteamVR_Controller.Device PLeftControllerDevice
    {
        get
        {
            if (m_leftControllerTrackedObject != null)
            {
                int controllerIndex = (int)m_leftControllerTrackedObject.index;
                if (IsValidControllerIndex(controllerIndex) == true)
                {
                    return SteamVR_Controller.Input(controllerIndex);
                }
            }

            return null;
        }
    }

    public Vector3 PHeadsetForwardDirection
    {
        get
        {
            if (m_viveHeadsetEyeObject != null)
            {
                return m_viveHeadsetEyeObject.transform.forward;
            }

            return Vector3.zero;
        }
    }

    public Vector3 PHeadsetRotation
    {
        get
        {
            if (m_viveHeadsetEyeObject != null)
            {
                return m_viveHeadsetEyeObject.transform.rotation.eulerAngles;
            }

            return Vector3.zero;
        }
    }

    public Quaternion PHeadsetRotationQuaternion
    {
        get
        {
            if (m_viveHeadsetEyeObject != null)
            {
                return m_viveHeadsetEyeObject.transform.rotation;
            }

            return Quaternion.identity;
        }
    }

    public Vector3 PHeadsetPosition
    {
        get
        {
            if (m_viveHeadsetEyeObject != null)
            {
                return m_viveHeadsetEyeObject.transform.position;
            }

            return Vector3.zero;
        }
    }

    public Vector3 PRightControllerForwardDirection
    {
        get
        {
            if (m_rightControllerObject != null)
            {
                return m_rightControllerObject.transform.forward;
            }

            return Vector3.zero;
        }
    }

    public Vector3 PRightControllerRotation
    {
        get
        {
            if (m_rightControllerObject != null)
            {
                return m_rightControllerObject.transform.rotation.eulerAngles;
            }

            return Vector3.zero;
        }
    }

    public Quaternion PRightControllerRotationQuaternion
    {
        get
        {
            if (m_rightControllerObject != null)
            {
                return m_rightControllerObject.transform.rotation;
            }

            return Quaternion.identity;
        }
    }

    public Vector3 PRightControllerPosition
    {
        get
        {
            if (m_rightControllerObject != null)
            {
                return m_rightControllerObject.transform.position;
            }

            return Vector3.zero;
        }
    }

    public Vector3 PLeftControllerForwardDirection
    {
        get
        {
            if (m_leftControllerObject != null)
            {
                return m_leftControllerObject.transform.forward;
            }

            return Vector3.zero;
        }
    }

    public Vector3 PLeftControllerRotation
    {
        get
        {
            if (m_leftControllerObject != null)
            {
                return m_leftControllerObject.transform.rotation.eulerAngles;
            }

            return Vector3.zero;
        }
    }

    public Quaternion PLeftControllerRotationQuaternion
    {
        get
        {
            if (m_leftControllerObject != null)
            {
                return m_leftControllerObject.transform.rotation;
            }

            return Quaternion.identity;
        }
    }

    public Vector3 PLeftControllerPosition
    {
        get
        {
            if (m_leftControllerObject != null)
            {
                return m_leftControllerObject.transform.position;
            }

            return Vector3.zero;
        }
    }

    private SteamVR_Controller.Device PHeadsetControllerDevice
    {
        get
        {
            return SteamVR_Controller.Input((int)SteamVR_TrackedObject.EIndex.Hmd);//The VR headset always has index 0.
        }
    }

    /*
    Description: Constructor for the controller
    Parameters(Optional): SteamVR_ControllerManager aControllerManager- The SteamVR Controller manager. This has reference
                            to the left and right controllers
                          GameObject aHeadsetEyeObject- The headset (Main(eyes) in SteamVR camera rig prefab)- This is passed in order
                           to have the rotation and position of the Vive headset in game world. This also provides us with the headset 
                           tracked game objects                           
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 24, 2016
    */
    public CViveController(SteamVR_ControllerManager aControllerManager, GameObject aHeadsetEyeObject)
    {
        m_viveButtons = new SViveButtons();//Make the vive buttons for easier handling

        m_viveButtons.SetViveKeyBinding();

        m_steamControllerManager = aControllerManager;//Save the steam controller managaer

        if (m_steamControllerManager != null)//If the controller manager is valid
        {
            //Set the controller game objects and their respective tracked objects
            SetTrackedAndControllerObject(m_steamControllerManager.right, ref m_rightControllerObject, ref m_rightControllerTrackedObject);//Right controller
            SetTrackedAndControllerObject(m_steamControllerManager.left, ref m_leftControllerObject, ref m_leftControllerTrackedObject);//Left controller
            SetTrackedAndControllerObject(aHeadsetEyeObject, ref m_viveHeadsetEyeObject, ref m_viveHeadsetTrackedObject, true);//Headset

            //Create the weapon controls
            m_rightWeaponControlInput = new CWeaponViveControlInput(EWeaponHand.RightHand, PRightControllerDevice, m_viveButtons.PWeaponKeys);//Right
            m_leftWeaponControlInput = new CWeaponViveControlInput(EWeaponHand.LeftHand, PLeftControllerDevice, m_viveButtons.PWeaponKeys);//Left
        }
    }

    /*
    Description: Make the controllers vibrate each time while there timer is still on.              
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 27, 2016
    */
    public void UpdateController()
    {
        //Right Controller
        m_timerRightRumble -= Time.unscaledDeltaTime;//Decrease timer

        if (m_timerRightRumble > 0.0f)//if timer is not over
        {
            VibrateController(PRightControllerDevice, m_rumbleRightStrength);
        }

        //Left Controller
        m_timerLeftRumble -= Time.unscaledDeltaTime;

        if (m_timerLeftRumble > 0.0f)//if timer is not over
        {
            VibrateController(PLeftControllerDevice, m_rumbleLeftStrength);
        }

        //Ensure the controllers devices are udpated, because of the order in which SteamVR does things this may
        // or may not be set correctly at start. This also helps if the tracking is lost
        if (m_rightWeaponControlInput != null)
        {
            m_rightWeaponControlInput.PControllerDevice = PRightControllerDevice;

            //Update the controller
            m_rightWeaponControlInput.Update();
        }

        if (m_leftWeaponControlInput != null)
        {
            m_leftWeaponControlInput.PControllerDevice = PLeftControllerDevice;

            //Update the controller
            m_leftWeaponControlInput.Update();
        }
    }

    /*
    Description:Function to easily save a controller game object into a holder controller game object, which would be
    a class variable, and get from it  (and save) the steamVR_TrackedObject component
    Parameters:GameObject aControllerObject-The original game object that will be saved as the aHolderControllerObject
                ref GameObject aHolderControllerObject - The variable where the original controller object will be saved into.
                ref SteamVR_TrackedObject aHolderTrackedObject -The variable where the SteamVR_TrackedObject component of the controller
                                                                object will be saved to.
                bool aIsHeadset- A bool to check if the controller object we are setting is the headset. If this is true there will just 
                                be an extra check when setting the tracked object to ensure it has the correct tracked object index.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, December 22, 2016
    */
    private void SetTrackedAndControllerObject(GameObject aControllerObject, ref GameObject aHolderControllerObject,
        ref SteamVR_TrackedObject aHolderTrackedObject, bool aIsHeadset = false)
    {
        //If we have a controller object
        if (aControllerObject != null)
        {
            //Save the object in the holder controller object
            aHolderControllerObject = aControllerObject;

            //If the object is not a headet
            if (aIsHeadset == false)
            {
                //Get the steam VR Tracked Object component
                SteamVR_TrackedObject tempTrackedObject = aHolderControllerObject.GetComponent<SteamVR_TrackedObject>();

                //if it has a steamVR tracked Object component
                if (tempTrackedObject != null)
                {
                    //If we are passing a headset, check that the tracked object index belongs really to the headset
                    if (tempTrackedObject.index == SteamVR_TrackedObject.EIndex.Hmd || aIsHeadset == false)
                    {
                        //Save the tracked object
                        aHolderTrackedObject = tempTrackedObject;
                    }
                }
            }
            else
            {
                //PENDING
                //Get the steam VR Tracked Object component in the parent
                //SteamVR_TrackedObject tempTrackedObject = aHolderControllerObject.GetComponentInChildren<SteamVR_TrackedObject>();

                ////If we are passing a headset, check that the tracked object index belongs really to the headset
                //if (tempTrackedObject.index == SteamVR_TrackedObject.EIndex.Hmd)
                //{
                //    //Save the tracked object
                //    aHolderTrackedObject = tempTrackedObject;
                //}
            }
        }
    }

    /*
    Description:Function to check if a certain button is being pressed down in any of the Vive Controllers.
    Parameters:EVRButtonId aButton-The button that will be checked if it is being pressed down 
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, December 21, 2016
    */
    private bool IsButtonPressedDownAnyController(EVRButtonId aButton)
    {
        //Check right controller
        if (PRightControllerDevice != null)
        {
            //If the button is pressed in the right controller
            if (PRightControllerDevice.GetPressDown(aButton))
            {
                return true;
            }
        }

        //Check left controller
        if (PLeftControllerDevice != null)
        {
            //If the button is pressed in the left controller
            if (PLeftControllerDevice.GetPressDown(aButton))
            {
                return true;
            }
        }

        //Button is not pressed in any controller
        return false;
    }

    /*
    Description:Function to check if a certain axis is smaller than a mavimum value vertically, in any of the Vive Controllers.
    Parameters:EVRButtonId aAxis-The button that wiill be compared vertically (ONly Y value)
                float aMaxValue- The max value that the axis has to be smaller than.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, December 22, 2016
    */
    private bool IsAxisSmallerVerticallyThanMaxValueAnyController(EVRButtonId aAxis, float aMaxValue)
    {
        //Check right controller
        if (PRightControllerDevice != null)
        {
            if (PRightControllerDevice.GetAxis(aAxis).y < aMaxValue)
            {
                return true;
            }
        }

        //Check left controller
        if (PLeftControllerDevice != null)
        {
            if (PLeftControllerDevice.GetAxis(aAxis).y < aMaxValue)
            {
                return true;
            }
        }

        return false;
    }

    /*
    Description:Function to check if a certain axis is bigger than a minimum value vertically, in any of the Vive Controllers.
    Parameters:EVRButtonId aAxis-The button that wiill be compared vertically (ONly Y value)
                float aMinValue- The minimum value that the axis has to be bigger than.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, December 21, 2016
    */
    private bool IsAxisBiggerVerticallyThanMinValueAnyController(EVRButtonId aAxis, float aMinValue)
    {
        //Check right controller
        if (PRightControllerDevice != null)
        {
            if (PRightControllerDevice.GetAxis(aAxis).y > aMinValue)
            {
                return true;
            }
        }

        //Check left controller
        if (PLeftControllerDevice != null)
        {
            if (PLeftControllerDevice.GetAxis(aAxis).y > aMinValue)
            {
                return true;
            }
        }

        //Button is not pressed in any controller
        return false;
    }

    /*
    Description:Not used in VR, get the headset position instead.
    Parameters:SteamVR_Controller.Device aDevice - The steam vr device to check for input.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private bool CheckAnyKeyPressedDevice(SteamVR_Controller.Device aDevice)
    {
        bool keyPressed = false;

        // If the device is valid
        if (aDevice != null)
        {
            // Go through all the HTC Vive controller buttons
            for (int i = 0; i < (int)EVRButtonId.k_EButton_Max; i++)
            {
                // Check if they are pressed
                keyPressed = aDevice.GetPress((EVRButtonId)(i));

                // If any button was pressed
                if (keyPressed == true)
                {
                    // Return true, a button was pressed
                    return keyPressed;
                }
            }
        }

        //Return false, no button was pressed
        return keyPressed;
    }

    /*
    Description:Not used in VR, get the headset position instead.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 24, 2016
    */
    public Vector3 GetMoveInput()
    {
        return Vector3.zero;
    }

    /*
    Description:Get the headset rotation instead of actual look inpt
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 24, 2016
    */
    public Vector3 GetLookInput()
    {
        return PHeadsetForwardDirection;
    }

    /*
    Description:it returns whether the user is pressing the pause button or not in any of the controllers
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 27, 2016
    */
    public bool GetPause()
    {
        return IsButtonPressedDownAnyController(m_viveButtons.PMenuButton);
    }

    /*
    Description:It returns whether the user is pressing the accept down button or not in any of the controllers
    Parameters: bool aIgnoreMovementKeys - Not used since no keys are used for movement in VR.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    public bool GetAnyKeyPressed()
    {
        //Returns if any key was pressed in the right or left controller device. as pressed
        return CheckAnyKeyPressedDevice(PRightControllerDevice) || CheckAnyKeyPressedDevice(PLeftControllerDevice);
    }

    /*
    Description:Returns if the user want to skip any actions. Since the movement is done without button presss
                this function merely checks if a button has been pressed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 30th, 2017
    */
    public bool GetInterruptKeyPressed()
    {
        return GetAnyKeyPressed();
    }


    /*
    Description:If the right controller device is valid, it makes the controller rumble according to the passed in value
    Parameters(Optional): ushort aStrength- The duration in micro seconds of the rumble
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 25, 2016
    */
    public void SetRumbleController(float aDuration, ushort aStrength, EWeaponHand aHand = EWeaponHand.RightHand)
    {
        switch (aHand)
        {
            case EWeaponHand.None:
                break;
            case EWeaponHand.RightHand:
                //Set variables for  right rumble
                m_timerRightRumble = aDuration;
                m_rumbleRightStrength = aStrength;
                break;
            case EWeaponHand.LeftHand:
                //Set variables for  left rumble
                m_timerLeftRumble = aDuration;
                m_rumbleLeftStrength = aStrength;
                break;
            case EWeaponHand.BothHands:
                //Set variables for right and  left rumble
                SetRumbleController(aDuration, aStrength, EWeaponHand.RightHand);
                SetRumbleController(aDuration, aStrength, EWeaponHand.LeftHand);
                break;
            default:
                break;
        }
    }

    /*
    Description:If the controller device is valid, it makes the controller rumble according to the passed in value
    Parameters(Optional):SteamVR_Controller.Device aControllerDevice- The controller device that will vibrate
                        ushort aStrength- The duration in micro seconds of the rumble
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 25, 2016
    */
    public void VibrateController(SteamVR_Controller.Device aControllerDevice, ushort aStrength)
    {
        if (aControllerDevice != null)
        {
            aControllerDevice.TriggerHapticPulse(aStrength);
        }
    }

    /*
     Description:Empty function, implemented only for mouse/keyboard and gamepad
     Parameters(Optional): float aInputSensitivity
     Creator: Alvaro Chavez Mixco
     */
    public void SetInputSensitivity(float aInputSensitivity)
    {
    }

    /*
     Description:Empty function, implemented only for mouse/keyboard and gamepad
     Parameters(Optional): bool aIsInverted
     Creator: Alvaro Chavez Mixco
     */
    public void SetInvertedYAxis(bool aIsInverted)
    {
    }
}