using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

/*
Description: Class to link pure C# networking with unity
Creator: Alvaro Chavez Mixco
Creation Date: Friday, January 13, 2017
*/
public class COnlineManager : MonoBehaviour
{
    /*
    Description: Enum to determine when should the online manager to the online leaderbaord server.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public enum EConnectToLeaderboardServerWhen
    {
        ConnectManually,//Connect manually through calls from button or other classs.
        ConnectAtSceneAwake,//Connect on the OnlineManager awake
        ConnectAtEndGame//Connect when the game reaches the end state
    }

    /*
    Description: Class to control a leaderbaord request. This class is used to update a leaderboard, 
    even if the online manager didn't have immediate access to the requested online leaderboard. So
    it will constantly check if the online leaderboard has been send, and used it to fill the leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public class CLeaderboardRequest
    {
        private ELevelState m_requestedLeaderboardLevel;
        private CLeaderboard m_leaderboardAwaitingData;
        private float m_waitTimer;

        public ELevelState PRequestedLeaderboardLevel { get { return m_requestedLeaderboardLevel; } }
        public float PWaitTimer { get { return m_waitTimer; } set { m_waitTimer = value; } }

        /*
        Description: Constructor for CLeaderboardRequest. This saves all the pertinent data to the
        leaderboard awaiting data. 
        Parameters: ref CLeaderboard aLeaderboardAwaitingData - A reference to the leaderboard object that is
                                                                requesting the online leaderboard data.
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 19, 2017
        */
        public CLeaderboardRequest(ref CLeaderboard aLeaderboardAwaitingData)
        {
            //Set initial timer to 0
            m_waitTimer = 0.0f;

            //Initialize the requested leaderboard level
            m_requestedLeaderboardLevel = ELevelState.NoMotion;

            //If the leaderboard awaiting data is valid
            if (aLeaderboardAwaitingData != null)
            {
                //Save the leaderboard level it is requesting
                m_requestedLeaderboardLevel = aLeaderboardAwaitingData.PLeaderboardLevel;
            }

            //Save the reference of the leaderboard awaiting data
            m_leaderboardAwaitingData = aLeaderboardAwaitingData;
        }

        /*
        Description: Adds entries to the leaderboard awaiting data.
        Parameters: List<SPlayerEntry> aEntries - The entries that will be added to the leaderboard awaiting data.
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 19, 2017
        */
        public void AddDataToLeaderboardAwaitingData(List<SPlayerEntry> aEntries)
        {
            //If the leaderboard and the entries are both valid.
            if (m_leaderboardAwaitingData != null && aEntries != null)
            {
                //Add the entries to the leaderboard.
                m_leaderboardAwaitingData.AddToLeaderboard(aEntries);
            }
        }
    }

    public const float M_SEND_RECEIVE_WAIT_TIME = 60.0f;

    //Singleton instance
    public static COnlineManager s_instanceOnlineManager;

    //Networking variables
    private bool m_isConnecting = false;//Used when the coroutine function to connect to server is still running
    private COnlineLeaderboardClient m_onlineLeaderboardClient;

    private List<CLeaderboardRequest> m_leaderboardRequests;

    //Setup variables
    [Header("Server Setup")]
    public Text m_textFieldServerIP;
    public string m_serverIPToConnectTo = CServerClientConstants.M_SERVER_IP_ADDRESS;

    public EConnectToLeaderboardServerWhen m_whenToConnectToLeaderboardServer = EConnectToLeaderboardServerWhen.ConnectManually;

    [Space(20)]
    [Tooltip("How long should the client try to connect to the server before giving up.")]
    public float m_connectingToServerTimeOut = 2;

    //Events- Note that this functions are based on when a request is made, and don't actually guarantee 
    //that the client conencted to the server
    public delegate void delegLeaderboardConnectionStatus();
    public event delegLeaderboardConnectionStatus OnConnectToServerRequest;
    public event delegLeaderboardConnectionStatus OnDisconnectToServerRequest;

    public delegate void delegStringLogUpdate(string aMessage);
    public event delegStringLogUpdate OnEventLogUpdate;
    public event delegStringLogUpdate OnMessageLogUpdate;
    public event delegStringLogUpdate OnErrorLogUpdate;

    public delegate void delegBoolLogUpdate(bool aStatus);
    public event delegBoolLogUpdate OnConnectionStatusDisplayUpdate;

    public string PServerIPAddress
    {
        get
        {
            //If there is an online leaderboard client
            if (m_onlineLeaderboardClient != null)
            {
                //Return the address it has
                return m_onlineLeaderboardClient.PServerIPAddress;
            }

            return string.Empty;
        }
    }

    /*
    Description: Singleton pattern. It creates the online network client.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 13, 2017 - Unfinished
    */
    private void Awake()
    {
        //Singleton pattern
        //If there is no online manager instance
        if (s_instanceOnlineManager == null)
        {
            //Set this as the instance
            s_instanceOnlineManager = this;

            //Create the network client
            m_onlineLeaderboardClient = new COnlineLeaderboardClient();

            //Create list of leaderboard requests
            m_leaderboardRequests = new List<CLeaderboardRequest>();

            //Set when the online manager will connect
            SetStartingConnections();
        }
        else//If there was previously another instance of online manager
        {
            //Destroy this gameobject
            Destroy(gameObject);
        }
    }

    /*
    Description: Ensure that the client has disconnected from the server, and unsuscribe from 
    any event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 11, 2017
    */
    private void OnDestroy()
    {
        //Disconnect from the leaderboard server
        DisconnectFromLeaderboardServer();
    }

    /*
    Description: Updates all the leaderboard requests.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private void Update()
    {
        //Update the leaderboard requests
        UpdateLeaderboardRequests();

        //Update the client events
        UpdateClientEvents();
    }

    /*
    Description: Set when the online manager will connect to the online leaderboard server.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private void SetStartingConnections()
    {
        //Set when the leaderboard will connect to the server
        switch (m_whenToConnectToLeaderboardServer)
        {
            //The code will manually tell the online manager when to connect, no action required.
            //A button click can then be used to connect for example.
            case EConnectToLeaderboardServerWhen.ConnectManually:
                break;
            //This code is called at start, so immediately connect to server
            case EConnectToLeaderboardServerWhen.ConnectAtSceneAwake:
                ConnectToLeaderboardServer();
                break;

            //It will suscribe the connection event to the OnEndGameState of the game manager
            case EConnectToLeaderboardServerWhen.ConnectAtEndGame:
                //If there is a game manager
                if (CGameManager.PInstanceGameManager != null)
                {
                    //Suscribe the connect to leaderboard function to the OnEndGameState event
                    CGameManager.PInstanceGameManager.OnEndGameState += ConnectToLeaderboardServer;
                }
                break;
            default:
                break;
        }
    }

    private void UpdateConnectionToServer(bool aIsOnline)
    {
        if (aIsOnline == false)
        {
            if (m_onlineLeaderboardClient != null)
            {
                m_onlineLeaderboardClient.DisconnectClient();
            }

        }
    }

    /*
    Description: Go through all the leaderboard requests, and check if the data that they are requesting
    has arrived. If it has, add the data to them. Otherwise if a certain amount of time passes,
    and the data hasn't arrived, remove the request.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private void UpdateLeaderboardRequests()
    {
        //If the leaderboard request list and the online leaderboard clients are valid
        if (m_leaderboardRequests != null && m_onlineLeaderboardClient != null)
        {
            List<SPlayerEntry> listEntries;

            //Go through all the leaderboard requests
            for (int i = 0; i < m_leaderboardRequests.Count; i++)
            {
                listEntries = new List<SPlayerEntry>();

                //If the current leaderboard requet is valid
                if (m_leaderboardRequests[i] != null)
                {
                    //Increase the timer for wait time
                    m_leaderboardRequests[i].PWaitTimer += Time.unscaledDeltaTime;

                    //Get the online leaderboard, stored in the leaderboard client. Note
                    //that a single client only supports one leaderboard per level
                    listEntries = m_onlineLeaderboardClient.GetLeaderboardEntries(
                        m_leaderboardRequests[i].PRequestedLeaderboardLevel);

                    //Add the data to the leaderboard awaiting data
                    m_leaderboardRequests[i].AddDataToLeaderboardAwaitingData(listEntries);

                    //If the list of entries is valid
                    if (listEntries != null)
                    {
                        //If the list of entries is not empty(It successfulyl added data)
                        if (listEntries.Count > 0)
                        {
                            //Remove the entry from the list of leaderboard requests
                            m_leaderboardRequests.Remove(m_leaderboardRequests[i]);
                        }
                    }
                    else if (m_leaderboardRequests[i].PWaitTimer > M_SEND_RECEIVE_WAIT_TIME)//If the entry timer is above time 
                    {
                        //If the requeste timed out
                        UpdateErrorMessageUI(CServerClientConstants.M_ERROR_CANT_FIND_REQUESTED_LEADERBOARD + m_leaderboardRequests[i].PRequestedLeaderboardLevel.ToString());

                        //Remove the entry from the list of leaderboard requests
                        m_leaderboardRequests.Remove(m_leaderboardRequests[i]);

                    }

                }
                else//If the current leaderboard requets is not valid
                {
                    //Remove the entry from the list of leaderboard requests
                    m_leaderboardRequests.Remove(m_leaderboardRequests[i]);
                }
            }
        }
    }

    private void UpdateClientEvents()
    {
        //If there is an online client
        if (m_onlineLeaderboardClient != null)
        {
            //Get the list of network events
            List<SNetworkClientEvents> listOfClientEvents = m_onlineLeaderboardClient.PClientNetworkEvents;

            //If there is a list of events
            if (listOfClientEvents != null)
            {
                //Go through all the events
                foreach (SNetworkClientEvents clientEvent in listOfClientEvents)
                {
                    //According to the event type
                    switch (clientEvent.m_eventType)
                    {
                        case ENetworkEventTypes.Log:
                            UpdateEventLog(clientEvent.m_message);
                            break;
                        case ENetworkEventTypes.Error:
                            UpdateErrorMessageUI(clientEvent.m_message);
                            break;
                        case ENetworkEventTypes.Message:
                            UpdateMessageLog(clientEvent.m_message);
                            break;
                        case ENetworkEventTypes.ConnectionStatus:
                            UpdateConnectionStatusUI(clientEvent.m_connectionStatus);
                            break;
                        default:
                            break;
                    }
                }
            }

            //Clear the list of events
            m_onlineLeaderboardClient.ClearEventList();
        }
    }

    /*
    Description: Debug log the requested message, and call the OnEventLogUpdate event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function gets called when the COnlineLeaderboardClient OnEventLogUpdate event is called.
    */
    private void UpdateEventLog(string aMessage)
    {
        //DEBUGLIST-AAA
        Debug.Log(aMessage);

        //If the event is valid
        if (OnEventLogUpdate != null)
        {
            //Call the event
            OnEventLogUpdate(aMessage);
        }
    }

    /*
    Description: Call the OnMessageLogUpdate event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function gets called when the COnlineLeaderboardClient OnMessageLogUpdate event is called.
    */
    private void UpdateMessageLog(string aMessage)
    {
        //If the event is valid
        if (OnMessageLogUpdate != null)
        {
            //Call the event
            OnMessageLogUpdate(aMessage);
        }
    }

    /*
    Description: Call the OnErrorLogUpdate event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function gets called when the COnlineLeaderboardClient OnErrorLog event is called.
    */
    private void UpdateErrorMessageUI(string aMessage)
    {
        //If the event is valid
        if (OnErrorLogUpdate != null)
        {
            //Call the event
            OnErrorLogUpdate(aMessage);
        }
    }

    /*
    Description: Call the OnConnectionStatusDisplayUpdate event.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function gets called when the COnlineLeaderboardClient OnOnlineStatusDisplayChange event is called.
    */
    private void UpdateConnectionStatusUI(bool aIsOnline)
    {
        //If the event is valid
        if (OnConnectionStatusDisplayUpdate != null)
        {
            //Call the event
            OnConnectionStatusDisplayUpdate(aIsOnline);
        }
    }

    /*
    Description: Coroutine function to connect to the leaderbaord server. This function also is in charge
    of update the isConnectign status of the online manager.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private IEnumerator ClientConnectToServerCoroutine(string aServerIP)
    {
        //Set that the user is currently connecting
        m_isConnecting = true;

        //If there is a online leaderboard client, 
        if (m_onlineLeaderboardClient != null)
        {
            //Connect the leaderboard client to the leaderboard server
            m_onlineLeaderboardClient.ConnectToServer(aServerIP, m_connectingToServerTimeOut);

            //Wait until the next frame, time just to ensure that the connection doesn't have any issue
            yield return null;
        }

        //Set that the user is no longer connecting, this doesn't mark if the connection was successful or not
        m_isConnecting = false;

        //Finish the coroutine
        yield return null;
    }

    /*
    Description: Try to connect the leaderboard client to the leaderboard server.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    */
    public void ConnectToLeaderboardServer()
    {
        //If the user is connecting to the server
        if (m_isConnecting == true)
        {
            //Display a message that he is connecting
            UpdateErrorMessageUI(CServerClientConstants.M_ERROR_CURRENTLY_CONNECTING_TO_SERVER);
        }
        //If the user is already connected to the server
        else if (GetIsConnectedToLeaderboardServer() == true)
        {
            //Display a message that he is already connected
            UpdateErrorMessageUI(CServerClientConstants.M_ERROR_ALREADY_CONNECTED);
        }
        else//If the user is not connecting to the server, or is already connected
        {
            //Get the server IP address to conect to
            string serverIPToConnect = string.Empty;

            //If there is a text field storing the IP Address
            if (m_textFieldServerIP != null)
            {
                //Use the IP stored in the text mesh
                serverIPToConnect = m_textFieldServerIP.text;
            }
            else//If there is no text field
            {
                //Use the string m_ServerIPToConnectTo that is set through the editor.
                serverIPToConnect = m_serverIPToConnectTo;
            }

            //Stat a coroutine to connect to the leaderboard client
            StartCoroutine(ClientConnectToServerCoroutine(serverIPToConnect));

            //If the connect event is valid
            if (OnConnectToServerRequest != null)
            {
                //Call the connection event
                OnConnectToServerRequest();
            }
        }
    }

    /*
    Description: Try to disconnect the leaderboard client from the leaderboard server.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    */
    public void DisconnectFromLeaderboardServer()
    {
        //If there is a leaderboard client
        if (m_onlineLeaderboardClient != null)
        {
            //Disconnect it from the server
            m_onlineLeaderboardClient.DisconnectFromServer();

            //Unsuscribe from the update message log event
            //m_onlineLeaderboardClient.OnEventLogUpdate -= UpdateMessageLog;

            //If the disconnect event is valid
            if (OnDisconnectToServerRequest != null)
            {
                //Call the disconnect event
                OnDisconnectToServerRequest();
            }
        }
    }

    /*
    Description: Send a player entry to be added to the online leaderboard
    Parameters: ELevelState aLeaderboardLevel - The leaderboard level the entry belongs to.
                SPlayerEntry aPlayerEntry - The entry that will be added to the leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public void SendEntryToLeaderboard(ELevelState aLeaderboardLevel, SPlayerEntry aPlayerEntry)
    {
        //If the leaderboard is connected to server
        if (GetIsConnectedToLeaderboardServer() == true)
        {
            //Send a write to leaderboard command to the server
            m_onlineLeaderboardClient.SendCommandWriteToLeaderboard(aLeaderboardLevel,
                aPlayerEntry);
        }
    }

    /*
    Description: Get the requested entries from the online leaderboard. This will try to get
    them instantly, but if the online manager doesn't receive them immediately it will create
    a leaderboard request. So the requested entries may be added to the leaderboard at a later
    time.
    Parameters: ref CLeaderboard aLeaderboard - The leaderboard where the requested entries will be added.
                ELevelState aLevel - The level, or identification, of the online leaderboard we are requesting data to.
                int aNumEntries - The number of entries being requested
                int aStartingIndex - The index from which the entries will start being searched.
                ELeaderboardSortingMethods aSortingMethod - The sorting method of the leaderboard. This is to ensure that if
                                                            the online leaderboard and the local leaderboard have a different 
                                                            sorting method, both will display the correct data.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public List<SPlayerEntry> GetOnlineLeaderboardEntries(ref CLeaderboard aLeaderboard,
        ELevelState aLevel, int aNumEntries, int aStartingIndex = 0,
        ELeaderboardSortingMethods aSortingMethod = ELeaderboardSortingMethods.HighestScore)
    {
        List<SPlayerEntry> entries = new List<SPlayerEntry>();

        //If the leaderboard client and the leaderboard being passed are valid
        if (m_onlineLeaderboardClient != null && aLeaderboard != null)
        {
            //If the client is connected to the server
            if (m_onlineLeaderboardClient.GetIsConnectedToServer() == true)
            {
                //Send a command to the server requesting him to send the requested leaderboard entries
                m_onlineLeaderboardClient.SendCommandSendLeaderboard(aLevel, aNumEntries, aStartingIndex, aSortingMethod);

                //Get the requested entries from the leaderboard client
                entries = m_onlineLeaderboardClient.GetLeaderboardEntries(aLevel);

                //If the entries haven't been received by the client.
                if (entries == null)
                {
                    //Make a leaderboard request
                    m_leaderboardRequests.Add(new CLeaderboardRequest(ref aLeaderboard));
                }
                else if (entries.Count <= 0) //If the entries haven't been received by the client.
                {
                    //Make a leaderboard request
                    m_leaderboardRequests.Add(new CLeaderboardRequest(ref aLeaderboard));
                }
                else //If the entries have been received by the client.
                {
                    //Add the entries to the leaderboard
                    aLeaderboard.AddToLeaderboard(entries);
                }
            }
        }

        //Return that no valid entries were found
        return null;
    }

    /*
    Description: Get if the leaderboardClient is connected to the server or not.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 11, 2017
    */
    public bool GetIsConnectedToLeaderboardServer()
    {
        //If there is a leaderboard client
        if (m_onlineLeaderboardClient != null)
        {
            //Return whether is connected to the server or not
            return m_onlineLeaderboardClient.GetIsConnectedToServer();
        }

        //Return the user is not connected
        return false;
    }
}
