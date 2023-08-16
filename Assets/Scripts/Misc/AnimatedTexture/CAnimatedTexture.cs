using UnityEngine;
using System.Collections;

using System;

/*
Description: Class used to store a series of images as an "animated" texture. The class contains information
             about how the animated texture should be played                  
Creator: Alvaro Chavez Mixco
Creation Date: Monday, March 20th, 2017
*/
[Serializable]
public class CAnimatedTexture
{
    private bool m_isPlaying = false;
    private float m_durationOfAnimation;
    private uint m_framesOfAnimation;

    private uint m_currentFrame = 0;

    [SerializeField]
    public float m_framesPerSecond = 30.0f;

    [Tooltip("The images that make each frame of the animated texture.")]
    [SerializeField]
    public Texture[] m_animationTexture;

    [SerializeField]
    public bool m_isLooping = true;

    [Tooltip("If the image sequence should be sorted at start alphanumericaly.")]
    [SerializeField]
    public bool m_sortAtStart = false;

    public float PFramesPerSecond
    {
        get
        {
            return m_framesPerSecond;
        }
    }

    public uint PFramesOfAnimation
    {
        get
        {
            return m_framesOfAnimation;
        }
    }

    public float PDurationOfAnimation
    {
        get
        {
            return m_durationOfAnimation;
        }
    }

    public uint PCurrentFrame
    {
        get
        {
            return m_currentFrame;
        }

        set
        {
            m_currentFrame = value;
        }
    }

    public bool PIsPlaying
    {
        get
        {
            return m_isPlaying;
        }

        set
        {
            m_isPlaying = value;
        }

    }

    public bool PIsLooping
    {
        get
        {
            return m_isLooping;
        }

        set
        {
            m_isLooping = value;
        }

    }

    /*
    Description: Function to set the initial stats of an animated texture.                
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: This is not done in the constructor, since the variables are made
                 to be set in Unity Inspector. So it is unclear at what time the constructor 
                 would actually get called.
    */
    public void Init()
    {
        //If there are images to make an animation
        if (m_animationTexture != null)
        {
            //Get the number of frames accodrding t
            m_framesOfAnimation = (uint)(m_animationTexture.Length);

            //If the image sequence will be sorted alphanumerically at start
            if(m_sortAtStart==true)
            {
                //Sort the image sequence
                Sort();
            }
        }

        //Set starting values
        m_currentFrame = 0;
        m_durationOfAnimation = m_framesOfAnimation * m_framesPerSecond;
        m_isPlaying = false;
    }

    /*
    Description: Sort the image sequence array              
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public void Sort()
    {
        //If texture array is valid
        if(m_animationTexture!=null)
        {
            //Sort texture
            CUtilitySorting.SortArray(ref m_animationTexture);
        }
    }
}
