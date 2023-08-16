using UnityEngine;
using System.Collections;
using System;

/*
Description: Stopping trailing particle when projectile hit something
Creator: Juan Calvin Raymond
Creation Date: 30 Mar 2017
*/
public class CProjStopTrailWhenHit : MonoBehaviour
{
    //Particle system component
    private ParticleSystem m_particleSystem;

    //Lens flare component
    private LensFlare m_lensFlare;

    //Projectile script
    public CProjectile m_projectile;

    /*
    Description: Initializing variable
    Creator: Juan Calvin Raymond
    Creation Date: 30 Mar 2017
    */
    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_lensFlare = GetComponent<LensFlare>();
    }

    /*
    Description: Subscribe event
    Creator: Juan Calvin Raymond
    Creation Date: 30 Mar 2017
    */
    private void Start()
    {
        if(m_projectile != null)
        {
            m_projectile.OnProjectileDeath += StopParticle;
        }
    }

    /*
    Description: Unsubscribe event
    Creator: Juan Calvin Raymond
    Creation Date: 30 Mar 2017
    */
    private void OnDestroy()
    {
        if (m_projectile != null)
        {
            m_projectile.OnProjectileDeath -= StopParticle;
        }
    }

    private void OnEnable()
    {
        m_lensFlare.enabled = true;

    }

    /*
    Description: Stopping the particle
    Creator: Juan Calvin Raymond
    Creation Date: 30 Mar 2017
    */
    private void StopParticle(bool aDiedDueCollision)
    {
        m_particleSystem.Stop();
        m_lensFlare.enabled = false;
    }
    
}
