using UnityEngine;
using System.Collections;

/*
Description: Class used to create decals where a weapon or projectile hit an object
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, March 22nd, 2017
*/
public class CWeaponAndProjectileDecal : MonoBehaviour
{
    private AWeapon m_weapon;
    private CProjectile m_projectile;

    [Tooltip("The decal that will be placed wherever the shot landed on.")]
    public GameObject m_decalPrefab;

    /*
    Description: Get weapon and/or projectile component
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void Awake()
    {
        m_weapon = GetComponent<AWeapon>();
        m_projectile = GetComponent<CProjectile>();
    }

    /*
    Description: Suscribe to weapon and/or projectile event
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void Start()
    {
        //If there is a weapon
        if(m_weapon!=null)
        {
            //Suscribe to events
            m_weapon.OnObjectShotClosest += CreateDecal;
        }
        
        //If there is a projectile
        if(m_projectile!=null)
        {
            m_projectile.OnProjectileHit += CreateDecal;
        }
    }

    /*
    Description: Unsuscribe from weapon and/or projectile event
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void OnDestroy()
    {
        //If there is a weapon
        if (m_weapon != null)
        {
            //Suscribe to events
            m_weapon.OnObjectShotClosest -= CreateDecal;
        }

        //If there is a projectile
        if (m_projectile != null)
        {
            m_projectile.OnProjectileHit -= CreateDecal;
        }
    }

    /*
    Description: Function to instantiate an object (decal) in the place where the playe shoot
    Parameters:GameObject aObjectHit-The object hit by shooting
                Vector3 aHitPosition- The position where the shot landed
                Vector3 aHitNormal - The angle of the object hit.
                int aDamage - Not used in this function. The damage made by teh weapon
    Creator: Alvaro Chavez Mixco-
    */
    public void CreateDecal(GameObject aObjectHit, Vector3 aHitPosition, Vector3 aHitNormal, int aDamage)
    {
        //Instantiate bullet decal prefab
        if (m_decalPrefab != null && aObjectHit != null)
        {
            //Instantiate the bullet decal, and place it as a child of the object it hit
            GameObject decalObject = (GameObject)GameObject.Instantiate(m_decalPrefab, aObjectHit.transform);
            decalObject.transform.position = aHitPosition;//Create a decal on the position it was hit.
            decalObject.transform.rotation = Quaternion.LookRotation(aHitNormal);//Place the object along the normals it hit
        }
    }
}
