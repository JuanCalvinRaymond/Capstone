using UnityEngine;
using System.Collections;

using Valve.VR;

/*
Description: Class that implements singleton design pattern, and that doesn't get destroyed when a 
new scene is loaded. This class stores, in the playerprefs and in game, all the settings options
for the game.
Creator: Alvaro Chavez Mixco
*/
public class CSettingsStorer : MonoBehaviour
{
    //The singleton instance for this class
    private static CSettingsStorer s_instanceSettingsStorer;

    //The name that the values will have as keys when writte to the playerPrefs file
    private const string M_SHOW_AIMING_AIDS_KEY_NAME = "IsShowingAimingAids";
    private const string M_SHOW_PLATFORM_KEY_NAME = "IsShowingPlatform";
    private const string M_INPUT_SENSITIVITY_KEY_NAME = "InputSensitivity";
    private const string M_INVERT_Y_AXIS_KEY_NAME = "InvertedYAxis";
    private const string M_VOLUME_PERCENT_KEY_NAME = "VolumePercent";
    private const string M_SOUND_EFFECTS_VOLUME_PERCENT_KEY_NAME = "SoundEffectsVolumePercent";
    private const string M_MENU_SOUNDS_VOLUME_PERCENT_KEY_NAME = "MenuSoundsVolumePercent";
    private const string M_MUSIC_VOLUME_PERCENT_KEY_NAME = "MusicVolumePercent";
    private const string M_QUALITY_LEVEL_KEY_NAME = "QualityLevel";
    private const string M_BRIGHTNESS_PERCENT_KEY_NAME = "BrightnessPercent";
    private const string M_GAMMA_PERCENT_KEY_NAME = "GammaPercent";
    private const string M_SCREEN_RESOLUTION_WIDTH_KEY_NAME = "ScreenResolutionWidth";
    private const string M_SCREEN_RESOLUTION_HEIGHT_KEY_NAME = "ScreenResolutionHeight";
    private const string M_SCREEN_RESOLUTION_FULL_SCREEN_KEY_NAME = "ScreenResolutionFullscreen";
    private const string M_DRAW_DISTANCE_KEY_NAME = "DrawDistance";

    //The default values of the settings
    //Gameplay
    public const bool M_SHOW_AIMING_AIDS_DEFAULT = true;
    public const bool M_SHOW_PLATFORM_DEFAULT = true;
    public const float M_INPUT_SENSITIVTY_DEFAULT = 1.0f;
    public const bool M_INVERT_Y_AXIS_DEFAULT = false;
    //Audio
    public const float M_VOLUME_DEFAULT = 1.0f;
    public const float M_SOUND_EFFECTS_VOLUME_DEFAULT = 0.9f;
    public const float M_MENU_SOUNDS_VOLUME_DEFAULT = 1.0f;
    public const float M_MUSIC_VOLUME_DEFAULT = 0.8f;
    //Graphics
    public const int M_QUALITY_LEVEL_DEFAULT = 5;
    public const float M_BRIGHTNESS_DEFAULT = 0.5f;
    public const float M_GAMMA_DEFAULT = 0.0f;
    public const int M_SCREEN_RESOLUTION_HEIGHT_DEFAULT = 1080;
    public const int M_SCREEN_RESOLUTION_WIDTH_DEFAULT = 1920;
    public const bool M_SCREEN_RESOLUTION_FULLSCREEN_DEFAULT = false;
    public const float M_DRAW_DISTANCE_DEFAULT = 1000.0f;

    //The type of input used by the player
    private EControllerTypes m_inputMethod;

    //Variables for the values
    //Gameplay variables
    private bool m_isShowingAimingAids;
    private bool m_isShowingPlatform;
    private float m_inputSensitivity;
    private bool m_isInvertedYAxis;
    //Sound variables
    private float m_volumePercent;
    private float m_soundEffectsVolumePercent;
    private float m_menuSoundsVolumePercent;
    private float m_musicVolumePercent;
    //Graphics variables
    private int m_qualityLevel;
    private float m_brightnessPercent;
    private float m_gammaPercent;
    private SScreenResolution m_screenResolution;
    private float m_drawDistance;

    //This are the starting weapons for the player, if they aer set as such by the 
    //player creator. This weapons are saved through multiple scenes.
    private GameObject m_startingRightWeapon;
    private GameObject m_startingLeftWeapon;

    [Header("Player starting weapons")]
    public EWeaponTypes m_startingRightWeaponType;
    public EWeaponTypes m_startingLeftWeaponType;
    public CWeaponCreator m_weaponCreator;

    //Events and delegates for when some of these values change
    //Float settings
    public delegate void delegateSettingFloatChange(float aPercent);
    public event delegateSettingFloatChange OnInputSensitivityChange;
    public event delegateSettingFloatChange OnVolumePercentChange;
    public event delegateSettingFloatChange OnSoundsEffectVolumeChange;
    public event delegateSettingFloatChange OnMenuSoundsVolumeChange;
    public event delegateSettingFloatChange OnMusicVolumeChange;
    public event delegateSettingFloatChange OnBrightnessPercentChange;
    public event delegateSettingFloatChange OnGammaPercentChange;
    public event delegateSettingFloatChange OnDrawDistanceChange;

    //Bool settings
    public delegate void delegateSettingBoolChange(bool aValue);
    public event delegateSettingBoolChange OnIsShowingAimingAidsChange;
    public event delegateSettingBoolChange OnIsShowingPlatformChange;
    public event delegateSettingBoolChange OnInvertedYAxisChange;

    //Int Settings
    public delegate void delegateSettingIntChange(int aValue);
    public event delegateSettingIntChange OnQualityLevelChange;

    //Screen Resolution Settings
    public delegate void delegateScreenResolutionChange(SScreenResolution aScreenResolution);
    public event delegateScreenResolutionChange OnScreenResolutionChange;

    public static CSettingsStorer PInstanceSettingsStorer
    {
        get
        {
            return s_instanceSettingsStorer;
        }
    }

    public EControllerTypes PInputMethod
    {
        get { return m_inputMethod; }

        private set { m_inputMethod = value; }
    }

    public bool PIsShowingAimingAids
    {
        get { return m_isShowingAimingAids; }

        set
        {
            m_isShowingAimingAids = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsBool(M_SHOW_AIMING_AIDS_KEY_NAME, m_isShowingAimingAids);

            //If the event is valid
            if (OnIsShowingAimingAidsChange != null)
            {
                //Call the event in all listeners
                OnIsShowingAimingAidsChange(m_isShowingAimingAids);
            }
        }
    }

    public bool PIsShowingPlatform
    {
        get { return m_isShowingPlatform; }

        set
        {
            m_isShowingPlatform = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsBool(M_SHOW_PLATFORM_KEY_NAME, m_isShowingPlatform);

            //If the event is valid
            if (OnIsShowingPlatformChange != null)
            {
                //Call the event in all listeners
                OnIsShowingPlatformChange(m_isShowingPlatform);
            }
        }
    }

    public float PInputSensitivity
    {
        get { return m_inputSensitivity; }

        set
        {
            m_inputSensitivity = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_INPUT_SENSITIVITY_KEY_NAME, m_inputSensitivity);

            //If the event is valid
            if (OnInputSensitivityChange != null)
            {
                //Call the event in all listeners
                OnInputSensitivityChange(m_inputSensitivity);
            }
        }
    }

    public bool PIsInvertedYAxis
    {
        get { return m_isInvertedYAxis; }

        set
        {
            m_isInvertedYAxis = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsBool(M_INVERT_Y_AXIS_KEY_NAME, m_isInvertedYAxis);

            //If the event is valid
            if (OnInvertedYAxisChange != null)
            {
                //Call the event in all listeners
                OnInvertedYAxisChange(m_isInvertedYAxis);
            }
        }
    }

    public float PVolumePercent
    {
        get { return m_volumePercent; }

        set
        {
            m_volumePercent = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_VOLUME_PERCENT_KEY_NAME, m_volumePercent);

            //If the event is valid
            if (OnVolumePercentChange != null)
            {
                //Call the event in all listeners
                OnVolumePercentChange(m_volumePercent);
            }
        }
    }

    public float PSoundEffectsVolume
    {
        get { return m_soundEffectsVolumePercent; }

        set
        {
            m_soundEffectsVolumePercent = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_SOUND_EFFECTS_VOLUME_PERCENT_KEY_NAME, m_soundEffectsVolumePercent);

            //If the event is valid
            if (OnSoundsEffectVolumeChange != null)
            {
                //Call the event in all listeners
                OnSoundsEffectVolumeChange(m_soundEffectsVolumePercent);
            }
        }
    }

    public float PMenuSoundsVolume
    {
        get { return m_menuSoundsVolumePercent; }

        set
        {
            m_menuSoundsVolumePercent = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_MENU_SOUNDS_VOLUME_PERCENT_KEY_NAME, m_menuSoundsVolumePercent);

            //If the event is valid
            if (OnMenuSoundsVolumeChange != null)
            {
                //Call the event in all listeners
                OnMenuSoundsVolumeChange(m_menuSoundsVolumePercent);
            }
        }
    }

    public float PMusicVolume
    {
        get { return m_musicVolumePercent; }

        set
        {
            m_musicVolumePercent = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_MUSIC_VOLUME_PERCENT_KEY_NAME, m_musicVolumePercent);

            //If the event is valid
            if (OnMusicVolumeChange != null)
            {
                //Call the event in all listeners
                OnMusicVolumeChange(m_musicVolumePercent);
            }
        }
    }

    public float PBrightnessPercent
    {
        get { return m_brightnessPercent; }

        set
        {
            m_brightnessPercent = value;

            //Write value to Player Prefss
            CUtilityFiles.WritePlayerPrefsFloat(M_BRIGHTNESS_PERCENT_KEY_NAME, m_brightnessPercent);

            //If the event is valid
            if (OnBrightnessPercentChange != null)
            {
                //Call the event in all listeners
                OnBrightnessPercentChange(m_brightnessPercent);
            }
        }
    }

    public float PGammaPercent
    {
        get { return m_gammaPercent; }

        set
        {
            m_gammaPercent = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_GAMMA_PERCENT_KEY_NAME, m_gammaPercent);

            //If the event is valid
            if (OnGammaPercentChange != null)
            {
                //Call the event in all listeners
                OnGammaPercentChange(m_gammaPercent);
            }
        }
    }

    public SScreenResolution PScreenResolution
    {
        get { return m_screenResolution; }

        set
        {
            m_screenResolution = value;

            //If the input is not VR
            if (m_inputMethod != EControllerTypes.ViveController)
            {
                //Check that the values being set are actually different that the current ones
                if (Screen.currentResolution.height != m_screenResolution.m_height && Screen.currentResolution.width != m_screenResolution.m_width)
                {
                    //Set the screen resolution in the engine
                    Screen.SetResolution(m_screenResolution.m_width, m_screenResolution.m_height, m_screenResolution.m_fullscreen);
                }
            }

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsInt(M_SCREEN_RESOLUTION_WIDTH_KEY_NAME, m_screenResolution.m_width);
            CUtilityFiles.WritePlayerPrefsInt(M_SCREEN_RESOLUTION_HEIGHT_KEY_NAME, m_screenResolution.m_height);
            CUtilityFiles.WritePlayerPrefsBool(M_SCREEN_RESOLUTION_FULL_SCREEN_KEY_NAME, m_screenResolution.m_fullscreen);

            //If the event is valid
            if (OnScreenResolutionChange != null)
            {
                //Call the event in all listeners
                OnScreenResolutionChange(m_screenResolution);
            }
        }
    }

    public float PDrawDistance
    {
        get { return m_drawDistance; }

        set
        {
            m_drawDistance = value;

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsFloat(M_DRAW_DISTANCE_KEY_NAME, m_drawDistance);

            //If the event is valid
            if (OnDrawDistanceChange != null)
            {
                //Call the event in all listeners
                OnDrawDistanceChange(m_drawDistance);
            }
        }
    }

    public int PQualityLevel
    {
        get { return m_qualityLevel; }

        set
        {
            m_qualityLevel = value;

            //Set the quality level in the engine
            QualitySettings.SetQualityLevel(m_qualityLevel);

            //Write value to Player Prefs
            CUtilityFiles.WritePlayerPrefsInt(M_QUALITY_LEVEL_KEY_NAME, m_qualityLevel);

            //If the event is valid
            if (OnQualityLevelChange != null)
            {
                //Call the event in all listeners
                OnQualityLevelChange(m_qualityLevel);
            }
        }
    }

    public GameObject PStartingRightWeapon
    {
        get
        {
            if (m_weaponCreator != null)
            {
                //According to the starting weapon type, create the weapon
                m_startingRightWeapon = m_weaponCreator.GetWeaponPrefab(m_startingRightWeaponType);
            }

            return m_startingRightWeapon;

        }

        set
        {
            m_startingRightWeapon = value;

            m_startingRightWeaponType = CUtilitySetters.GetWeaponType(m_startingRightWeapon);
        }
    }

    public GameObject PStartingLeftWeapon
    {
        get
        {
            if (m_weaponCreator != null)
            {
                //According to the starting weapon type, create the weapon
                m_startingLeftWeapon = m_weaponCreator.GetWeaponPrefab(m_startingLeftWeaponType);
            }

            return m_startingLeftWeapon;
        }

        set
        {
            m_startingLeftWeapon = value;

            m_startingLeftWeaponType = CUtilitySetters.GetWeaponType(m_startingLeftWeapon);
        }
    }

    public EWeaponTypes PStartingRightWeaponType
    {
        get { return m_startingRightWeaponType; }
    }

    public EWeaponTypes PStartingLeftWeaponType
    {
        get { return m_startingLeftWeaponType; }
    }

    /*
    Description: Ensure there is only 1 instance of the object, and that the object is not destroyed
    when a new scene is loaded. It also reads and sets the setting according to the PlayerPrefs file.
    Creator: Alvaro Chavez Mixco
    */
    private void Awake()
    {

        //If the instance doesn't exist
        if (s_instanceSettingsStorer == null)
        {
            //Set this as the instance
            s_instanceSettingsStorer = this;

            //Ensure the game object is not destroyed when a new scene is loaded
            DontDestroyOnLoad(this);

            //Create the screen resolution struct
            m_screenResolution = new SScreenResolution();

            //Create the weapons


            //Determine which input method is being used
            DetermineInputMethod();

            //Read the current player preferences
            ReadPlayerPrefs();
        }
        else//If the instance already exists
        {
            //Destroy this object
            Destroy(gameObject);
        }
    }

    /*
    Description: Function used to determine which input method is the player using.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void DetermineInputMethod()
    {
        // Check if vive vr has been initialised
        bool viveHeadsetConnected = OpenVR.IsHmdPresent() && (SteamVR.instance != null);

        // If Vive vr hasn been initiliased
        if (viveHeadsetConnected == true)
        {
            // Set that the vive controller is being used
            PInputMethod = EControllerTypes.ViveController;
        }
        else if (Input.GetJoystickNames().Length <= 0)// If there are no gamepads connected
        {
            // Set that a mouse and keyboard are being used
            PInputMethod = EControllerTypes.MouseAndKeyboardController;
        }
        else// There is at least one gamepad connected
        {
            // Set that the gamepad controller is being used
            PInputMethod = EControllerTypes.GamepadController;
        }
    }

    /*
    Description: The function reads and stores all the settings from the PlayerPrefs file.
    Creator: Alvaro Chavez Mixco
    */
    private void ReadPlayerPrefs()
    {
        //Gameplay Settings
        PIsShowingAimingAids = CUtilityFiles.ReadPlayerPrefsBool(M_SHOW_AIMING_AIDS_KEY_NAME, M_SHOW_AIMING_AIDS_DEFAULT);
        PIsShowingPlatform = CUtilityFiles.ReadPlayerPrefsBool(M_SHOW_PLATFORM_KEY_NAME, M_SHOW_PLATFORM_DEFAULT);
        PInputSensitivity = CUtilityFiles.ReadPlayerPrefsFloat(M_INPUT_SENSITIVITY_KEY_NAME, M_INPUT_SENSITIVTY_DEFAULT);
        PIsInvertedYAxis = CUtilityFiles.ReadPlayerPrefsBool(M_INVERT_Y_AXIS_KEY_NAME, M_INVERT_Y_AXIS_DEFAULT);

        //Audio Settings
        PVolumePercent = CUtilityFiles.ReadPlayerPrefsFloat(M_VOLUME_PERCENT_KEY_NAME, M_VOLUME_DEFAULT);
        PSoundEffectsVolume = CUtilityFiles.ReadPlayerPrefsFloat(M_SOUND_EFFECTS_VOLUME_PERCENT_KEY_NAME, M_SOUND_EFFECTS_VOLUME_DEFAULT);
        PMenuSoundsVolume = CUtilityFiles.ReadPlayerPrefsFloat(M_MENU_SOUNDS_VOLUME_PERCENT_KEY_NAME, M_MENU_SOUNDS_VOLUME_DEFAULT);
        PMusicVolume = CUtilityFiles.ReadPlayerPrefsFloat(M_MUSIC_VOLUME_PERCENT_KEY_NAME, M_MUSIC_VOLUME_DEFAULT);

        //Graphics Settings
        PQualityLevel = CUtilityFiles.ReadPlayerPrefsInt(M_QUALITY_LEVEL_KEY_NAME, M_QUALITY_LEVEL_DEFAULT);
        PBrightnessPercent = CUtilityFiles.ReadPlayerPrefsFloat(M_BRIGHTNESS_PERCENT_KEY_NAME, M_BRIGHTNESS_DEFAULT);
        PGammaPercent = CUtilityFiles.ReadPlayerPrefsFloat(M_GAMMA_PERCENT_KEY_NAME, M_GAMMA_DEFAULT);
        m_screenResolution.m_width = CUtilityFiles.ReadPlayerPrefsInt(M_SCREEN_RESOLUTION_WIDTH_KEY_NAME, M_SCREEN_RESOLUTION_WIDTH_DEFAULT);
        m_screenResolution.m_height = CUtilityFiles.ReadPlayerPrefsInt(M_SCREEN_RESOLUTION_HEIGHT_KEY_NAME, M_SCREEN_RESOLUTION_HEIGHT_DEFAULT);
        m_screenResolution.m_fullscreen = CUtilityFiles.ReadPlayerPrefsBool(M_SCREEN_RESOLUTION_FULL_SCREEN_KEY_NAME, M_SCREEN_RESOLUTION_FULLSCREEN_DEFAULT);
        PScreenResolution = m_screenResolution;
        PDrawDistance = CUtilityFiles.ReadPlayerPrefsFloat(M_DRAW_DISTANCE_KEY_NAME, M_DRAW_DISTANCE_DEFAULT);
    }

    /*
    Description: Resets all the gameplay related settings.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    */
    public void ResetGameplaySetttings()
    {
        PIsShowingAimingAids = M_SHOW_AIMING_AIDS_DEFAULT;
        PIsShowingPlatform = M_SHOW_PLATFORM_DEFAULT;
        PInputSensitivity = M_INPUT_SENSITIVTY_DEFAULT;
        PIsInvertedYAxis = M_INVERT_Y_AXIS_DEFAULT;
    }

    /*
    Description: Resets all the audio related settings.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    */
    public void ResetAudioSettings()
    {
        PVolumePercent = M_VOLUME_DEFAULT;
        PSoundEffectsVolume = M_SOUND_EFFECTS_VOLUME_DEFAULT;
        PMenuSoundsVolume = M_MENU_SOUNDS_VOLUME_DEFAULT;
        PMusicVolume = M_MUSIC_VOLUME_DEFAULT;
    }

    /*
    Description: Resets all the graphics related settings.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    */
    public void ResetGraphicsSettings()
    {
        PQualityLevel = M_QUALITY_LEVEL_DEFAULT;
        PBrightnessPercent = M_BRIGHTNESS_DEFAULT;
        PGammaPercent = M_GAMMA_DEFAULT;
        PScreenResolution = new SScreenResolution(M_SCREEN_RESOLUTION_WIDTH_DEFAULT,
            M_SCREEN_RESOLUTION_HEIGHT_DEFAULT, M_SCREEN_RESOLUTION_FULLSCREEN_DEFAULT);
        PDrawDistance = M_DRAW_DISTANCE_DEFAULT;
    }

    /*
    Description: Resets all the settings of the game.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    */
    public void ResetAllSettings()
    {
        ResetGameplaySetttings();
        ResetAudioSettings();
        ResetGraphicsSettings();
    }
}
