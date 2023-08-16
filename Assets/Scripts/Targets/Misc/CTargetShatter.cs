using UnityEngine;
using System.Collections;
using System;

/*
Description: Class used to create shaterring effect on a target made of fragmented pieces.  The force
             applied to the shattered pieces is done using rigid bodies, and is relative to the damage the 
             target received.        
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, March 09th, 2017
Extra Notes:All the fragmented pieces must be children of this game object and possess a rigid body.
*/
public class CTargetShatter : MonoBehaviour
{
    //Cap the max force of the explosion to avoid unexepcted behavior, besidesthe fact it looks better
    private const float M_MAX_FORCE_ALLOWED = 1000.0F;

    private IShootable m_targetShootable;
    private Rigidbody[] m_shatteredPiecesRigidBodyRigidBody;
    private Transform[] m_shatteredPiecesTransform;
    private STransform[] m_shatteredPiecesOriginalTransform;

    [Tooltip("If we want the target collider to be disabled when it shatters.")]
    public bool m_disableCollider = true;
    [Tooltip("The radius of the explosion/shattering")]
    public float m_explosionRadius = 50.0f;
    [Tooltip("A multiplier to the damage the shattered piece receives /This is used when calculating how much force is applied to the piece" +
        "Note: To avoid unexpected behaviors, the total force (damage * multiplier) can't exceed the Max force allowed of 1000.0f")]
    public float m_targetExplosionDamageMultiplier = 1.0f;

    /*
    Description: Get the shattered pieces rigidbody, and suscribe to the onshot event         
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void Awake()
    {
        //Get the target shootable interface
        m_targetShootable = GetComponentInParent<IShootable>();

        //If the target is shootable
        if (m_targetShootable != null)
        {
            //Suscribe to events
            //Done in Awake, since the onshot event may be called immediately after the objet is created
            m_targetShootable.OnShot += ShatterTarget;

            //Get the rigid body of all the shattering pieces
            m_shatteredPiecesRigidBodyRigidBody = GetComponentsInChildren<Rigidbody>();

            //Get the pieces transform
            m_shatteredPiecesTransform = CUtilityGame.GetChildrenTransfromFromParent(gameObject);

            //Save the original transform of the pieces
            m_shatteredPiecesOriginalTransform = CUtilitySetters.GetTransformsValues(m_shatteredPiecesTransform);

        }
        else//If target is not shootable
        {
            //Disable component
            enabled = false;
        }
    }

    /*
    Description: If desired, disable the collider of the target        
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void Start()
    {
        //If we want the collider to be disabled
        if (m_disableCollider == true)
        {
            //Get the collider component, since this script is supposed to go
            //in a children damage state gameobject of the main target
            Collider targetCollider = GetComponentInParent<Collider>();

            //If the object has a collider
            if (targetCollider != null)
            {
                //Disable the collider
                targetCollider.enabled = false;
            }
        }
    }

    /*
    Description: When disabled, reset all the properties of the target.       
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void OnDisable()
    {
        Reset();
    }

    /*
    Description: Unsuscribe from target IShootable events        
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void OnDestroy()
    {
        //If there is a shootable target
        if (m_targetShootable != null)
        {
            //Unsuscribe from events
            m_targetShootable.OnShot -= ShatterTarget;
        }
    }

    /*
    Description: Reset the position, rotation and velocity of the target fragments        
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void Reset()
    {
        //If the shattered pieces have rigid body
        if (m_shatteredPiecesRigidBodyRigidBody != null)
        {
            //Go through all the rigid bodies
            for (int i = 0; i < m_shatteredPiecesRigidBodyRigidBody.Length; i++)
            {
                //Reset their velocities
                CUtilitySetters.ResetRigidBodyVelocities(ref m_shatteredPiecesRigidBodyRigidBody[i]);
            }
        }

        //If the transforms of the shattered pieces were saves
        if (m_shatteredPiecesOriginalTransform != null && m_shatteredPiecesTransform != null)
        {
            //Go through all of them
            for (int i = 0; i < m_shatteredPiecesTransform.Length; i++)
            {
                //Match the current trnasform of the object with its original transform
                CUtilitySetters.SetTransformLocalValues(m_shatteredPiecesTransform[i], m_shatteredPiecesOriginalTransform[i]);
            }
        }
    }

    /*
    Description: Apply explosion force to all the fragments rigid bodies       
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    private void ShatterTarget(GameObject aHitter, int aDamage, Vector3 aHitPosition, Vector3 aHitDirection)
    {
        //Go through all the shaterred pieces
        for (int i = 0; i < m_shatteredPiecesRigidBodyRigidBody.Length; i++)
        {
            //Get the force according to weapon damage
            float force = m_targetExplosionDamageMultiplier * aDamage;

            //To avoid unusual/unexpected behaviors. Clamp the max force that can be applied
            force = Mathf.Clamp(force, -M_MAX_FORCE_ALLOWED, M_MAX_FORCE_ALLOWED);

            //Apply explosion force to each piece
            m_shatteredPiecesRigidBodyRigidBody[i].AddExplosionForce(
                force, 
                aHitPosition, m_explosionRadius,
                0.0F, ForceMode.Impulse);
        }
    }
}
