using UnityEngine;
using System.Collections;

using System;

/*
Description:Abstract class that serves as an AI of a target. It executes a single behavior.
Creator: Alvaro Chavez Mixco
Creation Date: October 8, 2016
*/
[Serializable]
public abstract class ATargetBehavior : MonoBehaviour
{
    private GameObject m_player;
    private ETargetBehavior m_typeAI;

    public ETargetBehavior PTypeAI
    {
        get
        {
            return m_typeAI;
        }
        set
        {
            m_typeAI = value;
        }
    }

    public GameObject PPlayer
    {
        get
        {
            return m_player;
        }
    }

    private void Start()
    {
        //Find the player object
        if (CGameManager.PInstanceGameManager != null)
        {
            m_player = CGameManager.PInstanceGameManager.PPlayerScript.gameObject;
        }
    }

    /*
    Description: Constructor for the Target AI manager. It  Saves the the object that is being controlled and initializes
    the different AIs.
    Parameters:  GameObject aControlledTarget-The target gameobject that will be controlled by the AI
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public abstract void Init();

    /*
    Description: virtual function that is intended to be called in the AI Fixedupdate.
    Parameters:  GameObject aControlledTarget-The target gameobject that will be controlled by the AI
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void FixedUpdateAI(GameObject aControlledTarget)
    {
    }

    /*
    Description: Abstract function that is intended to be called in the AI update.
    Parameters:  GameObject aControlledTarget-The target gameobject that will be controlled by the AI
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public abstract void UpdateAI(GameObject aControlledTarget);


    /*
    Description: virtual function that is intended when this behavior is activated, or start being used.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void Activate()
    {
    }

    /*
    Description: virtual function that is intended when this behavior is deactivated, or stopped being used
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    public virtual void Deactivate()
    {
    }
}
