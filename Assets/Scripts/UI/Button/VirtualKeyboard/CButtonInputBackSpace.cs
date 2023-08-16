using UnityEngine;
using System.Collections;

using UnityEngine.UI;

/*
Description: Class that inherits from AButtonFunctionality. This class is used remove the last string  character from the
determined text object.                                           
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CButtonInputBackSpace : AButtonFunctionality
{
    public Text m_inputField;//The text that this input will "Write" to

    /*
    Description: Override of the AButtonFunctionality OnExecution method. The function will merely remove the last character of the string
    in the text object we are modifying.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    */
    public override void OnButtonExecution()
    {
        //If there is a text object to modify
        if (m_inputField != null)
        {
            //If the string is not empty
            if (m_inputField.text.Length > 0)
            {
                //Remove the last element in the input field
                m_inputField.text = m_inputField.text.Remove(m_inputField.text.Length - 1);
            }
        }
    }
}