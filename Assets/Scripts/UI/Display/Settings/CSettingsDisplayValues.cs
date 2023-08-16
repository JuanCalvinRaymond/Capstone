using UnityEngine;
using System.Collections;

/*
Description: Abstract class used  to display setting values in a 3D Text object. This
abstract class handles suscribing and unsuscribing from events. Besides some helper functions
to format the values when they are displayed.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, Novemeber 23, 2016
Extra Notes:
*/
public abstract class CSettingsDisplayValues : MonoBehaviour
{
    //Messages that will be used to dispaly alongside the score
    protected const string M_ON_MESSAGE = "On";
    protected const string M_OFF_MESSAGE = "Off";
    protected const string M_PERCENT_SIGN = "%";
    protected const string M_X_SIGN = "X";

    /*
    Description: This will suscribe to the desired events, and set the initial values of the text
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, Novemeber 23, 2016
    Extra Notes: For optimization, it will disable this script if there is no settings storer, since there
    won't be any settings.
    */
    protected void Awake()
    {
        //If there is no setting storer
        if (CSettingsStorer.PInstanceSettingsStorer == null)
        {
            //Disable this component
            enabled = false;
        }
        else//If there is a settings storer
        {
            //Suscribe to all the events it will display
            SuscribeToEvents();

            //Set the initial state of all the values
            SetDisplayValues();
        }
    }

    /*
    Description: Unsuscribe from all the events of Setting Storer
    Parameters(Optional):
    Creator: Alvaro, Chavez Mixco
    */
    protected void OnDestroy()
    {
        //If there is a settings storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe to all the events it will display
            UnsuscribeToEvents();
        }
    }

    /*
    Description: Helper function to display text different from true or false when we get
    a bool setting.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, Novemeber 23, 2016
    Extra Notes:
    */
    protected string OnOrOffText(bool aValue)
    {
        //If the value is on
        if (aValue == true)
        {
            //Return the on message
            return M_ON_MESSAGE;
        }
        else//If the value is off
        {
            //Return off message
            return M_OFF_MESSAGE;
        }
    }


    /*
    Description: Helper function to display a screen resolution in a 3D text object with the appropiate formatting.
    Parameters: ref TextMesh aText - The 3D text object that will display the value 
                SScreenResolution aResolution - A screen resolution.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    */
    protected void SetScreenResolutionText(ref TextMesh aText, SScreenResolution aResolution)
    {
        //Display the screen resolution as a text
        CUtilitySetters.SetTextMeshText(ref aText, aResolution.m_width.ToString() + "\n"+ 
            M_X_SIGN + "\n" + aResolution.m_height.ToString());
    }

    /*
    Description: Helper function to display a percent in a 3D text object with the appropiate formatting.
    Parameters: ref TextMesh aText - The 3D text object that will display the value 
                          float aPercent - The percent (0 - 1) that will be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    */
    protected void SetPercentText(ref TextMesh aText, float aPercent)
    {
        //Display the percent in the text as value without decimals
        CUtilitySetters.SetTextMeshText(ref aText, Mathf.Round((aPercent * 100.0f)).ToString() + M_PERCENT_SIGN);
    }

    /*
    Description: Helper function to display a float in a 3D text object with the appropiate formatting.
    Parameters: ref TextMesh aText - The 3D text object that will display the value 
                          float aValue - The value that will be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    Extra Notes: Currently this function only calls CUtilityGame.SetTextMeshText, but is wrapped in
    this function in case we want a special formatting for the float.
    */
    protected void SetFloatText(ref TextMesh aText, float aValue)
    {
        //Set the text in the mesh
        CUtilitySetters.SetTextMeshText(ref aText, CUtilityMath.RoundTo2Digits(aValue).ToString());
    }

    /*
    Description: Helper function to display an int in a 3D text object with the appropiate formatting.
    Parameters: ref TextMesh aText - The 3D text object that will display the value 
                int aValue - The value that will be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 30, 2017
    */
    protected void SetIntText(ref TextMesh aText, int aValue)
    {
        //Display the value
        CUtilitySetters.SetTextMeshText(ref aText, aValue.ToString());
    }

    /*
    Description: Helper function to display a bool in a 3D text object with the appropiate formatting.
    Parameters: ref TextMesh aText - The 3D text object that will display the value 
                          bool aValue - The value that will be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    */
    protected void SetBoolText(ref TextMesh aText, bool aValue)
    {
        //Display the value
        CUtilitySetters.SetTextMeshText(ref aText, OnOrOffText(aValue));
    }

    /*
    Description: Abstract function that will be called at Awake. This function is where the class will
    suscribe to all the events it cares about.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    */
    protected abstract void SuscribeToEvents();

    /*
    Description: Abstract function that will be called OnDestroy. This function is where the class will
    unsuscrbie from all the events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    */
    protected abstract void UnsuscribeToEvents();

    /*
    Description: Abstract function used to update the values displayed in all the texts.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, January 9, 2017
    */
    protected abstract void SetDisplayValues();
}
