using UnityEngine;
using System.Collections;

using System;

/*
Description: The base class for targets, that implement some common fucntions between different types of targets.
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, March 7th, 2017
*/
[RequireComponent(typeof(Collider))]
public abstract class ACoreTarget : MonoBehaviour, ITarget, IShootable
{
    private ETargetStates m_currentState = ETargetStates.Alive;
    private COnFireSystem m_onFireSystem;
    private Collider m_collider;
    private GameObject m_objectThatHit;

    private int m_health;

    [Header("Life Settings")]
    public int m_maxHealth;
    public float m_durationDying;
    public bool m_isRespawnable = false;
    public float m_respawnDuration;

    [Header("Score Settings")]
    public int m_scoreValue;
    public float m_styleIncreaseAmount = 0.04f;

    [Space(20)]
    public bool m_isBlockingShots = false;

    //Interfaces Events
    //IShootable
    public event delegOnObjectShot OnShot;

    //ITarget
    public event delegTargetDamaged OnTargetDamaged;
    public event delegTargetDying OnTargetDying;
    public event delegTargetReset OnTargetReset;

    public ETargetStates PCurrentState
    {
        get
        {
            return m_currentState;
        }
        set
        {
            m_currentState = value;
        }
    }

    public int PScoreValue
    {
        get
        {
            return m_scoreValue;
        }
        set
        {
            m_scoreValue = value;
        }
    }

    public int PMaxHealth
    {
        get
        {
            return m_maxHealth;
        }
        set
        {
            m_maxHealth = value;
        }
    }

    public int PHealth
    {
        get
        {
            return m_health;
        }
    }

    public GameObject PObjectThatHit
    {
        get
        {
            return m_objectThatHit;
        }
    }

    /*
    Description:Set the initial health and get the audio source 
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    protected virtual void Awake()
    {
        //Set the initial target state
        SetInitialState();

        //Set the current health of the target to max
        m_health = m_maxHealth;

        m_collider = GetComponent<Collider>();
    }

    /*
    Description:Set the on fire system script
    Creator: Juan Calvin Raymond
    Creation Date: 14 Mar 2017
    */
    protected void Start()
    {
        m_onFireSystem = CGameManager.PInstanceGameManager.POnFireSystem;

    }

    /*
    Description: Ensures all the coroutines have been properly stopped.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 23rd, 2017
    */
    protected void OnDestroy()
    {
        //Ensure all coroutines have been stopped
        StopAllCoroutines();
    }

    /*
    Description: Returns if the shootable object allows the shot to pass through it.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 21, 2017
    */
    public bool GetIsBlockingShot()
    {
        return m_isBlockingShots;
    }

    /*
    Description: Implementation of interface method. This function should be called when the target is hit by a "bullet". 
    Parameters:GameObject aHitter-The gameobject that hit the target
                int aDamage-The amount of damage the target will receive
                Vector3 aHitPosition-The position where the target was shot
                Vector3 aHitDirection- The direction from where the target was shot
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public void ObjectShot(GameObject aHitter, int aDamage, Vector3 aHitPosition, Vector3 aHitDirection)
    {
        m_objectThatHit = aHitter;

        //Apply damage
        ApplyDamage(aDamage);

        //If there is an onfire system
        if (m_onFireSystem != null)
        {
            //Increase style
            m_onFireSystem.ChangeStyleMeter(m_styleIncreaseAmount);
        }

        //If there is anyone suscribed to onshot event
        if (OnShot != null)
        {
            //Call it
            OnShot(aHitter, aDamage, aHitPosition, aHitDirection);
        }
    }

    /*
    Description: Implementation of interface method. This function should be called when the target is destroyed.
                 Current this will set the target state to dead, and disactivated.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public void DestroyTarget()
    {
        //Set target as dead
        SetDeadState();

        //Disable the objects
        gameObject.SetActive(false);

        //Ensure all coroutines have been stopped
        StopAllCoroutines();
    }

    /*
    Description: Implementation of interface method. This function is used to damage the target. If the target health reaches 0,
    //it sets the target as dying.
    Parameters(Optional): int aAmount-The amount of "life points" that will be added to current health. To actually reduce health
    //this value must be negative.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public void ApplyDamage(int aAmount)
    {
        //Decrease the health
        m_health -= aAmount;

        //Ge the health left as a percent
        float healthPercent = (float)m_health / (float)m_maxHealth;

        //If the target has more than the max health
        if (m_health > m_maxHealth)
        {
            //Set its health to max
            m_health = m_maxHealth;
        }
        else if (m_health <= 0)//If the target doesn't have health
        {
            //Disable the collider
            m_collider.enabled = false;//No "if" check done, since it is a required component

            //Set target as dying
            SetDyingState();

            //Start dying function
            StartCoroutine(WaitWhileDying());

            //If event has suscribers
            if (OnTargetDying != null)
            {
                //Call the event
                OnTargetDying(m_durationDying);
            }
        }

        //If event is valid
        if (OnTargetDamaged != null)
        {
            //Call event that target has been damaged
            OnTargetDamaged(aAmount, m_health, healthPercent, m_scoreValue);
        }
    }

    /*
    Description: Reset the target after it has been destroyed. This enable the target and reset its health.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    public void Reset()
    {
        //Set the initial state of the target
        SetInitialState();

        //Make the object active
        gameObject.SetActive(true);

        //Replenish the target health
        m_health = m_maxHealth;

        //Reset the collider
        m_collider.enabled = true;

        //If there are suscribers to the target reset event
        if (OnTargetReset != null)
        {
            //Call the event
            OnTargetReset();
        }
    }

    /*
    Description: Set the initial state of the target. This is called in the ACoreTarget Awake function.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    protected virtual void SetInitialState()
    {
        PCurrentState = ETargetStates.Alive;
    }

    /*
    Description: Set the dying state of the target. This is called in the ACoreTarget ApplyDamage function,
                 once the target has reached 0 health.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    protected virtual void SetDyingState()
    {
        PCurrentState = ETargetStates.Dying;
    }

    /*
    Description: Set the dead state of the target. This is called in the ACoreTarget DestroyTarget function.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    protected virtual void SetDeadState()
    {
        PCurrentState = ETargetStates.Dead;
    }

    /*
    Description: Coroutine function called when a target health is set to 0. This after the duration dying has passed,
                 regardless of the game manager time scale, will either destroy the target or kill it.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 23th, 2017
    Extra Notes: This function uses Unity time scale, and not the game manager time scale
    */
    protected virtual IEnumerator WaitWhileDying()
    {
        //Wait for the target to die
        yield return new WaitForSeconds(m_durationDying);

        //If the target is respawnable
        if (m_isRespawnable == true)
        {
            Reset();
        }
        else//If target is not respawnable
        {
            DestroyTarget();//Destroy the target
        }

        yield return null;
    }
}
