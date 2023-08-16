using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/*
Description:Class using singleton pattern used to control the Unity Audio Mixer to alter the volume of the objects in the game.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CAudioManager : MonoBehaviour
{
    //Singleton Instance
    private static CAudioManager s_instanceAudioManager;

    //Constants for sound DB range
    private const float M_UNITY_MAX_DB = 6.0f;
    private const float M_UNITY_MIN_DB = -20.0f;
    private const float M_NO_SOUND_DB = -80.0f;

    //Constants for mixer parameters
    private const string M_MAIN_VOLUME_PARAMETER = "m_mainVolume";
    private const string M_MENU_VOLUME_PARAMETER = "m_menuSoundsVolume";
    private const string M_SOUND_EFFECTS_VOLUME_PARAMETER = "m_soundEffectsVolume";
    private const string M_MUSIC_VOLUME_PARAMETER = "m_musicVolume";

    //Audio mixers
    public AudioMixer m_masterMixer;
    public AudioMixer m_soundEffectsMixer;
    public AudioMixer m_menuSoundMixer;

    public static CAudioManager PInstanceAudioManager
    {
        get
        {
            return s_instanceAudioManager;
        }
    }

    /*
    Description:Ensure there is only one instance of hte Audio Manager by executing the singleton pattern. This
    also sets up all the initial values for the audio, and ensures the audio manager is not deleted when loading a new scene.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Awake()
    {
        //Singleton pattern
        //If there is no audio manager instance
        if (s_instanceAudioManager == null)
        {
            //Set this as the instance
            s_instanceAudioManager = this;
            
            //Don't destroy the scene when a new object is loaded
            DontDestroyOnLoad(this);
        }
        else//If there was previously another instance of audio manager
        {
            //Destroy this gameobject
            Destroy(gameObject);
        }
    }

    /*
    Description: Set the initial values for the audio manager and suscribe to the desired events
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 23rd, 2017
    */
    private void Start()
    {
        //Set the initial values for the audio mixer
        InitialSetup();
    }

    /*
    Description:Ensure there is only one instance of hte Audio Manager by executing the singleton pattern. This
    also sets up all the initial values for the audio, and ensures the audio manager is not deleted when loading a new scene.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void OnDestroy()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe to setting storer sound properties
            CSettingsStorer.PInstanceSettingsStorer.OnVolumePercentChange -= MainVolumeChange;
            CSettingsStorer.PInstanceSettingsStorer.OnMenuSoundsVolumeChange -= MenuVolumeChange;
            CSettingsStorer.PInstanceSettingsStorer.OnSoundsEffectVolumeChange -= SoundEffectsVolumeChange;
            CSettingsStorer.PInstanceSettingsStorer.OnMusicVolumeChange -= MusicVolumeChange;
        }
    }

    /*
    Description:Ensure there is only one instance of hte Audio Manager by executing the singleton pattern. This
    also sets up all the initial values for the audio, and ensures the audio manager is not deleted when loading a new scene.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void InitialSetup()
    {
        //Reset all values to their  value
        SnapShotReset();

        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Suscribe to setting storer sound properties
            CSettingsStorer.PInstanceSettingsStorer.OnVolumePercentChange += MainVolumeChange;
            CSettingsStorer.PInstanceSettingsStorer.OnSoundsEffectVolumeChange += SoundEffectsVolumeChange;
            CSettingsStorer.PInstanceSettingsStorer.OnMusicVolumeChange += MusicVolumeChange;
            CSettingsStorer.PInstanceSettingsStorer.OnMenuSoundsVolumeChange += MenuVolumeChange;

            //Set starting values
            MainVolumeChange(CSettingsStorer.PInstanceSettingsStorer.PVolumePercent);
            SoundEffectsVolumeChange(CSettingsStorer.PInstanceSettingsStorer.PSoundEffectsVolume);
            MusicVolumeChange(CSettingsStorer.PInstanceSettingsStorer.PMusicVolume);
            MenuVolumeChange(CSettingsStorer.PInstanceSettingsStorer.PMenuSoundsVolume);
        }
    }

    /*
    Description: Clears all the values of the current audio mixer snapshot to their default values.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void SnapShotReset()
    {
        //Reset the snapshots value to match their original vlaue
        m_masterMixer.ClearFloat(M_MAIN_VOLUME_PARAMETER);
        m_masterMixer.ClearFloat(M_MUSIC_VOLUME_PARAMETER);
        m_masterMixer.ClearFloat(M_SOUND_EFFECTS_VOLUME_PARAMETER);
        m_masterMixer.ClearFloat(M_MENU_VOLUME_PARAMETER);
    }

    /*
    Description: Sets the desired volume value, in the parameter of the Master Audio Mixer
    Parameters: string aParameterName - The parameter in the Master Audio Mixer that will be set 
                float aPercentVolume - The volume that will be set, as a percent (0.0 to 1.0)
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void VolumeChange(string aParameterName, float aPercentVolume)
    {
        float DBValue;
        
        //If we want to set sound to 0, rounded to 0 and fraction for precision
        if(aPercentVolume<=0.09f)
        {
            //Set the sound to absolute minimum so that it can't be heard at all
            DBValue = M_NO_SOUND_DB;
        }
        else//If we want at least some sound
        {
            //Convert the percent value to a DB value, according to the desired min and max values
            DBValue = Mathf.Lerp(M_UNITY_MIN_DB, M_UNITY_MAX_DB, aPercentVolume);
        }

        //Set the value of the parameter
        m_masterMixer.SetFloat(aParameterName, DBValue);
    }

    /*
    Description: Sets the volume for the Master Audio Mixer
    Parameters:  float aPercentVolume - The volume that will be set, as a percent (0.0 to 1.0)
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function commonly called when the Setting Storer event OnVolumePercentChange is called. 
    */
    public void MainVolumeChange(float aPercentVolume)
    {
        //Set the desired volume in the main volume audio mixer parameter
        VolumeChange(M_MAIN_VOLUME_PARAMETER, aPercentVolume);
    }

    /*
    Description: Sets the volume for the Menu Sounds Audio Mixer
    Parameters:  float aPercentVolume - The volume that will be set, as a percent (0.0 to 1.0)
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function commonly called when the Setting Storer event OnMenuSoundsVolumeChange is called. 
    */
    public void MenuVolumeChange(float aPercentVolume)
    {
        //Set the desired volume in the menu sounds volume audio mixer parameter
        VolumeChange(M_MENU_VOLUME_PARAMETER, aPercentVolume);
    }

    /*
    Description: Sets the volume for the Sounds Effects Audio Mixer
    Parameters:  float aPercentVolume - The volume that will be set, as a percent (0.0 to 1.0)
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function commonly called when the Setting Storer event OnSoundsEffectVolumeChange is called. 
    */
    public void SoundEffectsVolumeChange(float aPercentVolume)
    {
        //Set the desired volume in the sounds effects volume audio mixer parameter
        VolumeChange(M_SOUND_EFFECTS_VOLUME_PARAMETER, aPercentVolume);
    }

    /*
    Description: Sets the volume for the Music Volume Audio Mixer
    Parameters:  float aPercentVolume - The volume that will be set, as a percent (0.0 to 1.0)
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function commonly called when the Setting Storer event OnMusicVolumeChange is called. 
    */
    public void MusicVolumeChange(float aPercentVolume)
    {
        //Set the desired volume in the music volume audio mixer parameter
        VolumeChange(M_MUSIC_VOLUME_PARAMETER, aPercentVolume);
    }
}
