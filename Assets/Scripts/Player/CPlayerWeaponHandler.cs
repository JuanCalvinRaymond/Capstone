using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: This class is responsible for handling all the weapons of the player. This includes storing(the variables),
grabbing, dropping, and checking the weapons if they are firing or receiving any other type of input from the player controller.
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, December 22th, 2016
*/
[RequireComponent(typeof(CPlayer))]
public class CPlayerWeaponHandler : MonoBehaviour
{
    private IController m_playerController;
    private AWeapon m_startignRightWeaponComponent;
    private AWeapon m_startingLeftWeaponComponent;

    //Current weapon equipped
    private GameObject m_currentRightWeaponGameObject;
    private GameObject m_currentLeftWeaponGameObject;

    private AWeapon m_currentRightWeaponScript;
    private AWeapon m_currentLeftWeaponScript;

    private CWeaponDataTracker m_currentRightWeaponDataTracker;
    private CWeaponDataTracker m_currentLeftWeaponDataTracker;
    
    private CWeaponPhysics m_currentRightWeaponPhysics;
    private CWeaponPhysics m_currentLeftWeaponPhysics;

    [Header("Player Hands")]
    [Tooltip("Where the weapons will be placed.")]
    public CPlayerHand m_rightHand;
    [Tooltip("Where the weapons will be placed.")]
    public CPlayerHand m_leftHand;

    [Header("Starting Weapons")]
    [Tooltip("Weapon to create at start, if the PlayerCreator doesn't override them.")]
    public GameObject m_startingRightWeapon;
    [Tooltip("Weapon to create at start, if the PlayerCreator doesn't override them.")]
    public GameObject m_startingLeftWeapon;

    public CPlayerHand PRightHand
    {
        get
        {
            return m_rightHand;
        }
    }

    public CPlayerHand PLeftHand
    {
        get
        {
            return m_leftHand;
        }
    }

    public EWeaponTypes PStartingRightWeaponType
    {
        get
        {
            if (m_startignRightWeaponComponent != null)
            {
                return m_startignRightWeaponComponent.PWeaponType;
            }

            return EWeaponTypes.None;
        }
    }

    public EWeaponTypes PStartingLefttWeaponType
    {
        get
        {
            if (m_startingLeftWeaponComponent != null)
            {
                return m_startingLeftWeaponComponent.PWeaponType;
            }

            return EWeaponTypes.None;
        }
    }

    public GameObject PCurrentRightWeapon
    {
        get
        {
            return m_currentRightWeaponGameObject;
        }
    }

    public GameObject PCurrentLeftWeapon
    {
        get
        {
            return m_currentLeftWeaponGameObject;
        }
    }

    public AWeapon PCurrentLeftWeaponScript
    {
        get
        {
            return m_currentLeftWeaponScript;
        }
    }

    public AWeapon PCurrentRightWeaponScript
    {
        get
        {
            return m_currentRightWeaponScript;
        }
    }


    public List<SWeaponData> PListOfRightWeaponData
    {
        get
        {
            if (m_currentRightWeaponDataTracker == null)
            {
                m_currentRightWeaponDataTracker = m_currentRightWeaponScript.GetComponent<CWeaponDataTracker>();
            }
            return m_currentRightWeaponDataTracker.PListOfWeaponData;
        }

    }
    public List<SWeaponData> PListOfLeftWeaponData
    {
        get
        {
            if (m_currentLeftWeaponDataTracker == null)
            {
                m_currentLeftWeaponDataTracker = m_currentLeftWeaponScript.GetComponent<CWeaponDataTracker>();
            }
            return m_currentLeftWeaponDataTracker.PListOfWeaponData;
        }
    }

    public EWeaponTypes PRightWeaponType
    {
        get
        {
            if (m_currentRightWeaponScript != null)
            {
                return m_currentRightWeaponScript.PWeaponType;
            }

            return EWeaponTypes.None;
        }
    }

    public EWeaponTypes PLeftweaponType
    {
        get
        {
            if (m_currentLeftWeaponScript != null)
            {
                return m_currentLeftWeaponScript.PWeaponType;
            }

            return EWeaponTypes.None;
        }
    }


    /*
    Description:Create the initial weapons for the player.
    that will be used by the player.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void Start()
    {
        //Get the player controller
        m_playerController = GetComponent<CPlayer>().PController;

        //Get and save the weapon scripts
        CreateWeapons();

        //If there is a right hand
        if (m_rightHand != null)
        {
            //Suscribe to weapon change event
            m_rightHand.OnWeaponHeldChange += UpdateRightWeaponComponents;

            //Get the hand weapon scripts
            m_rightHand.GetCurrentWeaponComponents(ref m_currentRightWeaponGameObject, ref m_currentRightWeaponScript, ref m_currentRightWeaponPhysics, ref m_currentRightWeaponDataTracker);
        }

        //If there is a left hand
        if (m_leftHand != null)
        {
            //Suscribe to weapon change event
            m_leftHand.OnWeaponHeldChange += UpdateLeftWeaponComponents;

            //Get the hand weapon scripts
            m_leftHand.GetCurrentWeaponComponents(ref m_currentLeftWeaponGameObject, ref m_currentLeftWeaponScript, ref m_currentLeftWeaponPhysics, ref m_currentLeftWeaponDataTracker);
        }
    }

    /*
    Description: Unsuscribe from events
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 21st, 2016
    */
    private void OnDestroy()
    {
        //Unsuscribe from right hand events
        if (m_rightHand != null)
        {
            m_rightHand.OnWeaponHeldChange -= UpdateRightWeaponComponents;
        }

        //Unsuscribe from left hand events
        if (m_leftHand != null)
        {
            m_leftHand.OnWeaponHeldChange -= UpdateLeftWeaponComponents;
        }
    }

    /*
    Description:Check if the player is firing any of his weapons.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, December 22, 2016
    */
    private void Update()
    {
        UpdateFiring();//Check if the player is firing or reloading       
    }

    /*
    Description:According to the weapons set in the inspector, instantiate the weapons prefabs and
    set all the weapon conditions to be used
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    Extra Notes: 
    */
    private void CreateWeapons()
    {
        GameObject weaponObject = null;

        //If the starting right weapon prefab is valid
        if (m_startingRightWeapon != null && m_rightHand != null)
        {
            //Create the gun
            weaponObject = (GameObject)Instantiate(m_startingRightWeapon);

            //Save the starting weapon component
            m_startignRightWeaponComponent = weaponObject.GetComponent<AWeapon>();

            //Place the weapon on the player hand
            PlaceWeaponOnHand(m_rightHand.transform, ref weaponObject);

            //Set the gun properties
            m_rightHand.GrabWeapon(weaponObject);
        }

        //If the starting left weapon prefab is valid
        if (m_startingLeftWeapon != null && m_leftHand != null)
        {
            //Create the gun
            weaponObject = (GameObject)Instantiate(m_startingLeftWeapon);

            //Save the starting weapon component
            m_startingLeftWeaponComponent = weaponObject.GetComponent<AWeapon>();

            //Place the weapon on the player hand
            PlaceWeaponOnHand(m_leftHand.transform, ref weaponObject);

            //Set the gun properties
            m_leftHand.GrabWeapon(weaponObject);
        }
    }

    /*
    Description: Fucntion to instantly place a weapon and rotate it according to a player's hand
    Parameters(Optional): Transform aHand- The hand where the weapon will be placed
                          ref GameObject aWeaponGameObject - The weapon gameobject that will be moved and rotated
    Creator: Alvaro Chavez Mixco
    Extra Notes: 
    */
    private void PlaceWeaponOnHand(Transform aHand, ref GameObject aWeaponGameObject)
    {
        //If the hand and the weapons are valid
        if (aHand != null && aWeaponGameObject != null)
        {
            aWeaponGameObject.transform.position = aHand.transform.position;//Set the guns position
            aWeaponGameObject.transform.rotation = aHand.transform.rotation;//Set the initial rotation to match the parent
        }
    }

    /*
    Description:Function to check if player is firing any of his weapons.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void UpdateFiring()
    {
        if (m_playerController != null)
        {
            CheckFiringInput(m_playerController.PRightWeaponControl, m_currentRightWeaponScript);//Right Weapon
            CheckFiringInput(m_playerController.PLeftWeaponControl, m_currentLeftWeaponScript);//LeFt Weapon
        }
    }

    /*
    Description:Function to check if the player is shooting or reloading one of his weapons
    Parameters: CWeaponControlInput aWeaponControls - The controls used for the weapon in the hand being passed.
                CWeapon aWeaponScript - The script that will actually  execute the shooting and reloading.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void CheckFiringInput(CWeaponControlInput aWeaponControls, AWeapon aWeaponScript)
    {
        //Make sure the  objects are not null
        if (aWeaponControls != null && aWeaponScript != null)
        {
            //If the player presses fire key
            if (aWeaponControls.GetIsFiringWeapon(aWeaponScript.m_automaticWeapon) == true)
            {
                //Fire Weapon
                aWeaponScript.Fire();

                if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)//if the player is playing the game normally
                {
                    //Add shot fired by 1
                    if (CGameManager.PInstanceGameManager.PScoringSystem != null)
                    {
                        CGameManager.PInstanceGameManager.PScoringSystem.PShotFired += 1;
                    }
                }
            }

            //If the gun have no ammo or player hit reload button and weapon ammo is more than 0
            if (aWeaponScript.PCurrentAmmo == 0 || (aWeaponControls.GetIsReloading() == true && aWeaponScript.PCurrentAmmo > 0))
            {
                //Reload weapon
                aWeaponScript.Reload();
            }
        }
    }



    /*
    Description: Helper function to set the weapon compoents for the right weapon.
    Parameters: GameObject aWeaponGameObjectHolder - The gameobject of the hand
                AWeapon aWeaponScriptHolder - The script of the weapon
                CWeaponPhysics aWeaponPhysicsHolder - The physics script of the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 241th, 2016
    */
    private void UpdateRightWeaponComponents(GameObject aWeaponGameObject, AWeapon aWeaponScript, CWeaponPhysics aWeaponPhysics, CWeaponDataTracker aWeaponDataTracker)
    {
        UpdateWeaponComponents(aWeaponGameObject, aWeaponScript, aWeaponPhysics, aWeaponDataTracker,
            ref m_currentRightWeaponGameObject, ref m_currentRightWeaponScript, ref m_currentRightWeaponPhysics, ref m_currentRightWeaponDataTracker);
    }

    /*
    Description: Helper function to set the weapon compoents for the left weapon.
    Parameters: GameObject aWeaponGameObjectHolder - The gameobject of the hand
                AWeapon aWeaponScriptHolder - The script of the weapon
                CWeaponPhysics aWeaponPhysicsHolder - The physics script of the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 241th, 2016
    */
    private void UpdateLeftWeaponComponents(GameObject aWeaponGameObject, AWeapon aWeaponScript, CWeaponPhysics aWeaponPhysics, CWeaponDataTracker aWeaponDataTracker)
    {
        UpdateWeaponComponents(aWeaponGameObject, aWeaponScript, aWeaponPhysics, aWeaponDataTracker,
            ref m_currentLeftWeaponGameObject, ref m_currentLeftWeaponScript, ref m_currentLeftWeaponPhysics, ref m_currentLeftWeaponDataTracker);
    }


    /*
    Description: Helper function to set the set the desired weapons compoents in the desired variables.
    Parameters: GameObject aWeaponGameObjectHolder - The gameobject of the hand
                AWeapon aWeaponScriptHolder - The script of the weapon
                CWeaponPhysics aWeaponPhysicsHolder - The physics script of the weapon
                ref GameObject aWeaponGameObjectHolder - The variable whre the game object of the weapon will be stored
                ref AWeapon aWeaponScriptHolder - The variable where the weapon script will be stored
                ref CWeaponPhysics aWeaponPhysicsHolder - The variable where the weapon physics will be stored
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 241th, 2016
    */
    private void UpdateWeaponComponents(GameObject aWeaponGameObject, AWeapon aWeaponScript, CWeaponPhysics aWeaponPhysics, CWeaponDataTracker aWeaponDataTracker,
       ref GameObject aWeaponGameObjectHolder, ref AWeapon aWeaponScriptHolder, ref CWeaponPhysics aWeaponPhysicsHolder, ref CWeaponDataTracker aWeaponDataTrackerHolder)
    {
        aWeaponGameObjectHolder = aWeaponGameObject;
        aWeaponScriptHolder = aWeaponScript;
        aWeaponPhysicsHolder = aWeaponPhysics;
        aWeaponDataTrackerHolder = aWeaponDataTracker;
    }

    /*
    Description: Function to check if the player is already holding a weapon in a certain arm.
    Parameters: EWeaponHand aHandToCheck - Which hand (left or right) should be checked
    Creator: Alvaro Chavez Mixco
    */
    public bool CheckIfWeaponHeld(EWeaponHand aHandToCheck)
    {
        //Accordign to which hand we want to check
        switch (aHandToCheck)
        {
            case EWeaponHand.RightHand:
                //If there is a weapon being held in the right hand
                return m_currentRightWeaponGameObject != null;
            case EWeaponHand.LeftHand:
                //If there is a weapon being held in the left hand
                return m_currentLeftWeaponGameObject != null;
            case EWeaponHand.BothHands:
                //If any hand is holding a weapon
                return m_currentRightWeaponGameObject != null || m_currentLeftWeaponGameObject != null;
            default:
                break;
        }

        return false;
    }

    /*
    Description: Function to check if the player is pressing the grab button for his left or right controller
    Parameters: EWeaponHand aHandToCheck - Which hand (left or right) should be checked
    Creator: Alvaro Chavez Mixco
    */
    public bool GetHandGrabbingWeapon(EWeaponHand aHandToCheck)
    {
        //If there is a controller
        if (m_playerController != null)
        {
            //Accordign to which hand we want to check
            switch (aHandToCheck)
            {
                case EWeaponHand.RightHand:
                    return m_playerController.PRightWeaponControl.GetIsGrabbing();
                case EWeaponHand.LeftHand:
                    return m_playerController.PLeftWeaponControl.GetIsGrabbing();
                case EWeaponHand.BothHands:
                    return GetHandGrabbingWeapon(EWeaponHand.RightHand) && GetHandGrabbingWeapon(EWeaponHand.LeftHand);
                default:
                    break;
            }
        }

        return false;
    }
}
