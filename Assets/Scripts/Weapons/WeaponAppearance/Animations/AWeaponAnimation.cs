using UnityEngine;
using System.Collections;

/*
Description: Class that manage weapon animation by subscribing to weapon OnFire event
Creator: Juan Calvin Raymond
Creation Date: 20 Dec 2016
*/
[RequireComponent(typeof(AWeapon))]
public abstract class AWeaponAnimation : MonoBehaviour
{
    //Weapon script
    protected AWeapon m_weapon;
    protected CWeaponPhysics m_weaponPhysics;

    //Player Weapon Control Input
    protected CWeaponControlInput m_playerWeaponController;

    //Animation variables
    protected Animator m_animatorGun;
    protected float m_triggerDefaultLocalRotation;
    protected float m_triggerPressedAmount;

    [Header("Trigger Press")]
    [Tooltip("Trigger GameObject")]
    public Transform m_trigger;

    [Tooltip("Max pressed amount for Trigger GameObject")]
    public float m_triggerPressedMaxRotation = 25.0f;

    public float PTriggerPressedAmount
    {
        set
        {
            m_triggerPressedAmount = value;
        }
    }

    /*
    Description: Initialize all Variable
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    Extra Notes:
    */
    protected void Awake()
    {
        //Get animator script
        m_animatorGun = GetComponentInChildren<Animator>();

        //Get weapon script
        m_weapon = GetComponent<AWeapon>();
        m_weaponPhysics = GetComponent<CWeaponPhysics>();

        //Save the starting local rotation of the object
        if (m_trigger != null)
        {
            m_triggerDefaultLocalRotation = m_trigger.localEulerAngles.x;
        }
    }

    /*
    Description: Subscribe to weapon OnFire event and get the player controller
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    protected virtual void Start()
    {
        //Suscribe to weapon events
        m_weapon.OnFire += PlayFireAnimation;

        //If there are weapon physics
        if (m_weaponPhysics != null)
        {
            //Suscribe to weapon physics event
            m_weaponPhysics.OnWeaponGrabbed += SetPlayerWeaponControl;
        }

        //Set initial player weapon control
        SetPlayerWeaponControl();

        //If there is a controller and a trigger
        if ( m_trigger!=null)
        {
            //Update the trigger rotation according to control
            StartCoroutine(CheckTriggerRotation());
        }
    }

    /*
    Description: Unsubscribe to weapon OnFire event
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    protected void OnDestroy()
    {
        //Unsuscribe from weapons events
        m_weapon.OnFire -= PlayFireAnimation;

        //If there are weapon physics
        if (m_weaponPhysics != null)
        {
            //Unsuscribe from weapon physics event
            m_weaponPhysics.OnWeaponGrabbed -= SetPlayerWeaponControl;
        }

        //Ensure all coroutines are stopped
        StopAllCoroutines();
    }


    /*
    Description:Coroutine function to animate the trigger being pressed according to player rotation. This coroutine
                runs in an loop until the object is destroyed, calling stop all coroutines
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, December 1, 2016
    */
    protected IEnumerator CheckTriggerRotation()
    {
        //Wait until the controls have been set
        while(m_playerWeaponController==null)
        {
            //Set the controls
            SetPlayerWeaponControl();

            yield return null;
        }

        //This coroutine runs in an loop until the object is destroyed, calling stop all coroutines
        while (true)
        {
            //If the weapon is grabbed
            if (m_weaponPhysics.PWeaponPhysiscsState == EWeaponPhysicsState.Grabbed)
            {
                //Get the amount the trigger is pressed from the controller
                m_triggerPressedAmount = m_playerWeaponController.GetTriggerPressedAxis();

                //The trigger rotation will be the percent pressed, between the starting rotation and the maximum rotation
                float currentTriggerRotation = Mathf.Lerp(m_triggerDefaultLocalRotation, m_triggerPressedMaxRotation, m_triggerPressedAmount);
                m_trigger.localEulerAngles = new Vector3(currentTriggerRotation, m_trigger.localEulerAngles.y, m_trigger.localEulerAngles.z);
            }

            //Wait for next frame
            yield return null;
        }
    }

    /*
    Description: Function to set the player Weapon Control Input variable according to the hand currently
                 holding the weapon.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, March 12th, 2017
    */
    protected void SetPlayerWeaponControl()
    {
        //According to the hand currently holdign the weapon
        switch (m_weapon.PHoldingHand)
        {
            case EWeaponHand.None:
                break;
            case EWeaponHand.RightHand:
                //Set controller for right
                m_playerWeaponController = CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl;
                break;
            case EWeaponHand.LeftHand:
                //Set controller for left hand
                m_playerWeaponController = CGameManager.PInstanceGameManager.PPlayerController.PLeftWeaponControl;
                break;
            case EWeaponHand.BothHands:
                //Not properly implemented, set controller for right hand
                m_playerWeaponController = CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl;
                break;
            default:
                break;
        }
    }

    /*
    Description: Vibrate respective controller and play weapon animation
    Parameters: aCurrentAmmo : weapon's current ammo
                          aWeaponHand : hand that hold the weapon
                          aTimeWhenShot : time when weapon shoot
    Creator: Juan Calvin Raymond
    Creation Date:  20 Dec 2016
    */
    protected virtual void PlayFireAnimation(int aCurrentAmmo, EWeaponHand aWeaponHand)
    {
        //Call FireAnimation function
        FireAnimation();
    }

    /*
    Description: Abstract function where any weapon animation class need to implement
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016 
    */
    protected abstract void FireAnimation();
}