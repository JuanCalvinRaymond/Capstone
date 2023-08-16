using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

/*
Description:Abstract class that will control when and how does the target changes (if he has many) his AI behaviors.
Creator: Alvaro Chavez Mixco
Creation Date: October 8, 2016
*/
[RequireComponent(typeof(ITarget))]
public abstract class ATargetAI : MonoBehaviour
{
    protected ATargetBehavior m_currentBehavior = null;

    public ETargetBehavior m_currentAIType = ETargetBehavior.Undefined;
    [Tooltip("The target gameobject that is being controlled by the AI")]
    public GameObject m_controlledTarget = null;

    [Header("AI Behaviours")]
    public List<ATargetBehavior> m_listOfBehaviour;

    public ETargetBehavior PCurrentBehaviorType
    {
        get
        {
            if (m_currentBehavior != null) //If the current AI is valid
            {
                return m_currentBehavior.PTypeAI;//Return its type
            }

            return ETargetBehavior.Undefined;//Return undefined
        }
    }

    /*
    Description: Virtual function that should be called in the object FixedUpdate. This will update the current AI fixed update.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void FixedUpdateTargetAI()
    {
        if (m_currentBehavior != null)
        {
            if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
            {
                m_currentBehavior.FixedUpdateAI(m_controlledTarget);
            }
        }
    }

    /*
    Description: Virtual function that should be called in the object Update. This will update the current AI update.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void UpdateTargetAI()
    {
        if (m_currentBehavior != null)
        {
            if(CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
            {
                m_currentBehavior.UpdateAI(m_controlledTarget);
            }
        }
    }

    /*
    Description: Abstract function that will be called to check a condition and change behaviour accordingly
    Creator: Juan Calvin Raymond
    Creation Date: October 18, 2016
    */
    public abstract void ChangeBehaviour();

    /*
    Description: Virtual function that should be called before the start of the program. This would be in charge of initializing
    the different AI behaviors.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void InitBehaviors()
    {
        if (m_listOfBehaviour != null)
        {
            for (int i = 0; i < m_listOfBehaviour.Count; i++)
            {
                if (m_listOfBehaviour[i] != null)
                {
                    m_listOfBehaviour[i].Init();
                }
            }
        }
    }

    /*
    Description: Virtual function that will iterate through list of behaviour and return a behaviour script if founded
    Creator: Juan Calvin Raymond
    Creation Date: October 18, 2016
    */
    public virtual ATargetBehavior GetBehaviorFromList(ETargetBehavior aBehaviour)
    {
        if (m_listOfBehaviour != null)
        {
            for (int i = 0; i < m_listOfBehaviour.Count; i++)
            {
                if (m_listOfBehaviour[i] != null)
                {
                    if (m_listOfBehaviour[i].PTypeAI == aBehaviour)
                    {
                        return m_listOfBehaviour[i];
                    }
                }
            }
        }
        return null;
    }

    /*
    Description: Virtual function that is used to easily deactivate the current behavior, and activate the new one.
    Parameters:  CTargetBehavior aBehavior - the new behavior we want to set.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void SetTargetAI(ATargetBehavior aBehavior)
    {
        //If the AI we want to set is not the one we currently have
        if (aBehavior != m_currentBehavior)
        {
            if (m_currentBehavior != null)//If the current behavior we have is valid
            {
                m_currentBehavior.Deactivate();//Deactivate the current behavior
            }

            if (aBehavior != null)//If the behavior we want to set is valid
            {
                aBehavior.Activate();//Activate the new behavior

                m_currentBehavior = aBehavior;//Set the new behavior as
            }
        }
    }
}
