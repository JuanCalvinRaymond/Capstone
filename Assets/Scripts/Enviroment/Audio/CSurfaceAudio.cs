using UnityEngine;
using System.Collections;

/*
Description: Simple class to store an audio surface type
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, March 28th, 2017
*/
public class CSurfaceAudio : MonoBehaviour
{
    public EAudioSurfaces m_audioSurfaceType;

    public EAudioSurfaces PAudioSurfaceTypes
    {
        get
        {
            return m_audioSurfaceType;
        }
    }
}
