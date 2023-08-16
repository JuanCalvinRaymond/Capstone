using UnityEngine;
using System.Collections;


/*
Description: Managing the animation of the platform and anything related to platform animation
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 7 Dec 2016
Extra Notes:
*/
[RequireComponent(typeof(Animator))]
public class CMovingPlatformAnimation : MonoBehaviour
{
    private const string M_BEGINNER_ANIMATION_NAME = "Beginner";
    private const string M_ADVANCED_ANIMATION_NAME = "Advanced";

    private const string M_BEGINNER_TRANSITION_BOOL_NAME = "m_isBeginner";
    private const string M_ADVANCED_TRANSITION_BOOL_NAME = "m_isAdvanced";

    //Animator component
    private Animator m_animator;
    private string m_currentAnimationName = string.Empty;

    [Tooltip("If true, this will prevent the game from going to play state to end game state. Keeping the game on play state")]
    public bool m_isPractice = false;

    public string PAnimationName
    {
        get
        {
            return m_currentAnimationName;
        }
    }

    public Animator PAnimator
    {
        get
        {
            return m_animator;
        }
    }

    /*
    Description: Initializing animator variable
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 7 Dec 2016
    Extra Notes:
    */
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    /*
    Description: Suscribe to game manager event
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 8th, 2017
    */
    private void Start()
    {
        //Suscribe to game manager event
        CGameManager.PInstanceGameManager.OnGameStateChange += PausePlatformMovement;
    }

    /*
    Description: Unsuscribe from game manager event
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 8th, 2017
    */
    private void OnDestroy()
    {
        //Unsuscribe from game manager event
        CGameManager.PInstanceGameManager.OnGameStateChange -= PausePlatformMovement;
    }

    /*
    Description: Managing platform animation
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 7 Dec 2016
    Extra Notes:
    */
    private void Update()
    {
        if (CGameManager.PInstanceGameManager != null)
        {
            if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play
                && CGameManager.PInstanceGameManager.PLevelState != ELevelState.Practice)
            {
                //If animation is finished
                if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && m_isPractice == false)
                {
                    //Call EndGame function on Game Manager script
                    CGameManager.PInstanceGameManager.PGameState = EGameStates.EndGame;

                }
            }
        }
    }

    /*
    Description: Set the animation name and trnastiion state for the platform.
    Parameters(Optional):
    Creator: Alvaro Chavez Mixco
    Creation Date: Alvaro Chavez Mixco
    Extra Notes:
    */
    public void SetAnimation(ELevelState aLevelState)
    {
        if (m_animator != null)
        {
            switch (aLevelState)
            {
                case ELevelState.Beginner:
                    m_animator.SetBool(M_BEGINNER_TRANSITION_BOOL_NAME, true);
                    m_currentAnimationName = M_BEGINNER_ANIMATION_NAME;
                    break;
                case ELevelState.Advanced:
                    m_animator.SetBool(M_ADVANCED_TRANSITION_BOOL_NAME, true);
                    m_currentAnimationName = M_ADVANCED_ANIMATION_NAME;
                    break;
                default:
                    break;
            }
        }
    }

    /*
    Description: Function used to pause the platform movement when the game is paused.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 8th, 2017
    Extra Notes: Function called from game manager OnGameStateChange event.
    */
    public void PausePlatformMovement(EGameStates aNewState)
    {
        //If the new game state is paused
        if (aNewState == EGameStates.Paused)
        {
            //Disable the animator to "pause" the animation
            m_animator.enabled = false;
        }
        else//If the game state  is not paused
        {
            //Ensure the animator is enabled
            m_animator.enabled = true;
        }
    }
}
