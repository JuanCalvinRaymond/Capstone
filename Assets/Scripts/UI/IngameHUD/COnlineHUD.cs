using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

/*
Description: HUD class used to display online error messages, and the current online
status. This HUD shows in all game states, EXCEPT PlayState
Creator: Alvaro Chavez Mixco
Creation Date:  Saturday, January 21, 2017
*/
public class COnlineHUD : MonoBehaviour
{
    private float m_timerRemoveMessage = 0.0f;

    private List<string> m_listErrorsMessage;
    private StringBuilder m_stringBuilder;

    [Tooltip("How long the text will be in screen before it is removed")]
    public float m_errorMessageScreenTime = 5.0f;

    //Text objecs to modify
    [Header("Text objects to modify")]
    public Text m_onlineStatusDisplay;
    public Text m_errorMessages;

    [Space(20)]
    [Tooltip("The parent object of anything that will be hidden during play game state")]
    public GameObject m_hiderObject;

    /*
    Description: Suscribe to the corresponding events for both the game manager and online manager, 
    and set the intial values.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void Start()
    {
        //Create the variables
        m_listErrorsMessage = new List<string>();
        m_stringBuilder = new StringBuilder();

        //If the game manager is valid
        if (CGameManager.PInstanceGameManager != null)
        {
            //Suscribe to its OnGameStateChange event
            CGameManager.PInstanceGameManager.OnGameStateChange += OnGameStateChange;

            //Hide or show the HUD according to the initial game state
            OnGameStateChange(CGameManager.PInstanceGameManager.PGameState);
        }

        //If the online manager is valid
        if (COnlineManager.s_instanceOnlineManager != null)
        {
            //Suscribe to its OnConnectionStatusDisplay, and MessageLog and ErrorLog Update events
            COnlineManager.s_instanceOnlineManager.OnConnectionStatusDisplayUpdate += SetOnlineStatus;
            COnlineManager.s_instanceOnlineManager.OnMessageLogUpdate += AddErrorMessage;
            COnlineManager.s_instanceOnlineManager.OnErrorLogUpdate += AddErrorMessage;

            //Set the intial connection status display according to if the online manager is connected or not
            SetOnlineStatus(COnlineManager.s_instanceOnlineManager.GetIsConnectedToLeaderboardServer());
        }
    }

    /*
    Description: Unsuscribe from the corresponding events for both the game manager and online manager, 
    and set the intial values.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void OnDestroy()
    {
        //If the game manager is valid
        if (CGameManager.PInstanceGameManager != null)
        {
            //Unsuscribe to its OnGameStateChange event
            CGameManager.PInstanceGameManager.OnGameStateChange -= OnGameStateChange;
        }

        //If the online manager is valid
        if (COnlineManager.s_instanceOnlineManager != null)
        {
            //Unsuscribe to its OnConnectionStatusDisplay, and MessageLog and ErrorLog Update events
            COnlineManager.s_instanceOnlineManager.OnConnectionStatusDisplayUpdate -= SetOnlineStatus;
            COnlineManager.s_instanceOnlineManager.OnMessageLogUpdate -= AddErrorMessage;
            COnlineManager.s_instanceOnlineManager.OnErrorLogUpdate -= AddErrorMessage;
        }
    }

    /*
    Description: Ensure that the error messages don't stay in screen for too long.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void Update()
    {
        //If the list of error message is valid
        if (m_listErrorsMessage != null)
        {
            //Remove any old message it may have
            RemoveOldMessages();
        }
    }

    /*
    Description: Ensure that the error messages don't stay in screen for too long.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function is normally called through this class RemoveOldMessages and AddErrorMessage functions.
    This functions do the corresponding checks for a valid list of error messages
    */
    private void UpdateErrorMessagesDisplay()
    {
        //If the list of error messages and the string builder are valid
        if (m_listErrorsMessage.Count > 0 && m_stringBuilder != null)
        {
            //Clear the string builder
            m_stringBuilder.Length = 0;

            //Go through each error message in the list
            for (int i = 0; i < m_listErrorsMessage.Count; i++)
            {
                //If the message is valid
                if (m_listErrorsMessage[i] != null)
                {
                    //Append it as a line in the string builder
                    m_stringBuilder.AppendLine(m_listErrorsMessage[i]);
                }
            }

            //Set the text of the error messages according to the content of the string builder
            CUtilitySetters.SetText2DText(ref m_errorMessages, m_stringBuilder.ToString());
        }
        else//If the error message list is empty
        {
            //Display an empty text in the error message text
            CUtilitySetters.SetText2DText(ref m_errorMessages, string.Empty);
        }
    }

    /*
    Description: Add an error message to the list of error messages.
    Parameters: string aError - The error message to be added
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function is normally called through the online manager OnMessageLogUpdate and OnErrorLogUpdate events.
    */
    public void AddErrorMessage(string aError)
    {
        //If the string is valid
        if (aError != null)
        {
            //Add it to the list of error messages
            m_listErrorsMessage.Add(aError);

            //Reset message removal timer
            m_timerRemoveMessage = m_errorMessageScreenTime;

            //Update the error message display
            UpdateErrorMessagesDisplay();
        }
    }

    /*
    Description: Remove the first (oldest) error message from the list of error messages
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function doesn't verify that the list of error messages is valid.
    */
    private void RemoveOldMessages()
    {
        //If there is an error in the list of error messages
        if (m_listErrorsMessage.Count > 0)
        {
            //Decrease time
            m_timerRemoveMessage -= Time.unscaledDeltaTime;

            //If timer is 0 or less
            if (m_timerRemoveMessage <= 0)
            {
                //Remove the first (oldest) message from the list
                m_listErrorsMessage.Remove(m_listErrorsMessage[0]);

                //Reset timer
                m_timerRemoveMessage = m_errorMessageScreenTime;

                //Update the error message display
                UpdateErrorMessagesDisplay();
            }

        }
    }

    /*
    Description: Changes the display of the online status according to the value being passed
    Parameters: bool aIsOnline - Is the online manager connected to the server or not
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function is normally called through the online manager OnConnectionStatusDisplayUpdate event.
    */
    private void SetOnlineStatus(bool aIsOnline)
    {
        //If the server is online
        if (aIsOnline == true)
        {
            //Change the text
            CUtilitySetters.SetText2DText(ref m_onlineStatusDisplay, CServerClientConstants.M_LABEL_ONLINE_TEXT);
        }
        else//If the server is offline
        {
            //Change the text
            CUtilitySetters.SetText2DText(ref m_onlineStatusDisplay, CServerClientConstants.M_LABEL_OFFLINE_TEXT);
        }
    }

    /*
    Description: Changes the display of the online status according to the value being passed
    Parameters: EGameStates aCurrentGameState - The current game state
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function is normally called through the game manager OnGameStateChange event.
    */
    private void OnGameStateChange(EGameStates aCurrentGameState)
    {
        //If the hider object is valid
        if (m_hiderObject != null)
        {
            //According to the game state
            switch (aCurrentGameState)
            {
                //If the game state is play
                case EGameStates.Play:
                    //Hide the HUD
                    m_hiderObject.SetActive(false);
                    break;
                //If the game state is not in play
                default:
                    //Show the HUD
                    m_hiderObject.SetActive(true);
                    break;
            }
        }
    }
}
