using UnityEngine;
using System.Collections;

using UnityEngine.UI;

/*
Description: Class used to store all the keys that are forming up a virtual keyboard
To work this class has to be the parent object of all of its keys.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CVirtualKeyboardHandler : MonoBehaviour
{
    //Keys that make up the virtual keyboard
    //NOTE: The caps and the enter key are not saved in this class since they don't use an input field.
    private CButtonInput[] m_inputkeys;
    private CButtonInputBackSpace[] m_backSpaceKeys;

    private bool m_isCaps = true;

    //Variables regarding the text whre the keyboard will write to
    [Tooltip("The text object that will be affected by all the keys in the virtual keyboard.")]
    public Text m_keyboardInputField;
    public int m_keyboardInputFieldMaxLength = 8;

    public bool PIsCaps
    {
        get
        {
            return m_isCaps;
        }

        set
        {
            m_isCaps = value;

            //Change all character to match if it's lower case or upper case
            ChangeCharacterCaseAllKeys(m_isCaps);
        }
    }

    public Text PKeyboardInputField
    {
        set
        {
            m_keyboardInputField = value;

            //Set the input key for all the keys linked to  this keyboard. Note that this will overwrite any previous 
            //input field the individual key had
            ChangeInputFieldAllKeys(m_keyboardInputField);
        }
    }

    public int PKeyboardInputFieldMaxLength
    {
        set
        {
            m_keyboardInputFieldMaxLength = value;

            //Change the max input length in every key
            ChangeInputFieldMaxLengthAllKeys(m_keyboardInputFieldMaxLength);
        }
    }

    /*
    Description:  At the start, save all the buttons components stored in the children of this object, and set default properties for all of them. 
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: Values stored in this class, sucha as InputFieldMaxLength, will overwrite all the values set in the CInputButton child object component 
    */
    private void Start()
    {
        m_inputkeys = GetComponentsInChildren<CButtonInput>();//Get all the button scripts from the children
        m_backSpaceKeys = GetComponentsInChildren<CButtonInputBackSpace>();//Get all the backspace buttons

        if (m_keyboardInputField != null)//If there is a keyboard input field
        {
            PKeyboardInputField = m_keyboardInputField;//Set it to be the active one for all the keys.
        }

        PKeyboardInputFieldMaxLength = m_keyboardInputFieldMaxLength;//Set in all the keys the max length of the input field
    }

    /*
    Description:  Changing every key in virtual keyboard to lower case or upper case
    Parameters: bool aCapital - Whether to change the keys to uppercase or lowercase.
    Creator: Juan Calvin Raymond
    Creation Date:  19 Dec 2016
    */
    private void ChangeCharacterCaseAllKeys(bool aCapital)
    {
        //Go through every key
        foreach (CButtonInput key in m_inputkeys)
        {
            //If the key is valid 
            if (key != null)
            {
                //Change its internal input value to lower case or upper case
                key.m_characterToInput = aCapital ? key.m_characterToInput.ToUpper() : key.m_characterToInput.ToLower();

                //Change it to lower case or upper case
                CUtilitySetters.SetText2DText(ref key.m_inputSymbol,
                    aCapital ? key.m_inputSymbol.text.ToUpper() : key.m_inputSymbol.text.ToLower());
            }
        }
    }

    /*
    Description:  Set the input field for all the keys linked to  this keyboard. 
    Parameters: Text aInputField - The new Text inputField that will be assigned to every key.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, January 13th, 2017
    Extra Notes: This doesn't check that the inputField is not null, in case we want to want to not write to anything.
    */
    private void ChangeInputFieldAllKeys(Text aInputField)
    {
        //Set the input field for all the keys linked to  this keyboard. Note that this will overwrite any previous 
        //input field the individual key had
        //Go through  input every key
        foreach (CButtonInput key in m_inputkeys)
        {
            //If the key is valid 
            if (key != null)
            {
                //Set its input field
                key.m_inputField = aInputField;
            }
        }

        //Go through every backspace key
        foreach (CButtonInputBackSpace backSpaceKey in m_backSpaceKeys)
        {
            //If the key is valid 
            if (backSpaceKey != null)
            {
                //Set its input field
                backSpaceKey.m_inputField = aInputField;
            }
        }
    }

    /*
    Description:  Set the input field  max length (number of characters)for all the keys linked to  this keyboard. 
    Parameters: int aNumCharacters- The max number of characters that can be in the input field
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, January 13th, 2017
    */
    private void ChangeInputFieldMaxLengthAllKeys(int aNumCharacters)
    {
        //Set the input field max length in all the keys
        //Go through every key
        foreach (CButtonInput key in m_inputkeys)
        {
            //If the key is valid 
            if (key != null)
            {
                //Set its input field
                key.m_inputFieldMaxLength = aNumCharacters;
            }
        }
    }
}
