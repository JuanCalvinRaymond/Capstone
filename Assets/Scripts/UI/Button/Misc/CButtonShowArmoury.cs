using UnityEngine;
using System.Collections;
using System;

/*
Description: Button functionality to show or hide the armoury.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CButtonShowArmoury : AButtonFunctionality
{
    /*
    Description: Simple enum to detect what interaction the button will have with the armoury
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public enum EArmouryInteraction
    {
        Spawn,
        Hide
    }

    public CArmoury m_armoury;
    public EArmouryInteraction m_armouryInteraction;

    /*
    Description: The function will merely call the armoury script functions to show or hide the armoury according
                 to the desired interaction type.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    Extra Notes: Function called when the button OnExecution event is called.
    */
    public override void OnButtonExecution()
    {
        //If tehre si an armoury
        if (m_armoury != null)
        {
            //If we want to show it
            if (m_armouryInteraction == EArmouryInteraction.Spawn)
            {
                //Create the weapons
                m_armoury.PlaceWeapons();
            }
            else if (m_armouryInteraction==EArmouryInteraction.Hide)//If we want to hide it
            {
                //Destroy all dropped weapons in the scene
                m_armoury.HideWeapons();
            }
        }
    }
}
