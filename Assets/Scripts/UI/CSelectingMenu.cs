using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Class for selecting menu
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
[RequireComponent(typeof(CWeaponPhysics))]
public class CSelectingMenu : MonoBehaviour
{
    //weapon script
    private CWeaponPhysics m_weapon;

    //Variable for raycasting
    private RaycastHit m_hit;
    private Ray m_ray;

    //Point from the head
    private Transform m_head;

    //Variable to check if the button is hovered or clicked
    private bool m_previousIsKeyPressed = false;
    private bool m_wasKeyPressedDown = false;
    private bool m_isKeyBeingPressed = false;

    private bool m_isWeaponGrabbed = false;

    //The current selectable object we are hovering
    private ISelectable m_hoveringSelectable;

    //The point where the raycast starts
    public Transform m_raycastPoint;

    [Header("Rumbling Features")]
    [Range(0, 3999)]
    public ushort m_shootingRumbleStrength = 500;
    public float m_shootingRumbleDuration = 0.15f;

    public Transform PRaycastPoint
    {
        set
        {
            m_raycastPoint = value;
        }
    }

    public Transform PHead
    {
        set
        {
            m_head = value;
        }
    }

    /*
    Description: Set weapon script
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    private void Awake()
    {
        m_weapon = GetComponent<CWeaponPhysics>();

        //If the weapon is grabbed
        if (m_weapon.PWeaponPhysiscsState == EWeaponPhysicsState.Grabbed)
        {
            m_isWeaponGrabbed = true;
        }

        //Suscribe to weapon event
        m_weapon.OnWeaponGrabbed += OnWeaponGrabbed;
        m_weapon.OnWeaponDropped += OnWeaponDropped;
    }

    /*
    Description: Unsuscribe from the weapon events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, Novemeber 23, 2016
    */
    private void OnDestroy()
    {
        //Unsuscribe from weapon event
        m_weapon.OnWeaponGrabbed -= OnWeaponGrabbed;
        m_weapon.OnWeaponDropped -= OnWeaponDropped;
    }

    /*
    Description: Inform this script that the weapon is grabbed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, Novemeber 23, 2016
    Extra Notes: Function called when the weapon OnWeaponGrabbed event is called.
    */
    private void OnWeaponGrabbed()
    {
        m_isWeaponGrabbed = true;
    }

    /*
    Description: Inform this script that the weapon is dropped.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, Novemeber 23, 2016
    Extra Notes: Function called when the weapon OnWeaponDropped event is called.
    */
    private void OnWeaponDropped()
    {
        m_isWeaponGrabbed = false;

        //If there is a currently hovering selectable
        if (m_hoveringSelectable != null)
        {
            //Unvhober it
            m_hoveringSelectable.OnUnHover();
        }

        //Set the hovering selectable as null
        m_hoveringSelectable = null;
    }

    /*
    Description: constantly raycasting, and if it hits the button will check if it's hovered or clicked base on if player press shoot button or not
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    private void Update()
    {
        //If there is a game manager and the weapon is grabbed.
        if (m_isWeaponGrabbed == true)
        {
            //If game state is not on Play
            if (CGameManager.PInstanceGameManager.PGameState != EGameStates.Play)
            {
                //If the player controller is valid
                if (CGameManager.PInstanceGameManager.PPlayerController != null)
                {
                    //See if the user has pressed the firing key
                    CheckFiringInput();

                    //Prepare the raycast point and other data
                    SetRaycast();

                    //Raycast  and call the corresponding functions on the objects hit
                    RaycastSelection();
                }
            }
        }
    }

    /*
    Description: Using the hand that is holding the weapon component, use the playerController
    to check if the user is pressing the firing weapon. This is using button press for continue press downs.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    Extra Notes: This function doesn't check the current game state, or if the gameManager or the playerController
    are valid
    */
    private void CheckFiringInput()
    {
        //Save the previous key that was pressed
        m_previousIsKeyPressed = m_isKeyBeingPressed;

        //According to the hand holding the weapon
        switch (m_weapon.PHoldingHand)
        {
            //No button being pressed
            case EWeaponHand.None:
                m_wasKeyPressedDown = false;
                m_isKeyBeingPressed = false;
                break;
            //Check right hand
            case EWeaponHand.RightHand:
                m_isKeyBeingPressed = CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl.GetIsFiringWeapon(true);
                m_wasKeyPressedDown = CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl.GetIsFiringWeapon(false);
                break;
            //Check left hand
            case EWeaponHand.LeftHand:
                m_isKeyBeingPressed = CGameManager.PInstanceGameManager.PPlayerController.PLeftWeaponControl.GetIsFiringWeapon(true);
                m_wasKeyPressedDown = CGameManager.PInstanceGameManager.PPlayerController.PLeftWeaponControl.GetIsFiringWeapon(false);
                break;
            //Check right or left hand
            case EWeaponHand.BothHands:
                m_isKeyBeingPressed = CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl.GetIsFiringWeapon(true) ||
                CGameManager.PInstanceGameManager.PPlayerController.PLeftWeaponControl.GetIsFiringWeapon(true);
                m_wasKeyPressedDown = CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl.GetIsFiringWeapon(false) ||
                CGameManager.PInstanceGameManager.PPlayerController.PLeftWeaponControl.GetIsFiringWeapon(false);
                break;
            default:
                break;
        }
    }

    /*
    Description: Set the required data to do the raycast, this depends on whether the
    control is the VR or nonVR
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    Extra Notes: This function doesn't check the current game state, or if the gameManager or the playerController
    are valid. Code written by Juan Calvin Raymond, Alvaro merely abstracted it into a function.
    */
    private void SetRaycast()
    {
        //If player is playing using VR
        if (CGameManager.PInstanceGameManager.PPlayerScript.PControllerType == EControllerTypes.ViveController)
        {
            //initialize the ray variable
            m_ray = new Ray(m_raycastPoint.position, m_raycastPoint.forward);
        }
        //If player is playing in NonVR
        else
        {
            if (m_head != null)
            {
                //Set the raycast to cast from the head in forward direction
                Ray headRay = new Ray(m_head.position, m_head.forward);
                RaycastHit hit;

                //If raycast hit something
                if (Physics.Raycast(headRay, out hit))
                {
                    if (m_raycastPoint != null)
                    {
                        //Find the direction from the raycast point to the point that the raycast hit
                        Vector3 direction = hit.point - m_raycastPoint.position;

                        //Set the raycast from from raycast point to the point
                        m_ray = new Ray(m_raycastPoint.position, direction);

                        //DEBUGLIST-AAA
                        //Debug.DrawRay(m_ray.origin, m_ray.direction * 500.0f, Color.yellow);
                    }
                }
                //If raycast didn't hit anything
                else
                {
                    //Set ray to ray from the head
                    m_ray = headRay;
                }
            }
        }
    }

    /*
    Description: Do a raycast in the forward direction, if the object hit implements the ISelectable interface,
                 call the appropiate methods.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, December 27, 2016
    Extra Notes: This function doesn't check the current game state, or if the gameManager or the playerController
    are valid.
    */
    private void RaycastSelection()
    {
        //If raycast hit something
        if (Physics.Raycast(m_ray, out m_hit))
        {
            //check if it's a selectable object
            ISelectable selectableObject = (ISelectable)m_hit.collider.GetComponent(typeof(ISelectable));
            if (selectableObject != null)
            {
                //if player press shoot button
                if (m_wasKeyPressedDown == true)
                {
                    //Call OnClick function
                    selectableObject.OnClick(m_hit.point);

                    //If the key is pressed, reset the currently hovering selectable
                    m_hoveringSelectable = null;
                }
                else if (m_isKeyBeingPressed == true)//If the button is being pressed
                {
                    //Call on pressed event
                    selectableObject.OnPress(m_hit.point);
                }
                //If the button was pressed the previous frame, but is no longer pressed this frame
                else if (m_previousIsKeyPressed == true && m_wasKeyPressedDown == false)
                {
                    //Call the OnUnclick object
                    selectableObject.OnUnClick(m_hit.point);
                }
                //if player didn't press any button
                else
                {
                    //if the selectable object is not the same selectable object
                    if (selectableObject != m_hoveringSelectable)
                    {
                        //If there was a hovering object
                        if (m_hoveringSelectable != null)
                        {
                            //Unhover the previous object 
                            m_hoveringSelectable.OnUnHover();
                        }

                        //Hover the new one
                        selectableObject.OnHover(m_hit.point);

                        //Make the controller rumble
                        CUtilityGame.RumbleControl(m_weapon.PHoldingHand, m_shootingRumbleDuration, m_shootingRumbleStrength);

                        //Set this as the new hovering object
                        m_hoveringSelectable = selectableObject;
                    }
                    else if (m_hoveringSelectable.GetIsSelected() == false)//Uf the selectable object is the hovering selectable
                    {
                        m_hoveringSelectable.OnHover(m_hit.point);
                    }

                }
            }
            else if (m_hoveringSelectable != null)
            {
                m_hoveringSelectable.OnUnHover();
                m_hoveringSelectable = null;//Set that the object is now null if we unhovered from it
            }
        }
        //If raycast hit nothing
        else
        {
            //If the previous object being hovered is valid
            if (m_hoveringSelectable != null)
            {
                m_hoveringSelectable.OnUnHover();//unhover the previous object
                m_hoveringSelectable = null;//Set that the object is now null if we unhovered from it
            }
        }

        //DEBUGLIST-AAA
        //Debug.DrawRay(m_ray.origin, m_ray.direction * 500.0f, Color.cyan);
    }


}
