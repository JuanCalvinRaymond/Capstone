using UnityEngine;
using System.Collections;
using System;

/*
Description: Moving forward by applying speed on firing direction
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
*/
[RequireComponent(typeof(CProjectile))]
public class CProjMovingForwardMotion : MonoBehaviour, IMotionBehaviour
{
    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    public void Init(CProjectile aProjectile)
    {
    }

    /*
    Description: Update the position of projectile
    Parameters: CProjectile aProjectile - Projectile that owns the behavior
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    public void UpdateMotion(CProjectile aProjectile)
    {
        //multiply speed with fire direction
        //Constant velocity
        Vector3 velocity = aProjectile.PVelocity + (aProjectile.transform.forward * aProjectile.PSpeed);

        //change bullet's position by velocity
        aProjectile.gameObject.transform.position += velocity * CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        //Update speed
        aProjectile.PSpeed = velocity.magnitude;
    }
}
