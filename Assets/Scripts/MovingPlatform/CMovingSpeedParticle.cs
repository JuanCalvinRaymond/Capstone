using UnityEngine;
using System.Collections;

/*
Description: Stop or play particle based on platform speed or game state
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 21 Mar 2017
Extra Notes:
*/
[RequireComponent(typeof(ParticleSystem))]
public class CMovingSpeedParticle : MonoBehaviour
{
    //On fire system script
    private COnFireSystem m_onFireSystem;

    //Particle system component
    private ParticleSystem m_particleSystem;

    //Speed threshold to play particle
    public float m_minimalSpeedToPlay = 0.75f;

    /*
    Description: Init variable
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes:
    */
    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    /*
    Description: Subscribe to events
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes: 
    */
    private void Start()
    {
        m_onFireSystem = CGameManager.PInstanceGameManager.POnFireSystem;
        m_onFireSystem.OnSpeedChanged += CheckPlatformSpeed;

        CGameManager.PInstanceGameManager.OnEndGameState += StopParticle;
        CGameManager.PInstanceGameManager.OnPauseState += StopParticle;
        CGameManager.PInstanceGameManager.OnMainMenuState += StopParticle;

        m_particleSystem.Stop();
    }

    /*
    Description: Unsubscribe to events
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes: 
    */
    private void OnDestroy()
    {
        m_onFireSystem.OnSpeedChanged -= CheckPlatformSpeed;

        CGameManager.PInstanceGameManager.OnEndGameState -= StopParticle;
        CGameManager.PInstanceGameManager.OnPauseState -= StopParticle;
        CGameManager.PInstanceGameManager.OnMainMenuState -= StopParticle;
    }

    /*
    Description: Stop or play particle based on argument
    Parameters(Optional): aPlatformSpeedMultiplier : Platform speed from on fire system
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes: 
    */
    private void CheckPlatformSpeed(float aPlatformSpeedMultiplier)
    {
        if(CGameManager.PInstanceGameManager.PLevelState != ELevelState.Practice)
        {
            //If speed is higher than minimal threshold
            if(aPlatformSpeedMultiplier >= m_minimalSpeedToPlay)
            {
                //Play particle
                PlayParticle();
            }
            //If speed is lower than minimal threshold
            else
            {
                //Stop particle
                StopParticle();
            }
        }
    }

    /*
    Description: Stop particle
    Parameters(Optional): 
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes: 
    */
    private void StopParticle()
    {
        m_particleSystem.Stop();
    }

    /*
    Description: Play particle
    Parameters(Optional): 
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes: 
    */
    private void PlayParticle()
    {
        m_particleSystem.Play();
    }

}
