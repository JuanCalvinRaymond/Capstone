using UnityEngine;
using System.Collections;

/*
Description: Class that manage weapon audio by subscribing to weapon event
Creator: Juan Calvin Raymond
Creation Date: 20 Dec 2016
*/
[RequireComponent(typeof(AWeapon),typeof(AudioSource))]
public class CWeaponAudio : MonoBehaviour
{
    //Weapon script
    private AWeapon m_weapon;

    //Audio Source variable
    protected AudioSource m_audioSourceGun;
    protected AudioSource m_audioSourceBarrel;

    //Audio clip Variables
    [Header("Firing Sounds")]
    public AudioClip[] m_audioFiring;

    [Header("Reload Sounds")]
    public AudioClip[] m_audioReloading;

    [Header("Firing When Out of Ammo Sounds")]
    public AudioClip[] m_audioFiringOutOfAmmo;

    /*
    Description: Initialize all variable
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void Awake()
    {
        //Get weapon script
        m_weapon = GetComponent<AWeapon>();

        //Get the audio source of the main gun
        m_audioSourceGun = GetComponent<AudioSource>();

        //If weapon have barrel set audio source barrel to audio source on the barrel, if not use main gun audio source
        m_audioSourceBarrel = m_audioSourceBarrel != null ? GetComponent<AudioSource>() : m_audioSourceGun;
    }

    /*
    Description: Subscribe to weapon event
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void Start()
    {
        m_weapon.OnFire += PlayFireAudio;
        m_weapon.OnStartReload += PlayReloadAudio;
        m_weapon.OnFireOutOfAmmo += PlayFireOutOfAmmoAudio;
    }

    /*
    Description: Unsubscribe to weapon event
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void OnDestroy()
    {
        m_weapon.OnFire -= PlayFireAudio;
        m_weapon.OnStartReload -= PlayReloadAudio;
        m_weapon.OnFireOutOfAmmo -= PlayFireOutOfAmmoAudio;
    }

    /*
    Description: Play a random fire audio  clip
    Parameter : aCurrentAmmo : weapon's current ammo
                aWeaponHand : hand that hold the weapon
                aTimeWhenShot : time when weapon shoot
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void PlayFireAudio(int aCurrentAmmo, EWeaponHand aWeaponHand)
    {
        //Play shooting sound effects
        CUtilitySound.PlayRandomSound(m_audioSourceBarrel, m_audioFiring);
    }

    /*
    Description: Play a random reload audio
    Parameter: aCurrentAmmo : weapon's current ammo
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void PlayReloadAudio(int aCurrentAmmo)
    {
        //Play reload sound effect
        CUtilitySound.PlayRandomSound(m_audioSourceGun, m_audioReloading);
    }

    /*
    Description: Play a random audio clip when the player tries to shoot and he is out of ammo
    Parameter : int aCurrentAmmo - Weapon current ammo
                EWeaponHand aWeaponHand - Hand that hold the weapon
                float aTimeWhenShot - Time when weapon was shot
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, March 12th, 2017
    */
    private void PlayFireOutOfAmmoAudio(int aCurrentAmmo, EWeaponHand aWeaponHand)
    {
        //Play random sound effect
        CUtilitySound.PlayRandomSound(m_audioSourceBarrel, m_audioFiringOutOfAmmo);
    }
}
