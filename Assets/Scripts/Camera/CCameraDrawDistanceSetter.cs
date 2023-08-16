using UnityEngine;
using System.Collections;

/*
Description:Class used to make the camera draw distance match the one stored in the settings storer.
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, January 28, 2017
*/
[RequireComponent(typeof(Camera))]
class CCameraDrawDistanceSetter:MonoBehaviour
{
    private Camera m_camera;

    /*
    Description: At start, get the camera component, suscribe to the draw distance change event
    from the setting storer and set the initial distance for the far clip plane.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void Start()
    {
        //Get the camera component
        m_camera = GetComponent<Camera>();

        //If there is a settings storer
        if(CSettingsStorer.PInstanceSettingsStorer!=null)
        {

            //Suscribe to the event when the draw distance changes
            CSettingsStorer.PInstanceSettingsStorer.OnDrawDistanceChange += ChangeFarClipPlaneDistance;

            //Set the initial far clip plane value
            ChangeFarClipPlaneDistance(CSettingsStorer.PInstanceSettingsStorer.PDrawDistance);
        }
    }

    /*
    Description: Unsuscribe from the setting storer on draw distance change event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void OnDestroy()
    {
        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe from the event when the draw distance changes
            CSettingsStorer.PInstanceSettingsStorer.OnDrawDistanceChange -= ChangeFarClipPlaneDistance;

        }
    }

    /*
    Description: Changes the far clip plane distance from the camera component
    Parameters : float aDistance - The distance to which set the far clip plane.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void ChangeFarClipPlaneDistance(float aDistance)
    {
        //Change the far clip plane distance of the camera
        m_camera.farClipPlane = aDistance;
    }
}
