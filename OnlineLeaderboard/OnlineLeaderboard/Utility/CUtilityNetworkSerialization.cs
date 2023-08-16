/*UPDATED AS OF: THURSDAY, FEBRUARY 2, 2017*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


/*
Description: Helper class used to serialize and deserialize multiple variables to and from an array of bytes.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, January 18, 2017
*/
public class CUtilityNetworkSerialization
{

    /*___________________________________________________________________________________________*/
    /* Functions to be used  to deserialize/serialize a single object from a single stream/byte[]*/
    /*___________________________________________________________________________________________*/

    /*
    Description: Serialize an object to a byte array. The function will return the serialize
    array of bytes
    Parameters: object aObject - The object to be serialized
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private static byte[] SerializeObject(object aObject)
    {
        //Create a binary formatter and memory stream
        BinaryFormatter formatter = CreateBinaryFormatter();
        MemoryStream stream = new MemoryStream();

        //Serialize the object
        formatter.Serialize(stream, aObject);

        //Convert the memory stream as an array
        return stream.ToArray();
    }

    /*
    Description: Deserialize an object out from an array of byes
    Parameters: byte[] aByteArray - The byte array that will be deserialized
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    private static object DeserializeObject(byte[] aByteArray)
    {
        //Create the binary formatter and memory stream
        BinaryFormatter formatter = CreateBinaryFormatter();
        MemoryStream stream = new MemoryStream(aByteArray);

        //Deserialize the memory stream
        return formatter.Deserialize(stream);
    }

    /*
    Description: Serialize a SPlayerEntry to a byte array. The function will return the serialize
    array of bytes
    Parameters: SPlayerEntry aEntry - The object to be serialized
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static byte[] SerializePlayerEntry(SPlayerEntry aEntry)
    {
        return SerializeObject(aEntry);
    }

    /*
    Description: Deserialize a SPlayerEntry from a byte array.
    Parameters: byte[] aByteArray - The byte array that will be deserialized
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static SPlayerEntry DeserializePlayerEntry(byte[] aByteArray)
    {
        //If the byte array is valid
        if (aByteArray != null)
        {
            //Deserialize the memory stream and cast it as a player entry
            return (SPlayerEntry)DeserializeObject(aByteArray);
        }

        //Return a new empty player entry
        return new SPlayerEntry();
    }

    /*
    Description: Serialize a string to a byte array. The function will return the serialize
    array of bytes
    Parameters: string aString - The object to be serialized
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static byte[] SerializeString(string aString)
    {
        return SerializeObject(aString);
    }

    /*
    Description: Deserialize a string from a byte array.
    Parameters: byte[] aByteArray - The byte array that will be deserialized
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static string DeserializeString(byte[] aByteArray)
    {
        //If the byte array is valid
        if (aByteArray != null)
        {
            //Deserialize the memory stream and cast it as a string
            return (string)DeserializeObject(aByteArray);
        }

        //Return an empty string
        return string.Empty;
    }

    /*___________________________________________________________________________________________*/
    /*Functions to be used  to deserialize/serialize multiple objects from a single stream/byte[]*/
    /* All of this are individual functions to guarantee the correct order in the byte arrray    */
    /*___________________________________________________________________________________________*/

    /*
    Description: Serialize a ELevelState and aPlayerEntry to a byte array. The function will return
    the byte array where the values were serialized.
    Parameters: ELevelState aLevel - The level where the player entry was made. This will be serialized.
                SPlayerEntry aPlayerEntry - The entry to be serialzied
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static byte[] SerializeLevelAndPlayerEntry(ELevelState aLevel, SPlayerEntry aPlayerEntry)
    {
        //Create a binary formatter and memory stream
        BinaryFormatter formatter = CreateBinaryFormatter();
        MemoryStream stream = new MemoryStream();

        //Serialize the enum as an int
        formatter.Serialize(stream, (int)aLevel);

        //Serialize the player struct
        formatter.Serialize(stream, aPlayerEntry);

        return stream.ToArray();
    }

    /*
    Description: Deserialize a ELevelState and a SPlayerEntry from a byte array.
    Parameters: byte[] aByteArray - The byte array to be deserialized
                ref ELevelState aLevelStateHolder - The variable where the deserialized ELevelState variable will be stored
                ref SPlayerEntry aPlayerEntryHolder - The variable where the deserialized SPlayerEntry variable will be stored
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static void DeserializeLevelAndPlayerEntry(byte[] aByteArray, ref ELevelState aLevelStateHolder,
       ref SPlayerEntry aPlayerEntryHolder)
    {
        //If the byte array is valid
        if (aByteArray != null)
        {
            //Create the binary formatter and memory stream
            BinaryFormatter formatter = CreateBinaryFormatter();
            MemoryStream stream = new MemoryStream(aByteArray);

            //Deserialize the level state and cast it as enum
            aLevelStateHolder = (ELevelState)formatter.Deserialize(stream);

            //Deserialize the memory stream and cast it as a player entry
            aPlayerEntryHolder = (SPlayerEntry)formatter.Deserialize(stream);
        }
    }

    /*
    Description: Serialize a ELevelState, a number of entries, and the first entries (up to the passed in number of entries) of
    a List of SPlayerEntry. The function will return the byte array where the values were serialized.
    Parameters: ELevelState aLevel - The level to be serialized, this is the level that the entries belong to.
                int aNumEntries - The int to be serialized, stores how many entries were serialized
                List<SPlayerEntry> aListPlayerEntries - The list of entries that may be partially or completely serialized, this
                                                        contains the actual player entries/
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static byte[] SerializeListPlayerEntries(ELevelState aLevel, int aNumEntries, List<SPlayerEntry> aListPlayerEntries)
    {
        //If the list of entries is valid
        if (aListPlayerEntries != null)
        {
            //Create a binary formatter and memory stream
            BinaryFormatter formatter = CreateBinaryFormatter();
            MemoryStream stream = new MemoryStream();

            //Serialzie the level to which the entries belong to , as an int
            formatter.Serialize(stream, (int)aLevel);

            //Serialize the number of entries
            formatter.Serialize(stream, aNumEntries);

            //Go through each of the entries in the list
            for (int i = 0; i < aNumEntries; i++)
            {
                //If the current index is valid
                if (i < aListPlayerEntries.Count)
                {
                    //Serialize it
                    formatter.Serialize(stream, aListPlayerEntries[i]);
                }
                else //If the index is out of range
                {
                    //Serialize an empty value to ensure size matches in both serializing and deserializing
                    formatter.Serialize(stream, new SPlayerEntry());
                }

                //If the stream position is bigger than the argument packet size
                if (stream.Position > CServerClientConstants.M_ARGUMENTS_PACKET_SIZE)
                {
                    //Quit the loop
                    break;
                }

            }

            //Return the stream as a byte array
            return stream.ToArray();
        }

        return null;
    }

    /*
    Description: Deserialize a ELevelState and a list of player entries from a byte array.
    Parameters: byte[] aByteArray - The byte array to be deserialized
                ref ELevelState aLevelStateHolder - The variable where the deserialized ELevelState variable will be stored
                ref List<SPlayerEntry> aListPlayerEntries- The variable where the deserialized list of SPlayerEntry variable will be stored
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static void DeserializeListPlayerEntries(byte[] aByteArray, ref ELevelState aLevelStateHolder,
        ref List<SPlayerEntry> aListPlayerEntries)
    {
        //If the byte array is valid
        if (aByteArray != null)
        {
            //If the list is null
            if (aListPlayerEntries == null)
            {
                //Initiliaze the list
                aListPlayerEntries = new List<SPlayerEntry>();
            }

            //Create the binary formatter and memory stream
            BinaryFormatter formatter = CreateBinaryFormatter();
            MemoryStream stream = new MemoryStream(aByteArray);

            //Deserialize the level the entries belong to
            aLevelStateHolder = (ELevelState)((int)formatter.Deserialize(stream));

            //Deserialize the value that stores the number of entries that will be read
            int numEntries = (int)formatter.Deserialize(stream);

            //NOTE: THIS CODE MAY BREAK IF THE NUMBER OF DESIRED ENTRIES ENDS UP BEING BIGGER THAN THE SIZE OF THE DATA PACKET
            //Go through the desired number of entries
            for (int i = 0; i < numEntries; i++)
            {
                //Deserialize each of them, cast it to the SPlayerEntry stuct, and add it to the list
                aListPlayerEntries.Add((SPlayerEntry)formatter.Deserialize(stream));

                //If the stream position is bigger than the argument packet size
                if (stream.Position > CServerClientConstants.M_ARGUMENTS_PACKET_SIZE)
                {
                    //Quit the loop
                    break;
                }
            }
        }
    }

    /*
    Description: Serialize a ELevelState, a number of entries, a starting entries index, and a ELeaderboardSortingMethods. 
    The function will return the byte array where the values were serialized.
    Parameters: ELevelState aLevel - The level to be serialized, this is the level that the entries belong to.
                int aNumEntries - The int to be serialized, stores how many entries were serialized
                int aLeaderboardStartingIndex - The int to be serialized, stores the starting index of the entries requested
                ELeaderboardSortingMethods aSortingMethod - The ELeaderboardSortingMethods to be serialized, stores how the
                                                            requested leaderboard has to be sorted.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static byte[] SerializeLeaderboardEntriesRequest(ELevelState aLevel, int aNumEntries,
        int aLeaderboardStartingIndex = 0, ELeaderboardSortingMethods aSortingMethod = ELeaderboardSortingMethods.HighestScore)
    {
        //Create a binary formatter and memory stream
        BinaryFormatter formatter = CreateBinaryFormatter();
        MemoryStream stream = new MemoryStream();

        //Serialize the enum as an int for the leaderborad level
        formatter.Serialize(stream, (int)aLevel);

        //Serialize the int for number of entries to send
        formatter.Serialize(stream, aNumEntries);

        //Serialize the int for the leaderboard starting index that will be looked for
        formatter.Serialize(stream, aLeaderboardStartingIndex);

        //Serialize the enum as an int
        formatter.Serialize(stream, (int)aSortingMethod);

        return stream.ToArray();
    }

    /*
    Description: Deserialize a ELevelState, a aNumEntries, a StartingIndex and ELeaderboardSortingMethods from a byte array.
    Parameters: byte[] aByteArray - The byte array to be deserialized
                ref ELevelState aLevelStateHolder - The variable where the deserialized ELevelState variable will be stored
                ref int aNumEntriesHolder - The variable where the deserialized int representing the requested number of entries will be stored
                ref int aLeaderboardStartingIndexHolder - The variable where the deserialized int representing the starting index
                                                          of the requested entries will be stored. 
                ref ELeaderboardSortingMethods aSortingMethodHolder - The variable where the deserialized ELeaderboardSortingMethods representing
                                                                      how the requested leaderboard has to be sorted will be stored.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, January 19, 2017
    */
    public static void DeserializeLeaderboardEntriesRequest(byte[] aByteArray, ref ELevelState aLevelStateHolder,
       ref int aNumEntriesHolder, ref int aLeaderboardStartingIndexHolder, ref ELeaderboardSortingMethods aSortingMethodHolder)
    {
        //If the byte array is valid
        if (aByteArray != null)
        {
            //Create the binary formatter and memory stream
            BinaryFormatter formatter = CreateBinaryFormatter();
            MemoryStream stream = new MemoryStream(aByteArray);

            //Deserialize the level state and cast it as enum of the leaderboard level
            aLevelStateHolder = (ELevelState)((int)formatter.Deserialize(stream));//This is serailized as an int first, because
                                                                                  //that is was serialized as an int. So to avoid 
                                                                                  //possible issues it is first deserialized
                                                                                  //  as an int and then cast to enum

            //Deserialize the memory stream and cast it as an int for the number of entries to send
            aNumEntriesHolder = (int)formatter.Deserialize(stream);

            //Deserialize the memory stream and cast it as an int for the starting index to search entries for in the leaderboard
            aLeaderboardStartingIndexHolder = (int)formatter.Deserialize(stream);

            //Deserialize the memory stream and cast is an enum
            aSortingMethodHolder = (ELeaderboardSortingMethods)((int)formatter.Deserialize(stream));
        }
    }

    /*___________________________________________________________________________________________*/
    /*Misc Helper functions*/
    /*___________________________________________________________________________________________*/


    /*
    Description: Helper class used to make a binary formatter, and set it up however we needed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 18, 2017
    */
    private static BinaryFormatter CreateBinaryFormatter()
    {
        //Make a binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        //Set the binder of the binary formatter so that it can serialize and deserialize structs
        //from different assembly projects
        formatter.Binder = new CAssemblySerializationBinder();

        //Return the requested binary formatter
        return formatter;
    }

}
