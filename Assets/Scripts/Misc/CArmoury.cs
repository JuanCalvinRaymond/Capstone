using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

/*
Description: Class used to enable/disable the weapons in the armoury. This includes creating, and placing
             the weapons in their desired spawn points.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
Extra Notes: This class automatically gets the active weapons at startup, plus any weapon it created.
*/
public class CArmoury : MonoBehaviour
{
    /*
    Description: Struct used to store where an individual weapon object will be placed, alongside its weapon component
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private struct SSingleWeaponSpawn
    {
        public GameObject m_weapon;
        public AWeapon m_weaponComponent;

        public Transform m_spawnPoint;

        /*
        Description: Merely saves the values passed in the constrcutor
        Parameters: GameObject aWeapon - The weapon game object
                    Transform aSpawnPoint - The transform where the weapon will be placed
                    AWeapon aWeaponComponent - The weapon component of the object
        Creator: Alvaro Chavez Mixco
        Creation Date: Friday, February 17th, 2017
        */
        public SSingleWeaponSpawn(GameObject aWeapon, Transform aSpawnPoint, AWeapon aWeaponComponent)
        {
            m_weapon = aWeapon;
            m_spawnPoint = aSpawnPoint;
            m_weaponComponent = aWeaponComponent;
        }
    }

    /*
    Description: Simple struct used to store a weapon prefab, and a list of
                 spawn positions for the weapon.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    [Serializable]
    public struct SWeaponSpawns
    {
        [SerializeField]
        public GameObject m_weaponToSpawn;

        [SerializeField]
        public Transform[] m_spawnPoints;
    }

    /*
    Description: Struct used to store a weapon type, and the corresponding
                 spawn point for that  weapon type.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    [Serializable]
    public struct SWeaponTypeSpawnPoint
    {
        [SerializeField]
        public EWeaponTypes m_weaponType;

        [SerializeField]
        public Transform m_spawnPoint;
    }

    //Lists of the weapons that will be hidden/shown    
    private List<SSingleWeaponSpawn> m_createdWeapons;
    private List<AWeapon> m_weaponsNotCreatedByArmory;

    [Tooltip("The weapons that will be spawned.")]
    public List<SWeaponSpawns> m_listWeaponsToSpawn;

    [Space(20)]
    [Tooltip("Whether the weapons should be spawned at start or not")]
    public bool m_initialActiveStatus = true;
    [Tooltip("Will the armory  hide/show weapons that were not created by the armory. Such as the starting weapons")]
    public bool m_affectWeaponsInScene = true;
    [Tooltip("Where weapons that where not created by the armory (e.g: starting weapons) will be placed if they are dropped in the armory" +
        ", and the player activates the armory again.")]
    public List<SWeaponTypeSpawnPoint> m_spawnPointWeaponsNotInArmory;


    /*
    Description: If applicable create the weapons in the scene, and then create the desired weapons at 
                 the desired position
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void Start()
    {
        //Get the weapons in the scene
        GetWeaponsInScene();

        //Create all the weapon objects
        SpawnWeapons(m_initialActiveStatus);
    }

    /*
    Description: If applicable, get all the currently active weapons in the scene. This is done by finding the gameobjects
                 according to the weapon tag, and then saving their weapon component into a list.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void GetWeaponsInScene()
    {
        //If it wants to affect the weapons in the scene not created by the armory
        if (m_affectWeaponsInScene == true)
        {
            //Get active weapon objects in scene by their tag
            GameObject[] weaponObjectsInScene = GameObject.FindGameObjectsWithTag(CGlobalTags.M_TAG_WEAPON);
            m_weaponsNotCreatedByArmory = new List<AWeapon>();

            AWeapon tempWeaponComponent = null;

            //Go through all objects tagged as weapon in the scene
            for (int i = 0; i < weaponObjectsInScene.Length; i++)
            {
                //Get the AWeapon component from the object
                tempWeaponComponent = weaponObjectsInScene[i].GetComponent<AWeapon>();

                //If the object has a AWeapon component
                if (tempWeaponComponent != null)
                {
                    //Add the weapon component to the list
                    m_weaponsNotCreatedByArmory.Add(tempWeaponComponent);
                }
            }
        }
    }


    /*
    Description: Go through the list of weapon spawns, and create the weapon
                 prefab at all the desired locations
    Parameters:  bool aInitialActiveStatus - A status should the objects be active or not when created.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void SpawnWeapons(bool aInitialActiveStatus = false)
    {

        //If the list of weapons to spawn is valid
        if (m_listWeaponsToSpawn != null)
        {
            m_createdWeapons = new List<SSingleWeaponSpawn>();

            GameObject tempWeapon = null;

            ///Go through each weapon to spawn
            for (int i = 0; i < m_listWeaponsToSpawn.Count; i++)
            {
                //If the weapon object we will spawn is valid
                if (m_listWeaponsToSpawn[i].m_weaponToSpawn != null)
                {
                    //Go through all the transforms where the weapon will be set
                    for (int j = 0; j < m_listWeaponsToSpawn[i].m_spawnPoints.Length; j++)
                    {
                        //Create the weapon object
                        tempWeapon = (GameObject)Instantiate(m_listWeaponsToSpawn[i].m_weaponToSpawn,
                            m_listWeaponsToSpawn[i].m_spawnPoints[j].position,
                            m_listWeaponsToSpawn[i].m_spawnPoints[j].rotation);

                        //Ensure it has no parent
                        tempWeapon.transform.parent = null;

                        //Set the active status of the weapon
                        tempWeapon.SetActive(aInitialActiveStatus);

                        //Add the weapons created to the list of creatd weapons
                        m_createdWeapons.Add(new SSingleWeaponSpawn(tempWeapon,
                            m_listWeaponsToSpawn[i].m_spawnPoints[j],
                            tempWeapon.GetComponent<AWeapon>()));
                    }
                }
            }
        }
    }

    /*
    Description: Function to place a single weapon object at the desired position. This function also ensures
                 that only dropped weapons are placed.
    Parameters:  GameObject aWeaponToPlace - The weapon game object that will be moded
                Transform aPositionToPlace - The postion where the object will be moved
                AWeapon aWeaponComponent - The weapon component of the object, this is used to know if the weapon is dropped or not
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void PlaceSingleWeapon(GameObject aWeaponToPlace, Transform aPositionToPlace, AWeapon aWeaponComponent)
    {
        //If the weapon object is valid
        if (aWeaponToPlace != null)
        {
            //If there is a valid weapon component
            if (aWeaponComponent != null)
            {
                //If the weapon is not dropped (is currently being hold or is traveling back to player)
                if (aWeaponComponent.PWeaponPhysiscsState != EWeaponPhysicsState.Dropped)
                {
                    //Exit function without placing the object
                    return;
                }
            }

            //Enable the object
            aWeaponToPlace.SetActive(true);

            //Set it at the desired positon
            CUtilitySetters.SetTransform(aWeaponToPlace.transform, aPositionToPlace);

            //Ensure it has no parent
            aWeaponToPlace.transform.parent = null;
        }
    }

    /*
    Description: Function to place a single weapon object at the desired position. This function also ensures
                 that only dropped weapons are placed.
    Parameters:  GameObject aWeaponToPlace - The weapon game object that will be moded
                Transform aPositionToPlace - The postion where the object will be moved
                AWeapon aWeaponComponent - The weapon component of the object, this is used to know if the weapon is dropped or not
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public void PlaceWeapons()
    {
        //If the list of create weapons is valid
        if (m_createdWeapons != null)
        {
            //Go through every weapon
            for (int i = 0; i < m_createdWeapons.Count; i++)
            {
                //Place each weapon in the corresponding position
                PlaceSingleWeapon(m_createdWeapons[i].m_weapon, m_createdWeapons[i].m_spawnPoint,
                    m_createdWeapons[i].m_weaponComponent);
            }
        }

        //If m_weaponsNotCreatedByArmory is null, it means that either there are no weapons, or m_affectWeaponsInScene is false
        //and no weapons were obtained
        if (m_weaponsNotCreatedByArmory != null && m_spawnPointWeaponsNotInArmory != null)
        {
            //Go through every weapon that was not created in the armory
            foreach (AWeapon weapon in m_weaponsNotCreatedByArmory)
            {
                //If the weapon is valid
                if (weapon != null)
                {

                    //Go through all the spawn points for weapons
                    for (int i = 0; i < m_spawnPointWeaponsNotInArmory.Count; i++)
                    {
                        //If the current weapon type matches the spawn point weapon type
                        if (weapon.PWeaponType == m_spawnPointWeaponsNotInArmory[i].m_weaponType)
                        {
                            //Place the weapon in the corresponding type spawn point
                            PlaceSingleWeapon(weapon.gameObject, m_spawnPointWeaponsNotInArmory[i].m_spawnPoint,
                                weapon);
                            
                            //Exit the loop to stop comparing weapon types
                            break;
                        }
                    }
                }
            }
        }
    }

    /*
    Description: Deactivate all the currently dropped weapons, the created ones by the armory, 
                 and if applicable, the ones that were already in the scene,.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public void HideWeapons()
    {
        //If the armory has create a weapon
        if (m_createdWeapons != null)
        {
            //Go through every weapon created
            for (int i = 0; i < m_createdWeapons.Count; i++)
            {
                //If the weapon is valid
                if (m_createdWeapons[i].m_weapon != null)
                {
                    //If the weapon component of the weapon is valid
                    if (m_createdWeapons[i].m_weaponComponent != null)
                    {
                        //If the weapon is currently dropped
                        if (m_createdWeapons[i].m_weaponComponent.PWeaponPhysiscsState == EWeaponPhysicsState.Dropped)
                        {
                            //Disable the weapon
                            m_createdWeapons[i].m_weapon.SetActive(false);
                        }
                    }
                    else//If the object has no weapon component
                    {
                        //Just disable it
                        m_createdWeapons[i].m_weapon.SetActive(false);
                    }
                }
            }
        }

        //If this is null, it means that either there are no weapons, or m_affectWeaponsInScene is false
        if (m_weaponsNotCreatedByArmory != null && m_spawnPointWeaponsNotInArmory != null)
        {
            //Go through every weapon in the scene not created by the armory
            foreach (AWeapon weapon in m_weaponsNotCreatedByArmory)
            {
                //If the weapon is valid
                if (weapon != null)
                {
                    //If the weapon is currently droped
                    if (weapon.PWeaponPhysiscsState == EWeaponPhysicsState.Dropped)
                    {
                        //Disable it
                        weapon.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
