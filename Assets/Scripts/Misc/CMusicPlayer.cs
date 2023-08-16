using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using System;

using System.Collections.Generic;


/*
Description: Class used to play different musics according to the player's current level
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
[RequireComponent(typeof(AudioSource))]
public class CMusicPlayer : MonoBehaviour
{
    /*
    Description: Struct to store a level and the music belonging to that level
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    [Serializable]
    public struct SLevelMusic
    {
        [SerializeField]
        public ELevelState m_level;

        [SerializeField]
        public AudioClip m_music;
    }

    private AudioClip m_musicClip;
    private AudioSource m_audioSource;

    public List<SLevelMusic> m_levelMusicClips;

    /*
    Description: Set initial music and play it
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Awake()
    {
        //Get the audio source compoennt
        m_audioSource = GetComponent<AudioSource>();

        //If there is a music clip
        if (m_levelMusicClips != null)
        {
            //Set first music on array
            m_musicClip = m_levelMusicClips[0].m_music;
        }

        //Play music
        PlayMusic();
    }

    /*
    Description: Suscribe to the load level event, and to the game state changes events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, Match 3rd, 2017
    */
    private void Start()
    {
        //If there is a scene manager
        if (CSceneManager.PInstanceSceneManager != null)
        {
            //Suscribe to level load event
            CSceneManager.PInstanceSceneManager.OnStartingLoadingNewScene += OnNewSceneBeginLoading;
            CSceneManager.PInstanceSceneManager.OnFinishedLoadingNewScene += OnNewSceneFinishLoading;
        }

        //Suscribe to game state changes
        SuscribeToGameStateChanges();

    }

    /*
    Description: Unsuscribe from the load level event, and to the game state changes events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void OnDestroy()
    {
        //If there is a scene manager
        if (CSceneManager.PInstanceSceneManager != null)
        {
            //Unsuscribe from level load event
            CSceneManager.PInstanceSceneManager.OnStartingLoadingNewScene -= OnNewSceneBeginLoading;
            CSceneManager.PInstanceSceneManager.OnFinishedLoadingNewScene -= OnNewSceneFinishLoading;
        }

        //Unsuscribe from game state changes
        UnsuscribeFromGameStateChanges();
    }

    /*
    Description: Suscribe to game manager game state changes events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void SuscribeToGameStateChanges()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //Suscribe to the game state changes event
            CGameManager.PInstanceGameManager.OnPlayState += PlayMusic;//Play music when playing game
            CGameManager.PInstanceGameManager.OnEndGameState += StopMusic;//Stop music at end game
            CGameManager.PInstanceGameManager.OnPauseState += PauseMusic;//Stop music on pause game
        }
    }

    /*
    Description: Unsuscribe from game manager game state changes events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void UnsuscribeFromGameStateChanges()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //Unsuscribe to the game state changes event
            CGameManager.PInstanceGameManager.OnPlayState -= PlayMusic;
            CGameManager.PInstanceGameManager.OnEndGameState -= StopMusic;
            CGameManager.PInstanceGameManager.OnPauseState -= PauseMusic;
        }
    }

    /*
    Description: When a new scene is being loaded, stop the current music 
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Note: Function called when the Scene Manager starts loading a new scene
    */
    private void OnNewSceneBeginLoading(string aSceneName, ELevelState aTypeOfLevel)
    {
        //Since the game manager is not kept through scenes
        //Unsuscribe from its events
        UnsuscribeFromGameStateChanges();

        //Stop the music
        StopMusic();
    }

    /*
    Description: When a new scene has finished loading, play the corresponding music for it
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Note: Function called when the Scene Manager finishes loading a new scene
    */
    private void OnNewSceneFinishLoading(string aSceneName, ELevelState aTypeOfLevel)
    {
        //Suscribe to the game manager events
        SuscribeToGameStateChanges();

        //If there is a list of music clips
        if (m_levelMusicClips != null)
        {
            //According to the level
            for (int i = 0; i < m_levelMusicClips.Count; i++)
            {
                //Search for the corresponding music
                if (m_levelMusicClips[i].m_level == aTypeOfLevel)
                {
                    //Set the new music clip that will be played
                    m_musicClip = m_levelMusicClips[i].m_music;
                }
            }
        }

        //Play the new music clip
        PlayMusic();
    }

    /*
    Description: Play the music clip
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void PlayMusic()
    {
        CUtilitySound.PlaySound(m_audioSource, m_musicClip);
    }

    /*
    Description: Pause the music clip
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void PauseMusic()
    {
        CUtilitySound.PauseSound(m_audioSource);
    }

    /*
    Description: Stop the music clip
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void StopMusic()
    {
        CUtilitySound.StopSound(m_audioSource);
    }
}

