using UnityEngine;
using System.Collections;
/*
Description: After certain of time walk back to original position
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 21 Mar 2017
Extra Notes: Works with alvaro's code architecture
*/
public class CWalkingBackBehaviour : ATargetBehavior
{
    //Target original position
    private Vector3 m_originalPosition;

    private float m_waitTimer;

    //Variable to see if this behaviour is finiehed
    private bool m_IsHidden = true;
    private bool m_IsWaiting = true;

    //distance squared
    public float m_arriveRadiusSquared = 0.005f;

    [Tooltip("How fast the target will move")]
    public float m_moveSpeed = 5.0f;

    [Tooltip("Radius to search for object to hide")]
    public float m_waitDuration = 5.0f;

    public bool PIsHidden
    {
        get
        {
            return m_IsHidden;
        }
    }

    /*
    Description: Simple Init
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes
    */
    public override void Init()
    {
        PTypeAI = ETargetBehavior.WalkingBack;
        m_waitTimer = 0.0f;
        m_originalPosition = transform.position;
        m_IsWaiting = true;
        m_IsHidden = true;
    }

    /*
    Description: Move to original position
    Parameters(Optional): aControlledTarget : which target the script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes
    */
    public override void FixedUpdateAI(GameObject aControlledTarget)
    {
        if(!m_IsWaiting)
        {
            //Lerp the target position to the target position
            aControlledTarget.transform.position = Vector3.Lerp(aControlledTarget.transform.position, m_originalPosition, m_moveSpeed * Time.fixedDeltaTime * CGameManager.PInstanceGameManager.PTimeScale);

            if (Vector3.SqrMagnitude(m_originalPosition - aControlledTarget.transform.position) <= m_arriveRadiusSquared)//when the target have arrived 
            {
                //It is not hidden
                m_IsHidden = !m_IsHidden;
            }
        }
    }
    /*
    Description: Update timer
    Parameters(Optional): aControlledTarget : which target the script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes
    */
    public override void UpdateAI(GameObject aControlledTarget)
    {
        if(m_IsWaiting)
        {
            m_waitTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();

            if (m_waitTimer > m_waitDuration)
            {
                m_IsWaiting = !m_IsWaiting;
            }

        }

    }


    /*
    Description: Activating the behaviour
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes: just setting back all the variable that need to initialize again
    */
    public override void Activate()
    {
        base.Activate();
        m_IsWaiting = true;
        m_IsHidden = true;
        m_waitTimer = 0;

    }
}
