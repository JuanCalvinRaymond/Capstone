using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
/*
Description: Exit Button class inherit from Abutton functionality
Creator: Juan Calvin Raymond
Creation Date: 11-1-2016
*/
public class CButtonExit : AButtonFunctionality
{
    /*
    Description: Exit the application
    Creator: Juan Calvin Raymond
    Creation Date: 11-1-2016
    */
    public override void OnButtonExecution()
    {
        //If on build quit the program
        Application.Quit();

#if UNITY_EDITOR
        //If on editor, stop playing
        EditorApplication.isPlaying = false;
#endif
    }
}
