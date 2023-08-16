using UnityEngine;
using System.Collections;
using System;

/*
Description: Button functionality to call events to play/pause a texture
Creator: Alvaro Chavez Mixco
Creation Date: Monday, March 20th, 2017
*/
public class CButtonSetAnimatedTexture : AButtonFunctionality
{
    //Variables to control when the animation should be player
    [Header("Play condtions")]
    public bool m_playOnClick = false;
    public bool m_playOnHover = true;
    public bool m_pauseOnUnHover = false;
    public bool m_stopTextureOnDisable = true;

    [Tooltip("The index in the CAnimatedTexturePlayer of the image sequence to play")]
    public uint m_movieIndex;
    [Tooltip("Should the video restart each time it is played")]
    public bool m_restartOnPlay = true;

    public delegate void delegPlayAnimatedTexture(uint aMovieIndex, bool aRestartAnimation);
    public event delegPlayAnimatedTexture OnPlayTexture;

    public delegate void delegPauseAnimatedTexture(uint aMovieIndex);
    public event delegPauseAnimatedTexture OnPauseTexture;
    public event delegPauseAnimatedTexture OnStopTexture;

    /*
    Description:Check that the object has a movie texture to play, if there is a button
                suscribe to its event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    protected override void Start()
    {
        base.Start();

        //Suscribe to the button events
        SuscribeToButtonsEvents();
    }

    /*
    Description: Unsuscribe to the button events
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    protected override void OnDestroy()
    {
        base.OnDestroy();   

        //Unsuscribe from button events
        UnsuscribeToButtonEvents();
    }

    /*
    Description: When disabled, if applicable disable the animated texture of the object
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void OnDisable()
    {
        //If we want to stop the texture on disable
        if(m_stopTextureOnDisable==true)
        {
            //If there are suscribers to event
            if(OnStopTexture!=null)
            {
                //Call event
                OnStopTexture(m_movieIndex);
            }
        }

    }

    /*
    Description: Save the button component and suscribe to its events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void SuscribeToButtonsEvents()
    {
        //Suscribe to the button events
        m_button.OnClickEvent += OnButtonClick;
        m_button.OnHoverEvent += OnButtonHover;
        m_button.OnUnHoverEvent += OnButtonUnHover;
    }

    /*
    Description: Unsuscribe to the button events
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void UnsuscribeToButtonEvents()
    {
        //Unsuscribe from the button events
        m_button.OnClickEvent -= OnButtonClick;
        m_button.OnHoverEvent -= OnButtonHover;
        m_button.OnUnHoverEvent -= OnButtonUnHover;
    }

    /*
    Description: When clicked, play the movie texture, if applicable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Called when the button component is clicked
    */
    public override void OnButtonClick()
    {
        base.OnButtonClick();

        //If we want to play the movie when the button is clicked
        if (m_playOnClick == true)
        {
            if (OnPlayTexture != null)
            {
                OnPlayTexture(m_movieIndex, m_restartOnPlay);
            }
        }
    }

    /*
    Description: When hovered, play the movie texture, if applicable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Called when the button component is hovered
    */
    public override void OnButtonHover()
    {
        base.OnButtonHover();

        //If we want to play the movie when the button is hovered
        if (m_playOnHover == true)
        {
            if(OnPlayTexture!=null)
            {
                OnPlayTexture(m_movieIndex, m_restartOnPlay);
            }
        }
    }

    /*
    Description: When unhovered, pause the movie texture, if applicable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Called when the button component is unhovered
    */
    public override void OnButtonUnHover()
    {
        base.OnButtonUnHover();

        //If we want to stop the movie when the button is unhovered
        if (m_pauseOnUnHover == true)
        {
            if(OnPauseTexture!=null)
            {
                OnPauseTexture(m_movieIndex);
            }
        }
    }

    /*
    Description: Noting done in execution.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public override void OnButtonExecution()
    {
    }
}
