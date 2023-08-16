/*UPDATED AS OF: FRIDAY, FEBRUARY 3, 2017*/

/*
Description: Class uses to store constant strings and variables that are used for
communication between the online leaderboard client and server
Creator: Alvaro Chavez Mixco
Creation Date: Tuesday, January 10, 2017
*/
public static class CServerClientConstants
{
    //Labels constants
    public const string M_LABEL_ONLINE_TEXT = "ONLINE";
    public const string M_LABEL_OFFLINE_TEXT = "OFFLINE";
    public const string M_SPACE = " ";

    //ERRORS and Exceptions
    public const string M_ERROR_LOST_CONNECTION = "Lost connection to server";
    public const string M_ERROR_INVALID_INTERNET_SOCKET = "Can't connect to online server, invalid Internet Socket";
    public const string M_ERROR_ALREADY_CONNECTED = "Already connected to a server.";
    public const string M_ERROR_CURRENTLY_CONNECTING_TO_SERVER = "Currently connecting to server";
    public const string M_ERROR_CANT_FIND_REQUESTED_LEADERBOARD = "Server couldn't find the requested leaderboard: ";
    public const string M_EXCEPTION_CONNECTION_TIMEOUT = "Couldn't connect to server on alloted time";
    public const string M_EXCEPTION_CONNECTION_REFUSED = "Requested server is offline";

    //Networking variables
    public const int M_SERVER_PORT_NUMBER = 8080;//Variable must match the value in the client
    public const int M_ARGUMENTS_PACKET_SIZE = 16384;
    public const int M_ARGUMENT_PACKET_READ_TIMEOUT = int.MaxValue;//Read Timeout in milliseconds
    public const int M_TCP_CLIENT_CONNECT_RECEIVE_SEND_TIMEOUT = 500;
    public const string M_SERVER_IP_ADDRESS = "127.0.0.1";//Hardcoded for testing purposes
    public const string M_ENCRYPTION_KEY = "2257-7777";
}