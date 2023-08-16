using UnityEngine;
using System.Collections;
using System;

/*
Description: Class that inherits from AButtonFunctionality, this class is used to show a game object,
             while hiding another.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, Novemeber 23, 2016
*/
public class CSubmenuButton : AButtonFunctionality
{
    [Header("Submenu Button Settings")]
    //Gameobjects that will be shown/hidden
    public GameObject m_objectToHide;
    public GameObject m_objectToShow;

    //Variables to move the object we are going to show to the position of the one we are hiding
    public bool m_showAtHiddenObjectLocation = true;
    public Vector3 m_offsetShowAtHiddenObjectLocation = new Vector3();
    public bool m_useHiddenObjectRotation = true;
    public Vector3 m_offsetRotationHiddenObject = new Vector3();

    /*
    Description: Custom function to activate an object
    Parameters: GameObject aObjectToActivate - The object that will be activated or shown.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void ActivateObject(GameObject aObjectToActivate)
    {
        //If the object to activate is valid
        if (aObjectToActivate != null)
        {
            //Check if the object has the IMenu interface
            IMenu menuObject = (IMenu)aObjectToActivate.GetComponent(typeof(IMenu));

            //If it has the interface
            if (menuObject != null)
            {
                //Call the interface activate function
                menuObject.Activate();
            }
            else//If the object doesn't have the interface
            {
                //Set its active status to true
                aObjectToActivate.SetActive(true);
            }
        }
    }

    /*
    Description: Custom function to deactivate an object
    Parameters: GameObject aObjectToDeactivate - The object that will be deactivated or hidden.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void DeactivateObject(GameObject aObjectToDeactivate)
    {
        //If the object to deactivate is valid
        if (aObjectToDeactivate != null)
        {
            //Check if the object has the IMenu interface
            IMenu menuObject = aObjectToDeactivate.GetComponent<IMenu>();

            //If it has the interface
            if (menuObject != null)
            {
                //Call the interface deactivate function
                menuObject.Deactivate();
            }
            else//If the object doesn't have the interface
            {
                //Set its active status to false
                aObjectToDeactivate.SetActive(false);
            }
        }
    }

    /*
    Description: Move the object that will be shown to the position of the object that is being hidden.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void MoveActiveObjectToHiddenObjectPosition()
    {
        //If we want to move the object we are showing to where the previous one was placed
        if (m_showAtHiddenObjectLocation == true && m_objectToHide != null && m_objectToShow != null)
        {
            //Make the transform of the objects match
            m_objectToShow.transform.position = m_objectToHide.transform.position;

            //Offset its local position
            m_objectToShow.transform.localPosition += m_offsetShowAtHiddenObjectLocation;
        }
    }

    /*
    Description: Rotate the object that will be shown to match the rotation of the object that is being hidden.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void MatchHiddenObjectRotation()
    {
        //If it will used the hidden object rotation and both the object hidden and object shown are valid
        if (m_useHiddenObjectRotation == true && m_objectToShow != null && m_objectToHide != null)
        {
            //Match both rotation
            m_objectToShow.transform.localRotation = m_objectToHide.transform.localRotation;

            //Offset the rotation
            m_objectToShow.transform.localEulerAngles = m_objectToShow.transform.localEulerAngles + m_offsetRotationHiddenObject;
        }
    }

    /*
    Description: Override of CButtonFunctionality OnExecution. This disables, hides, a gameobject, and at the same time enables, show another one.
                 When showing the object it can be placed at the position of the disabled object.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, Novemeber 23, 2016
    */
    public override void OnButtonExecution()
    {
        //Hide the object
        DeactivateObject(m_objectToHide);

        //Show the object
        ActivateObject(m_objectToShow);

        //If applicable, move the object being shown
        MoveActiveObjectToHiddenObjectPosition();

        //If applicable, rotate the object being shown
        MatchHiddenObjectRotation();
    }

}
