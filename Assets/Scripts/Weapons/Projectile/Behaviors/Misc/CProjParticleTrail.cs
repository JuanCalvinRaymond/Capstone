using UnityEngine;
using System.Collections;
using System;

/*
Description: Misc projectile behavior to emit a trail of particles behind a proejectile.
             The number of particles will be based on the speed of the projectile.
             The size of the particles will be based on the current life time/duration of the projectile.
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
[RequireComponent(typeof(CProjectile))]
public class CProjParticleTrail : MonoBehaviour, IMiscBehaviour
{
    private float m_initialLifeTime;
    private float m_timerLife;

    private int m_numParticlesToLaunch;

    [Header("Particle properties")]
    public ParticleSystem m_particleSystem;

    [Range(0.0f, 1.0f)]
    public float m_particleMinSize = 0.1f;

    [Range(0.0f, 1.0f)]
    public float m_particleMaxSize = 0.2f;

    [Tooltip("The number that will be multiplied to the speed of the projectile to determine the number of particles.")]
    public float m_particleNumberSpeedMultiplier = 5.0f;

    /*
    Description: Suscribe to projectile events, and initialize eases
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void Init(CProjectile aProjectile)
    {
        //Set all the other parametrs
        m_initialLifeTime = aProjectile.PLifeTime;//Inital total life time of projectile
        m_timerLife = aProjectile.PLifeTimer;
    }

    /*
    Description: Update function that is manually called by projectile. This will
                 emit particles, after determining their size and how many to emit.
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void UpdateMisc(CProjectile aProjectile)
    {
        ///If there is a particle system
        if (m_particleSystem != null)
        {
            //Get how many particles will be emitted according to ease
            m_numParticlesToLaunch = Mathf.RoundToInt(aProjectile.PSpeed * m_particleNumberSpeedMultiplier);

            //Set properties for the particle syste,
            m_particleSystem.startSize = Mathf.Lerp(m_particleMinSize, m_particleMaxSize, m_timerLife / m_initialLifeTime * m_timerLife / m_initialLifeTime);
            m_particleSystem.Emit(m_numParticlesToLaunch);
        }
    }
}
