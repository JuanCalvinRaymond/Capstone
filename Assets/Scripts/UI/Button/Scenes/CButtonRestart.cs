using UnityEngine;
using System.Collections;

using System;
using UnityEngine.SceneManagement;

/*
Description: Restart Button class inherit from AButtonFunctionality which will load the current scene
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
public class CButtonRestart : AButtonFunctionality
{
    /*
    Description: Reset the current scene
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    public override void OnButtonExecution()
    {
        Time.timeScale = 1.0f;//Set time scale to normal

        //If there is a scene manager
        if (CSceneManager.PInstanceSceneManager != null)
        {
            //Load the scene through the custom scene manager
            CSceneManager.PInstanceSceneManager.LoadSceneAsynchronous(CSceneManager.PInstanceSceneManager.GetCurrentSceneName(),
                CSceneManager.PInstanceSceneManager.GetCurrentSceneType());
        }
        else//If there is no custom scene manager
        {
            //Load the scene normally through unity
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
