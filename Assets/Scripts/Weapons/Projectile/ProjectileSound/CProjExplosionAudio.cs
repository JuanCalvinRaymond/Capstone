using UnityEngine;
using System.Collections;

/*
Description: Class to play multiple sounds for the projectile
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, March 16th, 2017
*/
[RequireComponent(typeof(CProjectile), typeof(AudioSource))]
public class CProjExplosionAudio : MonoBehaviour
{
    private CProjectile m_projectile;

    public AudioSource m_audioSource;

    [Header("Explosion Sounds")]
    public AudioClip[] m_explosionSounds;

    /*
    Description: Get the projectile and audio source component
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void Awake()
    {
        m_projectile = GetComponent<CProjectile>();
    }

    /*
    Description: Suscribe to projectile events
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void Start()
    {
        m_projectile.OnProjectileDeath += PlayProjectileDeadSound;
    }

    /*
    Description: Unsuscribe from projectile events
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void OnDestroy()
    {
        m_projectile.OnProjectileDeath -= PlayProjectileDeadSound;
    }

    /*
    Description: Play a random sounds when the projectile dies due a collision
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    private void PlayProjectileDeadSound(bool aDiedDueCollision)
    {
        //If the projectile died due a collision
        if (aDiedDueCollision == true)
        {
            //Play random sound
            CUtilitySound.PlayRandomSound(m_audioSource, m_explosionSounds);
        }
    }
}