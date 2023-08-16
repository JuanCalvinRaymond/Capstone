using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: Base class of Menu System, it have basic menu system behaviour
Creator: Alvaro Chavez Mixco
Creation Date: Sunday, February 5th, 2017
*/
public class CMenuSystem : MonoBehaviour, IMenu
{
    private List<CButton> m_buttons;

    [Header("Table of GameObjects")]
    [Tooltip("2D Array of game objects to show. The objects, starting from the last object in the list, are placed by columns from left to right"
       + "Then it repeats the columns, in the next row, from top to bottom. The last object in the list, is the top left object in menu.")]
    public List<GameObject> m_listOfGameObjects = new List<GameObject>();

    [Header("Table Size")]
    [Tooltip("This number is only used to determine object placement and not actually limit the size of the list of gameobjects.")]
    public int m_numberOfColumns = 1;
    [Tooltip("This number is only used to determine object placement and not actually limit the size of the list of gameobjects.")]
    public int m_numberOfRows = 1;

    [Header("Placement Settings")]
    public Vector3 m_totalPlacementOffset = Vector3.zero;
    [Tooltip("How far apart the button will spawn on in front of the player")]
    public float m_distanceFromPlayer = 1.5f;
    [Tooltip("How far horizontally each button will be of each other. Positive, place objects right, negative left.")]
    public float m_horizontalOffset = 0.0f;
    [Tooltip("How far vertically each button will be of each other. Positive, place objects upward, negative downward")]
    public float m_verticalOffset = 0.0f;
    [Tooltip("All the objects will use the same rotation. Which will be based on the rotation" +
        "of the first element")]
    public bool m_lockXRotation = true;
    public bool m_lockYRotation = true;
    public bool m_lockZRotation = true;

    [Header("Activation Conditions")]
    public bool m_placeAtStart = false;
    public bool m_activateElementsAtStart = false;
    public bool m_hideInPlayState = true;

    /*
    Description: If applicable, place and activate/deactivate the menu object and its children
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    protected virtual void Awake()
    {
        //If we want to place the elemtn at start
        if (m_placeAtStart == true)
        {
            //If we want to show the objects at start
            PlaceMenuObjectInFrontOfPlayer();
        }

        //Order all the objects in its list
        OrderButtonList();

        //If the object will be activate at start
        if (m_activateElementsAtStart == true)
        {
            //Activate it
            Activate();
        }
        else//If the object won't be activated at start
        {
            //Deactivate it
            Deactivate();
        }


        //Get the components in all the childrens, including the inactive ones
        m_buttons = new List<CButton>(GetComponentsInChildren<CButton>(true));
    }

    protected virtual void Start()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //Suscribe to its play state
            CGameManager.PInstanceGameManager.OnPlayState += DeactivateInPlayState;
        }

        //Suscribe to the button events
        SuscribeToButtonEvents();
    }

    /*
    Description: Unsuscribe from all the respective events
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    private void OnDestroy()
    {
        //If there is a game manger
        if (CGameManager.PInstanceGameManager != null)
        {
            //Unsuscribe from its onPlayState event
            CGameManager.PInstanceGameManager.OnPlayState -= DeactivateInPlayState;
        }

        //Unsuscribe from the button event
        UnsuscribeToButtonEvents();
    }

    /*
    Description: If applicable, deactivate the object
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    Extra Notes: Function called when the Game Manager event OnPlayState is called
    */
    private void DeactivateInPlayState()
    {
        //If we want to hide the object in play state
        if (m_hideInPlayState == true)
        {
            //Deactivaet it
            Deactivate();
        }
    }

    /*
    Description: Notify all the buttons components in its children, except the one that was just clicked and called this function,
                 that another button has been clicked. This is to prevent 2 or more buttons from being clickable 
                 while waiting for execution.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    Extra Notes: Function called when any of its children button components call its OnClickEvent event.
    */
    public void SetOtherButtonsAsClicked()
    {
        //Go through all the buttons
        foreach (CButton button in m_buttons)
        {
            //If this button wasn't the one that was clicked
            if (button.PIsClicked == false)
            {
                //Tell all the other buttons a button was clicked
                button.POtherButtonClicked = true;
            }
        }
    }

    /*
    Description: Notfy all the buttons that no button is currently being clicked. This allow all the buttons
                 to once again be clickable.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    Extra Notes: Function called when any of its children button components call its OnExecutionEvent event.
    */
    public void SetOtherButtonsAsUnclicked()
    {
        //Go through all the buttons
        foreach (CButton button in m_buttons)
        {
            //Set that none is currently clicked
            button.POtherButtonClicked = false;
        }
    }

    /*
    Description: Get all the button component in the children of this object, and suscribe
                 to its OnClick and OnExecution events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    private void SuscribeToButtonEvents()
    {
        //If there are button as children
        if (m_buttons != null)
        {
            //Go through all the buttons
            foreach (CButton button in m_buttons)
            {
                //Suscribe to the onclick and execution event from all buttons
                button.OnClickEvent += SetOtherButtonsAsClicked;
                button.OnExecutionEvent += SetOtherButtonsAsUnclicked;
            }
        }
    }

    /*
    Description: Get all the button component in the children of this object, and unsuscrbie from its events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    private void UnsuscribeToButtonEvents()
    {
        //If there are button as children
        if (m_buttons != null)
        {
            //Go through all the buttons
            foreach (CButton button in m_buttons)
            {
                //Unsuscribe from the onclick event of all buttons
                button.OnClickEvent -= SetOtherButtonsAsClicked;
                button.OnExecutionEvent -= SetOtherButtonsAsUnclicked;
            }
        }
    }

    /*
    Description: Lock the X/Y/Z rotation of an object. If the rotation is "locked" it will be set to 0.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    private void LockRotation(GameObject aGameObjectTolock)
    {
        //If the gameobject to lock is valid
        if (aGameObjectTolock != null)
        {
            Vector3 currentRotation = aGameObjectTolock.transform.localEulerAngles;

            //If the X rotation will be locked
            if (m_lockXRotation == true)
            {
                //Set the X rotation to 0
                currentRotation = new Vector3(0.0f, currentRotation.y, currentRotation.z);
            }

            //If the Y rotation will be locked
            if (m_lockYRotation == true)
            {
                //Set Y rotation to 0
                currentRotation = new Vector3(currentRotation.x, 0.0f, currentRotation.z);
            }

            //If the Z rotation will be locked
            if (m_lockZRotation == true)
            {
                //Set Z rotation to 0
                currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0.0f);
            }

            //Set the new local rotation of the object
            aGameObjectTolock.transform.localEulerAngles = currentRotation;
        }
    }

    /*
    Description: Order all the "buttons" on the list according to a 2D Grid of rows and columns
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    protected void OrderButtonList()
    {
        //If the list of objects to place are valid
        if (m_listOfGameObjects != null)
        {
            //If the list is not empty
            if (m_listOfGameObjects.Count > 0)
            {
                int totalIndex = 0;
                //Iterate through all the gameobject in the column
                for (int col = 0; col < m_numberOfColumns; col++)
                {
                    //Iterate through all the gameobject in the row
                    for (int row = 0; row < m_numberOfRows; row++)
                    {
                        //Get the total index that the list should be at
                        totalIndex = CUtilityMath.TotalIndex2DArray(col, row, m_numberOfColumns);

                        //If the index is within the range of the list
                        if (totalIndex < m_listOfGameObjects.Count)
                        {
                            //If the object in that index is valid
                            if (m_listOfGameObjects[totalIndex] != null)
                            {
                                //Place the objects in array
                                //Offset all the objects by a same amount
                                Vector3 newPosition = transform.localPosition + m_totalPlacementOffset;

                                //Offset the object according to its row and column
                                newPosition += new Vector3(col * m_horizontalOffset, row * m_verticalOffset, 0);

                                //Set the new postion of the object
                                m_listOfGameObjects[totalIndex].transform.localPosition = newPosition;
                            }
                        }
                    }
                }
            }
        }
    }

    /*
    Description: Set all the GameObject to active or inactive
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    public void ActivateAllObjectsInList(bool aActiveStatus)
    {
        //Go through all the objects
        foreach (GameObject gameObject in m_listOfGameObjects)
        {
            //If the game object is valid
            if (gameObject != null)
            {
                //Disable it
                gameObject.SetActive(aActiveStatus);
            }
        }

    }

    /*
    Description: Set the parent menu object in front of the player. This is done by 
                 placing the menu directly in front of the camera, which works effectively since it 
                 is a first person shooter.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    protected void PlaceMenuObjectInFrontOfPlayer()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the game manager has a camera and a platform
            if (CGameManager.PInstanceGameManager.PMainCamera != null && CGameManager.PInstanceGameManager.PPlayerObject != null)
            {
                //Get the postion of the camera
                Transform cameraObjectTransform = CGameManager.PInstanceGameManager.PMainCamera.gameObject.transform;
                Transform platformObjectTransform = CGameManager.PInstanceGameManager.PPlayerScript.gameObject.transform;

                //Find the point in front of the camera, ignoring the Y transform
                //transform.position = cameraObjectTransform.position+//Start with player position
                //    new Vector3(cameraObjectTransform.forward.x, 0.0f, cameraObjectTransform.forward.z).normalized //Get the transform forward, but ignore the Y value, 
                //                                                                                                    //Normalize it to ensure it has a value of 1
                //    * m_distanceFromPlayer;//Offset the position by the distance it should have of the pplayer

                ////Make the menu look at the camera
                //transform.rotation = Quaternion.LookRotation(cameraObjectTransform.position - transform.position
                //    , platformObjectTransform.transform.up); //Use the platform up direction, since camera may not be facing up




                //Vector3 localCameraForward = transform.InverseTransformDirection(cameraObjectTransform.forward);
                //Vector3 localPlatformForward = transform.InverseTransformDirection(platformObjectTransform.forward);
                //Vector3 platformLocalUp = transform.InverseTransformDirection(platformObjectTransform.up);

                //float angle = cameraObjectTransform.eulerAngles.y * Mathf.Deg2Rad;
                //float xDir = Mathf.Sin(angle);
                //float zDir = Mathf.Cos(angle);
                //Vector3 direction = cameraObjectTransform.forward;

                //Vector3 forward = new Vector3(
                //        xDir,
                //        0.0f,
                //        zDir);



                //forward = forward.normalized;

                ////Find the point in front of the player
                //transform.position = cameraObjectTransform.position +
                //     forward * m_distanceFromPlayer;


                //Vector3 position = cameraObjectTransform.position;
                //Debug.DrawLine(cameraObjectTransform.position, position + direction, Color.yellow, 5000);
                //Debug.DrawLine(cameraObjectTransform.position, position + forward, Color.green, 5000);

                ////Make the menu look at the camera
                //transform.rotation = Quaternion.LookRotation(cameraObjectTransform.position - transform.position
                //    , platformObjectTransform.up); //Use the platform up direction, since camera may not be facing u




                //Vector3 localCameraForward = transform.InverseTransformDirection(cameraObjectTransform.forward);
                //Vector3 localPlatformForward = transform.InverseTransformDirection(platformObjectTransform.forward);
                //Vector3 platformLocalUp = transform.InverseTransformDirection(platformObjectTransform.up);

                //float angle = cameraObjectTransform.eulerAngles.y * Mathf.Deg2Rad;
                //float xDir = Mathf.Sin(angle);
                //float zDir = Mathf.Cos(angle);
                //Vector3 direction = cameraObjectTransform.forward;
                //Vector3 forward = new Vector3(
                //        xDir,
                //        0.0f,
                //        zDir);


                //forward = platformObjectTransform.localRotation * forward; ;

                ////Find the point in front of the player
                //Vector3 desiredPosition = cameraObjectTransform.position +
                //     forward * m_distanceFromPlayer;

                //transform.position = Vector3.Slerp(cameraObjectTransform.position, desiredPosition, 1.0f);

                ////Make the menu look at the camera
                //transform.rotation = Quaternion.LookRotation(cameraObjectTransform.position - transform.position
                //    , platformObjectTransform.up); //Use the platform up direction, since camera may not be facing u




                //Vector3 localCameraForward = transform.InverseTransformDirection(cameraObjectTransform.forward);
                //Vector3 localPlatformForward = transform.InverseTransformDirection(platformObjectTransform.forward);
                //Vector3 platformLocalUp = transform.InverseTransformDirection(platformObjectTransform.up);

                //float angle = cameraObjectTransform.eulerAngles.y * Mathf.Deg2Rad;
                //float xDir = Mathf.Sin(angle);
                //float zDir = Mathf.Cos(angle);
                //Vector3 direction = cameraObjectTransform.forward;

                Quaternion noYRotation = Quaternion.Euler(platformObjectTransform.eulerAngles.x,
                    cameraObjectTransform.eulerAngles.y,
                    platformObjectTransform.eulerAngles.z);

                Vector3 forward = noYRotation * Vector3.forward;

                forward = forward.normalized;

                //Find the point in front of the player
                transform.position = cameraObjectTransform.position +
                     forward * m_distanceFromPlayer;

                //Make the menu look at the camera
                transform.rotation = Quaternion.LookRotation(cameraObjectTransform.position - transform.position
                    , platformObjectTransform.up); //Use the platform up direction, since camera may not be facing u

                //Lock the rotation of hte object
                LockRotation(gameObject);
            }

        }
    }

    /*
    Description: Implementation if IMenu interface, activate all the children of this
                 object. Doesn't change this object active status.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    public virtual void Activate()
    {
        //Set all children as active
        CUtilitySetters.SetActiveStatusAllChildObjects(gameObject, true);
    }

    /*
    Description: Implementation if IMenu interface, set as inactivate all the children of this
                 object. Doesn't change this object active status.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, February 5th, 2017
    */
    public virtual void Deactivate()
    {
        //Set all children as inactive
        CUtilitySetters.SetActiveStatusAllChildObjects(gameObject, false);
    }
}
