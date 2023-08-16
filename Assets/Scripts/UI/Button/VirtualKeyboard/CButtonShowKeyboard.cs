using UnityEngine;
using System.Collections;

/*
Description: Class that inherits from AButton. This class is used to show and hide the virtual keyboard.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CButtonShowKeyboard : AButtonFunctionality
{
    public GameObject m_keyboardObject;//The virtual keyboard we want to activate and deactivate

    /*
    Description: Function that overrides AButtonFunctionality OnExecution method. The method will merely toggle the 
    active state of the virtual keyboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016 
    */
    public override void OnButtonExecution()
    {
        //If there is a virtual keyboard object
        if (m_keyboardObject != null)
        {
            //Toggle, simply flip between active and inactive
            m_keyboardObject.SetActive(!m_keyboardObject.activeSelf);
        }
    }
}
