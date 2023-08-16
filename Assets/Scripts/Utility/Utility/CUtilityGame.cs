using UnityEngine;
using System.Collections;

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

/*
Description: Utility class used for doing multiple general game calculations functions
Creator: Alvaro Chavez Mixco
*/
public class CUtilityGame
{
    /*
    Description: Helper function to get the distance between a camera and a game object.
    Parameters:   Vector3 aGameObjectPos- The object that will be measured in distance against the camera
    Creator: Alvaro Chavez Mixco
    Extra Notes: This function requires a Game Manager
    */
    public static float GetDistanceToCameraGameManager(Vector3 aGameObjectPos)
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the instance manager has a camera
            if (CGameManager.PInstanceGameManager.PMainCameraGameObject != null)
            {
                //Return the distance of the object to the camera
                return Vector3.Distance(aGameObjectPos,
                    CGameManager.PInstanceGameManager.PMainCameraGameObject.transform.position);
            }
        }

        return 0.0f;
    }

    /*
    Description: Helper function to get the distance between a camera and a game object,ignoring the Y axis
    Parameters: Vector3 aGameObjectPos- The object that will be measured in distance against the camera
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    Extra Notes: This function requires a Game Manager
    */
    public static float GetDistanceXZToCameraGameManager(Vector3 aGameObjectPos)
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the instance manager has a camera
            if (CGameManager.PInstanceGameManager.PMainCameraGameObject != null)
            {
                //Get the camera position
                Vector3 cameraPosition = CGameManager.PInstanceGameManager.PMainCameraGameObject.transform.position;

                //Return the distance of the object to the camera ignoring the Y
                return Vector2.Distance(new Vector2(aGameObjectPos.x, aGameObjectPos.z),
                   new Vector2(cameraPosition.x, cameraPosition.z));
            }
        }

        return 0.0f;
    }

    /*
    Description: Helper function to get the distance  squared between a camera and a game object.
    Parameters:   Vector3 aGameObjectPos- The object that will be measured in distance against the camera
    Creator: Alvaro Chavez Mixco
    Extra Notes: This function requires a Game Manager
    */
    public static float GetDistanceSquaredToCameraGameManager(Vector3 aGameObjectPos)
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the instance manager has a camera
            if (CGameManager.PInstanceGameManager.PMainCameraGameObject != null)
            {
                //Return the square distance of the object to the camera
                return Vector3.SqrMagnitude(aGameObjectPos -
                    CGameManager.PInstanceGameManager.PMainCameraGameObject.transform.position);
            }
        }

        return 0.0f;
    }

    /*
    Description: Helper function to get the distance between a camera and a game object,ignoring the Y axis
    Parameters:   Vector3 aGameObjectPos- The object that will be measured in distance against the camera
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 27, 2017
    Extra Notes: This function requires a Game Manager
    */
    public static float GetDistanceXZSquaredToCameraGameManager(Vector3 aGameObjectPos)
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the instance manager has a camera
            if (CGameManager.PInstanceGameManager.PMainCameraGameObject != null)
            {
                //Get the camera position
                Vector3 cameraPosition = CGameManager.PInstanceGameManager.PMainCameraGameObject.transform.position;

                //Return the distance of the object to the camera ignoring the Y
                return Vector2.SqrMagnitude(new Vector2(aGameObjectPos.x, aGameObjectPos.z) -
                   new Vector2(cameraPosition.x, cameraPosition.z));
            }
        }

        return 0.0f;
    }

    /*
    Description: Helper function to iterate through a list of raycast hits and get the closest object hit by the 
    raycast, and the information normal and owner gameobject, belonging it to it.
    Parameters:   RaycastHit[] aHitList- Array of raycast hitpoints that will be searched
                  Vector3 aFiringPoint - The position from where the raycast was made 
                  ref Vector3 aHitNormal- Passed by reference, the normal of the object hit
                  ref GameObject aObjectHits- Passed by reference, the gameobject hit by the raycast
    Creator: Alvaro Chavez Mixco
    */
    public static Vector3 GetClosestHitPoint(RaycastHit[] aHitList, Vector3 aFiringPoint, ref Vector3 aHitNormal
        , ref GameObject aObjectHits)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestSqrDistance = float.MaxValue;
        float tempSqrDistance;

        //If the list of object is valid
        if (aHitList != null)
        {
            //If the list is not mepty
            if (aHitList.Length > 0)
            {
                //Iterate through it
                for (int i = 0; i < aHitList.Length; i++)
                {
                    //Get the distance of the current hit point from the firing position
                    tempSqrDistance = Vector3.SqrMagnitude(aFiringPoint - aHitList[i].point);

                    //if the distance is smaller than the previous smaller one
                    if (tempSqrDistance < closestSqrDistance)
                    {
                        //Save the hitpoint data since it is the closest one
                        closestPoint = aHitList[i].point;
                        aHitNormal = aHitList[i].normal;
                        aObjectHits = aHitList[i].collider.gameObject;
                        closestSqrDistance = tempSqrDistance;
                    }
                }

            }
        }

        return closestPoint;
    }

    /*
    Description: Helper function to iterate through a list of raycast hits and get the closest POSITION hit by the 
    raycast
    Parameters: RaycastHit[] aHitList- Array of raycast hitpoints that will be searched
                Vector3 aFiringPoint - The position from where the raycast was made 
    Creator: Alvaro Chavez Mixco
    Extra Notes: This function internally calls the more detailed version of GetClosestHitPoint, version that gives
    normal and gameobject hit by raycast info, in order to avoid duplicate code.
    So this method isn't more effective. However, it was made for ease of use 
    when you only care about position
    */
    public static Vector3 GetClosestHitPoint(RaycastHit[] aHitList, Vector3 aFiringPoint)
    {
        Vector3 closestPoint = Vector3.zero;
        Vector3 emptyNormal = Vector3.zero;
        GameObject emptyGameObject = null;

        closestPoint = GetClosestHitPoint(aHitList, aFiringPoint, ref emptyNormal, ref emptyGameObject);

        return closestPoint;
    }

    /*
    Description: Adding all the child gameobject to the list
    Parameters: aParent : parent gameobject that we want to iterate on
                aListChildGameObject : the list to add the child gameobject
    Creator: Juan Calvin Raymond
    Creation Date: 11-10-2016
    */
    public static List<GameObject> AddChildGameObjectToList(GameObject aParent, List<GameObject> aListChildGameObject)
    {
        //Get all child's transform
        Transform[] childTransform = aParent.GetComponentsInChildren<Transform>();

        //if waypoint have child object
        if (childTransform != null)
        {
            //for every child add it to the list
            foreach (Transform child in childTransform)
            {
                //check if the transform is the parent(for some reason it also get the parent transform)
                if (child.childCount == 0)
                {
                    aListChildGameObject.Add(child.gameObject);//Add the children to the list of gameobject
                }
            }
        }
        return null;
    }

    /*
    Description: A helper function to set check if any key is being pressed. It returns true if any 
    key was pressed, false if no key was pressed         
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    Extra Notes: This function makes use of the game manager to support the HTC Vive.
    */
    public static bool GetAnyKeyPressed()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If there is a player controlle
            if (CGameManager.PInstanceGameManager.PPlayerController != null)
            {
                //Return if the user has pressed any key
                return CGameManager.PInstanceGameManager.PPlayerController.GetAnyKeyPressed() == true;
            }
        }

        //If there is no game manager, use the built in Unity to check if any key was pressed.
        //This DOES NOT check for HTC Vive key presses
        return Input.anyKey;
    }

    /*
    Description: A helper function to get all the children gameobjects from a parent game object.
    Parameters: GameObject aParentObject - The gameobject that is the parent of the desired children objects.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 29, 2017
    */
    public static GameObject[] GetChildrenGameObjectFromParent(GameObject aParentObject)
    {
        //If the parent object is valid
        if (aParentObject != null)
        {
            //Create an array of game objects, accordign to its number of children
            GameObject[] children = new GameObject[aParentObject.transform.childCount];

            //For each children the parent object has
            for (int i = 0; i < aParentObject.transform.childCount; i++)
            {
                //Get the children transform, and from it get its game object
                children[i] = (aParentObject.transform.GetChild(i)).gameObject;
            }

            //Return the children that were found
            return children;
        }

        return null;
    }

    /*
    Description: A helper function to get all the transforms of children game objects from a parent game object.
    Parameters: GameObject aParentObject - The gameobject that is the parent of the desired children objects.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, March 05, 2017
    */
    public static Transform[] GetChildrenTransfromFromParent(GameObject aParentObject)
    {
        //If the parent object is valid
        if (aParentObject != null)
        {
            //Create an array of transforms, accordign to its number of children
            Transform[] children = new Transform[aParentObject.transform.childCount];

            //For each children the parent object has
            for (int i = 0; i < aParentObject.transform.childCount; i++)
            {
                //Get the children transform, and from it get its game object
                children[i] = (aParentObject.transform.GetChild(i));
            }

            //Return the children transforms that were found
            return children;
        }

        return null;
    }

    /*
    Description: A helper function to get the gameobjects hit by a series of raycast hits
    Parameters: RaycastHit[] aRaycastHits - The raycast hits that potentially hit an object
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, March 04th, 2017
    */
    public static GameObject[] GetGameObjecsFromRaycastHits(RaycastHit[] aRaycastHits)
    {
        //Initalize array
        GameObject[] objectsHitByRaycast = new GameObject[aRaycastHits.Length];

        //Go through every element in the raycast hits
        for (int i = 0; i < aRaycastHits.Length; i++)
        {
            //Get the gameobject from the raycast hits
            objectsHitByRaycast[i] = aRaycastHits[i].collider.gameObject;
        }

        return objectsHitByRaycast;
    }

    /*
    Description: A helper function to get the gameobjects hit by a series of raycast hits
    Parameters: RaycastHit[] aRaycastHits - The raycast hits that potentially hit an object
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, March 04th, 2017
    */
    public static Vector3[] GetPositionsFromRaycastHits(RaycastHit[] aRaycastHits)
    {
        //Initalize array
        Vector3[] positionHitByRaycast = new Vector3[aRaycastHits.Length];

        //Go through every element in the raycast hits
        for (int i = 0; i < aRaycastHits.Length; i++)
        {
            //Get the positions from the raycast hits
            positionHitByRaycast[i] = aRaycastHits[i].point;
        }

        return positionHitByRaycast;
    }

    /*
    Description: Find the direction to face player and rotate the object
    Creator: Juan Calvin Raymond
    Creation Date: 9 Mar 2017
    */
    public static Quaternion LookAtPlayer(GameObject aGameObjectToRotate)
    {
        return Quaternion.LookRotation(aGameObjectToRotate.transform.position - CGameManager.PInstanceGameManager.PPlayerObject.transform.position);
    }

    /*
    Description: Helper function to play a particle system at a specified position, and using a specified normal.
    Parameters: Vector3 aHitPosition - 
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 28th, 2017
    */
    public static void PlayParticleSystemAtLocation(Vector3 aHitPosition, Vector3 aHitNormal, ParticleSystem aParticleSystem)
    {
        //If the particle system is valid
        if (aParticleSystem != null)
        {
            //Set the particles transform
            aParticleSystem.transform.rotation = Quaternion.LookRotation(aHitNormal);
            aParticleSystem.transform.position = aHitPosition;

            //Play the particle system
            aParticleSystem.Play();
        }
    }

    /*
    Description: Make the controller for the designed hand rumble.
    Parameters: EWeaponHand aHand - The controller hand that will rumble.
                float aDuration - The duration of the rumble.
                ushort aStrength - The strength of the rumble
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, March 25th, 2017
    */
    public static void RumbleControl(EWeaponHand aHand, float aDuration, ushort aStrength)
    {
        //Check which hand is holding the weapon
        switch (aHand)
        {
            case EWeaponHand.None:
                break;
            //If it's right hand
            case EWeaponHand.RightHand:
                //Vibrate right controller
                CGameManager.PInstanceGameManager.PPlayerController.SetRumbleController(aDuration, aStrength,
                    CGameManager.PInstanceGameManager.PPlayerController.PRightWeaponControl.PWeaponHand);
                break;
            //If it's left hand
            case EWeaponHand.LeftHand:
                //Vibrate left controller
                CGameManager.PInstanceGameManager.PPlayerController.SetRumbleController(aDuration, aStrength,
                    CGameManager.PInstanceGameManager.PPlayerController.PLeftWeaponControl.PWeaponHand);
                break;
            case EWeaponHand.BothHands:
                //Vibrate both controls
                RumbleControl(EWeaponHand.RightHand, aDuration, aStrength);
                RumbleControl(EWeaponHand.LeftHand, aDuration, aStrength);
                break;
            default:
                break;
        }
    }
}

