using UnityEngine;
using System.Collections;

/*
Description: Utility class used for doing multiple math functions.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
*/
public class CUtilityMath
{
    public const float M_DOUBLE_PI = 6.2831853f;
    public const float M_360_DEGREES = 360.0f;

    /*
    Description: Function to rotate a float to 2 decimal points precision, commonly used to prepare
    a float for UI Display
    Parameters: float aNumberToRound-The number to rotate
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    */
    public static float RoundTo2Digits(float aNumberToRound)
    {
        return Mathf.Round(aNumberToRound * 100.0f) / 100.0f;//Round a float to only be 2 decimal points
    }

    /*
    Description: Helper function, somewhy I made it a function instead of an if statement, to 
    get the bool value of an int
    Parameters: int aIntValue-The value to convert
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 20, 2016
    */
    public static bool IntToBool(int aIntValue)
    {
        //If the int is positive
        if (aIntValue > 0)
        {
            return true;//Return true
        }
        else//If the int is 0, or a negative number
        {
            return false;//Return false
        }
    }

    /*
    Description: Helper function, somewhy I made it a function instead of an if statement, to 
    a bool to an int
    Parameters: int aBoolValue-The value to convert
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 20, 2016
    */
    public static int BoolToInt(bool aBoolValue)
    {
        //If the bool is true
        if (aBoolValue == true)
        {
            return 1;//Return 1
        }
        else//If the bool is false
        {
            return 0;//Return 0
        }
    }

    /*
    Description: Helper function to check if any of the values in a vector3
    is Not a Number
    Parameters: Vector3 aValueToCheck - The Vector3 that will be checked
    Creator: Alvaro Chavez Mixco
    */
    public static bool IsNaN(Vector3 aValueToCheck)
    {
        //If none of the values in the vector is Not a Number
        if (float.IsNaN(aValueToCheck.x) == false && float.IsNaN(aValueToCheck.y) == false
            && float.IsNaN(aValueToCheck.z) == false)
        {
            return false;
        }
        else//If any of the values in the vector is Not a Number
        {
            return true;
        }
    }

    /*
    Description: Helper function to get the total index of an element in a 2D Array
    Parameters: int aIndexX - The current X index of the element
                int aIndexY - The current Y index of the element
                int aArrayWidth - The total width of the 2D array
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, January 27, 2017
    */
    public static int TotalIndex2DArray(int aIndexX, int aIndexY, int aArrayWidth)
    {
        return aIndexY * aArrayWidth + aIndexX;
    }

    /*
    Description: Helper function to get the index X of an element in a 2D array
    Parameters: int aTotalIndex - The total index of the element in the 2D array
                int aArrayWidth - The total width of the 2D array
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, January 27, 2017
    */
    public static int IndexX2DArray(int aTotalIndex, int aArrayWidth)
    {
        return aTotalIndex % aArrayWidth;
    }

    /*
    Description: Helper function to get the index Y of an element in a 2D array
    Parameters: int aTotalIndex - The total index of the element in the 2D array
                int aArrayWidth - The total width of the 2D array
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, January 27, 2017
    */
    public static int IndexY2DArray(int aTotalIndex, int aArrayWidth)
    {
        return aTotalIndex / aArrayWidth;
    }

    /*
    Description: Helper function to get the 3D (X,Y, Z) index of an object in a array according to its 1D intex
                 and the array size.
    Parameters: int a1DIndex - The total or 1D index being converted to 3D.
                int aArrayWidth - The width, number fo columns, of the array
                int aArrayHeight - The height of the array
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 29, 2017
    */
    public static Vector3 Convert1DIndexTo3DArrayIndex(int a1DIndex, int aArrayWidth, int aArrayHeight)
    {
        Vector3 index3D;
        index3D.z = a1DIndex / (aArrayWidth * aArrayHeight);
        a1DIndex -= ((int)index3D.z * aArrayWidth * aArrayHeight);
        index3D.y = a1DIndex / aArrayWidth;
        index3D.x = a1DIndex % aArrayWidth;

        return index3D;
    }

    /*
    Description: Convert a 1D array of game objects into a 3D array of game objects. The function will 
                 return the new 3D array of game objects.
    Parameters: GameObject[] aArrayGameObject - The 1D array of game objects to convert
                Vector3 aArrayXYZDimensions - The dimensions for the 3D array
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 29, 20177
    */
    public static GameObject[,,] Convert1DArrayGameObjectsTo3DArray(GameObject[] aArrayGameObject, Vector3 aArrayXYZDimensions)
    {
        //Create the 3D array of game objects
        GameObject[,,] gameObjects3DArray = new GameObject[(int)aArrayXYZDimensions.x, (int)aArrayXYZDimensions.y, (int)aArrayXYZDimensions.z];

        Vector3 temp3DIndex;

        //If the 1D array matches in size with the desired 3D array
        if (aArrayXYZDimensions.x * aArrayXYZDimensions.y * aArrayXYZDimensions.z == aArrayGameObject.Length)
        {
            //Go through all the gameobjects
            for (int i = 0; i < aArrayGameObject.Length; i++)
            {
                //Get the 3D coordinates according to the 1D invex
                temp3DIndex = Convert1DIndexTo3DArrayIndex(i, (int)aArrayXYZDimensions.x, (int)aArrayXYZDimensions.y);

                //Set the game object at the desired 3D intex
                gameObjects3DArray[(int)temp3DIndex.x, (int)temp3DIndex.y, (int)temp3DIndex.z] = aArrayGameObject[i];
            }
        }

        return gameObjects3DArray;
    }

    /*
    Description: Get the value between elements in each axis.
    Parameters: Vector3 aTotalValues - The total value of the object
                Vector3 aTotalNumberOfElements - The number of object that will divide the total value.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 29, 20177
    */
    public static Vector3 GetValuesBetweenElements(Vector3 aTotalValues, Vector3 aTotalNumberOfElements)
    {
        Vector3 valuesBetweenElements = Vector3.zero;

        //Get the value between each x,y and z element, according to the total value
        //and the total number of elements
        valuesBetweenElements.x = aTotalValues.x / aTotalNumberOfElements.x;
        valuesBetweenElements.y = aTotalValues.y / aTotalNumberOfElements.y;
        valuesBetweenElements.z = aTotalValues.z / aTotalNumberOfElements.z;

        return valuesBetweenElements;
    }

    /*
    Description: Check a 3 axis Euler rotation to ensure that it is within a 0 to 360 range.
                 The function will return a rotation between 0 to 360 in all of its axes.
    Parameters: Vector3 aDegreesRotation - The euler rotation to check
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 29, 20177
    */
    public static Vector3 GetValidDegreesRotation(Vector3 aDegreesRotation)
    {

        Vector3 validRotation = aDegreesRotation;

        //Ensure no value is smaller than 0 in all degrees of rotation
        validRotation.x = GetValidDegreesRotation(validRotation.x);
        validRotation.y = GetValidDegreesRotation(validRotation.y);
        validRotation.z = GetValidDegreesRotation(validRotation.z);

        return validRotation;
    }

    /*
    Description: Check if a one axis of rotation is withing a 0 to 360 range.
    Parameters: float aDegreeRotation - The single rotation axis to check.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 29, 2017
    */
    public static float GetValidDegreesRotation(float aDegreeRotation)
    {
        float validRotation = aDegreeRotation;

        //While the rotation is less than 0
        while (aDegreeRotation < 0.0f)
        {
            //Add 360 to it
            aDegreeRotation += M_360_DEGREES;
        }

        //While the rotation is bigger than 360
        while (aDegreeRotation > M_360_DEGREES)
        {
            //Substract 360 from it
            aDegreeRotation -= M_360_DEGREES;
        }

        return validRotation;
    }

    /*
    Description: A function to calculate angle between current and previous forward direction and add it to comparison
    Parameters: aCurrentForwardDirection : Current forward direction
                aPreviousForwardDirection : Previous forward direction
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    public static float CalculatingAngleDifference(Vector3 aCurrentForwardDirection, ref Vector3 aPreviousForwardDirection)
    {
        //If previous forward direction is not set yet
        if (aPreviousForwardDirection == Vector3.zero)
        {
            //Set previous forward direction to the current one
            aPreviousForwardDirection = aCurrentForwardDirection;
        }
        //If previous forward direction is already set
        else
        {
            //Return the angle between current and previous forward direction
            float angleDifference = Vector3.Angle(aCurrentForwardDirection, aPreviousForwardDirection);

            //Set previous forward direction to the current one
            aPreviousForwardDirection = aCurrentForwardDirection;

            return angleDifference;
        }

        //Return 0.0f if it haven't been calculated
        return 0.0f;
    }

    /*
    Description: A function to calculate angle between current and previous rotation and add it to comparison
    Parameters: aCurrentRotation : Current rotation
                aPreviousRotation : Previous rotation
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    public static float CalculatingAngleDifference(float aCurrentRotation, ref float aPreviousRotation)
    {
        //If previous forward direction is not set yet
        if (aPreviousRotation == 0.0f)
        {
            //Set previous forward direction to the current one
            aPreviousRotation = aCurrentRotation;
        }
        //If previous forward direction is already set
        else
        {
            float angleDifference = aCurrentRotation - aPreviousRotation;

            //Set previous forward direction to the current one
            aPreviousRotation = aCurrentRotation;

            //Return the angle between current and previous forward direction
            return angleDifference;
        }

        //Return 0.0f if it haven't been calculated
        return 0.0f;
    }

    /*
    Description: A function to calculate angle between current and previous rotation and add it to comparison
    Parameters: out float aAngle - The angle of rotation according to axis
                out Vector3 aAxis - The axis that will be checked 
                Quaternion aRotation - The rotation where the axis and angle will be checked
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public static void RotationToAngleAxis360DegreesLimit(out float aAngle , out Vector3 aAxis, Quaternion aRotation)
    {
        //Get the amount of rotation in an axis
        aRotation.ToAngleAxis(out aAngle, out aAxis);

        //Check that the angle doesn't go beyond the "circle" 360 values
        if (aAngle > 180)
        {
            aAngle -= 360;
        }
    }


    public static float RescaleRange(float num, float low1, float high1, float low2, float high2)
    {
        return low2 + (num - low1) * (high2 - low2) / (high1 - low1);
    }

    public static float RescaleRangeClamp(float num, float low1, float high1, float low2, float high2)
    {
        return Mathf.Clamp01(low2 + (num - low1) * (high2 - low2) / (high1 - low1));

    }
}
