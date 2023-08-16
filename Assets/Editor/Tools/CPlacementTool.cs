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
public class CPlacementTool : EditorWindow
{
    /*
    Description: Enum of collider type that we want to add to the object
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, November 10th, 2016
    Note :
    */
    public enum EColliderType
    {
        None = 0,
        Box,
        Sphere,
        Mesh,
        Capsule
    }
    private EColliderType m_colliderType;

    //List of Fbx file in the folder
    private GameObject[] m_listOfGameObjectOnFolder;
    
    //Parent gameobject that contain child transform to spawn
    private GameObject m_spawnLocation = null;

    //Prefab to spawn
    private GameObject m_prefab = null;

    //Material to set on the object
    private Material m_material = null;
    
    //Bool if you want to keep connection to the prefab
    private bool m_connectionToPrefab = true;

    //Bool if you want to destroy the previous game object
    private bool m_destroyEmptyGameObject = false;

    //tag to add to the object
    private string m_tag = null;

    //Tag to find path to load all gameobject
    private string m_folderToLoad = null;

    //list of transform of the child game object
    private List<GameObject> m_listOfChildGameObject;

    /*
    Description: Show the tab on the editor
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, October 19th, 2016
    Note :
    */
    [MenuItem("Custom Tools/Level Editor")]
    static void Init()
    {
        CPlacementTool window = (CPlacementTool)EditorWindow.GetWindow(typeof(CPlacementTool));
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

        //Prefab field
        m_prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", m_prefab, typeof(GameObject), true);

        //Material field
        m_material = (Material)EditorGUILayout.ObjectField("Material", m_material, typeof(Material), true);

        //Collider field
        m_colliderType = (EColliderType)EditorGUILayout.EnumPopup("Collider", m_colliderType);

        //Tag field
        m_tag = EditorGUILayout.TextField("Tag", m_tag);

        //Tag field
        m_folderToLoad = EditorGUILayout.TextField("Folder to load for randomize", m_folderToLoad);

        //Bool field
        m_connectionToPrefab = EditorGUILayout.Toggle("Maintain prefab connection", m_connectionToPrefab);
        m_destroyEmptyGameObject = EditorGUILayout.Toggle("Destroy empty gameobject", m_destroyEmptyGameObject);

        if(m_prefab != null)
        {
            //if Spawn Object button is pressed
            if (GUILayout.Button("Spawn Object"))
            {
                //call SpawnObject function
                SpawnObject();
            }
        }
        else
        {
            if(GUILayout.Button("Modify Object"))
            {
                ModifyObject();
            }
            //if Random button is pressed
            if (GUILayout.Button("Random"))
            {
                //call Randomize function
                Randomize();
            }
        }
    }

    /*
    Description: iterate through all the list of empty gameobject and instantiate the prefab
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, October 19th, 2016
    Note :
    */
    private void SpawnObject()
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
                //Call instantiatingObject function
                InstantiatingObject(m_listOfChildGameObject[i], m_prefab);
            }
        }
    }

    private void ModifyObject()
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
                SwitchingProperties(m_listOfChildGameObject[i]);
            }
        }
    }

    /*
    Description: Load all the mesh/prefab from the folder and randomly put them on the empty gameobject position
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, October 19th, 2016
    Note :
    */
    private void Randomize()
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

        //Load all asset on the folder
        if(m_folderToLoad != null && m_tag != "")
        {
            m_listOfGameObjectOnFolder = Resources.LoadAll<GameObject>("3D_2D_Assets/" + m_folderToLoad);
        }

        //Add all the child gameobject to the list
        CUtilityGame.AddChildGameObjectToList(m_spawnLocation, m_listOfChildGameObject);

        //iterate through all the empty gameobject
        for(int i = 0; i < m_listOfChildGameObject.Count; i++)
        {
            //get a random fbx in the folder
            int randomIndex = Random.Range(0, m_listOfGameObjectOnFolder.Length);

            //call instantiatingObject function
            InstantiatingObject(m_listOfChildGameObject[i], m_listOfGameObjectOnFolder[randomIndex]);

        }
    }

    /*
    Description: Instantiate prefab on empty gameobject position
    Parameters(Optional): aTransformToSpawn : empty gameobject transform
                          aGameObjectPrefab : Gameobject to spawn
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, November 11th, 2016
    Note :
    */
    private void InstantiatingObject(GameObject aTransformToSpawn, GameObject aGameObjectPrefab)
    {

        //if prefab field is not empty
        if (aGameObjectPrefab != null && aTransformToSpawn != null)
        {
            //set the prefab position, rotation, and local scale to be the same as respective emty gameobject's
            aGameObjectPrefab.transform.position = aTransformToSpawn.transform.position;
            aGameObjectPrefab.transform.rotation = aTransformToSpawn.transform.rotation;
            aGameObjectPrefab.transform.localScale = aTransformToSpawn.transform.localScale;

            //if user want to keep the connection to the prefab
            if (m_connectionToPrefab)
            {
                SwitchingProperties(aGameObjectPrefab);

                //instantiate the prefab and maintain the connection
                PrefabUtility.InstantiatePrefab(aGameObjectPrefab);
            }

            //if user don't want to keep the connection to the prefab
            else
            {
                //create a clone
                GameObject clone = (GameObject)Instantiate(aGameObjectPrefab, aTransformToSpawn.transform.parent);

                //delete "(clone)" on the name because it's annoying
                clone.name = clone.name.Replace("(Clone)", "");

                SwitchingProperties(clone);
            }

            //if player want to destroy the previous game object
            if (m_destroyEmptyGameObject)
            {
                DestroyImmediate(aTransformToSpawn);
            }
            
        }
    }

    private void SwitchingProperties(GameObject aGameObject)
    {
        switch (m_colliderType)
        {
            case EColliderType.None:
                DestroyAllCollider(aGameObject);
                break;
            case EColliderType.Box:
                DestroyAllCollider(aGameObject);
                aGameObject.AddComponent<BoxCollider>();
                break;
            case EColliderType.Sphere:
                DestroyAllCollider(aGameObject);
                aGameObject.AddComponent<SphereCollider>();
                break;
            case EColliderType.Mesh:
                DestroyAllCollider(aGameObject);
                aGameObject.AddComponent<MeshCollider>();
                break;
            case EColliderType.Capsule:
                DestroyAllCollider(aGameObject);
                aGameObject.AddComponent<CapsuleCollider>();
                break;
            default:
                break;
        }

        //if material field is not empty
        if (m_material != null)
        {
            if (aGameObject.GetComponent<MeshRenderer>() != null)
            {
                aGameObject.GetComponent<MeshRenderer>().sharedMaterial = m_material;
            }
            else
            {
                aGameObject.AddComponent<MeshRenderer>();
                aGameObject.GetComponent<MeshRenderer>().sharedMaterial = m_material;
            }
        }

        //if tag field is not empty
        if (m_tag != null && m_tag != "")
        {
            aGameObject.tag = m_tag;
        }
    }


    private void DestroyAllCollider(GameObject aGameObject)
    {
        Collider[] listOfCollider;
        listOfCollider = aGameObject.GetComponents<Collider>();
        if (listOfCollider != null)
        {
            foreach (Collider collider in listOfCollider)
            {
                if (collider != null)
                {
                    DestroyImmediate(collider, true);
                }
            }
        }
    } 
}
#endif

