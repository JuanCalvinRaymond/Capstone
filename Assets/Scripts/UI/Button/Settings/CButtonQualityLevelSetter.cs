using UnityEngine;
using System.Collections;
using System;

/*
Description: Class that inherits from AButtonFunctionality. This class is used to change the 
quality level settings in the game
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, January 28, 2017
*/
class CButtonQualityLevelSetter : AButtonFunctionality
{
    [Tooltip("The quality level that will be set when the button is pressed.")]
    public EQualityLevels m_qualityLevelToSet = EQualityLevels.Fantastic;

    /*
    Description: This function will get called whenever the button is pressed. The function
    will set the quality level of the game to whatever value it has stored.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    public override void OnButtonExecution()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Set the desired quality level
            CSettingsStorer.PInstanceSettingsStorer.PQualityLevel = (int)m_qualityLevelToSet;
        }
        else//If there is no setting storer
        {
            //Set the quality level directly in the engine, this won't be saved through multiple playthroughs
            QualitySettings.SetQualityLevel((int)m_qualityLevelToSet);
        }
    }
}
