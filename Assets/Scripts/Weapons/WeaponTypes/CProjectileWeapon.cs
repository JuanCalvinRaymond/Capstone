using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
Description: A projectile type weapon that spawns projectile prefab
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
Extra Notes: Can be inherited to make a projectile weapon that doesn't have anything to do with target
*/
public class CProjectileWeapon : AWeapon
{
    //List of inactive projectile
    private List<GameObject> m_listOfInactiveProjectile;

    [Header("Projectile Weapon Properties")]
    //Projectile prefab
    public GameObject m_projectile;

    //How many object to create
    public int m_maxObjectInThePool = 5;

    //Projectile speed
    public float m_projectileSpeed;

    public List<GameObject> PListOfInactiveProjectile
    {
        get
        {
            return m_listOfInactiveProjectile;
        }
    }

    /*
    Description: Create the projectile prefab and add it to object pool
    Creator: Juan Calvin Raymond
    Creation Date: 20 Mar 2017
    */
    protected override void Awake()
    {
        base.Awake();

        m_listOfInactiveProjectile = new List<GameObject>();

        for (int i = 0; i < m_maxObjectInThePool; i++)
        {
            GameObject tempProjectile = GameObject.Instantiate(m_projectile);

            tempProjectile.SetActive(false);

            m_listOfInactiveProjectile.Add(tempProjectile);
        }
    }

    /*
    Description: Spawn a projectile prefab
    Parameters: aDirectionToShoot : Direction where the projectile is flying towards
    Creator: Juan Calvin Raymond
    Creation Date: 9 Dec 2016
    */
    private void SpawnProjectile(Quaternion aAngleToShoot)
    {
        if (m_projectile != null && m_raycastPoint != null && m_listOfInactiveProjectile.Count > 0)
        {
            //Get the projectile script and initialize all value
            m_listOfInactiveProjectile[0].SetActive(true);
            CProjectile projectileScript = m_listOfInactiveProjectile[0].GetComponent<CProjectile>();
            projectileScript.Init(m_raycastPoint.position, aAngleToShoot, m_projectileSpeed, this);

            //Remove it from the pool
            m_listOfInactiveProjectile.RemoveAt(0);
        }
    }

    /*
    Description: Check which controller player are using and spawn projectile based on it
    Creator: Juan Calvin Raymond
    Creation Date: 9 Dec 2016
    */
    protected override void FireMechanics()
    {
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //If player is not using vive controller
            if (CSettingsStorer.PInstanceSettingsStorer.PInputMethod != EControllerTypes.ViveController)
            {
                //Raycast from the head
                Ray ray = new Ray(m_head.position, m_head.forward);
                RaycastHit hit;

                //If the raycast hit something
                if (Physics.Raycast(ray, out hit, m_maxShootDistance))
                {
                    //Calculate the direction where projectile should be flying towards
                    Vector3 directionTowardsPoint = (hit.point - m_raycastPoint.position).normalized;
                    Quaternion angleToShoot = Quaternion.LookRotation(directionTowardsPoint);

                    //Spawn projectile towards the point where raycast hit
                    SpawnProjectile(angleToShoot);
                }
                //If the raycast doesn't hit anything
                else
                {
                    //Calculate the direction where projectile should be flying towards
                    Vector3 directionTowardsPoint = ((m_head.forward * m_maxShootDistance) - m_raycastPoint.position).normalized;
                    Quaternion angleToShoot = Quaternion.LookRotation(directionTowardsPoint);

                    //Spawn projectile towards the maximum distance of the raycast
                    SpawnProjectile(angleToShoot);
                }
            }
            else//If player is using vive controller
            {
                //Spawn the projectile using raycast forward direction
                SpawnProjectile(Quaternion.LookRotation(m_raycastPoint.forward));
            }
        }
    }
    
}