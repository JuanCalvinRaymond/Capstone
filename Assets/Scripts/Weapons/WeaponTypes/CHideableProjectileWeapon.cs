using UnityEngine;
using System.Collections;

/*
Description: Class that inherit from projectile weapon class which will hide the projectile when
             fired and show when weapon is ready to fire again
Creator: Juan Calvin Raymond
Creation Date: 2 Dec 2016
*/
public class CHideableProjectileWeapon : CProjectileWeapon
{
    //Projectile game object
    [Tooltip("The game object that will be hidden when the user run out of ammo")]
    public GameObject m_projectileGameObject;

    /*
    Description: Override Update function to show projectile game object again
    Creator: Juan Calvin Raymond
    Creation Date: 2 Dec 2016
    */
    protected override void Update()
    {
        //Do regular Update
        base.Update();

        //If firerate timer is finish and ammo is more than 0
        if (m_fireRateTimer <= 0 && m_currentAmmo > 0)
        {
            if (m_projectileGameObject != null)
            {
                //Show the projectile
                m_projectileGameObject.SetActive(true);
            }
        }
    }

    /*
    Description: Override FireMechanics function to hide projectile game object
    Creator: Juan Calvin Raymond
    Creation Date: 2 Dec 2016
    */
    protected override void FireMechanics()
    {
        //Do regular FireMechanics
        base.FireMechanics();

        if(m_projectileGameObject != null)
        {
            //Hide projectile game object
            m_projectileGameObject.SetActive(false);
        }
    }
}
