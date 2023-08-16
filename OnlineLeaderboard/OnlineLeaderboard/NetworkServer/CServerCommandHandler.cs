using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace OnlineLeaderboard
{
    /*
    Description: The command handler that will process the commands received by the online server.
    This class supports the commands:OnCommandUserConencted 
                                     OnCommandUserDisconnected
                                     OnCommandWriteToLeaderboard
                                     OnCommandSendLeaderboard 
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 11, 2017
    */
    public class CServerCommandHandler : CCommandHandler
    {
        //Constants
        private const string M_MESSAGE_LEFT_SERVER = "Left server";
        private const string M_MESSAGE_JOINED_SERVER = "Joined server";

        //Variables
        private ListBox m_eventLog;
        private COnlineLeaderboardServer m_leaderboardServer;

        /*
        Description: Set the leaderboard server and the list box to be used by the handler.
        Parameters: ListBox aEventLog - The list box where the event logs will be shown.
                    COnlineLeaderboardServer aLeaderboardServer - The server linked to this handler
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 11, 2017
        */
        public CServerCommandHandler(ListBox aEventLog, COnlineLeaderboardServer aLeaderboardServer)
        {
            //Set all the variables
            m_leaderboardServer = aLeaderboardServer;
            m_eventLog = aEventLog;
        }

        /*
        Description: Function called when the command userConnected is called. This sends
        a message to the clientIP confirming he successfully connected.
        Parameters:  string aClientIP - The IP of the client that send  the command
                     byte[] aArguments - Not used in this function. 
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 11, 2017
        Extra Notes: This command doesn't handle actually accepting the client in the server,
        its only as a confirmation that the client has joined.
        */
        private void OnCommandUserConnected(string aClientIP, byte[] aArguments)
        {
            //If the client ip is not empty
            if (string.IsNullOrEmpty(aClientIP) == false)
            {
                //If there is a server
                if (m_leaderboardServer != null)
                {
                    //Inform the user he has connected
                    m_leaderboardServer.SendMessageToClient(aClientIP, M_MESSAGE_JOINED_SERVER
                        + CServerClientConstants.M_SPACE + m_leaderboardServer.PServerIPAddress);
                }
            }
        }

        /*
        Description: Function called when the command userDisconnected is called.Currently it 
        doesn't do anything, besides a way of registering in the event log (In RunCommand 
        function, not this one precisely) when the user has left.
        Parameters:  string aClientIP - The IP of the client that send  the command
                     byte[] aArguments - Not used in this function. 
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 11, 2017
        Extra Notes: This command doesn't handle actually disconnecting the client in the server.
        */
        private void OnCommandUserDisconnected(string aClientIP, byte[] aArguments)
        {
        }

        /*
        Description: Function called when the command WriteToLeaderboard is called. This
        function, deserializess the player entry, and the level it belongs to, and adds
        it to the corresponding online leaderboard.
        Parameters:  string aClientIP - The IP of the client that send  the command
                     byte[] aArguments - The byte array containing the serialized ELevelState of the 
                                         leaderboard where the data will be added, and the 
                                         SPlayer entry that will be added
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 18, 2017
        Extra Notes: This command doesn't handle actually disconnecting the client in the server.
        */
        private void OnCommandWriteToLeaderboard(string aClientIP, byte[] aArguments)
        {
            ELevelState leaderboardLevel = ELevelState.NoMotion;
            SPlayerEntry playerEntry = new SPlayerEntry();

            //Deserialize the level and the player entry
            CUtilityNetworkSerialization.DeserializeLevelAndPlayerEntry(aArguments,
                ref leaderboardLevel, ref playerEntry);

            //If the leaderboard server is valid
            if (m_leaderboardServer != null)
            {
                //Add the entry to the online leaderboard
                m_leaderboardServer.AddEntryToServerLeaderboard(leaderboardLevel, playerEntry);
            }
        }

        /*
        Description: Function called when the command SendLeaderboard is called. This
        function, deserializess the data needed to search for entries in a leaderboard,
        then use that data to get the respective entries, to finally send them back to the
        client as a SendLeaderboard command
        Parameters:  string aClientIP - The IP of the client that send  the command
                     byte[] aArguments - The byte array containing the serialized data for the leaderboard
                                        entries request (ELevelState - The level of the leaderboard, int - number
                                        of entries requested, int - the starting index from where to get the entries
                                        ELeaderboardSortingMethods - The sorting method that we want the online
                                        leaderboard to be sorted in).
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 18, 2017
        Extra Notes: This command doesn't handle actually disconnecting the client in the server.
        */
        private void OnCommandSendLeaderboard(string aClientIP, byte[] aArguments)
        {
            //If there is a leaderboard server
            if (m_leaderboardServer != null)
            {
                //Initiliaze empty variables
                ELevelState leaderboardLevel = ELevelState.NoMotion;
                int numLeaderboardEntries = 0;
                int leaderboardStartingIndex = 0;
                ELeaderboardSortingMethods leaderboardSortingMethod = ELeaderboardSortingMethods.HighestScore;

                //Deserialize the request data
                CUtilityNetworkSerialization.DeserializeLeaderboardEntriesRequest(aArguments, ref leaderboardLevel,
                    ref numLeaderboardEntries, ref leaderboardStartingIndex, ref leaderboardSortingMethod);

                //Send to the client whatever data he requested
                m_leaderboardServer.SendLeaderboardEntriesToClient(aClientIP,
                    leaderboardLevel, numLeaderboardEntries, leaderboardStartingIndex, leaderboardSortingMethod);
            }
        }

        /*
        Description: Display the desired message as an event in the event log
        Parameters:  string aMessage - The message that will be displayed in the event log 
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 11, 2017
        Extra Notes: This function uses delegates to support multithreading, since only the
        main thread can edit the list box.
        */
        protected override void UpdateEventLog(string aMessage)
        {
            //Ensure this functions can be called from different threads.
            if (m_leaderboardServer.InvokeRequired)
            {
                //Make a delegate
                delegateUpdateEventLog messageDeleg = new delegateUpdateEventLog(UpdateEventLog);

                //Use the delegate to call back the function from the main thread
                m_leaderboardServer.Invoke(messageDeleg, new Object[] { aMessage });
            }
            else//If this is the main thread
            {
                //Add message to list box
                m_eventLog.Items.Add(aMessage);
                m_eventLog.SelectedIndex = m_eventLog.Items.Count - 1;
            }
        }

        /*
        Description: Implementation of abstract CCommandHandler function Initialize commands. This adds the commands
        supported by the handler to the list of commands
        Creator: Alvaro Chavez Mixco
        Creation Date: Wednesday, January 11, 2017
        */
        protected override void InitializeCommands()
        {
            AddCommand(CCommandConstants.M_COMMAND_USER_CONNECTED, OnCommandUserConnected);//Command UserConnected
            AddCommand(CCommandConstants.M_COMMAND_USER_DISCONNECTED, OnCommandUserDisconnected);//Command UserDisconnected
            AddCommand(CCommandConstants.M_COMMAND_WRITE_TO_LEADERBOARD, OnCommandWriteToLeaderboard);//Command WriteToLeaderboard
            AddCommand(CCommandConstants.M_COMMAND_SEND_LEADERBOARD, OnCommandSendLeaderboard);//Comand SendToLeaderboard
        }
    }
}
