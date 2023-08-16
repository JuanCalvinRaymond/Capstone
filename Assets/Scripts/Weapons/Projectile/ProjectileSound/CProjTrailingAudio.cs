using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: Play a looping sound of trail sound and change the volume based on player distance
Creator: Juan Calvin Raymond
Creation Date: 21 Mar 2017
*/
public class CProjTrailingAudio : MonoBehaviour
{
    /*
    Description: Simple struct that contain trail sound parameter
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    [Serializable]
    public struct STrailAudio
    {
        [SerializeField]
        public AudioClip m_audioClip;

        [SerializeField]
        public float m_minRange;

        [SerializeField]
        [Range(0,1)]
        public float m_minRangeVol;

        [SerializeField]
        public float m_maxRange;

        [SerializeField]
        [Range(0, 1)]
        public float m_maxRangeVol;
    }


    //List of audio sources
    private AudioSource[] m_audioSource;
    
    //Player gameobject
    private GameObject m_player;

    //Distance from player to projectile
    private float m_distanceFromPlayer;

    //List of trail sound
    public List<STrailAudio> m_trailSound;

    //Variable to tweak in inspector
    public float m_delayTime;

    /*
    Description: Get all audio source component
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void Awake()
    {
        m_audioSource = GetComponents<AudioSource>();
    }

    /*
    Description: Get player object
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void Start()
    {
        m_player = CGameManager.PInstanceGameManager.PPlayerScript.gameObject;
    }

    /*
    Description: Call PlayProjectileTrailSound when the object is enabled
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void OnEnable()
    {
        PlayProjectileTrailSound();

    }

    /*
    Description: Calculate distance from player and constantly call CalCulateVolume
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void Update()
    {
        m_distanceFromPlayer = Vector3.Distance(transform.position, m_player.transform.position);

        CalculateVolume(m_distanceFromPlayer);
    }
    
    /*
    Description: Play all trail sound
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void PlayProjectileTrailSound()
    {
        for (int i = 0; i < m_trailSound.Count; i++)
        {
            CUtilitySound.PlaySound(m_audioSource[i], m_trailSound[i].m_audioClip, m_delayTime);
        }
    }

    /*
    Description: Calculate the audio volume based on distance
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void CalculateVolume(float aDistance)
    {
        for (int i = 0; i < m_trailSound.Count; i++)
        {
            //Do falloff calculation
            float volume = CUtilityMath.RescaleRange(aDistance, m_trailSound[i].m_minRange, m_trailSound[i].m_maxRange, m_trailSound[i].m_minRangeVol, m_trailSound[i].m_maxRangeVol);
            
            m_audioSource[i].volume = volume;
        }
    }
}
