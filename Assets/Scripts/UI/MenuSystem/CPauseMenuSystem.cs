using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Pause Menu class which inherit from CMenuSystem, will show all the children object the player hit pause button and hide when player resume
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
public class CPauseMenuSystem : CMenuSystem
{
    //variable for playing the sound
    private AudioSource m_audioSource;

    //sound file variable
    [Header("Menu Sounds")]
    public AudioClip m_pauseSound;
    public AudioClip m_unpauseSound;

    /*
    Description: Get the AudioSource component
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    protected override void Awake()
    {
        base.Awake();

        m_audioSource = GetComponent<AudioSource>();
    }

    /*
    Description: Subscribe to Game Manager's OnPauseState and OnPlayState event
    Creator: Juan Calvin Raymond
    Creation Date: 19 Dec 2016
    */
    protected override void Start()
    {
        base.Start();

        //If there is a game manager instance
        if (CGameManager.PInstanceGameManager != null)
        {
            CGameManager.PInstanceGameManager.OnPauseState += OnPauseState;
            CGameManager.PInstanceGameManager.OnPlayState += OnPlayState;
        }
    }

    /*
    Description: Unsubscribe to Game Manager's OnPauseState and OnPlayState event
    Creator: Juan Calvin Raymond
    Creation Date: 19 Dec 2016
    */
    private void OnDestroy()
    {
        //If there is a game manager instance
        if (CGameManager.PInstanceGameManager != null)
        {
            CGameManager.PInstanceGameManager.OnPauseState -= OnPauseState;
            CGameManager.PInstanceGameManager.OnPlayState -= OnPlayState;
        }

        //Ensure the ShowPauseMenuCoroutine is stopped
        StopAllCoroutines();
    }

    /*
    Description: Coroutine used to place the pause menu in front of the player, and play the pause sound.
                 This is done in a coroutine so that the program will wait one frame after the player has 
                 paused to show the pause menu. This guarantees that the menu will be placed in front of the player
                 , since the menu will get his position once he has already stopped.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, February 15th, 2017
    */
    IEnumerator ShowPauseMenuCoroutine()
    {
        //Wait one frame until the player has completely stopped due pausing the game
        yield return null;

        //Place the menu in front of the player, once the player has already stopepd moving
        PlaceMenuObjectInFrontOfPlayer();

        //If there is an audio source and a pause sound
        if (m_audioSource != null && m_pauseSound != null)
        {
            //Play pause sound
            CUtilitySound.PlaySoundOneShot(m_audioSource, m_pauseSound);
        }

        //Activate the menu
        Activate();

        yield return null;
    }

    /*
    Description: Call the function to show the pause menu
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    private void OnPauseState()
    {
        //Start the coroutine to show the pause menu. This is done in a coroutine, so
        //that the menu is placed a frame after the player has stopped moving.
        StartCoroutine(ShowPauseMenuCoroutine());
    }

    /*
    Description: Hide all children object and play unpause audio
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    private void OnPlayState()
    {
        //Play unpause sound
        CUtilitySound.PlaySoundOneShot(m_audioSource, m_unpauseSound);

        //Deactivate the menu
        Deactivate();
    }
}