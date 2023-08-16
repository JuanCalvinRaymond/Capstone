using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace OnlineLeaderboard
{
    /*
    Description: Class used as a server for the online leaderboard. This will read clients messages, and
    send them, as commands, to a command handler. This uses the TCP protocol.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 13, 2017
    */
    public partial class COnlineLeaderboardServer : Form
    {
        //Constants
        private const string M_SERVER_LEADERBOARD_FILE_PATH = "";

        private const string M_BUTTON_TEXT_START = "Start";
        private const string M_BUTTON_TEXT_STOP = "Stop";

        //Lists of clients
        private Dictionary<string, Thread> m_clientsConnected;
        private Dictionary<string, NetworkStream> m_clientsNetworkStreams;

        //Networking
        private Thread m_listenerThread;
        private bool m_listenerThreadAlive;
        private TcpListener m_listenerTCP;

        private string m_serverIPAddress;

        private Object m_leaderboardLock = new Object();//Lock used to prevent multiple objects from modifying the leaderboard at once

        //Functionality variables
        private CServerCommandHandler m_commandHandler;
        private Dictionary<ELevelState, CLeaderboard> m_leaderboards;

        //Delegates for multithreading support
        private delegate void delegateDisconnectServer();

        public string PServerIPAddress
        {
            get
            {
                return m_serverIPAddress;
            }
        }

        /*
        Description: Initialize all the variables.
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednedsay, January 11, 2017
        */
        public COnlineLeaderboardServer()
        {
            InitializeComponent();

            //Starting values for variables
            m_clientsConnected = new Dictionary<string, Thread>();
            m_clientsNetworkStreams = new Dictionary<string, NetworkStream>();

            m_listenerThread = null;
            m_listenerThreadAlive = false;
            m_listenerTCP = null;

            //Create the command handler
            m_commandHandler = new CServerCommandHandler(ListBox_ServerEvents, this);

            //Set the stat button to begin with "Start" text
            SetButtonText(ref Button_Start, M_BUTTON_TEXT_START);

            //Load all the leaderboards
            LoadLeaderboards();
        }

        /*
        Description: Function called when the button start is clicked. This function
        will start up the server. But if the server is already online, this function
        will serve as a stop server function.
        Parameters :  object aSender - Unused
                      EventArgs aEventArguments - Unused
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 12, 2017
        */
        private void Button_Start_Click(object aSender, EventArgs aEventArguments)
        {
            //Check that the server isn't already online
            if (m_listenerThreadAlive == false)
            {
                //Start the thread to listen for new connections
                //Function creates thread listening on desired port
                m_listenerThread = new Thread(ListenConnections);

                //Start the thread
                m_listenerThread.Start();

                //Wait until the thread has started
                while (m_listenerThread.IsAlive == false) ;

                //Set that the thread is alive
                m_listenerThreadAlive = true;

                //Update form labels, messages
                SetLabelText(ref Label_ServerStatus, CServerClientConstants.M_LABEL_ONLINE_TEXT);
                SetButtonText(ref Button_Start, M_BUTTON_TEXT_STOP);

                //Save the server IP address and display it
                m_serverIPAddress = CUtilityNetworking.GetMachineIPAddress();
                SetLabelText(ref Label_ServerIP, m_serverIPAddress);
            }
            else//If the server is already online
            {
                //Stop the server
                ShutDownServer();

                //Change the stop button back to start
                SetButtonText(ref Button_Start, M_BUTTON_TEXT_START);

                //Clear the list box of events
                //If the list box is valid
                if (ListBox_ServerEvents != null)
                {
                    //Clear it
                    ListBox_ServerEvents.Items.Clear();
                }
            }
        }

        /*
        Description: Function called when the form of the sever is closing down. This will
        call the ShutDownServer function in order to close every connection.
        Parameters :  object aSender - Unused
                      FormClosingEventArgs aEventArguments - Unused
        Creator: Alvaro Chavez Mixco
        Creation Date: Friday, January 13, 2017
        */
        private void OnlineLeaderboardServer_FormClosing(object aSender, FormClosingEventArgs aEventArguments)
        {
            ShutDownServer();
        }

        /*
        Description: Function called when the button exit is clicked. This function
        will close the form.
        Parameters :  object aSender - Unused
                      EventArgs aEventArguments - Unused
        Creator: Alvaro Chavez Mixco
        Creation Date: Tuesday, January 10, 2017
        Extra Notes: By closing the form, the function "OnlineLeaderboardServer_FormClosing" will be called.
        This function will handle shutting down the server and closing all connection 
        */
        private void Button_Exit_Click(object aSender, EventArgs aEventArguments)
        {
            //If this function didn't call form closing we would have to shut down the server manually
            this.Close();
        }

        /*
        Description: Function used to listen and accept new client connections.
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 11, 2017
        */
        private void ListenConnections()
        {
            //If there is no listener (ensures that there is only 1 listener)
            if (m_listenerTCP == null)
            {
                //Create listener for desired port
                m_listenerTCP = new TcpListener(IPAddress.Any, CServerClientConstants.M_SERVER_PORT_NUMBER);
                m_listenerTCP.Start();

                //Check for incoming connections
                while (m_listenerThreadAlive == true && m_listenerTCP != null)
                {
                    //Accept incoming connections
                    Socket connection = m_listenerTCP.AcceptSocket();

                    //If a user has connected
                    if (connection.Connected == true)
                    {
                        //Start thread to receive message for client
                        //Create the thread to read messages
                        Thread process = new Thread(() => ReceiveMessages(connection));

                        //Start the thread
                        process.Start();

                        //Wait until the thread has started
                        while (process.IsAlive == false) ;

                        //Save IP address of client to dictionary  of connection threads
                        string clientIP = (connection.RemoteEndPoint as IPEndPoint).Address.ToString();
                        m_clientsConnected[clientIP] = process;
                    }
                }
            }
        }

        /*
        Description: Function used to read messages from an internet socket. Once the message
        is read, if it matches the encryption key, it will be passed to the commandHandler
        so that a command can be run.
        Parameters: Socket aConnection - The internet socket connected to the client 
                                            that will be used to read messages. 
        Creator: Alvaro Chavez Mixco
        Creation Date: Friday, January 13, 2017
        */
        private void ReceiveMessages(Socket aConnection)
        {
            //Get the newtwork stream
            NetworkStream networkStream = new NetworkStream(aConnection);

            //Save IP address of client to dictionary  of netowrk streams
            string clientIP = (aConnection.RemoteEndPoint as IPEndPoint).Address.ToString();
            m_clientsNetworkStreams[clientIP] = networkStream;

            //Cast the server command handler to the base class so that it can be used in the
            //receive network commands function
            CCommandHandler serverCommandHandler = (CCommandHandler)m_commandHandler;

            //Receive and process network commands until the disconenct message is found
            CUtilityNetworking.ReceiveNetworkCommands(ref networkStream,
                ref serverCommandHandler, clientIP, aConnection);

            //Remove the client from the dictionaries
            m_clientsConnected.Remove(clientIP);
            m_clientsNetworkStreams.Remove(clientIP);
        }

        /*
        Description: Function used to close down all the clients connection and cloe the server
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 12, 2017
        Extra Notes: ShutDownServer function works similar to a wrapper to DisconnectServer. Since ShutDownServer
        will ensure to notify all clients of the closure, and update any required display item.
        */
        private void DisconnectServer()
        {
            //Ensure this functions can be called from different threads , since we are modifying a label
            if (this.InvokeRequired)
            {
                Delegate disconnectDeleg = new delegateDisconnectServer(DisconnectServer);

                this.Invoke(disconnectDeleg, new Object[] { });
            }
            else
            {
                //Stop the TCP Listener
                if (m_listenerTCP != null)
                {
                    m_listenerTCP.Stop();
                    m_listenerTCP = null;
                }

                //Close the threads
                //Kill the listener thread
                m_listenerThreadAlive = false;
                if (m_listenerThread != null)
                {
                    m_listenerThread.Abort();
                    m_listenerThread = null;
                }

                //Go through all the clients receive message thread
                foreach (KeyValuePair<string, Thread> entry in m_clientsConnected)
                {
                    //If the client is valid
                    if (entry.Value != null)
                    {
                        //If the client thread is valid
                        if (entry.Value.IsAlive)
                        {
                            //Close it
                            entry.Value.Abort();
                        }
                    }
                }
            }
        }

        /*
        Description: This function will notify all the clients that the server is closing, then close all the 
        connections to the clients. Finally, it will update any pertinet label, etc., to display the offfline 
        status of the server
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 12, 2017
        Extra Notes: ShutDownServer function works similar to a wrapper to DisconnectServer. Since ShutDownServer
        will ensure to notify all clients of the closure, and update any required display item.
        */
        private void ShutDownServer()
        {
            //Tell all clients the server has disconnected
            BroadcastCommandToClient(CCommandConstants.M_COMMAND_USER_DISCONNECTED,
                new byte[CServerClientConstants.M_ARGUMENTS_PACKET_SIZE]);

            //End all the threads
            DisconnectServer();

            //Update label
            SetLabelText(ref Label_ServerStatus, CServerClientConstants.M_LABEL_OFFLINE_TEXT);

            //Clear both dictionaries
            m_clientsConnected.Clear();
            m_clientsNetworkStreams.Clear();
        }

        /*
        Description: This function will read/open the online leaderboard for all the levels.
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 18, 2017
        Extra Notes:Since we don't really care about memory usage in the server (since it would ideally be on a computer
        exclusive for it), but rather about doing the request features as fast as possiblE to reduce waiting
        online time  for the client. All the possible leaderboards will be opened at start, to have easier and
        more importantly faster access to them
        */
        private void LoadLeaderboards()
        {
            //Create the dictionary to easily access the values
            m_leaderboards = new Dictionary<ELevelState, CLeaderboard>();

            //Go through all the possible level states and load the corresponding server for it
            for (int i = 0; i < (int)ELevelState.LevelsCount; i++)
            {
                //Create the requested leaderboard
                CLeaderboard tempLeaderboard = new CLeaderboard((ELevelState)i);

                //Read the leaderboard
                tempLeaderboard.ReadLeaderboard(M_SERVER_LEADERBOARD_FILE_PATH);

                //Add it to the dictionary
                m_leaderboards.Add((ELevelState)i, tempLeaderboard);
            }
        }

        /*
        Description: This function will return a leaderboard according to the requested level.
        Parameters: ELevelState aLevel - The level of the leaderboard we want to get
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 19, 2017
        */
        private CLeaderboard GetLeaderboard(ELevelState aLevel)
        {
            CLeaderboard leaderboardFound = null;

            //Search the dictionary of leaderboard to check if there is one that matches the
            //requested level and assign it to the leaderboardFound varaible
            m_leaderboards.TryGetValue(aLevel, out leaderboardFound);

            return leaderboardFound;
        }


        /*
        Description: Helper function to set the text in a Label.
        Parameters: ref Label aLabel - The label that will be modified.
                    string aText  - The text that will be displayed in the label.
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 12, 2017
        */
        private void SetLabelText(ref Label aLabel, string aText)
        {
            //If the label is valid
            if (aLabel != null)
            {
                //Change its text
                aLabel.Text = aText;
            }
        }

        /*
        Description: Helper function to set the text in a button.
        Parameters: ref Button aButton - The button that will be modified.
                    string aText  - The text that will be displayed in the button.
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 12, 2017
        */
        private void SetButtonText(ref Button aButton, string aText)
        {
            //If the button is valid
            if (aButton != null)
            {
                //Change its text
                aButton.Text = aText;
            }
        }

        /*
        Description: Add an entry to an online leaderboard
        Parameters: ELevelState aLevel - The level that will be used to identify to which leaderboard
                                        to add the entry to.
                    SPlayerEntry aEntry - The entry that will be added to the leaderboard.
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 19, 2017
        Extra Notes: This function has a lock to ensure that there aren't 2 threads modifying
        the leaderboards at the same time.
        */
        public void AddEntryToServerLeaderboard(ELevelState aLevel, SPlayerEntry aEntry)
        {
            //Search for the corresponding leaderboard using the lvel
            CLeaderboard levelLeaderboard = GetLeaderboard(aLevel);

            //If there is a leaderboard for that level, store it in the local variable levelLeaderboard
            if (levelLeaderboard != null)
            {
                //Lock this block of code, to ensure only one thread can access this block of code as a time
                lock (m_leaderboardLock)
                {
                    //Add the entry to the leaderboard
                    levelLeaderboard.AddToLeaderboard(aEntry);

                    //Write the new online leaderboard to a file
                    levelLeaderboard.WriteToLeaderboard(M_SERVER_LEADERBOARD_FILE_PATH);
                }
            }
        }

        /*
        Description: Function used to send a message to the desired client
        Parameters: string aClientIP - The IP Address of the client that will be send a message to
                    strign aMessage - The message that will be send to the client
        Creator: Alvaro Chavez Mixco
        Creation Date: Friday, January 13, 2017
        */
        public void SendMessageToClient(string aClientIP, string aMessage)
        {
            //Get client network stream
            NetworkStream clientStream = GetClientNetworkStream(aClientIP);

            //Serialize the string to a byte array
            byte[] message = CUtilityNetworkSerialization.SerializeString(aMessage);

            //Send message command to client
            CUtilityNetworking.SendNetworkCommand(ref clientStream, CCommandConstants.M_COMMAND_MESSAGE,
                message);
        }

        /*
        Description: Search the entries for a leaderboard, and if they are found send them to the corresponding
        client IP.
        Parameters: string aClientIP - The IP requesting the leaderboard entries
                    LevelState aLeaderboardLevel - The level of the leaderboard being searched
                    int aNumLeaderboardEntries - The number of entries the client has requested
                    int aLeaderboardStartingIndex - The starting index from where the ntries should be searched
                    ELeaderboardSortingMethods aLeaderboardSortingMethod - How the leaderboard should be sorted when searching
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 19, 2017
        Extra Notes: This function has a lock to ensure that there aren't 2 threads modifying
        the leaderboards at the same time.
        */
        public void SendLeaderboardEntriesToClient(string aClientIP, ELevelState aLeaderboardLevel,
            int aNumLeaderboardEntries, int aLeaderboardStartingIndex = 0,
                ELeaderboardSortingMethods aLeaderboardSortingMethod = ELeaderboardSortingMethods.HighestScore)
        {
            //Get the leaderboard
            CLeaderboard requestedLeaderboard = GetLeaderboard(aLeaderboardLevel);
            List<SPlayerEntry> entriesToSend = new List<SPlayerEntry>();

            //If the requested leaderboard was found
            if (requestedLeaderboard != null)
            {
                //If the leaderboard is already sorted as we desire (this is supposing that after someone sets, the 
                //sorting method, for the leaderboard he actually sorts)
                if (requestedLeaderboard.PLeaderboardSortingMethod == aLeaderboardSortingMethod)
                {
                    //Lock this block of code, to ensure only one thread can access this block of code as a time
                    lock (m_leaderboardLock)
                    {
                        //Get from the current leaderboard the entries
                        entriesToSend = requestedLeaderboard.GetLeaderboardEntries(aNumLeaderboardEntries, aLeaderboardStartingIndex);
                    }
                }
                else//If the server leaderboard is not sorted as we desire
                {
                    CLeaderboard copiedLeaderboard = null;

                    //Lock this block of code, to ensure only one thread can access this block of code as a time
                    lock (m_leaderboardLock)
                    {
                        //POSSIBLE OPTIMIZATION - Make a deep copy of the leaderboard so that we can sort it as requested, and get
                        //the specified entries, without modifying the actual server leaderboards (since that would affect all clients).
                        copiedLeaderboard = (CLeaderboard)requestedLeaderboard.Clone();
                    }

                    //Set the sorting method in the copied leaderboard
                    copiedLeaderboard.PLeaderboardSortingMethod = aLeaderboardSortingMethod;

                    //Sort the leaderboard as desired
                    copiedLeaderboard.SortLeaderboard();

                    //Get the desired leaderboard entries from the leaderboard
                    entriesToSend = copiedLeaderboard.GetLeaderboardEntries(aNumLeaderboardEntries, aLeaderboardStartingIndex);
                }
            }

            //Ensure the list is the exact size requested by the client. The GetLeaderboardEntries can give less than the desired
            //values (if there are not enough entries), but can't not give more. So only check if the requested amount is smalled
            //than the size of the leaderboard

            //While the list of entries to send is smaller than the requested number of entries
            while (entriesToSend.Count < aNumLeaderboardEntries)
            {
                //Add an empty entry
                entriesToSend.Add(new SPlayerEntry());
            }

            //Selialize the data
            byte[] leaderboardData = CUtilityNetworkSerialization.SerializeListPlayerEntries(
                aLeaderboardLevel, aNumLeaderboardEntries, entriesToSend);

            //Get the network stream according to the client IP
            NetworkStream clientConnection = GetClientNetworkStream(aClientIP);

            //Send to the client the leaderboard data
            CUtilityNetworking.SendNetworkCommand(ref clientConnection,
                CCommandConstants.M_COMMAND_SEND_LEADERBOARD, leaderboardData);
        }

        /*
        Description: Send the requested command to all the connected clients
        Parameters: string aCommand - The name of the command to send
                    byte[] aArguments - The arguments of the command being send
        Creator: Alvaro Chavez Mixco
        Creation Date: Thursday, January 19, 2017
        Extra Notes: This function has a lock to ensure that there aren't 2 threads modifying
        the leaderboards at the same time.
        */
        public void BroadcastCommandToClient(string aCommand, byte[] aArguments)
        {
            NetworkStream clientStream = null;

            //Notify all clients the server closed
            //Go through each of the clients
            foreach (KeyValuePair<string, NetworkStream> entry in m_clientsNetworkStreams)
            {
                //Get the entry network stream
                clientStream = entry.Value;

                //Send the command to the client
                CUtilityNetworking.SendNetworkCommand(ref clientStream, aCommand, aArguments);
            }
        }

        /*
       Description: Function to get the network stream linked to a IP Addres.
       Parameters: string aClientIP- The IP address used to search for the network stream
       Creator: Alvaro Chavez Mixco
       Creation Date: Thursday, January 19, 2017
       */
        public NetworkStream GetClientNetworkStream(string aClientIP)
        {
            NetworkStream clientStream = null;

            //Check that the client IP is valid (stored in both dictionaries of the server)
            if (m_clientsConnected.ContainsKey(aClientIP) && m_clientsNetworkStreams.ContainsKey(aClientIP))
            {
                //Get client network stream
                clientStream = m_clientsNetworkStreams[aClientIP];
            }

            //Return the client stream
            return clientStream;
        }
    }
}