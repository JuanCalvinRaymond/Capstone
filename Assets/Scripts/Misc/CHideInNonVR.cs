using UnityEngine;
using System.Collections;

/*
Description: Class used to hide objects when the game is not using VR.
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, March 28th, 2017
*/
public class CHideInNonVR : MonoBehaviour
{
    public GameObject[] m_objectsToHideInNonVR;
    public Behaviour[] m_componentsToHideInNonVR;

    /*
    Description: At awake, if the input method is not vr disable all the desired objects.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2017
    */
    private void Awake()
    {
        //If there is a setting storer
        if(CSettingsStorer.PInstanceSettingsStorer!=null)
        {
            //If the input method is not VR
            if(CSettingsStorer.PInstanceSettingsStorer.PInputMethod != EControllerTypes.ViveController)
            {
                //If there are objects to hide
                if (m_objectsToHideInNonVR != null)
                {
                    //Go through every object
                    foreach (GameObject objectToHide in m_objectsToHideInNonVR)
                    {
                        //Disable the object
                        CUtilitySetters.SetActiveStatus(objectToHide, false);
                    }
                }
                
                //If there are components to hide
                if (m_componentsToHideInNonVR != null)
                {
                    //Go through every component
                    foreach (Behaviour componentToHide in m_componentsToHideInNonVR)
                    {
                        //If the component is valid
                        if (componentToHide != null)
                        {
                            //Disable it
                            componentToHide.enabled = false;
                        }
                    }
                }
            }
        }
    }
}
