/*UPDATED AS OF: THURSDAY, FEBRUARY 2, 2017*/

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
Description: Utility class used to store functions commonly networking functions used
by both clients and servers.
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, January 17, 2017
*/
public class CUtilityNetworking
{
    public const int M_SOCKET_POLL_TIME = 1000;

    /*
    Description: Sends a command and its arguments through the passed network stream.
    Parameters: ref NetworkStream aConnection - The connection where the data will be send to.
                string aCommand - The name of the command that wants to be executed.
                byte[] aCommandArguments - The serialized arguments of the command.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, January 18, 2017
    */
    public static void SendNetworkCommand(ref NetworkStream aConnection, string aCommand, byte[] aCommandArguments)
    {
        //If the connection network stream is valid
        if (aConnection != null)
        {
            //If the connection is writable
            if (aConnection.CanWrite == true)
            {
                //Write message to server
                //Prepare the writer and the formatter
                BinaryWriter writer = new BinaryWriter(aConnection);
                BinaryFormatter formatter = new BinaryFormatter();

                //Write the encryption key
                formatter.Serialize(writer.BaseStream, CServerClientConstants.M_ENCRYPTION_KEY);

                //Write the name of the command
                formatter.Serialize(writer.BaseStream, aCommand);

                //Write th arguments
                //If there is any argument
                if (aCommandArguments != null)
                {

                    //Ensure the arguments being send are the size expected by the sever
                    byte[] commandArguments = CUtilityNetworking.FillByteArrayToPacketSize(aCommandArguments);
                    int dataLeftToSend = aCommandArguments.Length;

                    //While there is data left to send
                    while (dataLeftToSend > 0)
                    {
                        //Write it to the server
                        writer.Write(commandArguments, 0, CServerClientConstants.M_ARGUMENTS_PACKET_SIZE);
                        dataLeftToSend -= CServerClientConstants.M_ARGUMENTS_PACKET_SIZE;
                    }
                }

                //Ensure all data is sent
                writer.Flush();
            }
        }
    }

    /*
    Description: This will constantly receive commands through the network stream, and process them through the command handler.
    Until the stopReceivingCommands or an error are found.
    Parameters: ref NetworkStream aConnection - The connection from where the data will be read.
                ref CCommandHandler - The command handler that will process all the commands that are read
                string aSenderIP - The IP addres of the machine sending the commands
                Socket aSocketToPoll - The internet socket that was used to make a connection (NOT CURRENTLY USE, SAVED FOR LATER USE)
                string aStopReceivingCommand - The command that will be used to mark the end of the receiving commands loop.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, January 18, 2017
    */
    public static void ReceiveNetworkCommands(ref NetworkStream aConnection, ref CCommandHandler aCommandHandler,
        string aSenderIP, Socket aSocketToPoll, string aStopReceivingCommand = CCommandConstants.M_COMMAND_USER_DISCONNECTED)
    {
        //If the connection network stream is valid
        if (aConnection != null)
        {
            //If the netowrk stream is readable
            if (aConnection.CanRead == true)
            {
                //If there is a command handler and the network stream can be read
                if (aCommandHandler != null && aConnection.CanRead == true)
                {
                    //Prepare to read data
                    BinaryFormatter formatter = new BinaryFormatter();

                    //Set max time to read messages
                    aConnection.ReadTimeout = CServerClientConstants.M_ARGUMENT_PACKET_READ_TIMEOUT;

                    //Variables used to store data that will be read
                    string encryptionKey = string.Empty;
                    string command = string.Empty;
                    Byte[] arguments = new Byte[CServerClientConstants.M_ARGUMENTS_PACKET_SIZE];

                    //Loop to read all incoming messages of client, until the stop receiving message is found
                    do
                    {
                        try
                        {
                            //Read the encryption key
                            encryptionKey = (string)formatter.Deserialize(aConnection);

                            //Read the name of the command
                            command = (string)formatter.Deserialize(aConnection);

                            //Read the arguments the command may have
                            aConnection.Read(arguments, 0, arguments.Length);
                        }

                        //If there is an error assume the connection has been terminated and quit the loop
                        catch(SocketException)
                        {
                            //Run the command to end the loop
                            aCommandHandler.RunCommand(aSenderIP, aStopReceivingCommand, arguments);
                            
                            //Break out of the receiving loop
                            break;
                        }

                        catch(TimeoutException)
                        {
                            //Run the command to end the loop
                            aCommandHandler.RunCommand(aSenderIP, aStopReceivingCommand, arguments);

                            //Break out of the receiving loop
                            break;
                        }

                        //If the encryption key is valid
                        if (encryptionKey == CServerClientConstants.M_ENCRYPTION_KEY)
                        {
                            //Pass the data to the command handler so that it can be run
                            aCommandHandler.RunCommand(aSenderIP, command, arguments);
                        }
                        else//If the encryption key doesn't match
                        {
                            //End the connection with the client
                            aCommandHandler.RunCommand(aSenderIP, aStopReceivingCommand, arguments);

                            //Exit the receiving messages loop
                            break;
                        }

                    } while (command != aStopReceivingCommand);
                }
            }
        }
    }

    /*
    Description: This will fill a byte array with empty data untill it matches the ArgumentPacketSize expected
    by both client and server.
    Parameters: byte[] aUsedContent - a byte array containing data
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 15, 2017
    */
    public static byte[] FillByteArrayToPacketSize(byte[] aUsedContent)
    {
        //If the array is using less than the size of the argument packet
        if (aUsedContent.Length < CServerClientConstants.M_ARGUMENTS_PACKET_SIZE)
        {
            //Concantenate  2 arrays so that it only ends up with 1 of the desired packet size

            //Make an array the size of the remaining packet sie
            byte[] emptyData = new byte[CServerClientConstants.M_ARGUMENTS_PACKET_SIZE - aUsedContent.Length];

            //Make the final array of data, it should be the argument packet size, or the size
            //of  the usedContent combined with the emptyData
            byte[] paddedArguments = new byte[aUsedContent.Length + emptyData.Length];

            //Concatenate the 2 arrays together into the paddedArguments array variable
            Buffer.BlockCopy(aUsedContent, 0, paddedArguments, 0, aUsedContent.Length);
            Buffer.BlockCopy(emptyData, 0, paddedArguments, aUsedContent.Length, emptyData.Length);

            //Return the filled out array
            return paddedArguments;
        }

        //Return the original array
        return aUsedContent;
    }

    /*
    Description: Helper function to check if a socket and a network stream are conencted. This use a socket
    connected property, and additionally it polls the internet socket.
    Parameters: Socket aSocket - The internet socket to check connected property for, and to poll
                NetworkStream aNetworkStream - The internet stream being used
    Creation Date:  Sunday, January 15, 2017
    */
    public static bool GetIsConnected(Socket aSocket, NetworkStream aNetworkStream)
    {
        //Check if the socket is valid, and additionally poll the connection socket
        //return GetIsSocketConnected(aSocket, aNetworkStream) && PollConnectionSocket(aSocket);

        //DEBUGLIST-AAA-2
        return GetIsSocketConnected(aSocket, aNetworkStream);
    }

    /*
    Description: Helper function to check the connected property of an internet socket
    Parameters: Socket aSocket - The internet socket to check connected property for
                NetworkStream aNetworkStream - The internet stream being used
    Creation Date:  Saturday, January 16, 2017
    */
    public static bool GetIsSocketConnected(Socket aSocket, NetworkStream aNetworkStream)
    {
        //If there is a client socket and a network stream
        if (aSocket != null && aNetworkStream != null)
        {
            //Return if it is connected or not
            return aSocket.Connected;
        }

        //Return there is no connection
        return false;
    }

    /*
    Description: Helper function to poll an internet socekt t osee if it is connected
    Parameters: Socket aSocket - The internet socket to poll.
    Creation Date:  Sunday, January 22, 2017
    */
    public static bool PollConnectionSocket(Socket aSocket)
    {
        //If the socket is valid
        if (aSocket != null)
        {
            bool socketHasBeenTerminated = aSocket.Poll(M_SOCKET_POLL_TIME, SelectMode.SelectRead);//true if the connection has been closed, reset, or terminated;
            bool socketHasError = aSocket.Poll(M_SOCKET_POLL_TIME, SelectMode.SelectError);//Poll the socket for errors
            bool hasDataAvailableToRead = aSocket.Available <= 0;//See if the socket has data available to read

            //If the connection hasn't been terminated, there isn't an error with the socket, and there is data available to read
            if (socketHasBeenTerminated == false && socketHasError == false && hasDataAvailableToRead == true)
            {
                //Return the connection was successful
                return true;
            }
        }

        //Return that the connection was not successful
        return false;
    }

    /*
    Description: The function will get the IP Address of this local machine.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 12, 2017
    */
    public static string GetMachineIPAddress()
    {
        //Get the host
        IPHostEntry IPHost =
                    Dns.GetHostEntry(Dns.GetHostName());

        //Go through all the IP adresses in the computer
        foreach (IPAddress ip in IPHost.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return string.Empty;
    }
}