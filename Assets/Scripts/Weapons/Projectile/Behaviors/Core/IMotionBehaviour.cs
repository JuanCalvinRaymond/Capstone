using UnityEngine;
using System.Collections;

/*
Description: Projectile's motion behaviour's interface
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
*/
public interface IMotionBehaviour
{
    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    void Init(CProjectile aProjectile);

    /*
    Description: Update the position of projectile
    Parameters: CProjectile aProjectile : projectile script
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    void UpdateMotion(CProjectile aProjectile);

}
