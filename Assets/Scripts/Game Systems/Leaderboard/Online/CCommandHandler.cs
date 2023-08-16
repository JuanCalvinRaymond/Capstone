/*UPDATED AS OF: THURSDAY, FEBRUARY 2 , 2017*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

/*
Description: This class will process the commands read by the server, and do "something"
with them.
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, January 10, 2017
*/
public abstract class CCommandHandler
{
    /*
    Description: Class used to store a string command, and the respective function linked
    to that command.
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 10, 2017
    */
    protected class CCommand
    {
        private string m_command; //The name the command will have
        private delegateFunctionHandler m_functionCall; //The name of the function delegate it will call

        public string PCommand { get { return m_command; } }
        public delegateFunctionHandler PFunctionCall { get { return m_functionCall; } }

        /*
        Description: Sets the command and the function that will be stored in this class.
        Parameters:  string aCommand - The name of the command
                     delegateFunctionHandler aFunctionCall - The function that will be run
                        when the command is run.
        Creator: Alvaro Chavez Mixco
        Creation Date: Tuesday, January 10, 2017
        */
        public CCommand(string aCommand, delegateFunctionHandler aFunctionCall)
        {
            //Set all values
            m_command = aCommand;
            m_functionCall = aFunctionCall;
        }
    }

    //Message Log constants
    protected const string M_LOG_NO_COMMAND_GIVEN = "No command was given.";
    protected const string M_LOG_UNABLE_TO_PROCESS_COMMAND = "Unable to process command '{0}'";
    protected const string M_LOG_UNKNOWN_COMMAND = "Unknown command '{0}',type 'info' for command list";
    protected const string M_LOG_SEMICOLON = " : ";

    //Commands variables
    protected Dictionary<string, CCommand> m_listOfCommands;

    protected delegate void delegateFunctionHandler(string clientIP, byte[] aArguments);
    protected delegate void delegateUpdateEventLog(string aMessage);

    /*
    Description: Sets all the respective variables, and all the commands that will be supported
    for by this class.
    Parameters: COnlineLeaderboardServer aLeaderboardServer - The server from where commands will be received
                ListBox aEventLog - The list box that will serve as an event log. 
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 10, 2017
    */
    public CCommandHandler()
    {
        //Add all the commands supported by this class
        m_listOfCommands = new Dictionary<string, CCommand>();

        //Add the commands that are supported by this command handler
        InitializeCommands();
    }

    /*
    Description: Adds a command to the list of supported commands
    Parameters: string aCommand - The name of the command.
                delegateFunctionHandler aHandler- The function that will run with the command. 
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, January 10, 2017
    */
    protected void AddCommand(string aCommand, delegateFunctionHandler aHandler)
    {
        //Add the command to the list
        m_listOfCommands.Add(aCommand, new CCommand(aCommand, aHandler));
    }

    /*
    Description: Adds a command to the list of supported commands
    Parameters:  string aMessage - The message that will be displayed in the event log 
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 11, 2017
    Extra Notes: This virual function is empty, since its implementation , while useful for debugging
    is not mandatory.
    */
    protected virtual void UpdateEventLog(string aMessage)
    {
    }

    /*
    Description: Updates the event log with the command that was send, then it checks if it has
    the desired command on its list of supported commands, and if it does it runs it.
    Parameters:  string aClientIP - The IP of the client that send  the command
                 string aCommand - The name of the command that wants to be run
                 byte[] aArguments - The arguments that will be used by the command 
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 12, 2017
    */
    public void RunCommand(string aClientIP, string aCommand, byte[] aArguments)
    {
        //Update the event log with the command that wants to be run
        UpdateEventLog(aClientIP + M_LOG_SEMICOLON + aCommand);

        //If no command was given
        if (aCommand == string.Empty)
        {
            //Display error message in event log
            UpdateEventLog(M_LOG_NO_COMMAND_GIVEN);

            //Exit function
            return;
        }

        CCommand commandToCall = null;
        //If the command wasn't found in the command list.
        if (m_listOfCommands.TryGetValue(aCommand,  //This searches the list of commands for the command we want,
            out commandToCall) == false) //then, if found, it stores it in the commandToCall variable.
        {
            //Display error message
            UpdateEventLog(string.Format(M_LOG_UNKNOWN_COMMAND, aCommand));

            //Exit function
            return;
        }
        else//If the command is supported
        {
            // Call the function linked to the command by passing the arguments
            commandToCall.PFunctionCall(aClientIP, aArguments);
        }
    }


    /*
    Description: Abstract function called in CCommandHandler constructor. This function
    is inteded to be used to add whatever commands are supported by this command handler.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 12, 2017
    */
    protected abstract void InitializeCommands();
}

