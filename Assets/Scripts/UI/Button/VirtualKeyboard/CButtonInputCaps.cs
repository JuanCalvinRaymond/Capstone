using UnityEngine;
using System.Collections;

/*
Description: Class inherits form AButtonFunctionality. Class to easily change all the keys in a virtual keyborad from
lowercase to upper case and viceversa.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CButtonInputCaps : AButtonFunctionality
{
    public CVirtualKeyboardHandler m_keyboard;
    public bool m_isCaps = true;

    /*
    Description: Override AButtonFunctionality OnExecution method. Function to easily change
    all the keys in a virtual keyborad from lowercase to upper case and viceversa.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    */
    public override void OnButtonExecution()
    {
        //If there is a virtual keyboard
        if (m_keyboard != null)
        {
            m_isCaps = !m_isCaps;//Toggle the values
            m_keyboard.PIsCaps = m_isCaps;//Set the value for all the keyboard
        }
    }
}