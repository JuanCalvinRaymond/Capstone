using UnityEngine;

using Valve.VR;

/*
Description:Class to be added to an empty gameobject at the start of the game. This class, using the given prefabs and pathway, will create the correct playe prefab
according if the player is using VR, Gamepad, or mouse and keyboard. If it is a non vr player it would also create the camera and set it to follow the player.
This class also sets the pathway that the player will use.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, October 24, 2016
*/
public class CPlayerCreator : MonoBehaviour
{
    private GameObject m_createdPlayer;
    private CPlayer m_createdPlayerScript;
    private CPlayerWeaponHandler m_createdPlayerWeaponHandler;
    private CMovingPlatformAnimation m_createdPlatformScript;
    private bool m_isNonVRPlayer;

    [Tooltip("The level in which the player will be created. This is used to determine which animation will the platform have")]
    public ELevelState m_startingLevel;

    [Tooltip("Offset when instantiating player during practice level")]
    public Vector3 m_practiceLevelSpawnOffset;

    [Header("Playes Prefab")]
    public GameObject m_VRPlayer;
    public GameObject m_nonVRPlayer;
    public GameObject m_nonVRCamera;

    [Space(20)]
    [Tooltip("Marks if the player to be created with the weapons stored in the setting storer")]
    public bool m_useSettingStorerStartingWeapons = true;
    [Tooltip("If the player is not using the setting storer starting weapons, the player weapons will be saved as the new setting storer weapons.")]
    public bool m_overwriteSettingStorerStartingWeapons = false;

    public GameObject PCreatedPlayerObject
    {
        get
        {
            return m_createdPlayer;
        }
    }

    public CPlayer PCreatedPlayerScript
    {
        get
        {
            return m_createdPlayerScript;
        }
    }

    public CPlayerWeaponHandler PCreatedPlayerWeaponHandler
    {
        get
        {
            return m_createdPlayerWeaponHandler;
        }
    }

    public CMovingPlatformAnimation PMovingPlatformScript
    {
        get
        {
            return m_createdPlatformScript;
        }
    }

    public ELevelState PPlayerLevel
    {
        get
        {
            return m_startingLevel;
        }
    }

    /*
    Description:This function is called when this object is initialised. This function create the 
    appropiate player prefab, with its controller, according to the data. It also sets the
    platform path for VR , and nonVR players.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 24, 2016
    */
    private void Awake()
    {
        //Create the palyer objects
        CreatePlayer();

        //If a player was created
        if (m_createdPlayer != null)
        {
            //Save the platform script
            m_createdPlatformScript = m_createdPlayer.GetComponent<CMovingPlatformAnimation>();

            //Save the weapon handler script
            m_createdPlayerWeaponHandler = m_createdPlayer.GetComponentInChildren<CPlayerWeaponHandler>();

            //If we want to use the setting storer starting weapons, and not the ones in the player prefab
            if (m_useSettingStorerStartingWeapons == true && CSettingsStorer.PInstanceSettingsStorer != null)
            {
                //Set the starting weapons for the player
                SetPlayerStartingWeapons(CSettingsStorer.PInstanceSettingsStorer.PStartingRightWeapon,
                    CSettingsStorer.PInstanceSettingsStorer.PStartingLeftWeapon);

            }
            else if (m_overwriteSettingStorerStartingWeapons == true 
                && CSettingsStorer.PInstanceSettingsStorer != null)//If there is a setting storer and we want to overwrite its values
            {
                CSettingsStorer.PInstanceSettingsStorer.PStartingRightWeapon = m_createdPlayerWeaponHandler.m_startingRightWeapon;
                CSettingsStorer.PInstanceSettingsStorer.PStartingLeftWeapon = m_createdPlayerWeaponHandler.m_startingLeftWeapon;
            }
        }
    }

    /*
    Description:This function is called when this object is activated. This creates and sets the camera for nonVR player; 
    appropiate player prefab, with its controller, according to the data.  The function also sets the animation that will be
    used by the moving platform.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 25, 2016
    */
    private void Start()
    {
        //Set the camera for non vr players
        if (m_nonVRCamera != null && m_isNonVRPlayer == true)//If there is a non vr camera prefab and the player is non vr
        {
            GameObject camera = (GameObject)Instantiate(m_nonVRCamera, Vector3.zero, Quaternion.identity);//Create the camera prefab

            CBasicCamera cameraScript = camera.GetComponent<CBasicCamera>();//Get the basic camera component script

            if (m_createdPlayerScript != null && cameraScript != null)//If none of them is null
            {
                cameraScript.m_playerHeadGameObject = m_createdPlayerScript.m_playerHeadNonVR; //Set the camera to follow the players head
            }
        }

        //If the platform script was created
        if (m_createdPlatformScript != null)
        {
            //Set the animation for it
            m_createdPlatformScript.SetAnimation(m_startingLevel);
        }
    }

    /*
    Description: Creates the player objects to be used in the game.                          
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void CreatePlayer()
    {
        //Initial values
        m_createdPlayer = null;
        m_isNonVRPlayer = true;

        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Get which type of input is being used from the setting storer
            //If it is a vive controller
            if (CSettingsStorer.PInstanceSettingsStorer.PInputMethod == EControllerTypes.ViveController && m_VRPlayer != null)
            {
                m_createdPlayer = (GameObject)Instantiate(m_VRPlayer, m_VRPlayer.transform.position + (m_startingLevel != ELevelState.Practice ? Vector3.zero : m_practiceLevelSpawnOffset), m_VRPlayer.transform.rotation);//Create the VR player
                CreatePlayerControls(m_createdPlayer, EControllerTypes.ViveController);//Create the vive controller
                m_isNonVRPlayer = false;//Set that this is a VR player
            }
            //If it is a gamepad controller
            else if (CSettingsStorer.PInstanceSettingsStorer.PInputMethod == EControllerTypes.GamepadController && m_nonVRPlayer != null)
            {
                m_createdPlayer = (GameObject)Instantiate(m_nonVRPlayer, m_nonVRPlayer.transform.position + (m_startingLevel != ELevelState.Practice ? Vector3.zero : m_practiceLevelSpawnOffset), m_nonVRPlayer.transform.rotation);//Create nonvr player
                CreatePlayerControls(m_createdPlayer, EControllerTypes.GamepadController);//Create gamepad controlle
            }
        }

        //If the player hasn't been created (the player is using a mouse and keyboard, or there is no setting storer)
        if (m_createdPlayer == null && m_nonVRPlayer != null)
        {
            m_createdPlayer = (GameObject)Instantiate(m_nonVRPlayer, m_nonVRPlayer.transform.position + (m_startingLevel != ELevelState.Practice ? Vector3.zero : m_practiceLevelSpawnOffset), m_nonVRPlayer.transform.rotation);//Create nonvr player
            CreatePlayerControls(m_createdPlayer, EControllerTypes.MouseAndKeyboardController);//Create mouse and keyboard controllerr
        }
    }

    /*
    Description: Creates the specified type of controls for the player being passed.
    Parameters: GameObject aPlayer- The gameobject that contains the player script
                EControllerTypes aControlType- The type of controller we want the player to have                            
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 24, 2016
    */
    private void CreatePlayerControls(GameObject aPlayer, EControllerTypes aControlType)
    {
        if (aPlayer != null)//if the player gameobject is valid
        {
            m_createdPlayerScript = aPlayer.GetComponentInChildren<CPlayer>();//Get the player script

            if (m_createdPlayerScript != null)//If it has a valid player script
            {
                m_createdPlayerScript.m_controllerType = aControlType;//Set the controller type
                m_createdPlayerScript.CreateController();//Create the controller
            }
        }
    }

    /*
    Description: Set player initial weapons
    Parameters: GameObject aPlayer- The gameobject that contains the player script                                            
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 24, 2016
    */
    private void SetPlayerStartingWeapons(GameObject aRightWeapon, GameObject aLeftWeapon)
    {
        //If the player gameobject is valid
        if (m_createdPlayerWeaponHandler != null)
        {
            //set the weapons for both of his hands
            m_createdPlayerWeaponHandler.m_startingRightWeapon = aRightWeapon;
            m_createdPlayerWeaponHandler.m_startingLeftWeapon = aLeftWeapon;
        }
    }
}
