using UnityEngine;
using System.Collections;

/*
Description: Utility class to play and control sounds and audio sources.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, October 31st, 2016
*/
public class CUtilitySound
{
    /*
     Description: Helper function, to play a sound one shot.
     Parameters: AudioSource aAudioSource - The source that will be play the sound
                 AudioClip aAudioClip - The sound that will be played
                 float aVolumeScale- The volume scale for the sound, by default is 1.0f
     Creator: Alvaro Chavez Mixco
     Creation Date:  Sunday, Novemeber 20, 2016
    */
    public static void PlaySoundOneShot(AudioSource aAudioSource, AudioClip aAudioClip, float aVolumeScale = 1.0f)
    {
        //If the audio source and the audio clip are valid
        if (aAudioSource != null && aAudioClip != null)
        {
            //If the audio source is enabled
            if (aAudioSource.enabled == true)
            {
                //Play the audio with the desired volume
                aAudioSource.PlayOneShot(aAudioClip, aVolumeScale);
            }
        }
    }

    /*
     Description: Helper function, to set an audio clip and play the sound on an audio source.
     Parameters: AudioSource aAudioSource - The source that will be play the sound
                 AudioClip aAudioClip - The sound that will be played
                 ulong aDelay- The delay the object will have before playing a sound.
     Creator: Alvaro Chavez Mixco
     Creation Date:  Friday, February 3rd, 2016
    */
    public static void PlaySound(AudioSource aAudioSource, AudioClip aAudioClip, float aDelay = 0)
    {
        //If the audio source and the audio clip are valid
        if (aAudioSource != null && aAudioClip != null)
        {
            //If the audio source is enabled
            if (aAudioSource.enabled == true)
            {
                aAudioSource.clip = aAudioClip;

                //Play the audio with the delay
                aAudioSource.PlayDelayed(aDelay);
            }
        }
    }

    /*
    Description: Helper function, to pause an audio source
    Parameters: AudioSource aAudioSource - The source that will be paused.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, February 3rd, 2016
    */
    public static void PauseSound(AudioSource aAudioSource)
    {
        //If the audio source is valid
        if (aAudioSource != null)
        {
            //Pause the audio source
            aAudioSource.Pause();
        }
    }

    /*
     Description: Helper function, to stop an audio source
     Parameters: AudioSource aAudioSource - The source that will be stopped.
     Creator: Alvaro Chavez Mixco
     Creation Date:  Friday, February 3rd, 2016
     */
    public static void StopSound(AudioSource aAudioSource)
    {
        //If the audio source is valid
        if (aAudioSource != null)
        {
            //Stop the audio source
            aAudioSource.Stop();
        }
    }

    /*
     Description: Helper function, to play a sound at a certain location. This works by creating a gameobject,
                  merely to play an audio source and then destroying. Unity usually does this behind the scene 
                  with its play at clip at point function, but is beign done manually in here in order to customize 
                  the audio source.
     Parameters: Vector3 aLocation - Where the sound will be played
                 AudioClip aAudioClip - The sound that will be played
                 SAudioSourceSettings aAudioSourceSettings - The settings for the audio source to play
                 float aVolumeScale- The volume scale for the sound, by default is 1.0f
     Creator: Alvaro Chavez Mixco
     Creation Date:  Sunday, Novemeber 20, 2016
     Extra Notes: 
    */
    public static void PlaySoundAtLocation(Vector3 aLocation, AudioClip aAudioClip,
        SAudioSourceSettings aAudioSourceSettings, float aVolumeScale = 1.0f)
    {
        //If  audio clip is valid
        if (aAudioClip != null)
        {
            //Create the object to play the audio source
            GameObject tempAudioSourceObject = new GameObject("PlaySoundAtLocation");

            //Set the audio source at desired position
            tempAudioSourceObject.transform.position = aLocation;

            //Add an audio source to the object
            AudioSource audioSource = tempAudioSourceObject.AddComponent<AudioSource>();

            //Set the audio clip that will be played
            audioSource.clip = aAudioClip;

            //Set the audio mixer group
            audioSource.outputAudioMixerGroup = aAudioSourceSettings.m_outputAudioMixerGroup;

            //Set the spatial blend
            audioSource.spatialBlend = aAudioSourceSettings.m_spatialBlend;

            //Play the sound
            audioSource.Play();

            //Set the object to be destroyed after the clip has played
            GameObject.Destroy(tempAudioSourceObject, audioSource.clip.length);

            //Don't use unity method in order to be able to customize the source settings
            //Play the audio at the desired location with the desired volume
            //AudioSource.PlayClipAtPoint(aAudioClip, aLocation, aVolumeScale);
        }
    }

    /*
    Description: Randomly picks an audio clip from an array and plays it at the desired audio source
    Parameters:AudioSource aAudioSource-The source from where the sound will be played
               AudioClip[] aListOfAudioClips- The pool of possible audios to play
               float aVolumeScale- The volume scale for the sound, by default is 1.0f  
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    Extra Notes: The null checks for the audio source and audio clips are being done inside the PlaySound function
    */
    public static void PlayRandomSound(AudioSource aAudioSource, AudioClip[] aListOfAudioClips, float aVolumeScale = 1.0f)
    {
        //If there are audio clips in the
        if (aListOfAudioClips.Length > 0)
        {
            //Get the index of a random audio clip in the list
            int indexSoundToPlay = Mathf.FloorToInt(Random.Range(0, aListOfAudioClips.Length));

            //Play the sound
            PlaySoundOneShot(aAudioSource, aListOfAudioClips[indexSoundToPlay], aVolumeScale);
        }
    }

    /*
    Description: Randomly picks an audio clip from an array and plays it at the desired location
    Parameters:Vector3 aPosition - The position where the audio will be played
               AudioClip[] aListOfAudioClips- The pool of possible audios to play
               SAudioSourceSettings aAudioSourceSettings - The settings for the audio source to play
               float aVolumeScale- The volume scale for the sound, by default is 1.0f  
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 22nd, 2017
    Extra Notes: The null checks for the audio source and audio clips are being done inside the PlaySound function
    */
    public static void PlayRandomSoundAtLocation(AudioClip[] aListOfAudioClips,Vector3 aPosition, 
        SAudioSourceSettings aAudioSourceSettings, float aVolumeScale = 1.0f)
    {
        //If there are audio clips in the
        if (aListOfAudioClips.Length > 0)
        {
            //Get the index of a random audio clip in the list
            int indexSoundToPlay = Mathf.FloorToInt(Random.Range(0, aListOfAudioClips.Length));

            //Play the sound
            PlaySoundAtLocation(aPosition,aListOfAudioClips[indexSoundToPlay], aAudioSourceSettings, aVolumeScale);
        }
    }

    /*
    Description: Randomly picks an audio clip from an array and plays it at a position. This is done for all the positions
    being passed in
    Parameters:Vector3[] aListOfPosition-The positions where a random clip will be palyed
               AudioClip[] aListOfAudioClips- The pool of possible audios to play
               SAudioSourceSettings aAudioSourceSettings - The settings for the audio source to play
               float aVolumeScale- The volume scale for the sound, by default is 1.0f  
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    Extra Notes: The null checks for the  audio clips are being done inside the PlaySoundAtLocation function
    */
    public static void PlayRandomClipsAtLocations( Vector3[] aListOfPosition, AudioClip[] aListOfAudioClips, 
        SAudioSourceSettings aAudioSourceSettings,float aVolumeScale = 1.0f)
    {
        //If the arrays are not empty and they have the same size
        if ((aListOfAudioClips.Length > 0 && aListOfPosition.Length > 0) && (aListOfAudioClips.Length == aListOfPosition.Length))
        {
            int indexSoundToPlay;

            //Go through all the positions
            for (int i = 0; i < aListOfPosition.Length; i++)
            {
                //Get the index of a random audio clip in the list
                indexSoundToPlay = Mathf.FloorToInt(Random.Range(0, aListOfAudioClips.Length));

                ////Play the sound at the desired location
                PlaySoundAtLocation(aListOfPosition[indexSoundToPlay], aListOfAudioClips[indexSoundToPlay],
                    aAudioSourceSettings, aVolumeScale);
            }
        }
    }
}