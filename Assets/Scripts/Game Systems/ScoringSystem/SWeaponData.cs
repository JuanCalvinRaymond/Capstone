using UnityEngine;
using System.Collections;

/*
Description:Struct used to organize and store in a simple way all the data
of a weapon that is needed by the scoring system.
Creator: Alvaro Chavez Mixco
Extra Notes: The scoring system keeps lists of SWeaponData registered each
frame in order to determine when a trick is made.
*/
public struct SWeaponData
{
    public Vector3 m_weaponForwardDirection;
    public Vector3 m_playerForwardDirection;

    public Vector3 m_weaponPosition;
    public Vector3 m_playerPosition;

    public Vector3 m_weaponRotation;
    public Vector3 m_playerRotation;
    public Quaternion m_playerQuaternion; 
    
    public float m_timeRegisteredToTheList;

    public Vector3 m_linearVelocity;
    public Vector3 m_angularVelocity;


    public EWeaponHand m_holdingHand;
    public EWeaponPhysicsState m_physicState;

    public bool m_active;
}
