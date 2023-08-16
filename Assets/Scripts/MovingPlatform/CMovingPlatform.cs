using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Object will move form 1 point to another point
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 10-08-2016
Extra Notes: It will work with how many point there are in the scene
*/
public class CMovingPlatform : MonoBehaviour
{
    //Variable to find the waypoint list and the next waypoint to go
    private Transform m_target;
    private int m_targetCounter;
    private List<GameObject> m_waypointList;

    //Variable for moving and rotating
    private float m_movingSpeed;
    private float m_rotationSpeed;
    private Vector3 m_surfaceVelocity;

    //Variable to change during in game
    private float m_newMovingSpeed;
    private float m_newRotationSpeed;
    private float m_InterpTime;

    //the dummy for dummy people, haha get it?
    public Transform m_dummyPlatform;

    //The waypoint
    public CWaypoint m_startingWaypoint;

    [Tooltip("Radius to say we arrive at the point")]
    public float m_squaredArriveRadius = 2.0f;
    public bool m_looping = true;

    //Default value for speed
    [Header("Speed Values")]
    public float m_defaultMovingSpeed = 15.0f;
    public float m_defaultRotationSpeed = 15.0f;

    public Transform PTarget
    {
        set
        {
            m_target = value;
        }
    }

    /*
    Description: Init all variable
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes: 
    */
    private void Awake()
    {
        //set the speed to default value
        m_movingSpeed = m_defaultMovingSpeed;
        m_rotationSpeed = m_defaultRotationSpeed;

        m_newMovingSpeed = m_defaultMovingSpeed;
        m_newRotationSpeed = m_defaultRotationSpeed;

        m_InterpTime = 0;
    }

    /*
    Description: Grab starting waypoint script and set the track
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes: 
    */
    private void Start()
    {
        //if the starting waypoint is set
        if (m_startingWaypoint != null)
        {
            //set the track to the waypoint list
            SettingTrack(m_startingWaypoint.PListOfWaypoint);
            //if there an object in the list
            if (m_waypointList.Count > 0)
            {
                //set the position to the first position of the waypoint
                transform.position = m_waypointList[0].transform.position;
            }
            else
            {
                //DEBUGLIST - AAA
                //Debug.Log("There's no waypoint on the list");
            }
        }
        else
        {
            //DEBUGLIST - AAA
            //Debug.Log("Can't find m_starting waypoint, probably you forgot to set it");
        }
    }


    /*
    Description: Contantly updating speed
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    */
    private void Update()
    {
        //Constantly updating moving speed and rotation speed 
        m_movingSpeed = Mathf.Lerp(m_movingSpeed, m_newMovingSpeed, m_InterpTime * CGameManager.PInstanceGameManager.GetScaledDeltaTime());
        m_rotationSpeed = Mathf.Lerp(m_rotationSpeed, m_newRotationSpeed, m_InterpTime * CGameManager.PInstanceGameManager.GetScaledDeltaTime());

        //Update the position
        MovementUpdate();
    }

    /*
    Description: Changing the target of waypoint to go and updating the position based on velocity
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    */
    private void MovementUpdate()
    {
        if (m_target != null)
        {
            //Check if we don't have more area to move to the target
            if (Vector3.SqrMagnitude(transform.position- m_target.position) < m_squaredArriveRadius)
            {
                //move the target to the next waypoint
                m_targetCounter++;
                //check if there's still waypoint on the list
                if (m_targetCounter < m_waypointList.Count)
                {
                    //set target to next waypoint
                    m_target = m_waypointList[m_targetCounter].transform;
                }
                //if it's the end of the list
                else
                {
                    //If it is looping
                    if (m_looping == true)
                    {
                        //Set teh waypoint back to start
                        m_target = m_waypointList[0].transform;
                    }
                    else//If no looping
                    {
                        //Set target to null
                        m_target = null;
                    }
                }
            }

            //find the direction to the waypoint without y axis
            Vector3 directionToWaypoint = (m_target.position - transform.position).normalized;


            if (m_dummyPlatform != null)
            {
                if (directionToWaypoint != Vector3.zero)
                {
                    //lerp the rotation to face the waypoint
                    m_dummyPlatform.rotation = Quaternion.Lerp(m_dummyPlatform.rotation,
                        Quaternion.LookRotation(directionToWaypoint), m_rotationSpeed *
                        CGameManager.PInstanceGameManager.GetScaledDeltaTime());

                }
                //Use Dummy's forward direction and multiply it by speed
                m_surfaceVelocity = m_dummyPlatform.transform.forward * m_movingSpeed;
            }
            else
            {
                //DEBUGLIST - AAA
                //Debug.Log("You forgot to set the dummy in the inspector");
            }

            //Move the platform forward
            transform.position += m_surfaceVelocity * CGameManager.PInstanceGameManager.GetScaledDeltaTime();
        }
    }

    /*
    Description: Drawing the gizmo of the target destination
    Parameters(Optional): 
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes:
    */
    //private void OnDrawGizmos()
    //{
    //    //checking if there's target
    //    if (m_target != null)
    //    {
    //        //DEBUGLIST-AAA
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireCube(m_target.position, transform.localScale);
    //    }
    //}

    /*
    Description: Changing the movement speed
    Parameters(Optional): aNewSpeed : the new speed value
                          aInterpTime : how long will it take
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes:
    */
    public void ChangeMovingSpeed(float aNewSpeed, float aInterpTime)
    {
        m_newMovingSpeed = aNewSpeed;
        m_InterpTime = aInterpTime;
    }

    /*
    Description: Changing the rotation speed
    Parameters(Optional): aNewRotation : the new rotation speed value
                          aInterpTime : how long will it take
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes:
      */
    public void ChangeRotationSpeed(float aNewRotation, float aInterpTime)
    {
        m_newRotationSpeed = aNewRotation;
        m_InterpTime = aInterpTime;
    }

    /*
   Description: Changing the rotation speed and movement speed to default value
   Parameters(Optional): aInterpTime : how long will it take
   Creator: Juan Calvin Raymond
   Creation Date: 10-08-2016
   Extra Notes:
     */
    public void ReturnToDefaultMovement(float aInterpTime)
    {
        m_newMovingSpeed = m_defaultMovingSpeed;
        m_newRotationSpeed = m_defaultRotationSpeed;
        m_InterpTime = aInterpTime;
    }

    /*
    Description: Changing the movement speed to default value
    Parameters(Optional): aInterpTime : how long will it take
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes:
    */
    public void ReturnToDefaultMovingSpeed(float aInterpTime)
    {
        m_newMovingSpeed = m_defaultMovingSpeed;
        m_InterpTime = aInterpTime;
    }

    /*
    Description: Changing the rotation speed to default value
    Parameters(Optional): aInterpTime : how long will it take
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes:
    */
    public void ReturnToDefaultRotation(float aInterpTime)
    {
        m_newRotationSpeed = m_defaultRotationSpeed;
        m_InterpTime = aInterpTime;
    }

    /*
   Description: Setting the list of waypoint that the platform will follow
   Parameters(Optional): aWaypoint : list of waypoint to follow
   Creator: Juan Calvin Raymond
   Creation Date: 10-08-2016
   Extra Notes:
   */
    public void SettingTrack(List<GameObject> aWaypoint)
    {
        //set the list and then set the destination to the first waypoint on the list
        m_waypointList = aWaypoint;
        m_target = m_waypointList[0].transform;
    }
}
