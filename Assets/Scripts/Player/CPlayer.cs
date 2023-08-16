using UnityEngine;
using System.Collections;

using Valve.VR;

/*
Description: Class to read input from the player controller, and handle the player movement and limiting/scaling.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
*/
public class CPlayer : MonoBehaviour
{
    private IController m_controller;

    private Vector3 m_currentMoveDirection;

    private float m_verticalHeadRotation;

    private Vector2 m_playAreaSize = new Vector2();

    [Header("Head Objects")]
    public GameObject m_playerHeadNonVR;//Game object that decides the camera position and rotation for non vr.
    public GameObject m_headsetEyeObject;

    [Header("Controllers")]
    public EControllerTypes m_controllerType = EControllerTypes.MouseAndKeyboardController;
    [Tooltip("This should normally be set to false. Since the Game Manager, through the Player Creator will make them.")]
    public bool m_createControllerAtStart = false;//In case another object creates the player controllers

    [Header("Non-VR Movement")]
    public float m_moveSpeed = 40.0f;
    public float m_horizontalRotationTurnSpeedNonVR = 5.0f;
    public float m_headVerticalRotationEaseSpeedNonVR = 10.0f;
    private bool m_headLimitVerticalRotationNonVR = true;
    [Range(0, 90)]
    public float m_downLimitVerticalAngleNonVR = 90;//Positive values
    [Range(-90, 0)]
    public float m_upLimitVerticalAngleNonVR = -90;//Negative values

    [Header("Limiting/Scaling Settings")]
    [Tooltip("The size of the platform used for limiting and scaling the player movement")]
    public Vector2 m_platformMaxSize = new Vector2(4.0f, 4.0f);
    public bool m_isLimitingNonVRMovementArea = true;
    public bool m_isScalingVRMovement = true;
    public bool m_allowPausedNonVRMovement = true;

    public EControllerTypes PControllerType
    {
        get
        {
            return m_controllerType;
        }
    }

    public IController PController
    {
        get
        {
            return m_controller;
        }

        set
        {
            m_controller = value;
        }
    }


    public bool PIsScalingVRMovement
    {
        get
        {
            return m_isScalingVRMovement;
        }
        set
        {
            m_isScalingVRMovement = value;
        }
    }

    /*
    Description:Awake is called when the object is initiliazed. This function creates the players controllers, if needed.
    that will be used by the player.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void Awake()
    {
        //Development tool to test 
        if (m_createControllerAtStart == true)//If we want to create the controllers, at start (and not use a player creator)
        {
                //Create the controller
                CreateController();
        }
    }

    /*
    Description:Check for any input the player may do.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    Extra Notes: 
    */
    private void Update()
    {
        if (m_controller != null)//If controller is valid 
        {
            m_currentMoveDirection = m_controller.GetMoveInput();//Check movement keys
            m_currentMoveDirection = Vector3.Normalize(m_currentMoveDirection);//Just to be safe normalize the direction

            m_controller.UpdateController();

            if (m_controllerType != EControllerTypes.ViveController)//if the controller is not the vive
            {
                UpdateMovementNonVR();//Update the player movement
                UpdateRotationNonVR();//Update the player rotation
            }
            else
            {
                UpdateMovementVR();//Update VR Movement   
            }

            //If the player pauses the game and there is an instance manager
            if (m_controller.GetPause() == true && CGameManager.PInstanceGameManager != null)
            {
                //If player is in the PlayingState
                if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
                {
                    //Pause the game
                    CGameManager.PInstanceGameManager.PGameState = EGameStates.Paused;
                }
                //If player is in the MenuState
                else if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Paused)
                {
                    //Unpause the game
                    CGameManager.PInstanceGameManager.PGameState = EGameStates.Play;
                }
            }
        }
    }

    /*
    Description:Move the player by getting his move direction according to input, and multiplying it by a speed. Then rotate the move direction so that it is relative
    to the player rotation. If necessary, we will also check that the player is within the specified boundaries.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void UpdateMovementNonVR()
    {
        Vector3 moveAmount = Vector3.zero;
        float deltaTime = Time.unscaledDeltaTime;//Set initially as the unscaled delta time

        //If the game doesn't allow movement when paused
        if (m_allowPausedNonVRMovement == false)
        {
            //Use the scale delta time, that when the game is paused will be 0.
            deltaTime = CGameManager.PInstanceGameManager.GetScaledDeltaTime();
        }

        if (m_currentMoveDirection.sqrMagnitude > Mathf.Epsilon)//If there is input to move
        {
            //Horizontal movement
            moveAmount.x += m_currentMoveDirection.x * (m_moveSpeed * deltaTime);//Movement in X direction
            moveAmount.z += m_currentMoveDirection.z * (m_moveSpeed * deltaTime);//Movemetn in Z direction
        }

        //Rotate the move amount to be relative  to the player rotation
        Vector3 rotatedMoveAmount = transform.localRotation * moveAmount;

        //If we want to limit the player movement to a certain area
        if (m_isLimitingNonVRMovementArea == true)
        {
            Vector2 halfMoveArea = m_platformMaxSize / 2.0f;

            //If it can no longer move in X axis       
            if (transform.localPosition.x + rotatedMoveAmount.x > halfMoveArea.x
                || transform.localPosition.x + rotatedMoveAmount.x < -halfMoveArea.x)
            {
                //Set move amount in X to 0
                rotatedMoveAmount.x = 0.0f;
            }

            //If it can no longer move in Z axis
            if (transform.localPosition.z + rotatedMoveAmount.z > halfMoveArea.y
                || transform.localPosition.z + rotatedMoveAmount.z < -halfMoveArea.y)
            {
                //Set move amount in Z to 0
                rotatedMoveAmount.z = 0.0f;
            }
        }

        //Move the player
        transform.localPosition += rotatedMoveAmount;
    }

    /*
    Description:Update the rotation of the player according to the look input
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void UpdateRotationNonVR()
    {
        //Horizontal rotation only for the body
        if (m_controller != null)//If the controller is valid
        {
            Vector3 lookInput = m_controller.GetLookInput();//Get the look input from the controller

            //Horizontal rotation
            float horizontalRotationPerFrame = lookInput.x * (m_horizontalRotationTurnSpeedNonVR * Time.unscaledDeltaTime);//Calculate how much the player is rotating per frame

            Quaternion horizontalRotation = Quaternion.Euler(0.0f, horizontalRotationPerFrame, 0.0f);//Create a new rotation that only stores the rotation change per frame

            //Rotate the player, and its children, horizontally
            transform.rotation *= horizontalRotation;

            //Rotate the gun and head vertically
            //Vertical rotation

            float verticalRotationPerFrame = lookInput.y * (m_headVerticalRotationEaseSpeedNonVR * Time.unscaledDeltaTime);//Get the vertical rotation this frame

            m_verticalHeadRotation += verticalRotationPerFrame;//Increase the total vertical head rotation

            if (m_headLimitVerticalRotationNonVR == true)
            {
                m_verticalHeadRotation = Mathf.Clamp(m_verticalHeadRotation, m_upLimitVerticalAngleNonVR, m_downLimitVerticalAngleNonVR);//Limit the rotation angle
            }

            //Calculate the player head rotation
            Quaternion playerHeadRotation = new Quaternion();
            playerHeadRotation = Quaternion.Euler(m_verticalHeadRotation, horizontalRotation.x, 0.0f);

            if (m_playerHeadNonVR != null)//If the head is valid
            {
                m_playerHeadNonVR.transform.localRotation = playerHeadRotation;//Rotate head
            }
        }
    }

    /*
    Description:Function to dynamically scale the player movement in the play area, in that way regardless of the size of his play area,
    he would be able to travel through the entire platform in the game.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, October 27th, 2016
    */
    private void UpdateMovementVR()
    {
        if (m_isScalingVRMovement == true && m_headsetEyeObject != null)//If we are scaling VR movement and we have a headset gameobject
        {
            if (OpenVR.Chaperone != null)//If OpenVR has been intialised
            {
                //Player Position Play Area/Player Area Size = Player Position Platform / Platform Size
                //Player Position Play Area/Player Area Size = Play Area Position / Max Play Area Size
                //Play Area Position = PlayerPositionPlayArea * Max Play Area Size /Player Area Size

                OpenVR.Chaperone.GetPlayAreaSize(ref m_playAreaSize.x, ref m_playAreaSize.y);//Get the size of the play area

                Vector2 maxMovablePlayAreaDistance = new Vector2(m_platformMaxSize.x - m_playAreaSize.x, m_platformMaxSize.y - m_playAreaSize.y);//Calculate the size in which we can move
                //the play area, without having the play area stick out of the platform

                Vector2 playerPositionInPlayArea = new Vector2(m_headsetEyeObject.transform.localPosition.x, m_headsetEyeObject.transform.localPosition.z);//Get the player position in the play area

                //Get the position that the center of the play area
                Vector3 playAreaPosition = new Vector3();
                playAreaPosition.x = playerPositionInPlayArea.x * maxMovablePlayAreaDistance.x / m_playAreaSize.x;
                playAreaPosition.z = playerPositionInPlayArea.y * maxMovablePlayAreaDistance.y / m_playAreaSize.y;

                //Check the play area position is valid. It may be unvalid if the headset is plugged in, but the 
                //base stations are not working
                if (float.IsNaN(playAreaPosition.x) == false && float.IsNaN(playAreaPosition.y) == false
                    && float.IsPositiveInfinity(playAreaPosition.y) == false && float.IsPositiveInfinity(playAreaPosition.x) == false
                    && float.IsNegativeInfinity(playAreaPosition.y) == false && float.IsNegativeInfinity(playAreaPosition.x) == false)

                {
                    transform.localPosition = new Vector3(playAreaPosition.x, transform.localPosition.y, playAreaPosition.z);//Move the play area center position in local space
                }
            }
        }
    }

    /*
    Description:Function to get whatever direction the player is facing. This will be used for shooting tricks detection. So it would actually return the direction of the game camera.
    In VR it would return the rotation of the headset, in nonvr it would return the rotation of the player head gameobject
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 25th, 2016
    */
    public Vector3 GetPlayerLookingDirection()
    {
        if (m_controller != null)//If there is a controller
        {
            if (m_controllerType == EControllerTypes.ViveController)//If we are using the vive controller
            {
                return m_controller.GetLookInput();
            }
            else
            {
                if (m_playerHeadNonVR != null)//if we have a head game object
                {
                    return m_playerHeadNonVR.transform.forward;//Return the head rotation (matches the camera rotation)
                }
            }

        }

        return transform.forward;//If we have no controller, return the player forward direction
    }

    /*
    Description:According to the starting controller type, create the correct controller class and assign it to the player controller
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    Extra Notes:
    */
    public void CreateController()
    {
        switch (m_controllerType)//According to the starting controller type set in inspector
        {
            case EControllerTypes.ViveController://Vive Controller
                m_controller = new CViveController(gameObject.GetComponent<SteamVR_ControllerManager>(), m_headsetEyeObject);
                break;
            case EControllerTypes.GamepadController://Gamepad Controller
                m_controller = new CGamepadController();
                break;
            case EControllerTypes.MouseAndKeyboardController://Mouse and Keyboard Controller
                m_controller = new CMouseAndKeyboardController();
                break;
            default:
                break;
        }
    }

}