using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
/*
Description: Take a list of child empty gameobject and place prefabs on the list's transform
Creator: Juan Calvin Raymond
Creation Date: Wednesday, November 10th, 2016
Note :
*/
public class CExpansionTool : EditorWindow
{
    //Parent gameobject that contain child transform to spawn
    private GameObject m_spawnLocation = null;

    private float m_expansionMultiplication = 1.0f;
    
    //list of transform of the child game object
    private List<GameObject> m_listOfChildGameObject;

    /*
    Description: Show the tab on the editor
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, October 19th, 2016
    Note :
    */
    [MenuItem("Custom Tools/Expansion Tool")]
    static void Init()
    {
        CExpansionTool window = (CExpansionTool)EditorWindow.GetWindow(typeof(CExpansionTool));
        window.Show();
    }

    /*
    Description: Show all the field that user can fill to place the prefabs
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, October 19th, 2016
    Note :
    */
    private void OnGUI()
    {
        //Spawn Location field
        m_spawnLocation = (GameObject)EditorGUILayout.ObjectField("Spawn Location", m_spawnLocation, typeof(GameObject), true);

        m_expansionMultiplication = EditorGUILayout.FloatField("Expansion Factor", m_expansionMultiplication);

        if (m_spawnLocation != null)
        {
            //if Spawn Object button is pressed
            if (GUILayout.Button("Adjust Spacing"))
            {
                //call SpawnObject function
                AdjustSpacing();
            }
        }
    }

    /*
    Description: Multiplies all the objects in the list position by the expansion multiplier, in the X and Z axis
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, October 19th, 2016
    Note :
    */
    private void AdjustSpacing()
    {
        //make sure list of child game object is empty before using it
        if (m_listOfChildGameObject == null)
        {
            m_listOfChildGameObject = new List<GameObject>();
        }
        else
        {
            m_listOfChildGameObject.Clear();
        }

        //Add all the child gameobject to the list
        CUtilityGame.AddChildGameObjectToList(m_spawnLocation, m_listOfChildGameObject);

        //iterate through all the list gameobject
        for (int i = 0; i < m_listOfChildGameObject.Count; i++)
        {
            //check if there;s an object inside the list
            if (m_listOfChildGameObject[i] != null)
            {
                float yPosition = m_listOfChildGameObject[i].transform.position.y;
                Vector3 childPosition = m_listOfChildGameObject[i].transform.position * m_expansionMultiplication;
                childPosition.y = yPosition;
                m_listOfChildGameObject[i].transform.position = childPosition;
            }
        }
    }
    
}
#endif
