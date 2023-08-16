using UnityEngine;
using System.Collections;
using System;

/*
Description: A target with a hiding AI behavior
Creator: Juan Calvin Raymond
Creation Date: 10-17-2016
*/
public class CHidingTarget : ACoreTarget
{
    [Tooltip("An AI manager that will manage the behaviour")]
    public CTargetAIHiding m_targetAIManager;

    /*
    Description: call fixed update function on AI manager
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    private void FixedUpdate()
    {
        //If we have an AI manager
        if (m_targetAIManager != null)
        {
            //Call fixed update on AI manager
            m_targetAIManager.FixedUpdateTargetAI();
        }
    }

    /*
    Description: Call update function on AI manager
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    protected virtual void Update()
    {
        //IF we have an AI manager
        if (m_targetAIManager != null)
        {
            //Update the manager
            m_targetAIManager.UpdateTargetAI();
        }
    }
}
