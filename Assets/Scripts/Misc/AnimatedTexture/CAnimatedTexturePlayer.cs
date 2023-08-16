using UnityEngine;
using System.Collections;

using System;

/*
Description: Class used to play an animated texture. The class can suscribe to buttons, that have an a
            CButtonSetAnimatedTexture to detect button events.           
Creator: Alvaro Chavez Mixco
Creation Date: Monday, March 20th, 2017
*/
[RequireComponent(typeof(MeshRenderer))]
public class CAnimatedTexturePlayer : MonoBehaviour
{
    //PENDING
    private enum EMovieState
    {
        Playing,
        Paused,
        Stop
    }

    //Constants
    private const string M_MATERIAL_TEXTURE_NAME = "_MainTex";

    private MeshRenderer m_renderer;
    private Material m_material;

    private uint m_currentlyActiveMovieIndex;

    private float m_frameTime = 0.0f;
    private float m_timerNextFrame = 0.0f;

    private EMovieState m_movieState = EMovieState.Stop;

    [Tooltip("The buttons that this component will suscribe to")]
    public CButtonSetAnimatedTexture[] m_buttonSetters;

    [Tooltip("The texture that will be set on the object if no animated texture is playing")]
    public Texture m_defaultTexture;

    [Tooltip("The image sequences that the animated texture player can play")]
    public CAnimatedTexture[] m_imageSequences;

    public bool m_useStorer = true;

    /*
    Description: Class used to play an animated texture. The class can suscribe to buttons, that have an a
                CButtonSetAnimatedTexture to detect button events.           
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void Awake()
    {

        if (m_useStorer == true)
        {
            GameObject storer = GameObject.FindGameObjectWithTag(CGlobalTags.M_TAG_ANIMATED_TEXTURE_STORER);

            if (storer != null)
            {
                CDataKeeper data = storer.GetComponent<CDataKeeper>();

                if (data != null)
                {
                    m_imageSequences = data.m_imageSequences;
                }

            }
        }

        //Get the componentets
        m_renderer = GetComponent<MeshRenderer>();
        m_material = m_renderer.material;

        //If there is no material or image sequence
        if (m_imageSequences == null || m_material == false)
        {
            //Disable this component
            enabled = false;
        }
        else//If there is an image sequence and a material
        {
            //If there are buttons
            if (m_buttonSetters != null)
            {
                //Go through every button
                for (int i = 0; i < m_buttonSetters.Length; i++)
                {
                    //Suscribe to button events
                    SuscribeToButtonSetterEvent(m_buttonSetters[i]);
                }
            }

            //Go through every image sequence object
            for (int i = 0; i < m_imageSequences.Length; i++)
            {
                //Initialize the sequence
                m_imageSequences[i].Init();
            }

            //Set the default movie texture
            CUtilitySetters.SetMaterialTexture(ref m_material, m_defaultTexture);
        }


    }

    //PENDING
    private void Update()
    {
        m_timerNextFrame += Time.deltaTime;

        if (m_timerNextFrame > m_frameTime && m_movieState == EMovieState.Playing)
        {
            m_timerNextFrame = 0.0f;

            //If the current frame is over its duration
            if (m_imageSequences[m_currentlyActiveMovieIndex].PCurrentFrame >=
                m_imageSequences[m_currentlyActiveMovieIndex].PFramesOfAnimation)
            {
                //If the image sequence is looping
                if (m_imageSequences[m_currentlyActiveMovieIndex].PIsLooping == true)
                {
                    //Reset the current frame back to 0
                    m_imageSequences[m_currentlyActiveMovieIndex].PCurrentFrame = 0;
                }
                else//If the image sequence is not looping
                {
                    //Set this image sequence as not playing
                    m_imageSequences[m_currentlyActiveMovieIndex].PIsPlaying = false;

                    //Stop the current movie texture
                    StopActiveMovieTexture();
                }
            }
            else//If the current frame is withing the animation duration
            {
                //Don't use utility setter for optimization, since it needs to avoid if checks
                m_material.SetTexture(M_MATERIAL_TEXTURE_NAME, m_imageSequences[m_currentlyActiveMovieIndex].
                    m_animationTexture[m_imageSequences[m_currentlyActiveMovieIndex].PCurrentFrame]);

                //Increase the current frame
                m_imageSequences[m_currentlyActiveMovieIndex].PCurrentFrame++;
            }
        }
    }

    /*
    Description: Unsuscribe from button events and stop all coroutines.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void OnDestroy()
    {
        //If there are buttons
        if (m_buttonSetters != null)
        {
            //Go through every button
            for (int i = 0; i < m_buttonSetters.Length; i++)
            {
                //Unsuscribe from its event
                UnsuscribeToButtonSetterEvent(m_buttonSetters[i]);
            }
        }

        //Stop all the coroutines
        StopAllCoroutines();
    }

    /*
    Description:  Suscribe to the desired button texture related events.
    Parameters: CButtonSetAnimatedTexture aButtonSetter - The button that contains the events we will suscribe to.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void SuscribeToButtonSetterEvent(CButtonSetAnimatedTexture aButtonSetter)
    {
        //If the button is valid
        if (aButtonSetter != null)
        {
            //Suscribe to its event
            aButtonSetter.OnPlayTexture += SetAnimatedTexture;
            aButtonSetter.OnPauseTexture += PauseMovieTexture;
            aButtonSetter.OnStopTexture += StopMovieTexture;
        }
    }

    /*
    Description:  Unsuscribe from the desired button texture related events.
    Parameters: CButtonSetAnimatedTexture aButtonSetter - The button that contains the events we will unsuscrbie from.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void UnsuscribeToButtonSetterEvent(CButtonSetAnimatedTexture aButtonSetter)
    {
        ///If the button is valid
        if (aButtonSetter != null)
        {
            //Unsuscribe from its event
            aButtonSetter.OnPlayTexture -= SetAnimatedTexture;
            aButtonSetter.OnPauseTexture -= PauseMovieTexture;
            aButtonSetter.OnStopTexture -= StopMovieTexture;
        }
    }

    /*
    Description: Coroutine used to play an animated texture. This is done by constantly changing the object's material texture
                 according to the corresponding frame (image sequence texture)      
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    private void PlayMovieTexture()
    {
        //Set the movie state as playing
        m_movieState = EMovieState.Playing;

        //Set the current image sequence as playing
        m_imageSequences[m_currentlyActiveMovieIndex].PIsPlaying = true;

        //Calculate how many time will be between each frame change
        m_frameTime = 1.0f / m_imageSequences[m_currentlyActiveMovieIndex].PFramesPerSecond;
    }

    /*
    Description:  Set the desired animated texture as active one and play it.
    Parameters:  uint aMovieIndex - The index in the m_imageSequences of the desired animated texture to play
                 bool aRestartInPlay - Should the texture start at frame 0 when it starts playing  
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public void SetAnimatedTexture(uint aMovieIndex, bool aRestartInPlay)
    {
        //If the index is valid
        if (aMovieIndex < m_imageSequences.Length)
        {
            //If the desired image sequence is valid
            if (m_imageSequences[aMovieIndex] != null)
            {
                //Stop the previous texture
                m_imageSequences[m_currentlyActiveMovieIndex].PIsPlaying = false;

                //Set the new active movie index
                m_currentlyActiveMovieIndex = aMovieIndex;

                //If we want to restart the animation
                if (aRestartInPlay == true)
                {
                    //Set the current frame to 0
                    m_imageSequences[aMovieIndex].PCurrentFrame = 0;
                }

                //Play the movie
                PlayMovieTexture();
            }
        }
    }

    /*
    Description:  Stop the desired animated texture
    Parameters:  uint aMovieIndex - The index in the m_imageSequences of the desired animated texture to stop 
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public void StopMovieTexture(uint aMovieIndex)
    {
        m_movieState = EMovieState.Stop;

        //If the movie index is valid
        if (aMovieIndex < m_imageSequences.Length)
        {
            //If the image sequence is valid
            if (m_imageSequences[aMovieIndex] != null)
            {
                //Stop the movie texture
                m_imageSequences[aMovieIndex].PIsPlaying = false;

                //Reset its current frame back to 0
                m_imageSequences[aMovieIndex].PCurrentFrame = 0;
            }
        }

        //Set the default movie texture
        CUtilitySetters.SetMaterialTexture(ref m_material, m_defaultTexture);
    }

    /*
    Description:  Stop the currently active animated textures
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public void StopActiveMovieTexture()
    {
        //Stop the movie texture
        StopMovieTexture(m_currentlyActiveMovieIndex);
    }

    /*
    Description:  Pause the desired animated textures
    Parameters:  uint aMovieIndex - The index in the m_imageSequences of the desired animated texture to pause 
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public void PauseMovieTexture(uint aMovieIndex)
    {
        m_movieState = EMovieState.Paused;

        //If the index is valid
        if (aMovieIndex < m_imageSequences.Length)
        {
            //If the desired image is valid
            if (m_imageSequences[aMovieIndex] != null)
            {
                //Stop the movie texture but don't change its current frame
                m_imageSequences[m_currentlyActiveMovieIndex].PIsPlaying = false;
            }
        }
    }

    /*
    Description: Function to sort in editor alphanumerically all the frames/images of the image sequences 
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Context menu function accessible by right clicking the component.
    */
    [ContextMenu("Sort Image Sequence Alphanumerically")]
    private void SortImageSequences()
    {
        //If there are iamge sequences
        if (m_imageSequences != null)
        {
            //Go through every image sequence
            for (int i = 0; i < m_imageSequences.Length; i++)
            {
                //If the image sequence is valid
                if (m_imageSequences[i] != null)
                {
                    //Sort it alphanumerically
                    m_imageSequences[i].Sort();
                }
            }
        }
    }
}
