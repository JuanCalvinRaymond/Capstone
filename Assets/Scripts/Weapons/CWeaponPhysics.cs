using UnityEngine;
using System.Collections;

/*
Description: Class used to handle the physics (Grabbing, Dropping, and Calling back) aspects of a weapon.
             This classs also handles the movement of the weapon toward the player hand, and its downward speed.
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, March 4th, 2017
Extra Notes: This class works in close relation with the CPlayerHand class, in order to call functions.
*/
[RequireComponent(typeof(Rigidbody), typeof(AWeapon))]
public class CWeaponPhysics : MonoBehaviour
{
    //Components
    private AWeapon m_weapon;
    private Rigidbody m_rigidBody;
    private Collider[] m_colliders;

    //Hand variables
    private EWeaponHand m_holdingHand;
    private GameObject m_holdingHandGameObject;

    //Physic variables
    private EWeaponPhysicsState m_physicsState = EWeaponPhysicsState.Dropped;
    private Vector3 m_rotationOffset;
    private Vector3 m_velocity;
    private Vector3 m_angularVelocity;

    private Vector3[] m_previousWeaponPositions;
    private Vector3[] m_previousWeaponRotations;

    [Header("Physics")]
    public float m_potentialGrabbedVelocityMultiplier = 1000.0f;
    public float m_potentialGrabbedAngularVelocityMultiplier = 100.0f;
    [Range(0.01f, float.MaxValue)]
    public float m_massFactor = 1.0f;
    [Tooltip("By how much the  linear velocity of the weapon will be multiplied when the weapon is released.")]
    public float m_releaseVelocityMultiplier = 1.5f;
    [Tooltip("By how much the  angular velocity of the weapon will be multiplied when the weapon is released.")]
    public float m_releaseAngularVelocityMultiplier = 2.0f;
    // How many past position sample frames to store data for.
    [Range(1, 90)]
    public int m_transformFrameSamples = 4;

    //Variables to control the rotation of the gun when grabbed
    [Header("Offset Rotation")]
    public Vector3 m_gunVROffsetRotation = new Vector3(45.0f, 0.0f, 0.0f);
    public Vector3 m_gunNonVROffsetRotation = new Vector3(-45.0f, 0.0f, 0.0f);

    public delegate void delegPhysicStateChanges();
    public event delegPhysicStateChanges OnWeaponGrabbed;
    public event delegPhysicStateChanges OnWeaponDropped;
    public event delegPhysicStateChanges OnWeaponTravelingBack;

    private Vector3 POldestPositionSample
    {
        get
        {
            return m_previousWeaponPositions[m_transformFrameSamples - 1];
        }
    }

    private Vector3 POldestRotationSample
    {
        get
        {
            return m_previousWeaponRotations[m_transformFrameSamples - 1];
        }
    }

    public AWeapon PWeapon
    {
        get
        {
            return m_weapon;
        }
    }

    public EWeaponPhysicsState PWeaponPhysiscsState
    {
        get
        {
            return m_physicsState;
        }
    }

    public GameObject PHoldingHandGameObject
    {
        get
        {
            return m_holdingHandGameObject;
        }
    }

    public EWeaponHand PHoldingHand
    {
        get
        {
            return m_holdingHand;
        }
    }

    public Vector3 PVelocity
    {
        get
        {
            return m_velocity;
        }
    }

    public Vector3 PAngularVelocity
    {
        get
        {
            return m_angularVelocity;
        }
    }

    /*
    Description: Set the rotation offset and get all the required components
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, March 4th, 2017
    */
    private void Awake()
    {
        if (CSettingsStorer.PInstanceSettingsStorer.PInputMethod == EControllerTypes.ViveController)
        {
            m_rotationOffset = m_gunVROffsetRotation;
        }
        else
        {
            m_rotationOffset = m_gunNonVROffsetRotation;

        }

        //Get the rigid body component and the collider
        m_rigidBody = GetComponent<Rigidbody>();
        m_colliders = GetComponentsInChildren<Collider>();

        //Get the weapon compononent
        m_weapon = GetComponent<AWeapon>();

        // Initialize past weapon positions.
        m_previousWeaponPositions = new Vector3[m_transformFrameSamples];
        m_previousWeaponRotations = new Vector3[m_transformFrameSamples];

        //Initalize the sample positions to the starting position
        for (int i = 0; i < m_transformFrameSamples; i++)
        {
            m_previousWeaponPositions[i] = transform.position;
            m_previousWeaponRotations[i] = transform.eulerAngles;
        }
    }

    /*
    Description: Reset the rigid body
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void OnDisable()
    {
        //Reset linear and angular velocities of the rigid body
        CUtilitySetters.ResetRigidBodyVelocities(ref m_rigidBody);
    }

    /*
    Description: Will run the corresponding fixed update functions according to the physic state of the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    */
    protected virtual void FixedUpdate()
    {
        //According to the weapon physics state, run the correct fixed update method
        switch (m_physicsState)
        {
            case EWeaponPhysicsState.Grabbed:
                FixedUpdateGrabbed();
                break;
            case EWeaponPhysicsState.TravelingBack:
                FixedUpdateTravelingBack();
                break;
            case EWeaponPhysicsState.Dropped:
                FixedUpdateDropped();
                break;
            default:
                break;
        }
    }

    /*
    Description: Fixed Update function for when the weapon is grabbed.This would move,without using the rigid body, the weapon to
                 match the player rotation and position
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    */
    protected virtual void FixedUpdateGrabbed()
    {
        //Set the velocity of the rigid body, so that they are updated even when weapon is grabbed.
        //Set velocity before setting it to sleep to ensure it doesn't wake it
        m_rigidBody.velocity = m_velocity;
        //Check that all the values in the vector3 are valid, since the ToAngleAxis function
        //may cause issues
        if (CUtilityMath.IsNaN(m_angularVelocity) == false)
        {
            //Set the rigid velocity of the object
            m_rigidBody.angularVelocity = m_angularVelocity;
        }

        //Disable rigidbody while being held.
        m_rigidBody.Sleep();

        //Move the weapon to player hand
        MatchHandTransform();
    }

    /*
    Description: Fixed Update function for when the weapon is traveling back to the player.This would move, using the weapon rigid body, the weapon to
                 match the player rotation and position
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    */
    protected virtual void FixedUpdateTravelingBack()
    {
        //Move the weapon to the player hand
        MatchHandTransformRigidBody();
    }

    /*
    Description: Fixed Update function for when the weapon is dropped.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    */
    protected virtual void FixedUpdateDropped()
    {
    }

    /*
     * Description: Cycles all previous position samples to the right, then updates position 0 with the current position.
     * Creator: Charlotte C. Brown
     */
    private void TrackWeaponPosition(Vector3 aWeaponPosition)
    {
        // Move each sample to the right.
        for (int i = m_transformFrameSamples - 1; i > 0; i--)
        {
            m_previousWeaponPositions[i] = m_previousWeaponPositions[i - 1];
        }

        // Get current position.
        m_previousWeaponPositions[0] = aWeaponPosition;
    }

    private void TrackWeaponRotation(Vector3 aWeaponRotation)
    {
        // Move each sample to the right.
        for (int i = m_transformFrameSamples - 1; i > 0; i--)
        {
            m_previousWeaponRotations[i] = m_previousWeaponRotations[i - 1];
        }

        // Get current position.
        m_previousWeaponRotations[0] = aWeaponRotation;
    }

    /*
    Description:Funtion to move and rotate the weapon to match the hand that is currently holding it. This is using the rigid body velocities.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    Extra Notes: This function doesn't work if the game is with a time scale of 0
    */
    private void MatchHandTransformRigidBody()
    {
        //Move weapon
        MoveToHandRigidBody();

        //Rotate weapon
        RotateToHandRigidBody();
    }

    /*
    Description: Funtion to move  the weapon to match the hand that is currently holding it. This is using the rigid body velocities.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    Extra Notes: This function doesn't work if the game is with a time scale of 0
    */
    private void MoveToHandRigidBody()
    {
        //Get the direction toward the hand
        Vector3 directionToHand = m_holdingHandGameObject.transform.position - transform.position;

        //Calculate the velocity
        m_velocity = directionToHand / (m_rigidBody.mass * m_massFactor);

        //Set the velocity of the rigid body
        m_rigidBody.velocity = m_velocity;
    }

    /*
    Description: Funtion to rotate  the weapon to match the hand that is currently holding it. This is using the rigid body velocities.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Thursday, December 2, 2016
    Extra Notes: This function doesn't work if the game is with a time scale of 0
    */
    private void RotateToHandRigidBody()
    {
        //Calculate the rotation we hand according to hand and offset
        Vector3 targetRotation = m_holdingHandGameObject.transform.rotation.eulerAngles + m_rotationOffset;

        //Get the difference in rotation between the target rotation and current rotation
        Quaternion differenceRotation = Quaternion.Euler(targetRotation) * Quaternion.Inverse(transform.rotation);

        float angle = 0.0f;
        Vector3 axis = Vector3.zero;

        //Get the amount of rotation in an axis
        CUtilityMath.RotationToAngleAxis360DegreesLimit(out angle, out axis, differenceRotation);

        //Calculate the angular vleocity of the  object
        m_angularVelocity = ((angle * axis) * Time.fixedDeltaTime
            / (m_rigidBody.mass * m_massFactor));

        //Check that all the values in the vector3 are valid, since the ToAngleAxis function
        //may cause issues
        if (CUtilityMath.IsNaN(m_angularVelocity) == false)
        {
            //Set the rigid velocity of the object
            m_rigidBody.angularVelocity = m_angularVelocity;
        }
    }

    /*
    Description:Funtion to move and rotate the weapon to match the hand that is currently holding it. This is done without a rigid body.
    Creator: Charlotte Brown
    Creation Date: Friday, March 24th, 2017
    */
    private void MatchHandTransform()
    {
        //Move weapon
        MoveToHand();

        //Rotate weapon
        RotateToHand();
    }

    /*
    Description: Funtion to move  the weapon to match the hand that is currently holding it.
    Creator: Charlotte Brown
    Creation Date:  Thursday, December 2, 2016
    Extra Notes:  This is not using the rigid body velocities.
    */
    private void MoveToHand()
    {
        //Get the position of the platform in world space
        Vector3 platformWorldPos = CGameManager.PInstanceGameManager.PPlayerObject.transform.position;

        //Make the hand position of the hand to "local space" manually by substracting the platform world position from it
        Vector3 handPosLocalPosition = m_holdingHandGameObject.transform.position - platformWorldPos;

        //Get the velocity,direction and speed toward the hand
        Vector3 velocity = handPosLocalPosition - POldestPositionSample;

        //Calculate the velocity
        m_velocity = velocity * m_potentialGrabbedVelocityMultiplier * Time.fixedDeltaTime;

        //Set the velocity of the game object manually
        transform.position = m_holdingHandGameObject.transform.position;

        //Save current position, and transform the world rotation to "local" rotation by affecting by platform rotation
        TrackWeaponPosition(transform.position - platformWorldPos);
    }

    /*
    Description: Funtion to rotate  the weapon to match the hand that is currently holding it.
    Creator: Charlotte Brown
    Creation Date:  Thursday, December 2, 2016
    Extra Notes:  This is not using the rigid body velocities.
    */
    private void RotateToHand()
    {
        //Get the rotation of the platform in world space
        Quaternion platformWorldRot = CGameManager.PInstanceGameManager.PPlayerObject.transform.rotation;

        //Get the hand rotation of the object in world space, and manually convert it to local space
        //by substracting the world platform rotation from it. Then, once its in local space, add the desired
        //offset from it.
        Quaternion localhandRot = (m_holdingHandGameObject.transform.rotation * Quaternion.Euler(m_rotationOffset)) * Quaternion.Inverse(platformWorldRot) ;

        //Calculate the difference between the current hand rotation and the previos frame hand rotation.
        Quaternion differenceRotation = localhandRot * Quaternion.Inverse(Quaternion.Euler(POldestRotationSample));

        float angle = 0.0f;
        Vector3 axis = Vector3.zero;

        //Get the amount of rotation in an axis
        CUtilityMath.RotationToAngleAxis360DegreesLimit(out angle, out axis, differenceRotation);

        //Calculate the angular vleocity of the  object
        m_angularVelocity = (angle * axis) * m_potentialGrabbedAngularVelocityMultiplier * Time.fixedDeltaTime;

        //Set rotation to player's hand.
        transform.rotation = m_holdingHandGameObject.transform.rotation * Quaternion.Euler(m_rotationOffset);

        //Save current position, and transform the world rotation to "local" rotation by affecting by platform rotation
        TrackWeaponRotation((transform.rotation * Quaternion.Inverse(platformWorldRot)).eulerAngles);
    }

    /*
    Description: Multiply the weapon velocity, and update the velocity in the rigid body. This
                 doesn't set the rigid body velocities.
    Parameters: float aVelocityMultiplier - By how much the velocity will be multiplied
                float aAngularMultiplier - By how much the angular velocity will be multiplied
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public void MultiplyVelocities(float aVelocityMultiplier, float aAngularMultiplier)
    {
        //Multiply linear velocity
        m_velocity *= aVelocityMultiplier;

        //Multiply angular velocity
        m_angularVelocity *= aAngularMultiplier;
    }

    /*
    Description: Set the holding hand for the weapon
    Parameters: GameObject aHoldingHandGameObject - The gameobject holding the weapon
                EWeaponHand aWeaponHand - Which hand is holding the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    private void SetHoldingHand(GameObject aHoldingHandGameObject, EWeaponHand aWeaponHand)
    {
        m_holdingHandGameObject = aHoldingHandGameObject;
        m_holdingHand = aWeaponHand;
    }

    /*
    Description: Grab the weapon, enabling its collider and making it follow the holding hand gameobject.
    Parameters: GameObject aHoldingHandGameObject - The gameobject holding the weapon
                EWeaponHand aWeaponHand - Which hand is holding the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public void GrabWeapon(GameObject aHoldingHandGameObject, EWeaponHand aHand)
    {
        //Check that the holding hand is valid and that the hand holding is not set to none
        if (aHoldingHandGameObject != null && aHand != EWeaponHand.None)
        {
            //Set weapon as grabbed
            m_physicsState = EWeaponPhysicsState.Grabbed;

            //Set the weapon holding hand
            SetHoldingHand(aHoldingHandGameObject, aHand);

            //Enable collider
            CUtilitySetters.SetEnabledColliders(m_colliders, true);

            //Disable gravity
            m_rigidBody.useGravity = false;
            
            //If event is valid
            if (OnWeaponGrabbed != null)
            {
                //Call event
                OnWeaponGrabbed();
            }
        }
    }

    /*
    Description: Call the weapon to the hand, disabling its collider and making it go toward the players hand object
                 using its rigid body.
    Parameters: GameObject aHoldingHandGameObject - The gameobject holding the weapon
                EWeaponHand aWeaponHand - Which hand is holding the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public void CallWeapon(GameObject aHoldingHandGameObject, EWeaponHand aHand)
    {
        //Check that the holding hand is valid and that the hand holding is not set to none
        //if (aHoldingHandGameObject != null && aHand != EWeaponHand.None)
        {
            //Set the weapon as being called
            m_physicsState = EWeaponPhysicsState.TravelingBack;

            //Set the hand gameobject holding the weapon
            SetHoldingHand(aHoldingHandGameObject, aHand);

            //Disable collider
            CUtilitySetters.SetEnabledColliders(m_colliders, false);

            //Disable gravity
            m_rigidBody.useGravity = false;

            //If event is valid
            if (OnWeaponTravelingBack != null)
            {
                //Call event
                OnWeaponTravelingBack();
            }
        }
    }

    /*
    Description: Drop the weapon, enablig its collider and making it fall due gravity
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public void DropWeapon()
    {
        //Set the weapon physic state as dropepd
        m_physicsState = EWeaponPhysicsState.Dropped;

        //Set that no hand is holding the weapon
        SetHoldingHand(null, EWeaponHand.None);

        //Enable collider
        CUtilitySetters.SetEnabledColliders(m_colliders, true);

        //Enable gravity
        m_rigidBody.useGravity = true;

        //Ensure the rigid body max angular velocity is set to 0
        m_rigidBody.maxAngularVelocity = Mathf.Infinity;

        //Affect the velocity of the weapon
        MultiplyVelocities(m_releaseVelocityMultiplier, m_releaseAngularVelocityMultiplier);

        //Set the rigid body velocities
        CUtilitySetters.SetRigidBodyVelocities(ref m_rigidBody, m_velocity, m_angularVelocity);
        
        //If event is valid
        if (OnWeaponDropped != null)
        {
            //Call event
            OnWeaponDropped();
        }

    }
}
