using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A projectile class script, contain common properties that a projectile have also Motion and Collision behaviour
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
Extra Notes: Can easily changed behaviour without need to make new class
*/
public class CProjectile : MonoBehaviour
{
    //Components
    private MeshRenderer[] m_renderers;

    //Variable for projectile to check if it collide with something
    private Vector3 m_previousPosition;

    //Common properties that a projectile will have
    private Vector3 m_firingDir;
    private Vector3 m_velocity;
    private float m_projectileSpeed;
    private CProjectileWeapon m_ownerWeapon;
    private float m_timerLife;
    private float m_timerDying;
    private float m_timeWhenShot;

    //Variable to cast it
    private IMotionBehaviour m_motionBehaviour;
    private ICollisionBehaviour m_collisionBehaviour;
    private IDeathBehaviour m_deathBehaviour;
    private IMiscBehaviour m_miscBehaviour;

    //Bool to check if the projectile is colliding with something
    private bool m_isDead = false;
    private bool m_isDeadDueCollision = false;

    //Layer mask of what layer the projectile won't collide
    private int m_layerMask;

    //Variable to tweak in the editor
    public float m_lifeTime;
    public float m_dyingTime;
    public int m_damage;

    //Variable to set the behaviour
    public MonoBehaviour m_startMotionBehaviour;
    public MonoBehaviour m_startCollisionBehaviour;
    public MonoBehaviour m_startDeathBehaviour;
    public MonoBehaviour m_startMiscBehaviour;

    public delegate void delegProjectileHit(GameObject aObjectHit, Vector3 aHitPosition, Vector3 aHitNormal,int aDamage);
    public event delegProjectileHit OnProjectileHit;

    public delegate void delegProjectileDeath(bool aDiedDueCollision);
    public event delegProjectileDeath OnProjectileDeath;


    public int PLayerMask
    {
        get
        {
            return m_layerMask;
        }
    }

    public bool PIsDead
    {
        get
        {
            return m_isDead;
        }
    }

    public Vector3 PVelocity
    {
        get
        {
            return m_velocity;
        }
    }

    public Vector3 PFiringDir
    {
        get
        {
            return m_firingDir;
        }
    }

    public float PSpeed
    {
        get
        {
            return m_projectileSpeed;
        }

        set
        {
            m_projectileSpeed = value;
        }
    }

    public CProjectileWeapon POwnerWeapon
    {
        get
        {
            return m_ownerWeapon;
        }
    }

    public IMotionBehaviour PMotionBehaviour
    {
        set
        {
            m_motionBehaviour = value;

        }
    }

    public ICollisionBehaviour PCollisionBehaviour
    {
        set
        {
            m_collisionBehaviour = value;
        }
    }

    public IDeathBehaviour PDeathBehaviour
    {
        set
        {
            m_deathBehaviour = value;
        }
    }

    public IMiscBehaviour PMiscBehaviour
    {
        set
        {
            m_miscBehaviour = value;
        }
    }

    public float PTimeWhenShot
    {
        get
        {
            return m_timeWhenShot;
        }
    }

    public float PLifeTime
    {
        get
        {
            return m_lifeTime;
        }
    }

    public float PLifeTimer
    {
        get
        {
            return m_timerLife;
        }
    }

    /*
    Description: Initialize all variable
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    private void Awake()
    {
        //Get the mesh renderer component
        m_renderers = GetComponentsInChildren<MeshRenderer>();

        //Cast all the behaviors
        if (m_startMotionBehaviour != null)
        {
            PMotionBehaviour = m_startMotionBehaviour as IMotionBehaviour;
        }

        if (m_startCollisionBehaviour != null)
        {
            PCollisionBehaviour = m_startCollisionBehaviour as ICollisionBehaviour;
        }

        if (m_startMiscBehaviour != null)
        {
            PMiscBehaviour = m_startMiscBehaviour as IMiscBehaviour;
        }

        if (m_startDeathBehaviour != null)
        {
            PDeathBehaviour = m_startDeathBehaviour as IDeathBehaviour;
        }
    }

    /*
    Description: Initialize all variable
    Parameters: aSpawnpoint : point to spawn
                aRotationToSpawn : rotation to spawn
                aSpeed : projectile starting speed
                aOwnerWeapon : weapon that spawn this projectile
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    public void Init(Vector3 aSpawnPoint, Quaternion aRotationToSpawn, float aSpeed, CProjectileWeapon aOwnerWeapon)
    {
        //Set the layer mask
        m_layerMask = aOwnerWeapon.PLayerMask;

        //Set object position and rotation
        transform.position = aSpawnPoint;
        transform.rotation = aRotationToSpawn;

        //Set all paramater based on the argument
        m_previousPosition = aSpawnPoint;
        m_firingDir = transform.forward;
        m_projectileSpeed = aSpeed;
        m_ownerWeapon = aOwnerWeapon;
        m_velocity = Vector3.zero;
        m_damage += m_ownerWeapon.m_damage;
        m_timerLife = m_lifeTime;
        m_timerDying = m_dyingTime;
        m_timeWhenShot = Time.time;

        //Reset all variable
        CUtilitySetters.SetEnabledRenderers(m_renderers, true);
        m_isDead = false;

        //Initialzie all the behaviors in the script
        InitializeBehaviors();
    }

    /*
    Description: Update all behaviour
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    private void Update()
    {
        //If the projectile is alive
        if (m_isDead == false)
        {
            //Update Motion behaviour
            if (m_motionBehaviour != null)
            {
                m_motionBehaviour.UpdateMotion(this);
            }

            //Calculate the new velocity
            m_velocity = transform.position - m_previousPosition;

            //Update Misc behaviour
            if (m_miscBehaviour != null)
            {
                m_miscBehaviour.UpdateMisc(this);
            }

            //Check if projectile is collided
            CollisionCheck();

            //Decrease the timer
            m_timerLife -= Time.deltaTime;

            //if the timer is finished
            if (m_timerLife < 0.0f)
            {
                //Reset current streak
                ResetStreak();

                //Ensure that the projectile didn't die because of a collision
                m_isDeadDueCollision = false;

                //Kill the projectile
                KillProjectile();
            }
        }
        //If the projectile is dead
        else
        {
            //Decrease dying timer
            m_timerDying -= Time.deltaTime;

            //If there is a death behavior
            if(m_deathBehaviour!=null)
            {
                //Update death behavior
                m_deathBehaviour.UpdateDeath(this);
            }

            //If dying timer is finished
            if (m_timerDying < 0.0f)
            {
                //Call OnDestroy function
                DisableProjectile();
            }
        }

        //Update the previous position
        m_previousPosition = transform.position;
    }

    /*
    Description: Function used to set a projectile as dead
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    private void KillProjectile()
    {
        //Hide the mesh and set the projectile to dead
        CUtilitySetters.SetEnabledRenderers(m_renderers, false);

        //Set projectile as dead
        m_isDead = true;

        //If there are suscribes to event
        if(OnProjectileDeath!=null)
        {
            //Call event that projectile died
            OnProjectileDeath(m_isDeadDueCollision);
        }
    }

    /*
    Description: Checking if the projectile is collide with something by raycasting from previous
                 position to current position and filter if it's collide with a projectile
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    private void CollisionCheck()
    {
        //Cast a ray between the previous and current position to detect collisions
        Vector3 distanceBetweenPosition = (transform.position - m_previousPosition);

        //make sure there's a change on distance
        float moveDist = distanceBetweenPosition.magnitude;
        if (moveDist <= 0.0f)
        {
            return;
        }

        //Initialize raycast direction and raycast hit
        Vector3 RaycastDir = distanceBetweenPosition.normalized;
        RaycastHit hitInfo;

        //if didn't collide with something
        if (!Physics.Raycast(m_previousPosition, RaycastDir, out hitInfo, moveDist, m_layerMask))
        {
            return;
        }

        //Handle the collision
        if (m_collisionBehaviour != null)
        {
            //Check if projectile hit a target or not
            bool hitTarget;
            hitTarget = m_collisionBehaviour.CollisionHandle(this, hitInfo.point, 
                hitInfo.normal, hitInfo.collider,out m_isDeadDueCollision);

            //If not target hit
            if (!hitTarget)
            {
                //Reset current streak
                ResetStreak();

                //Call weapon event that a non target object was shot
                m_ownerWeapon.DetectedNonTargetObjectShot(hitInfo.collider.transform.gameObject, 
                    hitInfo.point,hitInfo.normal, m_damage, hitInfo.textureCoord,hitInfo.collider);
            }

            //If the projectile is dead due a collision
            if(m_isDeadDueCollision==true)
            {
                //Kill it
                KillProjectile();
            }
        }
    }

    /*
    Description: Initializes all the behaviors currently being used by the projectile.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void InitializeBehaviors()
    {
        //Motion behavior
        if (m_motionBehaviour != null)
        {
            m_motionBehaviour.Init(this);
        }

        //Collision behavior
        if (m_collisionBehaviour != null)
        {
            m_collisionBehaviour.Init(this);
        }

        //Misc behavior
        if (m_miscBehaviour != null)
        {
            m_miscBehaviour.Init(this);
        }

        //Death behavior
        if (m_deathBehaviour != null)
        {
            m_deathBehaviour.Init(this);
        }
    }

    /*
    Description: Reseting current streak in scoring system
    Creator: Juan Calvin Raymond
    Creation Date: 12 Dec 2016
    */
    private void ResetStreak()
    {
        if (CGameManager.PInstanceGameManager != null)
        {
            if (CGameManager.PInstanceGameManager.PScoringSystem != null)
            {
                CGameManager.PInstanceGameManager.PScoringSystem.PStreak = 0;
            }
        }
    }

    /*
    Description: A function that get called when projectile is dead
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    private void DisableProjectile()
    {
        gameObject.SetActive(false);
        POwnerWeapon.PListOfInactiveProjectile.Add(gameObject);
    }

    /*
    Description: Function to call on projectile hit event.
    Parameters: GameObject aObjectHit - Object hit by projectile
                Vector3 aHitPosition - The position where the projectile hit
                Vector3 aHitNormal - The normal of the hit
                int aDamage - The damage made by the projectile
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    public void DetectedProjectileHit(GameObject aObjectHit, Vector3 aHitPosition, Vector3 aHitNormal,int aDamage)
    {
        if (OnProjectileHit != null)
        {
            OnProjectileHit(aObjectHit, aHitPosition, aHitNormal, aDamage);
        }
    }
}