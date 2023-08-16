using UnityEngine;
using System.Collections;

/*
Description: Class used to play audio when the target is hit or destroyed    
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, March 09th, 2017
*/
[RequireComponent(typeof(ITarget), typeof(AudioSource))]
public class CTargetAudio : MonoBehaviour
{
    private AudioSource m_audioSource;
    private ITarget m_target;

    [Tooltip("Audio clips pool that will be used for the random sound when the target is hit")]
    public AudioClip[] m_hitSounds;
    [Tooltip("Audio clips pool that will be used for the random sound when the target is destroyrf")]
    public AudioClip[] m_destroyedSound;

    /*
    Description: Get the desired components
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void Awake()
    {
        //Get the desired components
        m_audioSource = GetComponent<AudioSource>();
        m_target = GetComponent<ITarget>();
    }

    /*
    Description: Suscribe to target events, when target hit or destroyed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void Start()
    {
        m_target.OnTargetDamaged += PlayHitSound;
        m_target.OnTargetDying += PlayDestroyedSound;
    }

    /*
    Description: Unsuscribe from the target events
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void OnDestroy()
    {
        m_target.OnTargetDamaged -= PlayHitSound;
        m_target.OnTargetDying -= PlayDestroyedSound;
    }

    /*
    Description: Sound to be played when the target is hit.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    Extra Notes: This functions gets called by suscribing to the target OnTargetDamaged event.
    */
    private void PlayHitSound(int aDamagedAmount, int aHealthRemaining, float aHealthPercent, int aScoreValue)
    {
        //If the target is actually receiving damage
        if (aDamagedAmount >= 0)
        {
            CUtilitySound.PlayRandomSound(m_audioSource, m_hitSounds);
        }
    }

    /*
    Description: Sound to be played when the target is set to be dying.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    Extra Notes: This functions gets called by suscribing to the target OnTargetDying event.
    */
    private void PlayDestroyedSound(float aDyingTime)
    {
        CUtilitySound.PlayRandomSound(m_audioSource, m_destroyedSound);
    }

    /*
    Description: Function to sort in editor alphanumerically all the sound arrays 
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Context menu function accessible by right clicking the component.
    */
    [ContextMenu("Sort Sounds Alphanumerically")]
    public void SortImageSequences()
    {
        CUtilitySorting.SortArray(ref m_hitSounds);
        CUtilitySorting.SortArray(ref m_destroyedSound);
    }
}
