using UnityEngine;
using System.Collections;

/*
Description: Class to play the respective surface sound when colliding with an object
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, March 28th, 2017
*/
public class CAudioCollisionHandler : MonoBehaviour
{
    private Rigidbody m_rigidBody;

    [Tooltip("The minimum speed this object has to have when colliding with something to play a sound.")]
    public float m_minSquareSpeedForSound = 0.0f;

    [Header("Sounds")]
    public SAudioSourceSettings m_audioSourceSettings;

    [Space(20)]
    public AudioClip[] m_defaultClips;
    [Space(20)]
    public AudioClip[] m_metalClips;
    [Space(20)]
    public AudioClip[] m_woodClips;
    [Space(20)]
    public AudioClip[] m_concreteClips;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    /*
    Description: Play a respective landom clip according to the surface type, at the desire locati
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, March 28th, 2017
    */
    private void PlaySurfaceSound(EAudioSurfaces aAudioSurface, Vector3 aPosition)
    {
        //According to audio type
        switch (aAudioSurface)
        {
            case EAudioSurfaces.Metal:
                CUtilitySound.PlayRandomSoundAtLocation(m_metalClips, aPosition, m_audioSourceSettings);
                break;
            case EAudioSurfaces.Wood:
                CUtilitySound.PlayRandomSoundAtLocation(m_woodClips, aPosition, m_audioSourceSettings);
                break;
            case EAudioSurfaces.Concrete:
                CUtilitySound.PlayRandomSoundAtLocation(m_concreteClips, aPosition, m_audioSourceSettings);
                break;
            case EAudioSurfaces.Unknown:
                CUtilitySound.PlayRandomSoundAtLocation(m_defaultClips, aPosition, m_audioSourceSettings);
                break;
            default:
                break;
        }
    }

    /*
    Description: When there is a collision ,detects what type of surface it is and play asound
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, March 28th, 2017
    */
    private void OnCollisionEnter(Collision aCollision)
    {
        float currentSquareSpeed = 0.0f;

        //If the object has a rigid body
        if (m_rigidBody != null)
        {
            //Set the speed of the object according to the rigid body
            currentSquareSpeed = m_rigidBody.velocity.magnitude;
        }

        //If the speed is above the minimum to play a sound
        if (currentSquareSpeed > m_minSquareSpeedForSound)
        {
            //Get the surface component
            CSurfaceAudio surface = aCollision.gameObject.GetComponent<CSurfaceAudio>();
            EAudioSurfaces surfaceType = EAudioSurfaces.Unknown;

            //If there is a surface component
            if (surface != null)
            {
                //Save the surface type of the object
                surfaceType = surface.PAudioSurfaceTypes;

                //Play the respective surface sound
                PlaySurfaceSound(surfaceType, aCollision.transform.position);
            }
        }
    }
}
