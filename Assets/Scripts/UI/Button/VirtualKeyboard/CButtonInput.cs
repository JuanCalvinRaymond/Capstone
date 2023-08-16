using UnityEngine;
using System.Collections;

using UnityEngine.UI;

/*
Description: Class that inherits from AButtonFunctionality. This class is used to add a character string to the
determined text object.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CButtonInput : AButtonFunctionality
{
    public Text m_inputField;//The text that this input will "Write" to
    public Text m_inputSymbol;//The visual representation of what this button represents
    public string m_characterToInput;

    public int m_inputFieldMaxLength = 8;

    /*
    Description: Function ensure that the visual representaiton
    of the button matches the string value it stores.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    */
    protected override void Awake()
    {
        //Call the base awake
        base.Awake();

        //Make the text match the string to input
        CUtilitySetters.SetText2DText(ref m_inputSymbol, m_characterToInput);
    }

    /*
    Description: Override of the AButtonFunctionality OnExecution method. This function will add the string stored in
    this object to the desired text string.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016 
    */
    public override void OnButtonExecution()
    {
        //If there is a text where to write to
        if (m_inputField != null)
        {
            //If we can still add to the name
            if (m_inputField.text.Length < m_inputFieldMaxLength)
            {
                //Add to the current input field
                m_inputField.text += m_characterToInput;
            }
        }
    }
}