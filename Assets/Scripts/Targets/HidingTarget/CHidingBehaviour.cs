using UnityEngine;
using System.Collections;

using System;

/*
Description: Find a closest object and run behind it
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 10-17-2016
Extra Notes: Works with alvaro's code architecture
*/
public class CHidingBehaviour : ATargetBehavior
{ 
    //List of object within radius
    private Collider[] m_sphereHit;

    //Variable of the target object to hide
    private float m_nearestBuilding = float.MaxValue;
    private Vector3 m_positionToMove;
    private GameObject m_targetObjectToHide;

    //distance squared
    public float m_arriveRadiusSquared = 0.005f;


    //Variable to see if this behaviour is finiehed
    private bool m_IsHidden = false;
    
    [Tooltip("How fast the target will move")]
    public float m_moveSpeed = 5.0f;

    [Tooltip("Radius to search for object to hide")]
    public Vector3 m_ViewSize;

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
    Creation Date: 10-17-2016
    Extra Notes
    */
    public override void Init()
    {
        PTypeAI = ETargetBehavior.Hiding;
        m_IsHidden = false;
    }

    /*
    Description: Move to behind the object
    Parameters(Optional): aControlledTarget : which target the script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    Extra Notes
    */
    public override void FixedUpdateAI(GameObject aControlledTarget)
    {
        //If there's an object nearby
        if (m_targetObjectToHide != null)
        {
            //lerp the target position to the target position
            aControlledTarget.transform.position = Vector3.Lerp(aControlledTarget.transform.position, m_positionToMove, m_moveSpeed * Time.fixedDeltaTime * CGameManager.PInstanceGameManager.PTimeScale);
            if (Vector3.SqrMagnitude(m_positionToMove - aControlledTarget.transform.position) <= m_arriveRadiusSquared)//when the target have arrived 
            {
                //it is hidden
                m_IsHidden = true;
                //destroy the target,it's not going to play on destroy sound
                //Destroy(aControlledTarget);
            }

        }
    }
    /*
    Description: Look for an object, calculate which one is the closest
    Parameters(Optional): aControlledTarget : which target the script belongs to
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    Extra Notes
    */
    public override void UpdateAI(GameObject aControlledTarget)
    {
        if (m_targetObjectToHide == null)
        {
            //Check surrounding
            m_sphereHit = Physics.OverlapBox(aControlledTarget.transform.position, new Vector3(m_ViewSize.x, aControlledTarget.GetComponent<MeshRenderer>().bounds.size.y / 2, m_ViewSize.z));

            //iterate through all objects that we hit
            for (int i = 0; i < m_sphereHit.Length; i++)
            {
                //I need help on layer mask (i didn't understand it completely at jimmy's class, my bad)
                if (m_sphereHit[i].CompareTag(CGlobalTags.M_TAG_BUILDING) == true)
                {
                    //find the distance of the object
                    float distance = Vector3.Distance(m_sphereHit[i].transform.position, aControlledTarget.transform.position);

                    //if the object have shorter distance
                    if (distance < m_nearestBuilding)
                    {
                        //set the nearest distance to be this distance
                        m_nearestBuilding = distance;

                        //set the target to this object
                        m_targetObjectToHide = m_sphereHit[i].gameObject;
                    }
                }
            }
            //If target is not null
            if (m_targetObjectToHide != null)
            {
                //Check which side we need to go
                CheckingWhichSide(PPlayer, m_targetObjectToHide);
            }

        }

    }

    /*
    Description: Finding which side to hide based on player's position compare to the object to hide
    Parameters(Optional): aPlayer : the player object
                          aHidingSpot : the nearest object to hide
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    Extra Notes :
    */
    private void CheckingWhichSide(GameObject aPlayer, GameObject aHidingSpot)
    {
        //Check which direction the target should move to
        Vector3 direction = (aHidingSpot.transform.position - aPlayer.transform.position);
        direction = direction.normalized;
        direction.x *= aHidingSpot.GetComponent<Renderer>().bounds.size.x;
        direction.y *= aHidingSpot.GetComponent<Renderer>().bounds.size.y;
        direction.z *= aHidingSpot.GetComponent<Renderer>().bounds.size.z;

        //DEBUGLIST-AAA
        //Debug.DrawLine(aPlayer.transform.position, aPlayer.transform.position + direction * 50, Color.red, 10.0f);

        //Set the position the target should move
        m_positionToMove = aHidingSpot.transform.position + direction;
        m_positionToMove.y = transform.position.y;
    }

    /*
    Description: Activating the behaviour
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 10-17-2016
    Extra Notes: just setting back all the variable that need to initialize again
    */
    public override void Activate()
    {
        base.Activate();
        m_IsHidden = false;

        m_targetObjectToHide = null;
        m_nearestBuilding = float.MaxValue;
    }
}
