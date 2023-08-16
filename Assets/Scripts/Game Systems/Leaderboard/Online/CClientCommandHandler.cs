using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
Description: The command handler that will process the commands received by the online client.
This class supports the commands:OnCommandUserConencted 
                                 OnCommandUserDisconnected
                                 OnCommandMessage
                                 OnCommandSendLeaderboard 
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, January 17, 2017
*/
public class CClientCommandHandler : CCommandHandler
{
    //The online leaderbaord client
    private COnlineLeaderboardClient m_leaderboardClient;

    /*
    Description:Constructor for CClientCommandHandler, this merely sets the online leaderbaord client.
    Parameters: COnlineLeaderboardClient aLeaderboardClient - The online leaderboard client linked to 
                                                              this command handler.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 17, 2017
    */
    public CClientCommandHandler(COnlineLeaderboardClient aLeaderboardClient)
    {
        //Set the leaderboard client
        m_leaderboardClient = aLeaderboardClient;
    }

    /*
    Description: Function called when the command userConnected is called. This merely updates the online display
    Parameters: string aServerIP - The ip of the machine sending the command
                byte[] aArguments - The arguments for the command. In this case the arguments are not used and are
                                    therefore ignored.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 17, 2017
    */
    private void OnCommandUserConnected(string aServerIP, byte[] aArguments)
    {
        //If the leaderboard client is valid
        if (m_leaderboardClient != null)
        {
            //Change the display to confirm it connected
            m_leaderboardClient.UpdateOnlineStatusDisplay(true);
        }
    }

    /*
    Description: Function called when the command userDisconnected is called. This ensures that the client 
    is disconnected, update the online status display, and output and error message stating that the connection 
    was lost.
    Parameters: string aServerIP - The ip of the machine sending the command
                byte[] aArguments - The arguments for the command. In this case the arguments are not used and are
                                    therefore ignored.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 17, 2017
    */
    private void OnCommandUserDisconnected(string aServerIP, byte[] aArguments)
    {
        //If there is a leaderboard client
        if (m_leaderboardClient != null)
        {
            //Disconnect the client
            m_leaderboardClient.DisconnectClient();

            //Update label
            m_leaderboardClient.UpdateOnlineStatusDisplay(false);

            //Display error message
            m_leaderboardClient.UpdateErrorMessageDisplay(CServerClientConstants.M_ERROR_LOST_CONNECTION
                + aServerIP);
        }
    }

    /*
    Description: Function called when the command message is called. This reads the message send and output
    if through the UpdateMessageLog function.
    Parameters: string aServerIP - The ip of the machine sending the command
                byte[] aArguments - The arguments for the command. In this case the arguments conatain a string
                                    of the message that is being send.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 17, 2017
    */
    private void OnCommandMessage(string aServerIP, byte[] aArguments)
    {
        //If there is a leaderboard client
        if (m_leaderboardClient != null)
        {
            //Deserialize the message as a string
            string message = CUtilityNetworkSerialization.DeserializeString(aArguments);

            //Update the message on the leaderboard client
            m_leaderboardClient.UpdateMessageLog(message);
        }
    }

    /*
    Description: Function called when the command sendLeaderboard is called. This function is used to receive the
    entries from the online leaderboard.
    Parameters: string aServerIP - The ip of the machine sending the command
                byte[] aArguments - The arguments for the command. In this case the arguments contain a ELevelState
                                    reprensenting the level of the leaderbaord, and a List of SPlayerEntries that
                                    are the entries stored in the online leaderboard. 
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private void OnCommandSendLeaderboard(string aServerIP, byte[] aArguments)
    {
        //If there is a leaderboard client
        if (m_leaderboardClient != null)
        {
            ELevelState leaderboardLevel = ELevelState.NoMotion;
            List<SPlayerEntry> entries = null;

            //Deserialize the entries for the leaderboard
            CUtilityNetworkSerialization.DeserializeListPlayerEntries(aArguments, ref leaderboardLevel, ref entries);

            //Add the entries in the online leaderboard lists stored by the client
            m_leaderboardClient.AddOnlineLeaderboardEntries(leaderboardLevel, entries);
        }
    }

    /*
    Description: Override of the UpdateEventLog function from command handler. This merely
    calls the UpdateEventLog function in the online leaderbaord client.
    Parameters: string aMessage - The message to be displayed in the event log
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 17, 2017
    */
    protected override void UpdateEventLog(string aMessage)
    {
        //If there is a leaderboard client
        if (m_leaderboardClient != null)
        {
            //Update its event log
            m_leaderboardClient.UpdateEventLog(aMessage);
        }
    }

    /*
    Description: Implementation of abstract CCommandHandler function Initialize commands. This adds the commands
    supported by the handler to the list of commands
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 17, 2017
    */
    protected override void InitializeCommands()
    {
        AddCommand(CCommandConstants.M_COMMAND_USER_CONNECTED, OnCommandUserConnected);//Command user connected
        AddCommand(CCommandConstants.M_COMMAND_USER_DISCONNECTED, OnCommandUserDisconnected);//Command user disconnected
        AddCommand(CCommandConstants.M_COMMAND_MESSAGE, OnCommandMessage);//Command message
        AddCommand(CCommandConstants.M_COMMAND_SEND_LEADERBOARD, OnCommandSendLeaderboard);//Command send leaderboard
    }
}