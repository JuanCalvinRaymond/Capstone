using UnityEngine;
using System.Collections;

using System;
using System.Linq;

/*
Description: Utility class to store static functions related to sorting arrays/lists
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, March 20th, 2017                   
*/
public class CUtilitySorting
{
    /*
    Description: Function to read a string and get numeric or char (not numeric characters) from a string.
                 The function will return the values read.
    Parameters: string aValueToRead - The string of the value that will be reaed
                ref int aStartingIndex - The index from which it will start reading the string, this will be updated after
                                         the function executes
                int aStringToReadLength - The length (number of characters) of the string that will be read
    Creator: Alvaro Chavez Mixco
    Creation Date:  Wednesday, February 15th, 2017
    Extra Notes: Based on: https://www.dotnetperls.com/alphanumeric-sorting
                 This function is inteded to work with the function SortByAlphanumerically, so it misses a lot
                 of "if" checks.
    */
    private static char[] GetNumberCharsChunks(string aStringToRead, ref int aStartingIndex, int aStringToReadLength)
    {
        //Get the current character of both names
        char tempCharacter = aStringToRead[aStartingIndex];

        // Create arrays of the values read
        char[] readValues = new char[aStringToReadLength];

        //Index of the current values read
        int indexReadValues = 0;

        //While the character read is number matches the intial character is number condition
        //So it will read the string by chunks, determining if the char read is a number or not
        do
        {
            //Read the starting value
            readValues[indexReadValues] = tempCharacter;

            //Increase the index
            indexReadValues++;
            aStartingIndex++;

            //If the index is still valid
            if (aStartingIndex < aStringToReadLength)
            {
                //Read the next value
                tempCharacter = aStringToRead[aStartingIndex];
            }
            else//If the index is not valid
            {
                //Quit the loop
                break;
            }
        } while (char.IsNumber(tempCharacter) == char.IsNumber(readValues[0]));

        return readValues;
    }

    /*
    Description: Function intented to be assigned to a delegate. 
                 Function to compare 2 strings alhpanumerically according to their.
        Parameters: string aTextA - The text to compare
                    string aTextB - The other text to compare
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, February 14th, 2017
    Extra Notes: Based on: https://www.dotnetperls.com/alphanumeric-sorting
                 https://msdn.microsoft.com/en-us/library/tfakywbh(v=vs.110).aspx 
                 Value           Meaning          
                 Less than 0     x is less than y.
                 0               x equals y.  
                 Greater than 0  x is greater than y.                        
    */
    public static int SortByAlphanumerically(string aTextA, string aTextB)
    {
        //If both objects are valid
        if (string.IsNullOrEmpty(aTextA) == false && string.IsNullOrEmpty(aTextB) == false)
        {
            //Create an index to iterate over both objects
            int indexA = 0;
            int indexB = 0;

            string nameA = aTextA;
            string nameB = aTextB;

            //Go through each character in both of the objects name
            while (indexA < nameA.Length && indexB < nameB.Length)
            {
                // Create arrays of the values read, separate the numbers and characters in each array
                char[] readValuesA = GetNumberCharsChunks(nameA, ref indexA, nameA.Length);
                char[] readValuesB = GetNumberCharsChunks(nameB, ref indexB, nameB.Length);

                //Convert the chars arrays back into strings
                string stringReadA = new string(readValuesA);
                string stringReadB = new string(readValuesB);

                int result;

                //Check if the starting value of each string read is a number
                if (char.IsNumber(readValuesA[0]) == true && char.IsNumber(readValuesB[0]) == true)
                {
                    //Parse both chunks for int 
                    int numericChunkA = int.Parse(stringReadA);
                    int numericChunkB = int.Parse(stringReadB);

                    //Compare the ints normally
                    result = numericChunkA.CompareTo(numericChunkB);
                }
                else//If the strings start with a char
                {
                    //Compare strings normally
                    result = stringReadA.CompareTo(stringReadB);
                }

                //If we determine which string is bigger
                if (result != 0)
                {
                    //Return the result
                    return result;
                }
            }

            //Sort according to the actual length of the string
            return nameA.Length - nameB.Length;
        }

        //Return both values are equal
        return 0;
    }

    /*
    Description: Function intented to be assigned to a delegate. 
                 Function to compare 2 objects  inverse alhpanumerically according to their name. This is done
                by calling the normal SortByAlphanumerically function, but switching the order of the parameters.
    Parameters: string aTextA - The text to compare
                string aTextB - The other text to compare
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, February 14th, 2017
    */
    public static int SortByInverseAlphanumerically(string aTextA, string aTextB)
    {
        //Call the normal SortByAlphanumerically function, but switching the order of the parameters.
        return SortByAlphanumerically(aTextB, aTextA);
    }

    /*
    Description: Function intented to be assigned to a delegate. 
                 Function to compare 2 audiclip alhpanumerically according to their name.
    Parameters: AudioClip aClipA- An audio clip
                AudioClip aClipB - The audio clip that will be compared to aClipA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, March 20th, 2017                   
    */
    public static int SortByAlphanumerically(AudioClip aClipA, AudioClip aClipB)
    {
        //If both objects are valid
        if (aClipA != null && aClipB != null)
        {
            //Sort them according to their name
            return SortByAlphanumerically(aClipA.name, aClipB.name);
        }

        //Return both values are equal
        return 0;
    }

    /*
    Description: Function intented to be assigned to a delegate. 
                 Function to compare 2 texture alhpanumerically according to their name.
    Parameters: Texture aTextureA - A texture
                Texture aTextureB - The texture that will be compared to aTextureA
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, March 20th, 2017                   
    */
    public static int SortByAlphanumerically(Texture aTextureA, Texture aTextureB)
    {
        //If both textures are valid
        if (aTextureA != null && aTextureB != null)
        {
            //Sort them according to their name
            return SortByAlphanumerically(aTextureA.name, aTextureB.name);
        }

        //Return both values are equal
        return 0;
    }

    /*
    Description: Function intented to be assigned to a delegate. 
                 Function to compare 2 gameobjects alhpanumerically according to their name.
    Parameters: GameObject aObjectA - A gameobject
                GameObject aObjectB - The gameobject that will be compared to aObjectB
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, March 20th, 2017                   
    */
    public static int SortByAlphanumerically(GameObject aObjectA, GameObject aObjectB)
    {
        //If both game objects are valid
        if (aObjectA != null && aObjectB != null)
        {
            //Sort them according to their name
            return SortByAlphanumerically(aObjectA.name, aObjectB.name);
        }

        //Return both values are equal
        return 0;
    }

    /*
    Description: Function intented to be assigned to a delegate. This is done
                    by calling the normal SortByAlphanumerically function, but switching the order of the parameters.
                 Function to compare 2 gameobjects alhpanumerically according to their name.
    Parameters: GameObject aObjectA - A gameobject
                GameObject aObjectB - The gameobject that will be compared to aObjectB
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, March 20th, 2017                   
    */
    public static int SortByInverseAlphanumerically(GameObject aObjectA, GameObject aObjectB)
    {
        return SortByAlphanumerically(aObjectB, aObjectA);
    }

    /*
    Description: Function to sort a texture array alphanumerically according to each texture name.
    Parameters: ref Texture[] aTextureArray - The array of textures to sort
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public static void SortArray(ref Texture[] aTextureArray)
    {
        //Sort the array alphanumerically
        Array.Sort(aTextureArray, new Comparison<Texture>(SortByAlphanumerically));
    }

    /*
    Description: Function to sort a audioclip array alphanumerically according to each audioclip name.
    Parameters: ref AudioClip[] aAudiClipArray - The array of audio clips to sort
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    */
    public static void SortArray(ref AudioClip[] aAudiClipArray)
    {
        //Sort the array alphanumerically
        Array.Sort(aAudiClipArray, new Comparison<AudioClip>(SortByAlphanumerically));
    }


    /*
    Description: Function to sort a raycasthit array alphanumerically according to their distance
    Parameters: ref RaycastHit[] aRaycastHit - The array of raycast hits to be sorted
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 22, 2017
    */
    public static void SortRaycastHitFromClosestToFarthest(ref RaycastHit[] aRaycastHit)
    {
        aRaycastHit = aRaycastHit.OrderBy(x => x.distance).ToArray();
    }
}
