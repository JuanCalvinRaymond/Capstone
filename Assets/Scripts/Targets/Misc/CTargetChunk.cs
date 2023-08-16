using UnityEngine;
using System.Collections;
using System;

/*
Description: Simple class that implements the IBreakable interface. This class will then get feedback
            when its "shot" by a raycash/hitscan weapon. This class will merely disable kinematics 
            and collisions 
Creator: Alvaro Chavez Mixco
*/
[RequireComponent(typeof(Rigidbody))]
public class CTargetChunk : MonoBehaviour, IShootable
{
    private Rigidbody m_rigiBody;
    private bool m_hasExploded = false;

    [Header("Starting Conditions")]
    public bool m_startKinematic = true;
    public bool m_startGravity = false;
    public bool m_enableGravityOnHit = true;

    [Header("Collision Properties")]
    public bool m_isBlockingShots = false;
    public bool m_applyForce = true;
    public float m_hitForceModifier = 1.0f;
    public bool m_destroyOnCollision = true;
    public float m_destroyTime = 2.0F;

    [Header("Recursive Explosions")]
    public bool m_isExplosive = true;
    public float m_explosionRadius = 5.0f;
    public int m_explosionDamage = 20;

    public event delegOnObjectShot OnShot;

    public bool PIsExplosive
    {
        set
        {
            m_isExplosive = value;
        }
    }

    /*
    Description: Get the rigid body and set itnitial condiitons
    Parameters(Optional): 
    Creator: Alvaro Chavez Mixco
    */
    private void Awake()
    {
        m_rigiBody = GetComponent<Rigidbody>();
        m_rigiBody.isKinematic = m_startKinematic;
        m_rigiBody.useGravity = m_startGravity;
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
    Description: Implementation of IBreakable, this clas will get called when "shot, and dadd forfce to the right body
    Parameters:GameObject aHitter-The object hitting the breakable object
                         int aDamage- The weapon's damage applied to it, this is being passed so that it can be used for scaling  effects
                         or other purposes.
                         Vector3 aHitPosition-Where the object was hit by the weapon
                         Vector3 aHitDirection -The direction of the hit
    Creator: Alvaro Chavez Mixco
    */
    public void ObjectShot(GameObject aHitter, int aDamage, Vector3 aHitPosition, Vector3 aHitDirection)
    {
        //Disable kinematics
        m_rigiBody.isKinematic = false;

        //Set gravity
        m_rigiBody.useGravity = m_enableGravityOnHit;

        //Ensure the object is not parented
        transform.parent = null;

        //Apply force
        if (m_applyForce == true)
        {
            if (m_isExplosive == false)
            {
                m_rigiBody.AddForceAtPosition(aDamage * aHitDirection * m_hitForceModifier, aHitPosition, ForceMode.Impulse);
            }
            else
            {
                ExplodeChunk(aHitPosition);
            }
        }

        //If we want to destroy on collision
        if (m_destroyOnCollision == true)
        {
            //Destroy the chunk after a certain amount of time
            Destroy(gameObject, m_destroyTime);
        }
        
        //If there are suscribers
        if(OnShot!=null)
        {
            //Call interface event
            OnShot(aHitter, aDamage, aHitPosition, aHitDirection);
        }
    }

    /*
    Description: Simulates a explosion, affecting all the CTargetChunk objects nearby.
    Parameter: Vector3 aExplosionPoint-The position where to explode from
    Creator: Alvaro Chavez Mixco
    */
    public void ExplodeChunk(Vector3 aExplosionPoint)
    {
        //If this chunk hasn't exploded yet
        if (m_hasExploded == false)
        {
            //Do an overlap sphere on the range of hte explision
            Collider[] sphereHits = Physics.OverlapSphere(aExplosionPoint, m_explosionRadius);

            RaycastHit chunkRaycastHit;
            Vector3 directionBetweenChunks;
            CTargetChunk chunk = null;

            //If the overlap sphere hit anything
            if (sphereHits.Length > 0)
            {
                //Go through all the colliders it hit
                for (int i = 0; i < sphereHits.Length; i++)
                {
                    //Get the target chunk component
                    chunk = sphereHits[i].gameObject.GetComponent<CTargetChunk>();

                    //If it is a target chunk
                    if (chunk != null)
                    {
                        //Get the direction between the explision point and the chunk
                        directionBetweenChunks = chunk.transform.position - aExplosionPoint;
                        directionBetweenChunks.Normalize();

                        //Do a raycast toward the target chunk
                        if (Physics.Raycast(aExplosionPoint, directionBetweenChunks, out chunkRaycastHit, m_explosionRadius) == true)
                        {
                            //If the raycast hits the target chunk, there are no objects between them
                            if (chunkRaycastHit.collider.gameObject == chunk.gameObject)
                            {
                                //Set the chunk as not explosive, this is to prevent further performance issues and is in no way realistic
                                chunk.PIsExplosive = false;

                                //Call the onhit function on the chunk found
                                chunk.ObjectShot(gameObject, m_explosionDamage * (int)m_hitForceModifier, chunkRaycastHit.point, directionBetweenChunks);

                                //Set that this chunk has already exploded so that it doesn't explode again
                                m_hasExploded = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
