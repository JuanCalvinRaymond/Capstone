using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

/*
Description: Class to simply make a "splash screen". Once the time is over on this script, or
             if the user skips it, this script would load up a new scene.
Creator: Alvaro Chavez Mixco
*/
public class CSplashScreenManager : MonoBehaviour
{
    public string m_sceneToLoad;
    public ELevelState m_sceneToLoadType;

    public float m_splashTimer = 6.0f;
    public bool m_skippable = true;

    /*
    Description: Decrease the timer and check for input (if the splash screen will be skipped)
    Creator: Alvaro Chavez Mixco
    */
    private void Update()
    {
        //Decrease the timer
        m_splashTimer -= CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        //If timer is over
        if (m_splashTimer <= 0)
        {
            //Load desired scene
            LoadDesiredScene();
        }

        //If it is skippable
        if (m_skippable == true)
        {
            //Check if any key was pressed
            if(CUtilityGame.GetAnyKeyPressed()==true)
            {
                //If a key pressed, load  the desired scnee
                LoadDesiredScene();
            }
        }
    }

    /*
    Description: Load the desired scene
    Creator: Alvaro Chavez Mixco
    */
    private void LoadDesiredScene()
    {
        //If there is a scene manager
        if(CSceneManager.PInstanceSceneManager == null)
        {
            //Load the scene using the scene manager
            CSceneManager.PInstanceSceneManager.LoadScene(m_sceneToLoad, m_sceneToLoadType);
        }
        else//If there is no scene manager
        {
            //Load the desired scene
            SceneManager.LoadScene(m_sceneToLoad);
        }

    }

}
