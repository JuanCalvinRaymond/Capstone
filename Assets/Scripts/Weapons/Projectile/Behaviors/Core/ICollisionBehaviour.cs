using UnityEngine;
using System.Collections;

/*
Description: Projectile's collision behaviour's interface
Creator: Juan Calvin Raymond
Creation Date: 11-22-2016
*/
public interface ICollisionBehaviour
{
    /*
    Description: Initialize the required properties from the projectile.
    Parameters: CProjectile aProjectile - The projectile initializing the behavior
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    void Init(CProjectile aProjectile);

    /*
    Description: Activate different behaviour when the projectile collided. Returns true if a target was hit or not by the collision
    Parameters: aBullet : bullet script
                aHitPos : position that the bullet hit
                aNormal : the normal of point that the bullet hit
                aCollider : collider of the object that bullet hit
                out bool aIsDeadDueCollision - Returns if the collision killed the projectile
    Creator: Juan Calvin Raymond
    Creation Date: 11-22-2016
    */
    bool CollisionHandle(CProjectile aProjectile, Vector3 aHitPos, Vector3 aNormal, Collider aCollider,out bool aIsDeadDueCollision);

}
