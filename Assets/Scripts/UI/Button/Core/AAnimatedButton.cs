using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;


/*
Description: Class that inherits from CButton, this class works the
            same as CButton (detect click,hover,etc. events), but it can
            play animations.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
[RequireComponent(typeof(Animator))]
public class AAnimatedButton : CButton
{
    private const string M_DEFAULT_ANIMATION_NAME = "Default";
    private const string M_CLICKED_ANIMATION_NAME = "Clicked";

    //struct transform, for easier management of data and avoid possible pointer issues
    private Dictionary<GameObject, STransform> m_initialObjectsTransform;

    private const string M_CLICK_TRANSITION_TRIGGER_NAME = "m_isClicked";
    private const string M_HOVER_TRANSITION_BOOL_NAME = "m_isHovered";

    private Animator m_buttonAnimator;

    [Header("Animation Settings")]
    public bool m_playClickAnimation = true;
    public bool m_playHoverAnimation = false;
    public bool m_stopOnUnhover = false;
    [Tooltip("If the execution time of the button will match the time of the click animation.")]
    public bool m_matchExecutionTimeWithAnimation = true;
    [Tooltip("If matching execution time with animation. Amount of time when using match execution time with animation," 
    + "to ensure the animation properly finishes playing")]
    public float m_animationFinishTimeBias = 1.0f;

    [Tooltip("The label object that will be hidden when the click animation is played.")]
    public GameObject m_labelToHide;


    /*
    Description: Call the parent awake, and get the animator component.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected override void Awake()
    {
        //Call the base awake
        base.Awake();

        //Get the animator component
        m_buttonAnimator = GetComponent<Animator>();
    }

    /*
    Description: At start, save the initial transform of all the object children.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected void Start()
    {
        //Save the initial transform of its children
        SaveChildrenObjectTransforms();
    }

    /*
    Description: Save the initial transform of all the object children.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void SaveChildrenObjectTransforms()
    {
        //If the object has children
        if (gameObject.transform.childCount > 0)
        {
            //Initialize the dictionary
            m_initialObjectsTransform = new Dictionary<GameObject, STransform>();

            //Get all the children's transform
            Transform[] childrenTransform = gameObject.GetComponentsInChildren<Transform>();

            //Go through all the transform
            for (int i = 0; i < childrenTransform.Length; i++)
            {
                //Create a struct transform, for easier management of data and avoid possible pointer issues
                STransform tempTransform = new STransform(childrenTransform[i]);

                //From the transform, get the gameobject, and add it to the dictionary
                m_initialObjectsTransform.Add(childrenTransform[i].gameObject, tempTransform);
            }
        }
    }

    /*
    Description: Set the transform of all the children object to match
                 the transform that was saved at start.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void ResetChildrenObjectTransform()
    {
        //If the dictionary is valid
        if (m_initialObjectsTransform != null)
        {
            //Go through all the entries in the dictionary
            foreach (KeyValuePair<GameObject, STransform> entry in m_initialObjectsTransform)
            {
                //If the gameobjet is valid
                if (entry.Key != null)
                {
                    //Set its transform to match the one stored
                    CUtilitySetters.SetTransformLocalValues(entry.Key.transform, entry.Value);
                }
            }
        }
    }


    /*
    Description: Set the animation to its default state, at frame 0.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void ResetAnimation()
    {
        //Reset the animation
        m_buttonAnimator.Play(M_DEFAULT_ANIMATION_NAME, 0, 0.0f);
    }

    /*
    Description: If applicable play an animation, before calling the base OnClick function.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public override void OnClick(Vector3 aHitPosition)
    {
        //If it hasn't been previously clicked (play animation only once)
        if (PIsClicked == false && POtherButtonClicked == false)
        {
            //If it will play an animation when clicked
            if (m_playClickAnimation == true)
            {
                //Set the trigger in the animator to transition to the click animation
                CUtilitySetters.SetAnimatorTriggerParameter(ref m_buttonAnimator, M_CLICK_TRANSITION_TRIGGER_NAME);

                if (m_labelToHide != null)
                {
                    m_labelToHide.SetActive(false);
                }

            }
        }

        if (m_matchExecutionTimeWithAnimation == true)
        {
            //Save the execution time of the click animation
            PExecutionTime = m_buttonAnimator.GetCurrentAnimatorStateInfo(0).length + m_animationFinishTimeBias;
        }

        //Placed at the end, so that if applicable, click is set to true after this function
        //has run at least once
        base.OnClick(aHitPosition);
    }


    /*
    Description: If applicable play an animation, before calling the base OnHover function.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public override void OnHover(Vector3 aHitPosition)
    {
        //If the button is not clicked and is not hovering
        if (PIsClicked == false && PIsHovering == false)
        {
            //If it will play an animation when hovered
            if (m_playHoverAnimation == true)
            {
                //Set the bool to transition to the hover animation
                CUtilitySetters.SetAnimatorBoolParameter(ref m_buttonAnimator, M_HOVER_TRANSITION_BOOL_NAME, true);
            }
        }

        //Placed at the end, so that if applicable, hovering is set to true after this function
        //has run at least once
        base.OnHover(aHitPosition);
    }

    /*
    Description: If applicable stop the currently playing animation, before calling the base OnUnHover function.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public override void OnUnHover()
    {
        //If the button is  not clicked and is hovering
        if (PIsClicked == false && PIsHovering == true)
        {
            //If it will stop the animation when unhovered
            if (m_stopOnUnhover == true)
            {
                //Set the bool to transition back to the default animation
                CUtilitySetters.SetAnimatorBoolParameter(ref m_buttonAnimator, M_HOVER_TRANSITION_BOOL_NAME, false);
            }
        }

        //Placed at the end, so that if applicable, hovering is set to false after this function
        //has run at least once
        base.OnUnHover();
    }

    /*
    Description: Call the base disable, and reset the transform of all the children objects.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public override void OnDisable()
    {
        base.OnDisable();

        //Reset the transform of all the children object
        ResetChildrenObjectTransform();
    }

    /*
    Description: When the button is enabled, ensure it is showing the label, and that its animation
                its on its starting state.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnEnable()
    {
        //If there is a label to hide
        if (m_labelToHide != null)
        {
            //Ensure it is being shown
            m_labelToHide.SetActive(true);

            //Reset the animation
            ResetAnimation();
        }
    }
}
