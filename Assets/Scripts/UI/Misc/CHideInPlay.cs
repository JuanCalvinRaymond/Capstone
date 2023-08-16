using UnityEngine;
using System.Collections;
/*
Description: Classs used to hide an object during play state
Creator: Alvaro Chavez Mixco
Creation Date: Friday, March 3rd, 2017
Extra Note: Because the object will be disabled, if you wa
*/
public class CHideInPlay : MonoBehaviour
{
    /*
    Description: Suscribe from OnPlayState event
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void Start ()
    {
        //If there is a game manager
	    if(CGameManager.PInstanceGameManager != null)
        {
            //Suscribe to play state event
            CGameManager.PInstanceGameManager.OnPlayState += HideObject;
        }
	}

    /*
    Description: Unsuscribe from OnPlayState event
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void OnDestroy ()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //Unsuscribe from play state event
            CGameManager.PInstanceGameManager.OnPlayState -= HideObject;
        }
    }
  
    /*
    Description: Disable the object
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void HideObject()
    {
        //Disable the object
        gameObject.SetActive(false);
    }
}
