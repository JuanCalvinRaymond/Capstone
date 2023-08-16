using UnityEngine;
using System.Collections;

/*
Description: Helper class to switch a mesh/texture according to the distance an object has from the main camera. 
This is done for optimization purposes.
Creator: Alvaro Chavez Mixco
Extra Notes: The class makes use of the game manager
*/
public abstract class ALOD : MonoBehaviour
{
    private float m_squaredMaxDistanceFromCamera;
    private float m_timerCheckLOD;

    [Tooltip("The max distance that the object can have from the camera before applying a LOD ")]
    public float m_maxDistanceFromCamera = 4000.0f;
    [Tooltip("In seconds, how often will the object check if it has to change LODs.")]
    public float m_checkLODTime = 2.0f;

    /*
    Description: Get the required components to change the meshes/textures. Also get the distance to the main 
    camera squared.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void Start()
    {
        //Set whatever LOD elements are required.
        SetLODElements();

        //Set the initial status of the LOD as lowest poly
        CheckLODChanges(1.0f);

        //Square the distance
        m_squaredMaxDistanceFromCamera = m_maxDistanceFromCamera * m_maxDistanceFromCamera;

        //Get as a percentage how far the object is from the camera
        float percentageFromCamera = Mathf.InverseLerp(0.0f, m_squaredMaxDistanceFromCamera,
            CUtilityGame.GetDistanceXZSquaredToCameraGameManager(transform.position));

        //Change, if applicable, the LOD elements
        CheckLODChanges(percentageFromCamera);

    }

    /*
    Description: Check how far from the camera this object is as a percentage of the max camera distance property in this
    class. This percentage then will be used to determined if the mesh/texture should be switched or not.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void Update()
    {
        //Decrease the timer
        m_timerCheckLOD -= CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        //If its time to check the LOD
        if (m_timerCheckLOD < 0.0f)
        {
            //Reset the timer
            m_timerCheckLOD = m_checkLODTime;

            //Get as a percentage how far the object is from the camera
            float percentageFromCamera = Mathf.InverseLerp(0.0f, m_squaredMaxDistanceFromCamera,
                 CUtilityGame.GetDistanceXZSquaredToCameraGameManager(transform.position));

            //Change, if applicable, the LOD elements
            CheckLODChanges(percentageFromCamera);
        }
    }

    /*
    Description: Abstract function called at start. This function is supposed to be used to get the components (mesh renderers, etc.)
    needed to make the LOD changes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    protected abstract void SetLODElements();

    /*
    Description: Abstract function called in update, after the m_checkLODTime has passed. This function is intended to be used
    to change if the object should change its LOD objects.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    protected abstract void CheckLODChanges(float aPercentageDistanceFromCamera);
}