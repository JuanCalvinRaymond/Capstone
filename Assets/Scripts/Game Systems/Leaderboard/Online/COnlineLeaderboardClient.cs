using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

/*
Description: Class used to connect (send and receive) to a leaderboard server,
 using a TCP protocol
Creator: Alvaro Chavez Mixco
Creation Date: Friday, January 13, 2017
*/
public class COnlineLeaderboardClient
{
    //Constants
    private const string M_MESSAGE_STRING = "Message: ";
    private const string M_ONLINE_STATUS_STRING = "Connection Status: ";
    private const string M_ERROR_STRING = "Error: ";

    //Network stream
    private TcpClient m_clientSocket;
    private NetworkStream m_networkStream;

    private string m_serverIPAddress;

    //Listener thread
    private Thread m_listenerThread;
    private bool m_listenerThreadAlive;

    //Functionality
    private CClientCommandHandler m_commandHandler;
    private Dictionary<ELevelState, List<SPlayerEntry>> m_onlineLeaderboardEntries; //This are temporary
                                                                                    //copies of the online leaderboards. This are
                                                                                    //filled according to what is send by the server.

    private List<SNetworkClientEvents> m_listEvents;

    //Events
    //public delegate void delegateMessageLogUpdate(string aMessage);
    //public event delegateMessageLogUpdate OnEventLogUpdate;
    //public event delegateMessageLogUpdate OnMessageLogUpdate;
    //public event delegateMessageLogUpdate OnErrorMessageLogUpdate;

    //public delegate void delegateConnectedStatusChange(bool aStatus);
    //public event delegateConnectedStatusChange OnOnlineStatusDisplayChange;//Note that this event is based on 
    //commands sends and doesn't check the actual socket. 

    public string PServerIPAddress
    {
        get
        {
            return m_serverIPAddress;
        }
    }

    public bool PListenerThreadAlive
    {
        get
        {
            return m_listenerThreadAlive;
        }
    }

    public List<SNetworkClientEvents> PClientNetworkEvents
    {
        get
        {
            return m_listEvents;
        }
    }

    /*
    Description: Save the server IP adress and initiliaze the variables
    Parameters: string aServerIP - The IP Address of the server we want to connect to
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesay, January 11, 2017
    */
    public COnlineLeaderboardClient()
    {
        //Variables starting values
        m_clientSocket = null;
        m_networkStream = null;
        m_listenerThread = null;
        m_listenerThreadAlive = false;

        m_commandHandler = new CClientCommandHandler(this);
        m_onlineLeaderboardEntries = new Dictionary<ELevelState, List<SPlayerEntry>>();
        m_listEvents = new List<SNetworkClientEvents>();
    }

    /*
    Description: Receive messages from the server and send them to the command handler
    Parameters: Socket aConnection - The socket where the connection is made
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private void ReceiveMessages(Socket aConnection)
    {
        //If the client is connected to the server
        if (GetIsConnectedToServer() == true)
        {
            //Cast the command handler to its base class so that it can be used in function
            CCommandHandler clientCommandHandler = (CCommandHandler)m_commandHandler;

            //Loop to check for messages while the thread and the client socket are valid
            while (m_listenerThreadAlive == true && m_clientSocket != null)
            {
                //Receive  and process commands. This function (ReceiveNetworkCommaands) will end once it gets the disconnect command
                //, but the ReceiveMessages won't end until the m_listenerThreadAlive is set to false
                CUtilityNetworking.ReceiveNetworkCommands(ref m_networkStream, ref clientCommandHandler,
                    m_serverIPAddress, m_clientSocket.Client);
            }

            AddEventToList(new SNetworkClientEvents(ENetworkEventTypes.ConnectionStatus, false));
        }
    }


    /*
    Description: Function to handle socketExceptions. This would normally just output
    a message containing the exception rather than actually fixing the exception.
    Parameters :SocketException aException- The socket exception encountered
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    */
    private void HandleSocketException(SocketException aException)
    {
        //According to the exception found
        switch (aException.SocketErrorCode)
        {
            //If the exception is connection refused
            case SocketError.ConnectionRefused:
                //Output a clearer message, rather than the exception message
                UpdateErrorMessageDisplay(CServerClientConstants.M_EXCEPTION_CONNECTION_REFUSED);
                break;
            default:
                //For all the other exceptinos, display the exception message
                UpdateErrorMessageDisplay(aException.Message);
                break;
        }
    }

    private void AddEventToList(SNetworkClientEvents aEvent)
    {
        if (m_listEvents != null)
        {
            m_listEvents.Add(aEvent);
        }
    }

    public void ClearEventList()
    {
        if (m_listEvents != null)
        {
            m_listEvents.Clear();
        }
    }

    /*
    Description: Function to displays events on the debug log . This funtion has no actual
    functionality, but it merely calls an event that other can suscribe to.
    Parameters :string aMessage - Event to be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 11, 2017
    Extra Notes: This function merely calls an event, and doesn't output anything. This is to
    ensure that the pure c# networking can be separate from Unity code (debug log)
    */
    public void UpdateEventLog(string aMessage)
    {
        //If the message is valid
        //if (OnEventLogUpdate != null && string.IsNullOrEmpty(aMessage) == false)
        //{
        //    //Call event that the event log has been updated
        //    OnEventLogUpdate(aMessage);
        //}

        AddEventToList(new SNetworkClientEvents(ENetworkEventTypes.Log, aMessage));
    }

    /*
    Description: Function to call an event when the messageLog has been updated.
    Parameters :string aMessage - Message to be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function merely calls an event, and doesn't output anything. This is to
    ensure that the pure c# networking can be separate from Unity code (debug log, display)
    */
    public void UpdateMessageLog(string aMessage)
    {
        //If the message is valid
        //if (OnMessageLogUpdate != null && string.IsNullOrEmpty(aMessage) == false)
        //{
        //    //Call event that the message log has been updated
        //    OnMessageLogUpdate(M_MESSAGE_STRING + aMessage);
        //}

        AddEventToList(new SNetworkClientEvents(ENetworkEventTypes.Message, aMessage));

        //Also update the event log
        UpdateEventLog(M_MESSAGE_STRING + aMessage);
    }

    /*
    Description: Function to call an event when the error message log has been updated.
    Parameters :string aMessage - Error to be displayed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function merely calls an event, and doesn't output anything. This is to
    ensure that the pure c# networking can be separate from Unity code (debug log, display)
    */
    public void UpdateErrorMessageDisplay(string aMessage)
    {
        //If the message is valid
        //if (OnErrorMessageLogUpdate != null && string.IsNullOrEmpty(aMessage) == false)
        //{
        //    //Call event that the message log has been updated
        //    OnErrorMessageLogUpdate(M_ERROR_STRING + aMessage);
        //}

        AddEventToList(new SNetworkClientEvents(ENetworkEventTypes.Error, aMessage));

        //Also update the event log
        UpdateEventLog(M_ERROR_STRING + aMessage);
    }

    /*
    Description: Function to call an event when the error online status display has been updated.
    Parameters :bool aStatus - Whether the server is online or not
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    Extra Notes: This function merely calls an event, and doesn't output anything. This is to
    ensure that the pure c# networking can be separate from Unity code (debug log, display)
    */
    public void UpdateOnlineStatusDisplay(bool aStatus)
    {
        //If the event has suscribes
        //if (OnOnlineStatusDisplayChange != null)
        //{
        //    //Call the event
        //    OnOnlineStatusDisplayChange(aStatus);
        //}

        AddEventToList(new SNetworkClientEvents(ENetworkEventTypes.ConnectionStatus, aStatus));

        //If the status is online
        if (aStatus == true)
        {
            //Call event log
            UpdateEventLog(M_ONLINE_STATUS_STRING + CServerClientConstants.M_LABEL_ONLINE_TEXT);
        }
        else//If the status is offline
        {
            //Call event log
            UpdateEventLog(M_ONLINE_STATUS_STRING + CServerClientConstants.M_LABEL_OFFLINE_TEXT);
        }
    }

    /*
    Description: Connect to the leaderboard server using a TCP protocol
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesay, January 11, 2017
    */
    public void ConnectToServer(string aServerIPAddress, float aConnectWaitTime)
    {
        //Save the IP Address
        m_serverIPAddress = aServerIPAddress;

        //Check that the server IP is not empty text, and that there isn't already a connection
        if (!string.IsNullOrEmpty(m_serverIPAddress) && m_clientSocket == null)
        {
            //Try to conect to the server
            try
            {
                //DEBUGLIST-AAA
                //Create a connection to the server SYNC
                //m_clientSocket= new TcpClient(m_serverIPAddress, CServerClientConstants.M_SERVER_PORT_NUMBER);

                ////Create a connection to the server
                m_clientSocket = new TcpClient();

                //Try to connect async to the server
                IAsyncResult resultConnection = m_clientSocket.BeginConnect(m_serverIPAddress, CServerClientConstants.M_SERVER_PORT_NUMBER, null, null);
                WaitHandle waitHandle = resultConnection.AsyncWaitHandle;

                try
                {
                    //If in a certain amount of time a connection wasn't made
                    if (resultConnection.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(aConnectWaitTime), false) == false)
                    {
                        //Close the socket
                        m_clientSocket.Close();

                        //Throw a timeout
                        throw new TimeoutException();
                    }

                    //End the async connection attempt
                    m_clientSocket.EndConnect(resultConnection);
                }

                //If there is a connection timeout exception
                catch (TimeoutException)
                {
                    //Display an error message
                    UpdateErrorMessageDisplay(CServerClientConstants.M_EXCEPTION_CONNECTION_TIMEOUT);
                    m_clientSocket = null;

                    //Exit function
                    return;
                }

                //If the try code was executed
                finally
                {
                    //Close the wait handle
                    waitHandle.Close();
                }

            }

            //If there is an error, display it
            catch (SocketException aException)
            {
                //Handle exceptions
                HandleSocketException(aException);
                m_clientSocket = null;

                //Exit function
                return;
            }



            //If the client socket is valid
            if (m_clientSocket != null)
            {
                //If a connection was successfully made
                if (m_clientSocket.Connected == true)
                {
                    //Parameters for it to receive/send message without delay
                    m_clientSocket.NoDelay = true;
                    m_clientSocket.Client.NoDelay = true;

                    //Get the network stream
                    m_networkStream = m_clientSocket.GetStream();

                    //Start the thread to receive messages
                    Socket connection = m_clientSocket.Client;
                    m_listenerThread = new Thread(() => ReceiveMessages(connection));
                    m_listenerThread.Start();
                    m_listenerThreadAlive = true;

                    //Send the command that the user has connected to the server
                    SendCommandUserConnected();

                    //Update event log saying it connected to the server
                    UpdateOnlineStatusDisplay(true);
                }
            }
            else//If the client socket is not valid
            {
                //Display error message
                UpdateErrorMessageDisplay(CServerClientConstants.M_ERROR_INVALID_INTERNET_SOCKET);
            }
        }
    }

    /*
    Description: Disconnect the client from the server
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesay, January 11, 2017
    Extra Notes: "DisconnectFromServer" works as a wrapper for DisconnectClient. Since DisconnectFromServer
    ensures a message is send to the server informing it the client is disconnecting, and it
    also updates the message list.
    */
    public void DisconnectClient()
    {
        //Close the listener thread
        m_listenerThreadAlive = false;
        if (m_listenerThread != null)
        {
            m_listenerThread.Abort();
            m_listenerThread = null;
        }

        //Close the connection
        m_clientSocket = null;
        if (m_networkStream != null)
        {
            m_networkStream.Close();
            m_networkStream = null;
        }

        //Update label to display client is no longer online
        UpdateOnlineStatusDisplay(false);
    }

    /*
    Description: Disconnect the client from the server. This first notifies the server the user is disconnecting, then
    disconnects, and finally it updates the label status
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 12, 2017
    Extra Notes: "DisconnectFromServer" works as a wrapper for DisconnectClient. Since DisconnectFromServer
    ensures a message is send to the server informing it the client is disconnecting, and it
    also updates the message list.
    */
    public void DisconnectFromServer()
    {
        //Notify the server this client has disconnected
        SendCommandUserDisconnected();

        //Disconnect the client from the server
        DisconnectClient();
    }

    /*
    Description: Sends the command UserConnected to the server.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public void SendCommandUserConnected()
    {
        //If it is connected to the server
        if (GetIsConnectedToServer() == true)
        {
            //Send the command to the server, with an empty array of bytes.
            CUtilityNetworking.SendNetworkCommand(ref m_networkStream, CCommandConstants.M_COMMAND_USER_CONNECTED,
            new byte[CServerClientConstants.M_ARGUMENTS_PACKET_SIZE]);
        }
    }

    /*
    Description: Sends the command UserDisconnected to the server.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public void SendCommandUserDisconnected()
    {
        //If it is connected to the server
        if (GetIsConnectedToServer() == true)
        {
            //Send the command to the server, with an empty array of bytes.
            CUtilityNetworking.SendNetworkCommand(ref m_networkStream, CCommandConstants.M_COMMAND_USER_DISCONNECTED,
            new byte[CServerClientConstants.M_ARGUMENTS_PACKET_SIZE]);
        }
    }

    /*
    Description: Sends the command WriteToLeaderboard to the server.
    Parameters: ELevelState aLeaderboardLevel - The level in which the leaderboard score was obtained.
                SPlayerEntry aPlayerEntry - The player entry that will be added to the leaderboard
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public void SendCommandWriteToLeaderboard(ELevelState aLeaderboardLevel, SPlayerEntry aPlayerEntry)
    {
        //If it is connected to the server
        if (GetIsConnectedToServer() == true)
        {
            //Serialize the leaderboard level and the player entry into a byte array
            byte[] arguments = CUtilityNetworkSerialization.SerializeLevelAndPlayerEntry(aLeaderboardLevel, aPlayerEntry);

            //Send the command to the server
            CUtilityNetworking.SendNetworkCommand(ref m_networkStream,
                CCommandConstants.M_COMMAND_WRITE_TO_LEADERBOARD, arguments);
        }
    }

    /*
    Description: Sends the command SendLeaderboard to the server.
    Parameters: ELevelState aLeaderboardLevel - The level  of the leaderboard we want the server to send
                int aNumEntries - The number of entries we want the server to send
                int aStartingIndex - The starting index of the entry we requested from the leaderboard
                ELeaderboardSortingMethods aSortingMethod - The sorting method currently being used,
                                                            according to the index, and that the
                                                            online leaderboard will be sorted when
                                                            getting the entries/index.                                         
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    Extra Notes: This will make the leaderboard, "reply" back with its own SendLeaderboard command.
    That will contain the actual leaderboard entries data.
    */
    public void SendCommandSendLeaderboard(ELevelState aLeaderboardLevel, int aNumEntries,
        int aStartingIndex = 0, ELeaderboardSortingMethods aSortingMethod = ELeaderboardSortingMethods.HighestScore)
    {
        //If it is connected to the server
        if (GetIsConnectedToServer() == true)
        {
            //Serialize all the arguments into a byte array
            byte[] arguments = CUtilityNetworkSerialization.SerializeLeaderboardEntriesRequest(aLeaderboardLevel, aNumEntries,
            aStartingIndex, aSortingMethod);

            //Send the SendLeaderboard command to the server
            CUtilityNetworking.SendNetworkCommand(ref m_networkStream,
                CCommandConstants.M_COMMAND_SEND_LEADERBOARD, arguments);
        }
    }

    /*
    Description: Function to know if the client is connected to the server
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 11, 2017
    */
    public bool GetIsConnectedToServer()
    {
        //If the client socket is not valid 
        if (m_clientSocket == null)
        {
            //Return that there is no connection
            return false;
        }

        //Get if the socket is connected and poll to check if the connection is valid
        return CUtilityNetworking.GetIsConnected(m_clientSocket.Client, m_networkStream);
    }

    /*
    Description: Add a list of entries to the "online leaderboard" copies currently stored in
    the client, basically filling them up.
    Parameters: ELeaderboard aLevel - The level of the leaderboard where entries will be added.     
                List<SPlayerEntry> aListOfEntries - The entries that will be added to the leaderbaord
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    Extra Notes:This method only supports one set of entries per level at a time. Unless another
    client is created. 
    */
    public void AddOnlineLeaderboardEntries(ELevelState aLevel, List<SPlayerEntry> aListOfEntries)
    {
        //If the list is valid
        if (aListOfEntries != null)
        {
            //Add the list of entries to the leaderboard
            m_onlineLeaderboardEntries.Add(aLevel, aListOfEntries);
        }
    }

    /*
    Description: Get the entries from a leaderboard level stored in the client copies of the online
    leaderboard.
    Parameters: ELeaderboard aLevel - The level of the leaderboard that is storing the entries   
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    Extra Notes:This method only supports one set of entries per level at a time. Unless another
    client is created. 
    */
    public List<SPlayerEntry> GetLeaderboardEntries(ELevelState aLevel)
    {
        //Create an empty list
        List<SPlayerEntry> entriesFound = new List<SPlayerEntry>();

        //If the dictionary of entries is valid
        if (m_onlineLeaderboardEntries != null)
        {
            //If the entries were sucessfully found and copied
            if (m_onlineLeaderboardEntries.TryGetValue(aLevel, out entriesFound) == true)
            {
                //Remove them from the dictionary
                m_onlineLeaderboardEntries.Remove(aLevel);
            }
        }

        //Return the entries that were found
        return entriesFound;
    }
}