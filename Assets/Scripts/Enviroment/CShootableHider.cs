using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;

/*
Description: Class used to hide objects until this object is shot to death.                         
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 17th, 2017
*/
[RequireComponent(typeof(Collider))]
public class CShootableHider : MonoBehaviour, IShootable
{
    public bool m_isBlockingShots = false;
    public int m_health;

    public List<GameObject> m_objectsToShow;

    public event delegOnObjectShot OnShot;

    /*
    Description: If there are no objects to show, disable the component. Otherwise ensure the objects start disabled.                        
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void Awake()
    {
        //If there are no objects to show
        if (m_objectsToShow == null)
        {
            //Disable the components
            enabled = false;
        }
        else//If tehre are objects to show
        {
            //Ensure they start inactive
            SetObjectsActiveStatus(false);
        }
    }

    /*
    Description: Apply damage to the object and determine if it is death yet.              
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public void ApplyDamage(int aDamage)
    {
        //Reduce health
        m_health += aDamage;

        //If health is below 0
        if (m_health <= 0)
        {
            //Kill object
            OnDeath();
        }
    }

    /*
    Description: Disable this object, and enable the objects we want to show.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public void OnDeath()
    {
        //Set the objects we want to show as active
        SetObjectsActiveStatus(true);

        //Deactivate this object
        gameObject.SetActive(false);
    }

    /*
    Description: Set the active of all the objects we want to hide/show.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public void SetObjectsActiveStatus(bool aActiveStatus)
    {
        //Go through all the objects
        for (int i = 0; i < m_objectsToShow.Count; i++)
        {
            //If the object is valid
            if (m_objectsToShow[i] != null)
            {
                //Set its active status
                m_objectsToShow[i].SetActive(aActiveStatus);
            }
        }
    }

    /*
    Description: Returns if the shootable object allows shot to pass through it    
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 21st, 2017
    */
    public bool GetIsBlockingShot()
    {
        return m_isBlockingShots;
    }

    /*
    Description: Implementation of IShootable interface. When the object is shot it will apply damage to it.       
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public void ObjectShot(GameObject aHitter, int aDamage, Vector3 aHitPosition, Vector3 aHitDirection)
    {
        //Apply damage to the object
        ApplyDamage(aDamage);

        //If there are suscribers
        if (OnShot != null)
        {
            //Call interface event
            OnShot(aHitter, aDamage, aHitPosition, aHitDirection);
        }
    }
}