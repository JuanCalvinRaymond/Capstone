using UnityEngine;
using System.Collections;

/*
Description: Target will do nothing until it detect player nearby
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 10-17-2016
Extra Notes: Works with alvaro's code architecture
*/
public class CIdleBehaviour : ATargetBehavior
{
    //how far is the player
    private float m_distanceToPlayer;

    public float PDistanceToPlayer
    {
        get
        {
            return m_distanceToPlayer;
        }
    }

    /*
    Description: Simple Init
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public override void Init()
    {
        PTypeAI = ETargetBehavior.Idle;
    }


    /*
    Description: Constantly checking where the player is
    Parameters: aControlledTarget : which target this script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    */
    public override void UpdateAI(GameObject aControlledTarget)
    {
        m_distanceToPlayer = Vector3.Distance(aControlledTarget.transform.position, PPlayer.transform.position);
    }

    /*
   Description: Activating the behaviour
   Creator: Juan Calvin Raymond
   Creation Date: 10-17-2016
   Extra Notes: just setting back all the variable that need to initialize again
   */

    public override void Activate()
    {
        base.Activate();
    }
}
