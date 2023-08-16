using UnityEngine;
using System.Collections;
using System;

/*
Description: Abstract class of button, contain basic behaviour of button
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
[DisallowMultipleComponent]//Ensure that there is only 1 button per game object
public class CButton : MonoBehaviour, ISelectable
{
    //Audio Source component
    private AudioSource m_audioSource;

    private float m_timerExecution;

    //Flaggs to know button status
    private bool m_otherButtonClicked = false;//Call through events to know if another button is setting this
    private bool m_isClicked;
    private bool m_isHovering;

    //Material Component
    protected Material m_material;

    //Texture variable
    [Header("Appearance Settings")]
    public Texture m_normalTexture;
    public Texture m_hoverTexture;
    public Texture m_clickTexture;
    [Tooltip("An additional object, and its children, we want to change the texture of. " +
        "This is usually a children of the button object, if it is made of multiple parts. " +
        "The button and the other object are assigned the same texture.")]
    public GameObject m_otherObjectToChangeTexture;

    //sound file variable
    [Header("Sound Settings")]
    public AudioClip[] m_hoverSound;
    public AudioClip[] m_clickSound;

    [Space(20)]
    //variable for how long will it take for the button execute
    public float m_executionTime = 0.0f;

    //Events so that other classes can suscribe to it
    public delegate void delegateButtonEvent();
    public event delegateButtonEvent OnHoverEvent;    //On Hover
    public event delegateButtonEvent OnUnHoverEvent;//On Unhover
    public event delegateButtonEvent OnClickEvent;//On Click
    public event delegateButtonEvent OnUnClickEvent;//On Unclick
    public event delegateButtonEvent OnPressEvent;
    public event delegateButtonEvent OnExecutionEvent;//On Execution


    public bool PIsClicked
    {
        get
        {
            return m_isClicked;
        }
    }

    public bool PIsHovering
    {
        get
        {
            return m_isHovering;
        }
    }

    protected float PExecutionTime
    {
        get
        {
            return m_executionTime;
        }

        set
        {
            m_executionTime = value;
        }
    }

    public bool POtherButtonClicked
    {
        get
        {
            return m_otherButtonClicked;
        }

        set
        {
            m_otherButtonClicked = value;
        }
    }

    /*
    Description: Initialize all the variable
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    protected virtual void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            m_material = GetComponent<MeshRenderer>().material;
        }

        m_isHovering = false;
        m_isClicked = false;
        m_timerExecution = m_executionTime;

        //Set the texture for the button
        SetButtonTextures(m_normalTexture);
    }

    /*
    Description: Find is the button is hovering or clicked
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    protected virtual void Update()
    {
        if (m_isClicked == true)//if the button has been pressed
        {
            m_timerExecution -= Time.unscaledDeltaTime;//Decrease timer
            

            //If timer is over
            if (m_timerExecution < 0.0f)
            {
                //m_timerExecution = m_executionTime;//reset the timer
                m_isClicked = false;

                //If there is any listener to the event
                if (OnExecutionEvent != null)
                {
                    //Call the execution event
                    OnExecutionEvent();

                    //Ensure that after this execution, this button sets itself as not being clicked
                    POtherButtonClicked = false;
                }
            }

            OnUnHover();
        }
    }

    /*
    Description: When disabled, reset the clicked and hovering status of the button so that
                they can be used if the button is enabled again.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnDisable()
    {
        //Reset the button conditions
        m_isClicked = false;
        m_isHovering = false;
    }

    /*
    Description: Helper function to set the texture of the button and any other object we 
                 want to change the texture of
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void SetButtonTextures(Texture aTexture)
    {
        //Set the texture for this object
        CUtilitySetters.SetMaterialTexture(ref m_material, aTexture);

        //Set the texture for other object and the children it may have
        CUtilitySetters.SetTextureInObjectAndChildren(m_otherObjectToChangeTexture, aTexture);
    }

    /*
    Description: Function to know if the current selectable object is currently being selected by any object
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2016
    */
    public bool GetIsSelected()
    {
        //Return that the button is selected if it is either being clicked or hovering
        return m_isClicked || m_isHovering;
    }

    /*
    Description: this function get called when button is clicked
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    public virtual void OnClick(Vector3 aHitPosition)
    {
        //DEBUGLIST-AAA
        //Debug.Log(m_otherButtonClicked);

        //If the button hasn't been clicked yet, and no other button has been clicked
        if (m_isClicked == false && m_otherButtonClicked == false)
        {
            //Set the texture for the button
            SetButtonTextures(m_clickTexture);

            //Play the click sound
            CUtilitySound.PlayRandomSound(m_audioSource, m_clickSound);

            //Set the time for the execution
            m_timerExecution = m_executionTime;

            m_isClicked = true;

            //If there is any listener to the event
            if (OnClickEvent != null)
            {
                //Call the event
                OnClickEvent();
            }
        }
    }

    /*
    Description: This function get called when the user was pressing the clicked button in the selecting menu,
                 and then he stop clicking the button. The function merely calls the OnUnClickEvent
    Creator: Alvaro Chavez Mixco
    Creation Date: 11-1-2016
    */
    public void OnUnClick(Vector3 aHitPosition)
    {
        //If there event is valid
        if (OnUnClickEvent != null)
        {
            //Call the event
            OnUnClickEvent();
        }
    }

    /*
    Description: This function get called when button is hovered
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    public virtual void OnHover(Vector3 aHitPosition)
    {
        //If the button has not been clicked, and it currently wating for execution
        if (m_isClicked == false)
        {
            //If there is any listener to the event
            if (OnHoverEvent != null)
            {
                OnHoverEvent();//Call the event
            }

            //Set the texture for the button
            SetButtonTextures(m_hoverTexture);

            //If the button wasn't hovering befores
            if (m_isHovering == false)
            {
                //Play the hover sound
                CUtilitySound.PlayRandomSound(m_audioSource, m_hoverSound);
            }
        }

        //Set that the button is hovering
        m_isHovering = true;
    }

    /*
    Description: This function gets called when the selecting menu raycast is no longer hitting this button.
    Creator: Alvaro Chavez Mixco
    Creation Date: 11-1-2016
    */
    public virtual void OnUnHover()
    {
        //If the button has not been clicked, and it currently wating for execution
        if (m_isClicked == false)
        {
            //If there is any listener to the event
            if (OnUnHoverEvent != null)
            {
                OnUnHoverEvent();//Call the event
            }

            //Set the texture for the button
            SetButtonTextures(m_normalTexture);
        }

        //Set that the button is not hovering
        m_isHovering = false;
    }

    /*
    Description: This function gets called when the selecting menu raycast detects the user is
                 holding the continously pressing the button.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnPress(Vector3 aHitPosition)
    {
        //If event is valid
        if (OnPressEvent != null)
        {
            //Call event
            OnPressEvent();
        }
    }

    /*
    Description: Function to sort in editor alphanumerically all the sound arrays 
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Context menu function accessible by right clicking the component.
    */
    [ContextMenu("Sort Sounds Alphanumerically")]
    public void SortImageSequences()
    {
        CUtilitySorting.SortArray(ref m_hoverSound);
        CUtilitySorting.SortArray(ref m_clickSound);
    }
}