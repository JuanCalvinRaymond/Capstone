using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
Description: Class used to play a movie texture on an object.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CMovieTexturePlayer : MonoBehaviour
{
    private CButton m_button;

    [Header("Animation Properties")]
    public MovieTexture m_movieTexture;

    //Variables to control when the animation should be player
    public bool m_playOnClick = false;
    public bool m_playOnHover = true;
    public bool m_pauseOnUnHover = false;

    public bool m_loopMovie = true;

    [Space(20)]
    [Tooltip("Objects in which the animation should be played.")]
    public GameObject[] m_objectsToPlayMovie;
    [Tooltip("If the player should change the object texture when the object is stopped.")]
    public bool m_changeTextureWhenStopped = true;
    [Tooltip("The texture that the object will have if the texture is not playing and the m_changeTextureWhenStopped setting is on")]
    public Texture m_defaultObjectStoppedTexture;

    /*
    Description:Check that the object has a movie texture to play, if there is a button
                suscribe to its event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Start()
    {
        //If there is a movie texture
        if (m_movieTexture != null)
        {
            //Set if the movie should loop or not
            m_movieTexture.loop = m_loopMovie;

            //Set the objects movie texture
            CUtilitySetters.SetTextureInObjects(m_objectsToPlayMovie, m_movieTexture);

            //Suscribe to the button events
            SuscribeToButtonsEvents();
        }
        else//If the movie texture is invalid
        {
            //Disable this component
            enabled = false;
        }
    }

    /*
    Description: Unsuscribe to the button events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void OnDestroy()
    {
        //Unsuscribe from button events
        UnsuscribeToButtonEvents();
    }

    /*
    Description: When disabled, ensure that the movie texture is stopped.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void OnDisable()
    {
        //Stop the movie
        StopMovieTexture();
    }

    /*
    Description: Save the button component and suscribe to its events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void SuscribeToButtonsEvents()
    {
        //Save the button component
        m_button = GetComponent<CButton>();

        //If there is a button
        if (m_button != null)
        {
            //Suscribe to the button events
            m_button.OnClickEvent += OnButtonClicked;
            m_button.OnHoverEvent += OnButtonHovered;
            m_button.OnUnHoverEvent += OnButtonUnhovered;
        }
    }

    /*
    Description: Unsuscribe to the button events
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void UnsuscribeToButtonEvents()
    {
        //If there is a button
        if (m_button != null)
        {
            //Unsuscribe from the button events
            m_button.OnClickEvent -= OnButtonClicked;
            m_button.OnHoverEvent -= OnButtonHovered;
            m_button.OnUnHoverEvent -= OnButtonUnhovered;
        }
    }

    /*
    Description: When clicked, play the movie texture, if applicable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called when the button component is clicked
    */
    private void OnButtonClicked()
    {
        //If we want to play the movie when the button is clicked
        if (m_playOnClick == true)
        {
            //Set the objects movie texture
            CUtilitySetters.SetTextureInObjects(m_objectsToPlayMovie, m_movieTexture);

            //Play the movie
            PlayMovieTexture();
        }
    }

    /*
    Description: When hovered, play the movie texture, if applicable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called when the button component is hovered
    */
    private void OnButtonHovered()
    {
        //If we want to play the movie when the button is hovered
        if (m_playOnHover == true)
        {
            //Set the objects movie texture
            CUtilitySetters.SetTextureInObjects(m_objectsToPlayMovie, m_movieTexture);

            //Play the movie
            PlayMovieTexture();
        }
    }

    /*
    Description: When unhovered, pause the movie texture, if applicable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called when the button component is unhovered
    */
    private void OnButtonUnhovered()
    {
        //If we want to stop the movie when the button is unhovered
        if (m_pauseOnUnHover == true)
        {
            //Stop the movie
            PauseMovieTexture();
        }
    }

    /*
    Description: Play the movie texture
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void PlayMovieTexture()
    {
        //Play the movie texture
        m_movieTexture.Play();
    }

    /*
    Description: Stop the movie texture
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void StopMovieTexture()
    {
        //Stop the movie texture
        m_movieTexture.Stop();

        //If we  want to change the object texture when the video is not playing
        if(m_changeTextureWhenStopped==true)
        {
            //Change the object texture
            CUtilitySetters.SetTextureInObjects(m_objectsToPlayMovie, m_defaultObjectStoppedTexture);
        }
    }

    /*
    Description: Pause the movie texture
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void PauseMovieTexture()
    {
        //Pause the movie texture
        m_movieTexture.Pause();
    }
}
