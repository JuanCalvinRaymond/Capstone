using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
Description: Class used to control the objects/meshes used to give the illusion of damage sates
Creator: Alvaro Chavez Mixco
*/
[RequireComponent(typeof(ITarget))]
public class CTargetDamageStates : MonoBehaviour
{
    /*
    Description: Damage State data that contain game object prefab and health threshold
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    [Serializable]
    public struct SDamageStateData
    {
        [SerializeField]
        public GameObject m_gameObjectPrefab;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        public float m_healthPercentageThreshold;
    }

    //Target that has damage states
    private ITarget m_target;

    //Current damage state
    private GameObject m_currentActiveDamageState;

    //List of game object to set active or inactive 
    private List<GameObject> m_listOfGameObjectState;

    //Prefabs  used for creating the different damage states
    [Tooltip("Damage states should be ordered from highest damage to lowest damage." +
        "At start the state with lowest damage, the last one in the list, will be set")]
    public List<SDamageStateData> m_listOfDamageStateData;

    /*
    Description: Instantiate all variable
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void Awake()
    {
        m_listOfGameObjectState = new List<GameObject>();
        m_target = GetComponent<ITarget>();
    }

    /*
    Description: Get ther required components from the parent object, and insantiate
    the objects for the diffferent damage states.
    Creator: Alvaro Chavez Mixco
    */
    private void Start()
    {
        //If there is any damage state to set
        if (m_listOfDamageStateData.Count > 0)
        {
            //Instantiate the game objects
            foreach (SDamageStateData damageState in m_listOfDamageStateData)
            {
                CreateDamageStateObjects(damageState.m_gameObjectPrefab);
            }

            //Set mesh to null since there will be duplicate mesh if it's not null
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            if (meshFilter != null)
            {
                meshFilter.mesh = null;
            }

            //Set current active game object to be the first index in the list and set it to active
            m_currentActiveDamageState = m_listOfGameObjectState[m_listOfGameObjectState.Count - 1];
            m_currentActiveDamageState.SetActive(true);

            //Suscribe to target events
            m_target.OnTargetDamaged += SetDamageState;
            m_target.OnTargetReset += Reset;
        }
        else//If there are no damage states
        {
            //Disable this component
            enabled = false;
        }
    }

    /*
    Description: Unsunscribe from target event
    Creator: Alvaro Chavez Mixco
    */
    private void OnDestroy()
    {
        //Suscribe to target events
        m_target.OnTargetDamaged -= SetDamageState;
        m_target.OnTargetReset -= Reset;
    }

    /*
    Description: Reset all state to original
    Creator: Juan Calvin Raymond
    Creation Date: 14 Mar 2017
    */
    private void Reset()
    {
        m_currentActiveDamageState.SetActive(false);
        m_currentActiveDamageState = m_listOfGameObjectState[m_listOfGameObjectState.Count - 1];
        m_currentActiveDamageState.SetActive(true);
    }

    /*
    Description: Helper function to create, position,rotate and enable all the damage state objects
    accroding to the current state.
    Creator: Alvaro Chavez Mixco
    */
    private void CreateDamageStateObjects(GameObject aDamageStatePrefab)
    {
        //If the damage state prefab is valid
        if (aDamageStatePrefab != null)
        {
            //Create the game object for it
            GameObject tempObject = (GameObject)GameObject.Instantiate(aDamageStatePrefab,
                transform);

            //Make it match the object shape and rotation
            tempObject.transform.position = transform.position;
            tempObject.transform.rotation = transform.rotation;
            tempObject.transform.localScale = transform.localScale;

            //Disable it at start
            tempObject.SetActive(false);

            //Add it to the list of damage states
            m_listOfGameObjectState.Add(tempObject);
        }
    }

    /*
    Description: Set which current damage state game object to active, and set the previous 
                 one to inactive
    Parameters: int aDamagedAmount - Unused in this fucntion
                int aHealthRemaining - Unused in this function
                float aHealthPercent - percent of current health
                int  aScoreValue - How many points is the target worth.             
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    Extra Notes: This function is called by other script
    */
    public void SetDamageState(int aDamagedAmount, int aHealthRemaining, float aHealthPercent, int aScoreValue)
    {
        //Clamp the health percent
        aHealthPercent = Mathf.Clamp01(aHealthPercent);

        //Iterate through every data list
        for (int i = 0; i < m_listOfDamageStateData.Count; i++)
        {
            //If the health if less than threshold
            if (aHealthPercent <= m_listOfDamageStateData[i].m_healthPercentageThreshold)
            {
                //Set current active game object to false
                m_currentActiveDamageState.SetActive(false);

                //Set new state game object to true
                m_listOfGameObjectState[i].SetActive(true);

                //Set current active gameobject to new game object
                m_currentActiveDamageState = m_listOfGameObjectState[i];

                //Break out of loop
                break;
            }
        }
    }
}