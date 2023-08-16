using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

/*
Description: Class to set the conditions for the canvas to use. Since the player, and camera are created at runtime.
Creator: Alvaro Chavez Mixco
Creation Date: Monday, October 31st, 2016
*/
[RequireComponent(typeof(Canvas))]
public class CCanvasCameraSetter : MonoBehaviour
{
    private Canvas m_canvas;

    public float m_planeDistance;

    public float PPlaneDistance
    {
        set
        {
            m_planeDistance = value;
        }
    }

    /*
    Description: Set the conditions for the canvas to use. Since the player, and camera are created at runtime. This is done when the canvas is activated.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    */
    private void Start()
    {
        //Get the canvas componnent
        m_canvas = GetComponent<Canvas>();

        //Set the camera the canvas will used
        SetCanvasCamera();

        //Suscribe to level load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*
    Description: Unsuscribe to level load event
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, October 31st, 2016
    */
    private void OnDestroy()
    {
        //Suscribe to level load event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
     Description: Called when a new scene is loaded
     Parameters: Unused
     Creator: Alvaro Chavez Mixco
     Creation Date: Monday, October 31st, 2016
     */
    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        //Ensure the canvas uses the correct camera
        SetCanvasCamera();
    }

    /*
    Description: Set the conditions for the canvas to use. Since the player, and camera are created at runtime.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public void SetCanvasCamera()
    {
        //if there is a canvas
        if (m_canvas != null)
        {
            //Find the UI camera object
            GameObject cameraObject = GameObject.FindGameObjectWithTag(CGlobalTags.M_TAG_UI_CAMERA);

            //If the UI camera object is valid
            if (cameraObject != null)
            {
                //Set the canvas to use the UI Camera
                m_canvas.worldCamera = cameraObject.GetComponent<Camera>();
            }

            //Set the plane for the camera distance
            m_canvas.planeDistance = m_planeDistance;
        }
    }


}
