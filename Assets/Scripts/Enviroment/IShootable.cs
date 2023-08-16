using UnityEngine;
using System.Collections;

//Delegate event for when an object is shot
public delegate void delegOnObjectShot(GameObject aHitter, int aDamage, Vector3 aHitPosition, Vector3 aHitDirection);

/*
Description: Basic interface that can be used to get call back events from hitscan
        (raycast) weapons and proejctiles shots.
Creator: Alvaro Chavez Mixco
*/
public interface IShootable
{    
    event delegOnObjectShot OnShot;

    /*
    Description: Returns if the shootable object allows the shot to pass through it.        
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 21, 2017
    */
    bool GetIsBlockingShot();

    /*
    Description: Function called when the object is shot.
    Parameters:  GameObject aHitter - The object that shot the shootable object
                 int aDamage - The damage of the shot
                 Vector3 aHitPosition - The position where the object was shot
                 Vector3 aHitDirection - The direction from where the shootable object was shot
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 21, 2017
    */
    void ObjectShot(GameObject aHitter, int aDamage, Vector3 aHitPosition, Vector3 aHitDirection);
}
