using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: A hit scan type weapon that shoot raycast
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
Extra Notes: Can be inherited to make a hit scan weapon that doesn't have anything to do with target
*/
public class CHitScanWeapon : AWeapon
{
    //Variable for raycasting
    protected RaycastHit[] m_hit;
    protected Ray m_ray;

    //Varible list of target distance that get hit
    protected List<GameObject> m_listOfTarget;

    //Weapon properties for HitScanType Weapon
    [Header("Hitscan Weapon Properties")]
    public float m_offsetRadius = 0.0f;
    public int m_pellets = 1;

    public delegate void delegFireHitscanWeapon(Vector3 aShootPosition, Vector3 aHitPosition);
    public event delegFireHitscanWeapon OnFireHitscanWeapon;


    /*
    Description: Set all variable
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_listOfTarget = new List<GameObject>();

        //If there is a raycast point
        if (m_raycastPoint != null)
        {
            //Initialize the raycast with weapon forward direction
            m_ray = new Ray(m_raycastPoint.position, m_raycastPoint.forward);
        }
    }

    /*
    Description: This does the raycast that simulates the shooting in the weapon. The shooting will be done
                 according to the weapon rotation/position with the vive controller, otherwise it will use a mix of a raycast
                 from the players head (so that it matches the reticle position) with the raycasts from the weapons position/rotation
    Creator: Juan Calvin Raymond (Alvaro: I just made it into a function)
    Creation Date: Wednesday, December 21st, 
    */
    protected void RaycastShot()
    {
        //calculate the offset
        Vector3 offset = new Vector3(Random.Range(-m_offsetRadius, m_offsetRadius), Random.Range(-m_offsetRadius, m_offsetRadius), 0.0f);

        //If player is using vive controller
        if (CGameManager.PInstanceGameManager.PPlayerScript.PControllerType == EControllerTypes.ViveController)
        {
            //Set the raycast to the current position aiming forward when player hit fire
            m_ray = new Ray(m_raycastPoint.position, m_raycastPoint.forward + offset);
            m_hit = Physics.RaycastAll(m_ray, m_maxShootDistance, PLayerMask);
        }
        //If player is not using vive controller
        else
        {
            //Raycast from the head forward
            m_ray = new Ray(m_head.position, m_head.forward + offset);
            RaycastHit hit;

            //If the raycast hit something
            if (Physics.Raycast(m_ray, out hit, m_maxShootDistance, PLayerMask))
            {
                if (m_raycastPoint != null)
                {
                    //Find the direction from the point it hit to RaycastPoint
                    Vector3 direction = hit.point - m_raycastPoint.position;

                    //Raycast from RaycastPoint to that point
                    m_ray = new Ray(m_raycastPoint.position, direction);
                    m_hit = Physics.RaycastAll(m_ray, m_maxShootDistance, PLayerMask);
                }
            }
        }
    }

    /*
    Description: Sorts the array of hits from closest to farthest, and then iterates through it notifying all the
                 objects they were shot, if there wasn't another object blocking the shot in front of them.   
    Parameters: ref Vector3 aClosestHitPoint -The closest point where the raycast shot hit something                    
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, March 22nd, 2017.
    */
    private void ShootObjects(ref Vector3 aClosestHitPoint)
    {
        ITarget target;
        IShootable shootableObject;
        bool isBlockingShot = false;

        //Sort the raycast from closest hit point to farthest
        CUtilitySorting.SortRaycastHitFromClosestToFarthest(ref m_hit);

        //If something was hit
        if (m_hit.Length > 0)
        {
            //Get the closest hit point from the raycast and set it as closest point
            aClosestHitPoint = m_hit[0].point;

            //Call event for the closest object that was shot
            DetectedObjectShotClosest(m_hit[0].collider.gameObject, aClosestHitPoint, m_hit[0].normal, m_damage);

            //iterate through all objects that we hit, remembering they are from closest to farthest
            for (int i = 0; i < m_hit.Length; i++)
            {
                //Get their ITarget interface
                target = m_hit[i].collider.gameObject.GetComponent<ITarget>();

                //Get their IShootable interface
                shootableObject = m_hit[i].collider.gameObject.GetComponent<IShootable>();

                //If the object is a target
                if (target != null)
                {
                    //Add object to list of targets
                    m_listOfTarget.Add(m_hit[i].collider.gameObject);
                }
                else//If object is not a target
                {
                    //Call  the function for object shot that wasn't a target
                    DetectedNonTargetObjectShot(m_hit[i].collider.gameObject, m_hit[i].point,
                        m_hit[i].normal, m_damage,m_hit[i].textureCoord,m_hit[i].collider);
                }

                //If there is no shootable interface
                if (shootableObject == null)
                {
                    //Return that it is blocking the shot
                    isBlockingShot = true;
                }
                else//If it has a shootable interface
                {
                    //Notify the object it was shot
                    shootableObject.ObjectShot(gameObject, m_damage,
                        m_hit[i].point, m_ray.direction);

                    //Get the condition of the shootable component
                    isBlockingShot = shootableObject.GetIsBlockingShot();
                }

                //If the shot was block
                if (isBlockingShot == true)
                {
                    //Exit the loop, ignoring the objects farther away
                    break;
                }

            }
        }
    }

    /*
    Description: Raycast, and notifies, if the object has IShootable interface, that the object hit by the raycast was shot                        
    Creator: Juan Calvin Raymond
    Creation Date: 10-09-2016
    Extra Notes: can be inherit so diff kind of calculation can be implement.
    */
    protected override void FireMechanics()
    {
        //For every pellets
        for (int pellet = 0; pellet < m_pellets; pellet++)
        {
            //make sure the list of target is empty
            m_listOfTarget.Clear();
            m_hit = null;

            //Raycast the shot
            RaycastShot();

            //Set the starting closest hit point to be the max range of the raycast
            Vector3 closestHitPoint = m_raycastPoint.position +
                m_ray.direction * m_maxShootDistance;

            //Make sure there if there's a hit
            if (m_hit != null)
            {
                //If on play, notify objects that there were shots
                if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
                {
                    //Shoot all the shootable/target objects
                    ShootObjects(ref closestHitPoint);


                    //If there's a valid target that got hit
                    if (m_listOfTarget.Count <= 0)
                    {
                        if (CGameManager.PInstanceGameManager.PScoringSystem != null)
                        {
                            //Set streak back to 0
                            CGameManager.PInstanceGameManager.PScoringSystem.PStreak = 0;
                        }
                    }
                    else//If a target was hit
                    {

                        //Call TargetHit function from parent class
                        TargetHit(m_listOfTarget);
                    }
                }
            }

            //If there are suscribers to the event
            if (OnFireHitscanWeapon != null)
            {
                //Call event
                OnFireHitscanWeapon(m_raycastPoint.position, closestHitPoint);
            }
        }

        //Debug purposes
        //DEBUGLIST-AAA
        //Debug.DrawLine(m_raycastPoint.position, closestHitPoint, Color.yellow, 100);
        //Debug.DrawRay(m_ray.origin, m_ray.direction * m_maxShootDistance, Color.red, 100);
    }
}
