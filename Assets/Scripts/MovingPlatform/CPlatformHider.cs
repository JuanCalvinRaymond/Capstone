using UnityEngine;
using System.Collections;

public class CPlatformHider : MonoBehaviour
{
    /*
    Description: Suscribe to the show platform settings event
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, November 23, 2016
    */
    private void Awake()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Suscribe to the event when we change the showing platform
            CSettingsStorer.PInstanceSettingsStorer.OnIsShowingPlatformChange += HidePlatform;

            //Hide or show the platform according to initial state
            HidePlatform(CSettingsStorer.PInstanceSettingsStorer.PIsShowingPlatform);
        }
    }

    /*
    Description: Unsuscribe from the setting events
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, November 23, 2016
    */
    private void OnDestroy()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe to the is showing aiming aids event
            CSettingsStorer.PInstanceSettingsStorer.OnIsShowingPlatformChange -= HidePlatform;
        }
    }

    /*
    Description: Hide or show the platform.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, November 23, 2016
    Extra Notes: This function gets called with the  CSettingsStorer.s_instanceSettingsStorer.OnIsShowingPlatformChange event
    */
    private void HidePlatform(bool aShowStatus)
    {
        //Activated or deactivate the object, and its children
        gameObject.SetActive(aShowStatus);
    }
}
