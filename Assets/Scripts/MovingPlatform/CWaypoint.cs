using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: For this class it will only search through all the child's transform and add it to the list
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 10-08-2016
Extra Notes:
*/
public class CWaypoint : MonoBehaviour
{
    //Variable to find the waypoint list and the next waypoint to go
    private List<GameObject> m_listOfWaypoint;

    public List<GameObject> PListOfWaypoint
    {
        get
        {
            return m_listOfWaypoint;
        }
    }

    /*
    Description: Initializing list and adding all the child's transform to the list
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 10-08-2016
    Extra Notes: Initializing it before anything else so platform can get the track in Start()
    */
    private void Awake()
    {
        //Instantiate the list of child's transform
        m_listOfWaypoint = new List<GameObject>();
        CUtilityGame.AddChildGameObjectToList(gameObject, m_listOfWaypoint);
    }
}
