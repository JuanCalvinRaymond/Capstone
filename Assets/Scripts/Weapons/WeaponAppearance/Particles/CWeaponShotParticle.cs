using UnityEngine;
using System.Collections;

/*
Description: Class to emit particles whenever the weapon is fired
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, March 16th, 2017
*/
[RequireComponent(typeof(AWeapon))]
public class CWeaponShotParticle : MonoBehaviour
{
    private AWeapon m_weapon;
    private ParticleSystem m_firingParticleSystem;
    private Light m_muzzleLight;
    private float m_flashDurationRemaining = 0.0f;

    public float m_flashDuration = 0.025f;

    /*
    Description: Get weapon component
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    protected virtual void Awake ()
    {
        //Get the weapon component
        m_weapon = GetComponent<AWeapon>();
    }

    /*
    Description: Suscrbie to weapon events, and get additional components from weapon object.
    Creator: Charlotte Brown
    Creation Date: Thursday, March 16th, 2017
    */
    protected virtual void Start()
    {
        //Suscribe to weapon onfire event
        m_weapon.OnFire += OnWeaponFired;

        //Get the particle system
        if (m_weapon.PParticlePoint != null)
        {
            m_firingParticleSystem = m_weapon.PParticlePoint.GetComponent<ParticleSystem>();
            m_muzzleLight = m_weapon.PParticlePoint.GetComponent<Light>();
        }
    }

    /*
    Description: Unsuscribe from weapon event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    protected virtual void OnDestroy()
    {
        //Unsuscribe to weapon onfire event
        m_weapon.OnFire -= OnWeaponFired;
    }

    /*
    Description: Check if the weapon muzzle light should be turned off.
    Creator: Charlotte Brown
    Creation Date: Thursday, March 16th, 2017
    */
    protected virtual void Update()
    {
        //If there is a muzzle light
        if (m_muzzleLight != null)
        {
            //Use regular delta time to avoid light staying on in pause menus
            m_flashDurationRemaining -= Time.deltaTime;

            //If the light on time is over
            if (m_flashDurationRemaining <= 0.0f)
            {
                //Disable light component
                m_muzzleLight.enabled = false;
            }
        }
    }

    /*
    Description: Emit particles and turn on the muzzle light
    Creator: Charlotte Brown
    Creation Date: Thursday, March 16th, 2017
    */
    private void OnWeaponFired(int aCurrentAmmo, EWeaponHand aWeaponHand = EWeaponHand.None)
    {
        //If particle system is valid 
        if (m_firingParticleSystem != null)
        {
            // This was changed to not override the particle system's emit count.
            //Fire particles
            m_firingParticleSystem.Play();// Emit(m_particlesToEmit);
        }

        //If muzzle light is valid
        if(m_muzzleLight != null)
        {
            //Enable it and set its duration
            m_muzzleLight.enabled = true;
            m_flashDurationRemaining = m_flashDuration;
        }
    }
}
