using UnityEngine;
using System.Collections;

/*
Description: Watching the player for a certain amount of time to give player a time window to shot, then change the behaviour to hiding
Creator: Juan Calvin Raymond
Creation Date: 10-17-2016
Extra Notes : Works with alvaro's code architecture
*/
public class CWatchingPlayerBehaviour : ATargetBehavior
{
    //Timer variable
    private float m_lookingTimer;
    private float m_raycastTimer;

    //Variable for raycast
    private Ray m_ray;
    private RaycastHit m_hit;

    //how far is the player
    private float m_distanceToPlayer;

    //Variable to tweak in editor
    public float m_timeBetweenEachRaycast = 1.0f;
    public float m_viewDistance = 500.0f;

    public float PLookingTimer
    {
        get
        {
            return m_lookingTimer;
        }
    }

    public float PDistanceToPlayer
    {
        get
        {
            return m_distanceToPlayer;
        }
    }

    /*
    Description: Simple Init Function
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public override void Init()
    {
        PTypeAI = ETargetBehavior.WatchingPlayer;
        m_lookingTimer = 0;
        m_raycastTimer = 0;
    }

    /*
    Description: Constantly look at the player and check the distance
    Parameters: aControlledTarget : which object this script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public override void FixedUpdateAI(GameObject aControlledTarget)
    {
    }

    /*
    Description: It will raycast forward, check if it hit the player, wait for a certain amount of time, then change to hiding behaviour
    Parameters: aControlledTarget : which object this script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public override void UpdateAI(GameObject aControlledTarget)
    {
        //Increase raycast timer
        m_raycastTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        //if raycast timer finished
        if(m_raycastTimer > m_timeBetweenEachRaycast)
        {
            //Update ray's position
            m_ray = new Ray(aControlledTarget.transform.position, PPlayer.transform.position - aControlledTarget.transform.position);
            //If it hit something
            if (Physics.Raycast(m_ray, out m_hit, m_viewDistance))
            {
                //Check if it a player
                if (m_hit.collider.CompareTag(CGlobalTags.M_TAG_PLAYER) == true)
                {
                    //Start the timer
                    m_lookingTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();
                }
            }
        }

        //Calculate distance between target and player
        m_distanceToPlayer = Vector3.Distance(PPlayer.transform.position, aControlledTarget.transform.position);
        //Debug Purposes
        //DEBUGLIST-AAA
        //Debug.DrawRay(m_ray.origin, m_ray.direction * m_viewDistance, Color.blue, 2.0f);
    }

    /*
    Description: Activating the behaviour
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    Extra Notes: just setting back all the variable that need to reset again
    */
    public override void Activate()
    {
        base.Activate();

        m_lookingTimer = 0;
        m_raycastTimer = 0;
    }
}
