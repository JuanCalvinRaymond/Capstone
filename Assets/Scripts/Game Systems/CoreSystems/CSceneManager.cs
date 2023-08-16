using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

/*
Description:Class using singleton pattern used to load new scenes. This class will
handle things for loading, such as calling OnFinishedLoading event and managing the loading screens.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CSceneManager : MonoBehaviour
{
    //Singleton Instance
    private static CSceneManager s_intanceSceneManager;

    //Variables to store when a level is being loaded
    private bool m_isLoadingScene = false;
    private string m_nameSceneBeingLoaded;
    private ELevelState m_typeSceneBeingLoaded = ELevelState.NoMotion;

    private string m_previousNameSceneLoaded;
    private ELevelState m_previousTypeSceneLoaded = ELevelState.NoMotion;

    [Tooltip("Can be used to load the new scene ASync while using VR")]
    public SteamVR_LoadLevel m_SteamVRLoadLevel;
    public CLoadingSceneHUD m_loadingScreenHUD;

    //Events for loading scenes
    public delegate void delegLoadingScene(string aSceneName, ELevelState aTypeOfLevel);

    public event delegLoadingScene OnStartingLoadingNewScene;
    public event delegLoadingScene OnFinishedLoadingNewScene;

    public static CSceneManager PInstanceSceneManager
    {
        get
        {
            return s_intanceSceneManager;
        }
    }

    public string PPreviousNameSceneLoaded
    {
        get
        {
            return m_previousNameSceneLoaded;
        }
    }

    public ELevelState PPreviousTypeSceneLoaded
    {
        get
        {
            return m_previousTypeSceneLoaded;
        }
    }

    /*
    Description:Ensure there is only 1 instance of the scene manager, and that it is not destroyed when
    a new scene is loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Awake()
    {
        //Singleton instance
        //If the instance doesn't exist
        if (s_intanceSceneManager == null)
        {
            //Set this as the instance
            s_intanceSceneManager = this;

            //If there is a loading screen hud
            if (m_loadingScreenHUD != null)
            {
                //Disable it 
                m_loadingScreenHUD.gameObject.SetActive(false);
            }

            //Suscribe to the on scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            //Ensure the game object is not destroyed when a new scene is loaded
            DontDestroyOnLoad(this);
        }
        else//If the instance already exists
        {
            //Destroy this object
            Destroy(gameObject);
        }
    }

    /*
    Description:Unsuscribe from Unity on load scene event, and ensure that all coroutines are stopped.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void OnDestroy()
    {
        //Stop the loading coroutine
        StopCoroutine(LoadingAsyncCoroutine(string.Empty, ELevelState.NoMotion));

        //Unsuscrbie from the on scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
    Description: Once a new scene has been loaded, call the event that a new scene has been loaded.
    Parameters: Not used in this function
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: This function is called when the Unity Event SceneManager.sceneLoaded is called
    */
    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        //If the loading scene is set to true, the scene was loaded normally and not async
        if (m_isLoadingScene == true)
        {
            //If anyone suscribe to the event that a new scene has finished loading
            if (OnFinishedLoadingNewScene != null)
            {
                //Call the event that a new scene has been loaded
                OnFinishedLoadingNewScene(m_nameSceneBeingLoaded, m_typeSceneBeingLoaded);
            }

            //Set that the scene has finished loading
            m_isLoadingScene = false;
        }
    }

    /*
    Description: Sets the data for when a scene is being loaded.
    Parameters: string aSceneBeingLoadedName - The name of the scene being loaded.
                ELevelState aSceneBeingLoadedType - The general type (game, menu ,etc) of the scene being loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void SetSceneBeingLoadedData(string aSceneBeingLoadedName, ELevelState aSceneBeingLoadedType)
    {
        ///Set that a scene is loading
        m_isLoadingScene = true;

        //Save the data of the previous scene
        m_previousNameSceneLoaded = m_nameSceneBeingLoaded;
        m_previousTypeSceneLoaded = m_typeSceneBeingLoaded;

        //Save the data of the scene that is loading
        m_nameSceneBeingLoaded = aSceneBeingLoadedName;
        m_typeSceneBeingLoaded = aSceneBeingLoadedType;
    }

    /*
    Description: Loads a scene async using a coroutine.
    Parameters: string aSceneBeingLoadedName - The name of the scene being loaded.
                ELevelState aSceneBeingLoadedType - The general type (game, menu ,etc) of the scene being loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private IEnumerator LoadingAsyncCoroutine(string aSceneName, ELevelState aSceneType)
    {
        //Load the desired scene asynchronously
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(aSceneName, LoadSceneMode.Single);

        //Disable the activation of the scene while it is still being loaded
        loadingOperation.allowSceneActivation = false;

        //While the scene hasn't been loaded
        while (loadingOperation.isDone == false)
        {
            //If there is a hud
            if (m_loadingScreenHUD != null)
            {
                //Update the progress in the hud
                m_loadingScreenHUD.UpdatePercentDisplay(loadingOperation.progress);
            }

            //When allowSceneActivation is set to false then progress is stopped at 0.9.
            //The isDone is then maintained at false.
            //When allowSceneActivation is set to true isDone can complete.
            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;

                //Mark that the scene has finished loading
                m_isLoadingScene = false;
            }

            //Go to next pass in loop
            yield return null;
        }

        //If there is a hud
        if (m_loadingScreenHUD != null)
        {
            //Hide the HUD
            m_loadingScreenHUD.gameObject.SetActive(false);
        }

        //Go to next pass in coroutine
        yield return null;


        //If anyone suscribe to the event that a new scene has finished loading
        if (OnFinishedLoadingNewScene != null)
        {
            //Call the event that a new scene has been loaded
            OnFinishedLoadingNewScene(aSceneName, aSceneType);
        }

        //Exit function
        yield return null;
    }

    /*
    Description: Loads a scene async for non VR, using a coroutine
    Parameters: string aSceneBeingLoadedName - The name of the scene being loaded.
                ELevelState aSceneBeingLoadedType - The general type (game, menu ,etc) of the scene being loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void LoadAsyncNonVR(string aSceneName, ELevelState aSceneType)
    {
        //If there is a loading screen hud
        if (m_loadingScreenHUD != null)
        {
            //Enable it 
            m_loadingScreenHUD.gameObject.SetActive(true);
            m_loadingScreenHUD.SetLoadingImage(aSceneType);//Set the loading image for that screen

            //Start the coroutine to load the scene
            StartCoroutine(LoadingAsyncCoroutine(aSceneName, aSceneType));
        }
    }

    /*
    Description: Loads a scene async for VR using SteamVR_LoadLevel class
    Parameters: string aSceneBeingLoadedName - The name of the scene being loaded.
                ELevelState aSceneBeingLoadedType - The general type (game, menu ,etc) of the scene being loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void LoadAsyncVR(string aSceneName, ELevelState aSceneType)
    {
        //If the steam vr load level is valid
        if (m_SteamVRLoadLevel != null)
        {
            //Set the name of the scene to load
            m_SteamVRLoadLevel.levelName = aSceneName;

            //Set SteamVR to load the scene async
            m_SteamVRLoadLevel.loadAsync = true;

            //Load the scene Async using SteamVR Load level
            SteamVR_LoadLevel.Begin(m_SteamVRLoadLevel.levelName, m_SteamVRLoadLevel.showGrid, m_SteamVRLoadLevel.fadeOutTime,
                m_SteamVRLoadLevel.backgroundColor.r, m_SteamVRLoadLevel.backgroundColor.g, m_SteamVRLoadLevel.backgroundColor.b, m_SteamVRLoadLevel.backgroundColor.a);
        }
    }

    /*
    Description: Public function to load a scene asynchronous, this function will determine if the
    loading should be done for VR or nonVR.
    Parameters: string aSceneBeingLoadedName - The name of the scene being loaded.
                ELevelState aSceneBeingLoadedType - The general type (game, menu ,etc) of the scene being loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void LoadSceneAsynchronous(string aSceneName, ELevelState aSceneType)
    {
        //If a scene is not currently being loaded
        if (IsLoadingScene() == false)
        {
            //If anyone suscribe to the event that a new scene is beign loaded
            if (OnStartingLoadingNewScene != null)
            {
                //Call the event that a new scene is being load
                OnStartingLoadingNewScene(aSceneName, aSceneType);
            }

            //Set the loading scene data
            SetSceneBeingLoadedData(aSceneName, aSceneType);

            //If there is a setting storer and a SteamVRLoadLevel
            if (CSettingsStorer.PInstanceSettingsStorer != null && m_SteamVRLoadLevel != null)
            {
                //If the player is using the vive
                if (CSettingsStorer.PInstanceSettingsStorer.PInputMethod == EControllerTypes.ViveController)
                {
                    //Load the scene using SteamVR Load Level
                    LoadAsyncVR(aSceneName, aSceneType);

                    //Exit the function
                    return;
                }
            }

            //If there is no setting storers, or the player is using mouse and keyboard
            LoadAsyncNonVR(aSceneName, aSceneType);
        }
    }


    /*
    Description: Public function to load a scene synchronous.
    Parameters: string aSceneBeingLoadedName - The name of the scene being loaded.
                ELevelState aSceneBeingLoadedType - The general type (game, menu ,etc) of the scene being loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Even if VR is being used, this function DOESN'T make use of SteamVR_LoadLevel
    */
    public void LoadScene(string aSceneName, ELevelState aSceneType)
    {
        //If a scene is not currently being loaded
        if (IsLoadingScene() == false)
        {
            //If anyone suscribe to the event that a new scene is beign loaded
            if (OnStartingLoadingNewScene != null)
            {
                //Call the event that a new scene is being load
                OnStartingLoadingNewScene(aSceneName, aSceneType);
            }

            //Set the loading scene data
            SetSceneBeingLoadedData(aSceneName, aSceneType);

            //Load the new scene
            SceneManager.LoadScene(aSceneName);
        }
    }


    /*
    Description: Wrapper, currently doesn't do anything special, to get the name
    of the currently active scene.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /*
    Description: Get the type of the current scene.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: This function only works if the current was loading the custom scene manager and not
                 through unity scene manager directly.
    */
    public ELevelState GetCurrentSceneType()
    {
        //This function only works if the current was loading the custom scene manager and not
        //through unity scene manager directly.
        return m_typeSceneBeingLoaded;
    }

    /*
    Description: Get the type of the current scene.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: This function only works if the current was loading the custom scene manager and not
                    through unity scene manager directly.
    */
    public bool IsLoadingScene()
    {
        //If there is a SteamVR_LoadLevel component
        if (m_SteamVRLoadLevel != null)
        {
            //Return the loading status according to a combination of the script and
            // the steamVRLoadLevel component
            return m_isLoadingScene == true && SteamVR_LoadLevel.loading == true;
        }
        else //If there is no SteamVR_LoadLevel
        {
            //Return the loading status according to the script
            return m_isLoadingScene;
        }
    }
}
