using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
Description: Singleton class to manage the game, it also provides access to multiple common elements such as the player and the camera
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, October 29, 2016
Extra Notes:This class is a singleton.In the script execution order this class is called after the SteamVRRender and the playerCreator, but before the default.
*/
public class CGameManager : MonoBehaviour
{
    //Singleton instance
    private static CGameManager s_instanceGameManager;

    //Time scale of the game
    private float m_timeScale = 1.0f;

    //Current level
    private ELevelState m_levelState;

    //Game systems
    private CPlayerCreator m_playerCreator;
    private CScoringSystem m_scoringSystem;
    private CTrickPerformTracker m_trickPerformTracker;
    private C3DTextPooling m_3DTextPooling;
    private COnFireSystem m_onFireSystem;
    private CLeaderboardHandler[] m_leaderboards;//This is an array so at the leaderboard scene we can easily switch between them

    //Player properties
    private GameObject m_playerObject;
    private CPlayer m_playerScript;
    private CPlayerWeaponHandler m_playerWeaponHandler;
    private IController m_playerController;
    private CMovingPlatformAnimation m_movingPlatformScript;

    //Camera properties
    private GameObject m_mainCameraGameObject;
    private Camera m_mainCamera;

    //Game state, I make it public because we need to set it in the inspector
    public EGameStates m_gameState;

    [Tooltip("Whatever ground level is, in meters.")]
    public float m_shaderGroundHeight = 0.0f;

    public bool m_lockCursor = true;//Set as a variable for easier testing

    [Header("Leaderboard Valid Weapons")]
    [Tooltip("The weapons being used by the player that can be written to the competitive leaderboard.")]
    public List<EWeaponTypes> m_validRankingRightWeapons;
    [Tooltip("The weapons being used by the player that can be written to the competitive leaderboard.")]
    public List<EWeaponTypes> m_validRankingLeftWeapon;

    //Game State delegates
    public delegate void delegateGameStateChange();
    public delegate void delegateGameStateUpdated(EGameStates aNewState);

    //Game state events
    public event delegateGameStateChange OnPauseState;
    public event delegateGameStateChange OnEndGameState;
    public event delegateGameStateChange OnPlayState;
    public event delegateGameStateChange OnMainMenuState;

    public event delegateGameStateUpdated OnGameStateChange;

    public static CGameManager PInstanceGameManager
    {
        get
        {
            return s_instanceGameManager;
        }
    }

    public float PTimeScale
    {
        get
        {
            return m_timeScale;
        }

        private set
        {
            m_timeScale = value;
        }

    }

    public ELevelState PLevelState
    {
        get
        {
            return m_levelState;
        }
    }

    public EGameStates PGameState
    {
        get
        {
            return m_gameState;
        }

        set
        {
            //Set the value of the game states
            m_gameState = value;

            //According to the value of gmae state call the corresponding function
            switch (m_gameState)
            {
                case EGameStates.MainMenu:
                    PrepateMainMenuState();
                    break;
                case EGameStates.Play:
                    PreparePlayState();
                    break;
                case EGameStates.Paused:
                    PreparePauseState();
                    break;
                case EGameStates.EndGame:
                    PrepareEndGameState();
                    break;
                default:
                    break;
            }

            //Call the game state change event
            if (OnGameStateChange != null)
            {
                OnGameStateChange(m_gameState);
            }
        }
    }

    public CScoringSystem PScoringSystem
    {
        get
        {
            return m_scoringSystem;
        }
    }

    public CTrickPerformTracker PTrickPerformTracker
    {
        get
        {
            return m_trickPerformTracker;
        }
    }

    public List<TextMesh> PListOfInactive3DText
    {
        get
        {
            return m_3DTextPooling.PInactiveList;
        }
    }

    public COnFireSystem POnFireSystem
    {
        get
        {
            return m_onFireSystem;
        }
    }

    public GameObject PPlayerObject
    {
        get
        {
            return m_playerObject;
        }
    }

    public CPlayer PPlayerScript
    {
        get
        {
            return m_playerScript;
        }
    }

    public CPlayerWeaponHandler PPlayerWeaponHandler
    {
        get
        {
            return m_playerWeaponHandler;
        }
    }

    public CMovingPlatformAnimation PMovingPlatform
    {
        get
        {
            return m_movingPlatformScript;
        }
    }

    public IController PPlayerController
    {
        get
        {
            return m_playerController;
        }
    }

    public GameObject PMainCameraGameObject
    {
        get
        {
            return m_mainCameraGameObject;
        }
    }

    public Camera PMainCamera
    {
        get
        {
            return m_mainCamera;
        }
    }

    public float GetScaledDeltaTime()
    {
        return Time.deltaTime * PTimeScale;
    }

    /*
    Description: Ensures that there is only 1 instance of this class.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, October 29, 2016
    Extra Notes:In the script execution order this class is called after the SteamVRRender and the playerCreator, but before the default.
    */
    private void Awake()
    {
        //Get the game systems that are in this game object
        m_playerCreator = GetComponent<CPlayerCreator>();
        m_scoringSystem = GetComponentInChildren<CScoringSystem>();
        m_trickPerformTracker = GetComponentInChildren<CTrickPerformTracker>();
        m_3DTextPooling = GetComponentInChildren<C3DTextPooling>();
        m_onFireSystem = GetComponent<COnFireSystem>();
        m_leaderboards = GetComponentsInChildren<CLeaderboardHandler>();

        //If the instance doesn't exist
        if (s_instanceGameManager == null)
        {
            //Set this as the instance
            s_instanceGameManager = this;
        }
        else//If the instance already exists
        {
            //Destroy this object
            Destroy(gameObject);
        }

        //If true lock the cursor when this is set
        if (m_lockCursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;//Lock cursor to be inside the game screen
        }

        //If there is a player creator from it
        if (m_playerCreator != null)
        {
            //Get the player values from it
            m_playerObject = m_playerCreator.PCreatedPlayerObject;
            m_playerScript = m_playerCreator.PCreatedPlayerScript;
            m_playerWeaponHandler = m_playerCreator.PCreatedPlayerWeaponHandler;
            m_movingPlatformScript = m_playerCreator.PMovingPlatformScript;
            m_levelState = m_playerCreator.PPlayerLevel;
        }
        else//If the player wasn't made with a player creator
        {
            //Find the game object of type player
            m_playerObject = GameObject.FindGameObjectWithTag(CGlobalTags.M_TAG_PLAYER);

            if (m_playerObject != null)
            {
                m_playerScript = m_playerObject.GetComponent<CPlayer>();
                m_playerWeaponHandler = m_playerObject.GetComponent<CPlayerWeaponHandler>();
                m_movingPlatformScript = m_playerObject.GetComponentInParent<CMovingPlatformAnimation>();
            }
        }

        //Set initial game state
        PGameState = m_gameState;

        //DEBUGLIST-AAA
        //Application.targetFrameRate = 90;
    }

    /*
    Description: Saves the data of the player, player script, scoring system , and  camera
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, October 29, 2016
    Extra Notes:In the script execution order this class is called after the SteamVRRender and the playerCreator, but before the default.
    */
    private void Start()
    {
        //If the player script was found set its properties
        if (m_playerScript != null)
        {
            m_playerController = m_playerScript.PController;
        }

        //Get the main camera
        m_mainCameraGameObject = GameObject.FindGameObjectWithTag(CGlobalTags.M_TAG_MAIN_CAMERA);
        if (m_mainCameraGameObject != null)
        {
            m_mainCamera = m_mainCameraGameObject.GetComponentInChildren<Camera>();
        }

        // Set the ground height in all stylistic lighting shaders.
        Shader.SetGlobalFloat("u_groundHeight", m_shaderGroundHeight);
    }

    /*
    Description: Lock the mouse cursor if desired.
    Creator: Alvaro Chavez Mixco
    */
    private void Update()
    {
        //Set the cursor lock mode
        //If true lock the cursor when this is set
        if (m_lockCursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;//Lock cursor to be inside the game screen
        }
    }

    /*
    Description: Function that will be called by player when player hit pause button
    Creator: Juan Calvin Raymond
    Creation Date: 12 Dec 2016
    */
    private void PreparePauseState()
    {
        //Set timescale to 0
        PTimeScale = 0.0f;

        //Call OnPauseState event
        if (OnPauseState != null)
        {
            OnPauseState();
        }
    }

    /*
    Description: Function that will be called by player when player hit resume button
    Creator: Juan Calvin Raymond
    Creation Date: 12 Dec 2016
    */
    private void PreparePlayState()
    {
        //Set timescale to 1
        PTimeScale = 1.0f;

        //Call OnPlayState event
        if (OnPlayState != null)
        {
            OnPlayState();
        }
    }

    /*
    Description: Function that will be called by platform when it reach the end of animation
    Creator: Juan Calvin Raymond
    Creation Date: 12 Dec 2016
    */
    private void PrepareEndGameState()
    {
        //Call OnEndGameState event
        if (OnEndGameState != null)
        {
            OnEndGameState();
        }
    }

    /*
    Description: Function that will be called by MainMenuSystem when the scene is load
    Creator: Juan Calvin Raymond
    Creation Date: 12 Dec 2016
    */
    private void PrepateMainMenuState()
    {
        //Set the time scale to 1
        PTimeScale = 1.0f;

        //Call OnMainMenuState event
        if (OnMainMenuState != null)
        {
            OnMainMenuState();
        }
    }

    /*
    Description: Function to check if a weapon type is in the valid list of weapons
    Parameters:  List<EWeaponTypes> aListOfValidWeapons - The list containing the type of weapons valid
                                                         for the leaderboard.
                EWeaponTypes aWeaponToCompare - The weapon that will be checked/compared.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private bool CheckSingleWeaponValidForLeaderboard(List<EWeaponTypes> aListOfValidWeapons, EWeaponTypes aWeaponToCompare)
    {
        //If the list of weapons is valid
        if (aListOfValidWeapons != null)
        {
            //Go through each weapon type
            foreach (EWeaponTypes type in aListOfValidWeapons)
            {
                //If the weapon being compared is found in the list
                if (type == aWeaponToCompare)
                {
                    //Return weapon valid
                    return true;
                }
            }
        }

        //Return the weapon is not valid
        return false;
    }

    /*
    Description: Function to get the first leaderboard stored in the array of leaderboards that matches the level desired.
    Creator: Alvaro Chavez Mixco
    */
    public CLeaderboard GetLeaderboard(ELevelState aLevel)
    {
        //If the leaderboard array is not empty
        if (m_leaderboards.Length >= 0)
        {
            //Go through all the leaderboards
            for (int i = 0; i < m_leaderboards.Length; i++)
            {
                //If the leaderboard is not null
                if (m_leaderboards[i] != null)
                {
                    //If the current leaderboard matches the desired level
                    if (m_leaderboards[i].PLeaderboardLevel == aLevel)
                    {
                        return m_leaderboards[i].PLeaderboard;
                    }
                }
            }
        }

        return null;
    }

    /*
    Description: Function to check if the weapons the player started the level with are valid for the leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: The function will normally use the values stored in the setting storer. If there is no settings storer
    //it will use weapon handlers weapons
    */
    public bool CheckWeaponsValidForLeaderboard()
    {
        //If the setting storer is valid
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Check weapon types from the setting storer, in case the player current weapon has been dropped
            return
                CheckSingleWeaponValidForLeaderboard(m_validRankingLeftWeapon, CSettingsStorer.PInstanceSettingsStorer.PStartingLeftWeaponType) &&
                CheckSingleWeaponValidForLeaderboard(m_validRankingRightWeapons, CSettingsStorer.PInstanceSettingsStorer.PStartingRightWeaponType);
        }
        else if (PPlayerWeaponHandler)//If there is a weapon handler
        {
            //WARNING: BECAUSE OF EXECUTION ORDER WHEN USING THE WEAPON HANDLER THE VALUES MAY BE NONE/NULL AT START

            //Check weapon types from the weapon handle storer, in case the player current weapon has been dropped
            return
                CheckSingleWeaponValidForLeaderboard(m_validRankingLeftWeapon, PPlayerWeaponHandler.PStartingLefttWeaponType) &&
                CheckSingleWeaponValidForLeaderboard(m_validRankingRightWeapons, PPlayerWeaponHandler.PStartingRightWeaponType);
        }

        return false;
    }
}


