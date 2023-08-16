using UnityEngine;
using System.Collections;

//Delegate for event when target is damaged
public delegate void delegTargetDamaged(int aDamagedAmount, int aHealthRemaining, float aHealthPercent, int aScoreValue);

//Delegate for event when target is set to be dying
public delegate void delegTargetDying(float aDyingTime);

//Delegate for event when target is reset
public delegate void delegTargetReset();

/*
Description: Interface that the targets object will have, this is made in order to support a health system, that serves for the targets to have damaged states.
Creator: Alvaro Chavez Mixco
Creation Date: October 8, 2016
*/
public interface ITarget
{
    int PScoreValue { get; set; }

    int PMaxHealth { get; set; }

    int PHealth { get; }
    GameObject PObjectThatHit { get; }

    event delegTargetDamaged OnTargetDamaged;
    event delegTargetDying OnTargetDying;
    event delegTargetReset OnTargetReset;

    /*
    Description:This function should be called when the target is destroyed.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    void DestroyTarget();

    /*
    Description:Function to apply damage to the target.
    Creator: Alvaro Chavez Mixco
    Creation Date: October 8, 2016
    */
    void ApplyDamage(int aAmount);

    /*
    Description: Function to reset a target variables and conditions.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    void Reset();
}
