using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

/*
Description: Go to scene Button class inherit from AButtonFunctionality which will load scene with string variable tweaked in inspector
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
public class CButtonGoToScene : AButtonFunctionality
{
    //variable of scene name to go to
    [Header("Go to Scene Button Settings")]
    public string m_sceneName = string.Empty;
    public ELevelState m_sceneType = ELevelState.NoMotion;

    /*
    Description: load the scene
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    public override void OnButtonExecution()
    {
        //If there is a scene name 
        if (m_sceneName != string.Empty)
        {
            //If there is a scene manager
            if (CSceneManager.PInstanceSceneManager != null)
            {
                //Load the new scene
                CSceneManager.PInstanceSceneManager.LoadSceneAsynchronous(m_sceneName, m_sceneType);
            }
            else//If there is no custom scene manager
            {
                //Load the scene using normal Unity scene manager
                SceneManager.LoadScene(m_sceneName);
            }
        }
    }


}
