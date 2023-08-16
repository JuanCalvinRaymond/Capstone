using UnityEngine;
using System.Collections;

/*
Description: Bullet's death behaviour's interface
Creator: Juan Calvin Raymond
Creation Date: 11-24-2016
*/
public interface IDeathBehaviour
{
    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    void Init(CProjectile aProjectile);
    
    /*
    Description: Update the behaviour when the projectile is dead
    Parameters: aProjectile : projectile that own this script
    Creator: Juan Calvin Raymond
    Creation Date: 11-24-2016
    Extra Notes:
    */
    void UpdateDeath(CProjectile aProjectile);

    void OnProjectileKilled(bool aCollisionDeath);

}
