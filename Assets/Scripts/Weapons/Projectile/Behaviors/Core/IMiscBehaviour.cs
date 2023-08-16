using UnityEngine;
using System.Collections;

/*
Description: Projectile's misc behaviour's interface
Creator: Juan Calvin Raymond
Creation Date: 11-24-2016
*/
public interface IMiscBehaviour
{
    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    void Init(CProjectile aProjectile);
    
    /*
    Description: Update any behaviour that doesn't match Motion, Collision, or Death
    Parameters(Optional): aProjectile : projectile that own this script
    Creator: Juan Calvin Raymond
    Creation Date: 11-24-2016
    */
    void UpdateMisc(CProjectile aProjectile);

}
