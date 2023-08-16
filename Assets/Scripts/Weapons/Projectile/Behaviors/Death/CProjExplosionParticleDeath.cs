using UnityEngine;
using System.Collections;
using System;

/*
Description: Emit Explosion particle
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 11-24-2016
Extra Notes:
*/
[RequireComponent(typeof(CProjectile))]
public class CProjExplosionParticleDeath : MonoBehaviour, IDeathBehaviour
{
    private CProjectile m_projectile;

    //Particle system component
    public ParticleSystem m_particleSystem;

    //How many particle to emit
    //public int m_numParticlesToEmit = 5;

    /*
    Description: Get the projectile component and suscribe to its event
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void Awake()
    {
        m_projectile = GetComponent<CProjectile>();
        m_projectile.OnProjectileDeath += OnProjectileKilled;
    }

    /*
    Description: Unsuscrbie from projectile event
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void OnDestroy()
    {
        m_projectile.OnProjectileDeath -= OnProjectileKilled;
    }

    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    public void Init(CProjectile aProjectile)
    {
    }

    /*
    Description: Any update to do while projectile is dead
    Parameters: aProjectile : projectile that own this script
    Creator: Juan Calvin Raymond
    Creation Date: 11-24-2016
    */
    public void UpdateDeath(CProjectile aProjectile)
    {
    }

    /*
    Description: Any update to do while projectile is dead
    Parameters: aProjectile : projectile that own this script
    Creator: Juan Calvin Raymond
    Creation Date: 11-24-2016
    */
    public void OnProjectileKilled(bool aCollisionDeath)
    {
        //If the projectile died because of a collision
        if(aCollisionDeath==true)
        {
            //If there is a particle system and it hasn't emitted yes
            if (m_particleSystem != null)
            {
                m_particleSystem.Play();
                //m_particleSystem.Emit(m_numParticlesToEmit);
            }
        }
    }
}
