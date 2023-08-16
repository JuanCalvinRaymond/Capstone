using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: Use overlap sphere to hit all collder in radius
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
*/
public class CProjExplosionCollision : MonoBehaviour, ICollisionBehaviour
{
    private Vector3 m_hitPosition;
    private Vector3 m_hitNormal;

    //List of target that got hit by the explosion
    private List<GameObject> m_listOfTarget;
    private Collider[] m_overlapSphereHit;

    //Projectile script that own this behaviour
    private CProjectile m_projectile;

    //Variable to know how far is the nearest non target object, set it to the max value of float
    //private float m_nonTargetDistance = float.MaxValue;

    //Radius of the explosion
    public float m_explosionRadius = 100.0f;
    

    //Fall off damage variable
    public float m_fallOffStarts = 0.0f;
    public int m_damageMinimal = 0;

    //Bool of explosion go through the building or not (May need it for the future)
    //public bool m_ignoreBuilding = false;

    /*
    Description: Check if the collider is a target then add it to the list 
    Parameters: aHit : list of collider that got hit
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    protected void CalculateTargetListIgnoreBuilding(Collider[] aHit)
    {
        //Iterate through all collider that we hit
        if (aHit.Length > 0)
        {
            for (int i = 0; i < aHit.Length; i++)
            {
                //Cast it to ITarget script
                ITarget tempTarget = (ITarget)aHit[i].GetComponent(typeof(ITarget));

                //If it's a target
                if (tempTarget != null)
                {
                    //Add the gameobject to the list
                    m_listOfTarget.Add(aHit[i].gameObject);
                }
            }

        }
        //Return the list of target
        //return m_listOfTarget;
    }


    /*
    Description: Function where all the shootable objects in the explosion will be "Shot" or notified of the
    explosion.This function is also in charge of making the decals for the explosion
    Parameters: Vector3 aHitPosition : position that the projectile hit
                Vector3 aHitNormal : the normal of point that the projectile hit
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, December 21st, 2016
    */
    protected void ExplodeObjects(Vector3 aHitPosition, Vector3 aHitNormal)
    {
        if (CGameManager.PInstanceGameManager.PGameState != EGameStates.Play)
        {
            return;
        }

        IShootable shootableObject = null;
        for (int i = 0; i < m_overlapSphereHit.Length; i++)
        {
            shootableObject = m_overlapSphereHit[i].gameObject.GetComponent<IShootable>();

            //Set intial damage to be full damage, no falloff
            int fallOffDistanceDamage = m_projectile.m_damage;

            //If it is a shootable object
            if (shootableObject != null)
            {
                //Calculate the distance
                float distance = Vector3.Distance(m_projectile.transform.position, m_overlapSphereHit[i].transform.position);

                //If the distance haven't enter fall off distance
                if (distance < m_fallOffStarts)
                {
                    //Damage the target with full damage
                    shootableObject.ObjectShot(m_projectile.POwnerWeapon.gameObject, fallOffDistanceDamage, m_hitPosition, m_hitNormal);
                }
                //if the distance is inside the fall off range
                else
                {
                    //calculate the fall off damage
                    fallOffDistanceDamage = (int)(m_projectile.m_damage - ((m_projectile.m_damage - m_damageMinimal) * (distance - m_fallOffStarts) / (m_explosionRadius - m_fallOffStarts)));

                    //Damage the target with fall off damage
                    shootableObject.ObjectShot(m_projectile.gameObject, fallOffDistanceDamage, m_hitPosition, m_hitNormal);
                }
            }

            //Reset the shootable object
            shootableObject = null;

            //If there is a projectile              
            if (m_projectile != null)
            {
                //Call that an object was hit
                m_projectile.DetectedProjectileHit(m_overlapSphereHit[i].gameObject, aHitPosition, aHitNormal, fallOffDistanceDamage);
            }
        }
    }

    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    public void Init(CProjectile aProjectile)
    {
        //Set the projectile script variable
        m_projectile = aProjectile;
    }

    /*
    Description: Use overlap sphere to hit all target in radius. Returns true if an object was hit
    Parameters: aProjectile : projectile script
                aHitPos : position that the projectile hit
                aNormal : the normal of point that the projectile hit
                aCollider : collider of the object that projectile hit
                out bool m_isDeadDueCollision - Returns if the collision killed the projectile
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    public bool CollisionHandle(CProjectile aProjectile, Vector3 aHitPos, Vector3 aNormal, Collider aCollider, out bool aIsDeadDueCollision)
    {
        m_hitPosition = aHitPos;
        m_hitNormal = aNormal;

        //Hit all collider in radius
        m_overlapSphereHit = Physics.OverlapSphere(aHitPos, m_explosionRadius, m_projectile.PLayerMask);

        //if we hit something
        if (m_overlapSphereHit.Length > 0)
        {
            if (CGameManager.PInstanceGameManager.PScoringSystem != null)
            {
                //If the list of target is null make a new one, if not make sure it's empty
                if (m_listOfTarget == null)
                {
                    m_listOfTarget = new List<GameObject>();
                }
                else
                {
                    m_listOfTarget.Clear();
                }
                //Calculate all the target list and set it on scoring system
                CalculateTargetListIgnoreBuilding(m_overlapSphereHit);

                if(m_listOfTarget.Count > 0)
                {
                    if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
                    {
                        m_projectile.POwnerWeapon.TargetHit(m_listOfTarget);
                    }
                }

            }

            //Shoot all the shootable objects
            ExplodeObjects(aHitPos, aNormal);

            //Set projectile is dead
            aIsDeadDueCollision = true;
        }
        else//If nothing was hit
        {
            //Set projectile is alive
            aIsDeadDueCollision = false;
        }

        //Return true or false based on if we hit a target or not
        return m_listOfTarget.Count > 0 ? true : false;
    }
}
