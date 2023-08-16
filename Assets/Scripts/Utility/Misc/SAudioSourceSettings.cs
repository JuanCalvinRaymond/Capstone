using UnityEngine;
using System.Collections;

using System;
using UnityEngine.Audio;

//PENDING
[Serializable]
public struct SAudioSourceSettings
{
    [SerializeField]
    public AudioMixerGroup m_outputAudioMixerGroup;

    [SerializeField]
    [Range(0.0f,1.0f)]
    [Tooltip("0.0 is 2D, 1.0 is 3D")]
    public float m_spatialBlend;
}
