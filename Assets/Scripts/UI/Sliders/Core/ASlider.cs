using UnityEngine;
using System.Collections;
using System;

/*
Description: Abstract class to make a slider that works with VR. This class works by 
             implementing the ISelectable interface so that it can work in conjuction
             with the Selecting Menu script. This script works by having a large "bar" object
             that the player aim at, at the extremes of this object there is a min and max objects
             that serve to mark the boundaries of the slider. Then there is a slider object that 
             will moved wherever in the bar object the player is aiming at.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public abstract class ASlider : MonoBehaviour, ISelectable
{
    private bool m_isSelected = false;

    //Material for the slider object. This is only the slider object that actually moves in the slider
    private Material m_sliderMaterial;

    //Audio Source for the slider object. This is only the slider object that actually moves in the slider
    private AudioSource m_audioSource;

    protected float m_sliderPercentValue = 0.0f;

    [Header("Appearance Settings")]
    public Texture m_sliderDefaultTexture;
    public Texture m_sliderSelectedTexture;

    [Header("Sound Settings")]
    public AudioClip m_clickSound;
    public AudioClip m_unClickSound;

    [Header("Slider Game Objects")]
    public GameObject m_sliderGameObject;
    public GameObject m_minGameObjectLimit;
    public GameObject m_maxGameObjectLimit;

    [Header("Slider Settings")]
    public bool m_unselectOnUnHover = true;

    public delegate void delegSliderPercentageChange(float aPercent);

    public event delegSliderPercentageChange OnSliderValueChange;

    public float PSliderPercentValue
    {
        get
        {
            return m_sliderPercentValue;
        }

        set
        {
            m_sliderPercentValue = value;

            //Ensure the value being set is between 0 and 1
            m_sliderPercentValue = Mathf.Clamp01(m_sliderPercentValue);

            //Set the position of the slider object
            MoveSliderObject(m_sliderPercentValue);
        }
    }

    /*
    Description: Get the material and audiosource of the object, and sets its initial texture.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Start()
    {
        //If there is a slider game object
        if (m_sliderGameObject != null)
        {
            //Get the renderer of the object
            MeshRenderer tempRenderer = m_sliderGameObject.GetComponent<MeshRenderer>();

            //If the slider has a renderer
            if (tempRenderer != null)
            {
                //Get its material
                m_sliderMaterial = tempRenderer.material;
            }

            //Get the audio source of the object
            m_audioSource = m_sliderGameObject.GetComponent<AudioSource>();

        }

        //Set the initial texture of the object
        CUtilitySetters.SetMaterialTexture(ref m_sliderMaterial, m_sliderDefaultTexture);
    }

    /*
    Description: When this object is disabled, ensure that it sets the current values fo the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2017
    */
    private void OnDisable()
    {
        //If there is a lsider object
        if(m_sliderGameObject!=null)
        {
            //Call OnPress to ensure the values are set correctly
            OnPress(m_sliderGameObject.transform.position);

            //Call OnUnclick to ensure the button is no longer selected
            OnUnClick(m_sliderGameObject.transform.position);
        }
    }

    /*
    Description: Function to know if the current selectable object is currently being selected by any object
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2016
    */
    public bool GetIsSelected()
    {
        //Return if the slider is selected or not
        return m_isSelected;
    }

    /*
    Description: Set the slider as clicked, and change its texture and play a sound.
    Paramters: Vector3 aHitPosition - The world position where the slider was clicked
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called by the selecting menu script when the slider is clicked.
    */
    public virtual void OnClick(Vector3 aHitPosition)
    {
        //If the slider wasn't previously selected
        if (m_isSelected == false)
        {
            //Set its texture as selected
            CUtilitySetters.SetMaterialTexture(ref m_sliderMaterial, m_sliderSelectedTexture);

            //Play the  click sound
            CUtilitySound.PlaySoundOneShot(m_audioSource, m_clickSound);

            //Set that the slider was selected
            m_isSelected = true;
        }
    }

    /*
    Description: Set that the button isn't clicked anymore (reset textures, play sound, etc.)
    Paramters: Vector3 aHitPosition - The world position where the slider was unclicked
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called by the selecting menu script when the slider is unclicked.
    */
    public void OnUnClick(Vector3 aHitPosition)
    {
        //If the slider was previously selected
        if (m_isSelected == true)
        {
            //Set its texture as default
            CUtilitySetters.SetMaterialTexture(ref m_sliderMaterial, m_sliderDefaultTexture);

            //Play the unclick sound
            CUtilitySound.PlaySoundOneShot(m_audioSource, m_unClickSound);

            //Set that the slider is no longer selected
            m_isSelected = false;
        }
    }

    /*
    Description: Empty virtual function so that it can be overwritten.
    Paramters: Vector3 aHitPosition - The world position where the slider was hovered
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called by the selecting menu script when the slider is hovered.
    */
    public virtual void OnHover(Vector3 aHitPosition)
    {
    }

    /*
    Description: Called when the slider is unhovered. 
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called by the selecting menu script when the slider is hovered.
    */
    public virtual void OnUnHover()
    {

        //If the menu was selected
        if (m_isSelected == true && m_unselectOnUnHover == true)
        {
            //If there was a slider game object
            if (m_sliderGameObject != null)
            {
                //Call the onUnClick event with the current position of the slider object (so it won't move)
                OnUnClick(m_sliderGameObject.transform.position);
            }
            else//If there is no unslider game object
            {
                //Call the OnUnClick event with this object position
                OnUnClick(transform.position);
            }
        }
    }

    /*
    Description: If the slider is being pressed,  calculate the percent value of the slider and move it
                 to the position being pointed by the selecting menu.
    Paramters: Vector3 aHitPosition - The world position where the slider was pressed
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Called by the selecting menu script when the slider is pressed.
    */
    public virtual void OnPress(Vector3 aHitPosition)
    {
        //If the button is currently pressed
        if (m_isSelected == true)
        {
            //Calculate the value of the slider
            CalculateSliderPercent(aHitPosition);

            //Move the slider
            MoveSliderObject(m_sliderPercentValue);

            //If the event is valid
            if (OnSliderValueChange != null)
            {
                //Call the event the slider value changed
                OnSliderValueChange(m_sliderPercentValue);
            }
        }
    }

    /*
    Description: Abstract function used to calculate the percent value of the slider.
    Paramters: Vector3 aWorldHitPosition - The world position where the slider was pressed
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected abstract void CalculateSliderPercent(Vector3 aWorldHitPosition);

    /*
    Description: Abstract function used to move the slider according to its percent value.
    Paramters: float aPercent - The current percent value of the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public abstract void MoveSliderObject(float aPercent);
}
