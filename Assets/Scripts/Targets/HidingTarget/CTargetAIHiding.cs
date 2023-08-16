using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: An AI manager that manage all the bahaviour
Creator: Juan Calvin Raymond
Creation Date: 10-17-2016
Extra Notes: Work's with alvaro's code architecture
*/
[RequireComponent(typeof(CHidingTarget))]
public class CTargetAIHiding : ATargetAI
{
    [Header("Hiding AI Settings")]

    [Tooltip("Variable to detect how far the player be to change behaviour")]
    public float m_playerDetectionRadius = 500.0f;

    [Tooltip("Variable of how long the target will look at the player before change to hiding behaviour")]
    public float m_lookDuration = 2.0f;

    /*
    Description: Simple Constructor
    Parameters: aControlledTarget : which object this script belong to
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public void Start()
    {
        //Initialize all the behaviours
        m_controlledTarget = gameObject;
        InitBehaviors();
    }

    /*
    Description: A function that will update the current behaviour, and change behaviour
    Creator: Juan Calvin Raymond
    Creation Date: 10-18-2016
    */
    public override void UpdateTargetAI()
    {

        base.UpdateTargetAI();
        if (m_currentBehavior != null)
        {
            ChangeBehaviour();
        }
    }

    /*
    Description: Change behaviour with different condition for each behaviour
    Creator: Juan Calvin Raymond
    Creation Date: 10-18-2016
    */
    public override void ChangeBehaviour()
    {
        //Switch statement
        switch (m_currentBehavior.PTypeAI)
        {
            //if it's Idle
            case ETargetBehavior.Idle:
                //Cast behaviour script
                CIdleBehaviour tempIdleBehaviour = (CIdleBehaviour)m_currentBehavior;

                //Check if player is close enough
                if (tempIdleBehaviour.PDistanceToPlayer < m_playerDetectionRadius)
                {
                    //Set the behaviour to watching player
                    SetTargetAI(GetBehaviorFromList(ETargetBehavior.WatchingPlayer));
                }
                m_currentAIType = ETargetBehavior.Idle;
                break;

            //if it's watching player
            case ETargetBehavior.WatchingPlayer:
                //Cast behaviour script
                CWatchingPlayerBehaviour tempWatchBehaviour = (CWatchingPlayerBehaviour)m_currentBehavior;

                //if the target look long enough at the player
                if (tempWatchBehaviour.PLookingTimer > m_lookDuration)
                {
                    //change the behaviour to hiding
                    SetTargetAI(GetBehaviorFromList(ETargetBehavior.Hiding));
                }
                //if player is outside the range
                if (tempWatchBehaviour.PDistanceToPlayer > m_playerDetectionRadius)
                {
                    //change the behaviour to idle
                    SetTargetAI(GetBehaviorFromList(ETargetBehavior.Idle));
                }
                m_currentAIType = ETargetBehavior.WatchingPlayer;
                break;

            //if it's hiding
            case ETargetBehavior.Hiding:
                //Cast behaviour script
                CHidingBehaviour tempHidingBehaviour = (CHidingBehaviour)m_currentBehavior;

                //if the target is hidden
                if (tempHidingBehaviour.PIsHidden)
                {
                    //change the behaviour to Idle
                    SetTargetAI(GetBehaviorFromList(ETargetBehavior.WalkingBack));
                }
                m_currentAIType = ETargetBehavior.Hiding;
                break;

            //If it's walking back
            case ETargetBehavior.WalkingBack:
                //Cast behaviour script
                CWalkingBackBehaviour tempWalkingBackBehaviour = (CWalkingBackBehaviour)m_currentBehavior;

                //If the target is not hidden
                if(!tempWalkingBackBehaviour.PIsHidden)
                {
                    //change the behaviour to Idle
                    SetTargetAI(GetBehaviorFromList(ETargetBehavior.WatchingPlayer));
                }
                m_currentAIType = ETargetBehavior.WalkingBack;
                break;

            //just habbit
            default:
                break;
        }
    }

    /*
    Description: initializing all the behaviour
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public override void InitBehaviors()
    {
        //Call parent's InitBehaviour function
        base.InitBehaviors();

        //Set the first behaviour to Idle
        SetTargetAI(GetBehaviorFromList(ETargetBehavior.WatchingPlayer));
        m_currentAIType = ETargetBehavior.WatchingPlayer;
    }
}
