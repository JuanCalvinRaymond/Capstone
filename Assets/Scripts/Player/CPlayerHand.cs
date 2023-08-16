using UnityEngine;
using System.Collections;

/*
 Description: Class closely linked to the player class, the object of this class is to work as the actual 
 hands of the player. This class doesn't handle shooting the weapons, but rather it deals with the physics 
 interaction of the weapon with the player's hands.
 Creator: Alvaro Chavez Mixco
 */
public class CPlayerHand : MonoBehaviour
{
    private CPlayerWeaponHandler m_playerWeaponHandler;//The player that owns this hand
    private CWeaponPhysics m_callingBackWeaponPhysics;//The weapon the hand is currently calling

    private GameObject m_currentWeaponGameObject;
    private AWeapon m_currentWeaponScript;
    private CWeaponPhysics m_currentWeaponPhysics;//The weapon the hand is currently calling
    private CWeaponDataTracker m_currentWeaponDataTracker;

    [Tooltip("The distance from which the hand can grab the weapon.")]
    public float m_grabbingDistance = 0.25f;
    [Tooltip("Which hand this object represents.")]
    public EWeaponHand m_hand;

    public delegate void delegWeaponHeldChange(GameObject aWeaponGameObject, AWeapon aWeaponScript, CWeaponPhysics aWeaponPhysics, CWeaponDataTracker aWeaponDataTracker);
    public event delegWeaponHeldChange OnWeaponHeldChange;

    /*
    Description: At start it will get the player that owns this hand, and if it doesn't find
    any it will disable itself.
    Creator: Alvaro Chavez Mixco
    */
    private void Start()
    {
        //Get the player script
        m_playerWeaponHandler = GetComponentInParent<CPlayerWeaponHandler>();

        if (m_playerWeaponHandler == null)
        {
            enabled = false;
        }
    }

    /*
     Description: Every frame the funciton will check if the player has pressed the grab button,
     and accordingly will either call a new weapon or drop the current one. It will also check if 
     an unused weapon is in range
     Creator: Alvaro Chavez Mixco
     */
    private void Update()
    {
        //If the player has pressed the grabbed hand
        if (m_playerWeaponHandler.GetHandGrabbingWeapon(m_hand) == true)
        {
            //If the hand is not holding a weapon
            if (m_playerWeaponHandler.CheckIfWeaponHeld(m_hand) == false)
            {
                //Save the nearest weapon
                m_callingBackWeaponPhysics = FindNearestWeapon();

                //Call the nearest weapon to the hand
                CallWeapon(m_callingBackWeaponPhysics);
            }
            else//If the hand is holding a weapon
            {
                //Drop the current weapon
                DropWeapon();
            }
        }

        //Check if the player can grab a close weapon
        GrabWeaponIfClose();
    }

    /*
    Description: The function will check if the weapon that the player is calling is close by,
                 if it is it will grab it.
    Creator: Alvaro Chavez Mixco
    */
    private void GrabWeaponIfClose()
    {
        //If there is a weapon being called
        if (m_callingBackWeaponPhysics != null)
        {
            //If the weapon we are calling has not been grabbed
            if (m_callingBackWeaponPhysics.PWeaponPhysiscsState != EWeaponPhysicsState.Grabbed)
            {
                //If  the hand is not currently holding any weapon
                if (m_playerWeaponHandler.CheckIfWeaponHeld(m_hand) == false)
                {
                    //If the hand is within range
                    if (Vector3.Distance(transform.position, m_callingBackWeaponPhysics.transform.position) < m_grabbingDistance)
                    {
                        //Store values in new variables, since we can't pass properties by ref
                        GameObject weaponObject = m_callingBackWeaponPhysics.gameObject;

                        //Set the weapon in the player
                        GrabWeapon(weaponObject);
                    }
                }
            }
            else//If the weapon that was being called was grabbed by other hand
            {
                //Set the weapon that was being called as null
                m_callingBackWeaponPhysics = null;
            }
        }
    }

    /*
    Description: The function will find the nearest ungrabbed weapon
    Creator: Alvaro Chavez Mixco
    */
    private CWeaponPhysics FindNearestWeapon()
    {
        //Get all the game objects tagged as weapons, done at runt ime instead of start to account for starting inactive weapons (armory, etc.)
        GameObject[] listOfWeapons = GameObject.FindGameObjectsWithTag(CGlobalTags.M_TAG_WEAPON);

        //Clear the previous data
        if (m_callingBackWeaponPhysics != null)
        {
            //If the weapon wasn't grabbed
            if (m_callingBackWeaponPhysics.PWeaponPhysiscsState != EWeaponPhysicsState.Grabbed)
            {
                //Set the previous hand as dropped
                m_callingBackWeaponPhysics.DropWeapon();
            }
        }

        //If at least 1 weapon was found and the calling hand is valid
        if (listOfWeapons.Length > 0)
        {
            float sqrDistanceToWeapon = float.MaxValue;
            float shortestSqrDistanceToWeapon = float.MaxValue;
            CWeaponPhysics closestWeapon = null;
            CWeaponPhysics tempWeapon;

            //Go through all the weapons found
            for (int i = 0; i < listOfWeapons.Length; i++)
            {
                //If the weapon is valid
                if (listOfWeapons[i] != null)
                {
                    //Calculate the distance between the hand calling the weapon and the weapon found
                    sqrDistanceToWeapon = Vector3.SqrMagnitude(transform.position - listOfWeapons[i].transform.position);

                    //Get the weapon component
                    tempWeapon = listOfWeapons[i].GetComponent<CWeaponPhysics>();

                    //If it has a weapon component
                    if (tempWeapon != null)
                    {
                        //If the distance to this weapon is closest to the previously calculated one
                        if (sqrDistanceToWeapon < shortestSqrDistanceToWeapon && tempWeapon.PWeaponPhysiscsState == EWeaponPhysicsState.Dropped)
                        {
                            //Save this as the shortest distance
                            shortestSqrDistanceToWeapon = sqrDistanceToWeapon;

                            //Save this as the closest weapon
                            closestWeapon = tempWeapon;
                        }
                    }
                }
            }

            //Return the closest weapon found
            return closestWeapon;
        }
        else//If no weapons were found
        {
            return null;
        }
    }

    /*
    Description: The function will call the desired weapon, and set it to travel back to this hand
    Parameters: CWeaponPhysics aWeaponPhysicsToCall-The weapon that the hand will be calling back
    Creator: Alvaro Chavez Mixco
    */
    private void CallWeapon(CWeaponPhysics aWeaponPhysicsToCall)
    {
        //If the weapont is valid
        if (aWeaponPhysicsToCall != null)
        {
            //Call back the weapon
            aWeaponPhysicsToCall.CallWeapon(gameObject,m_hand);
        }
    }

    /*
    Description: Function to set a weapon as being grabbed by the player
    Parameters: GameObject aOriginalWeaponGameObject-The weapon game object that contains all the "weapon" scripts and components, 
                                                    besides the mesh
                Transform aHand - The game object that represents the player hands, and therefore which object is holding the weapon
                EWeaponHand aGrabbingHand - Which hand will hold the player (left or right)
                ref GameObject aHolderWeaponGameObject - The variable where the aOriginalWeaponGameObject will be stored.
                ref CWeaponPhysics aHolderWeaponPhysicsScript - The variable where we will save the CWeaponPhysics script that is stored in the weapon 
                                                game object
                ref CSelectingMenu aHolderSelectingMenu - The variable where we will save the CSelectingMenu script that is stored in 
                                                        the weapon game object
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    public void GrabWeapon(GameObject aWeaponBeingGrabbed)
    {
        //If the hand object is valid and the weapon we want to set is valid
        if (aWeaponBeingGrabbed != null)
        {
            //Save the original weapon game object into the holderWeaponGameObject variable
            m_currentWeaponGameObject = aWeaponBeingGrabbed;

            //Get CWeapon Component from the object prefab
            m_currentWeaponScript = m_currentWeaponGameObject.GetComponent<AWeapon>();

            //If there is a weapon physics
            if (m_currentWeaponScript != null)
            {
                //Set the weapon physics script
                m_currentWeaponPhysics = m_currentWeaponScript.PWeaponPhysics;
                m_currentWeaponDataTracker = m_currentWeaponScript.PWeaponDataTracker;
            }

            //If there is a weapon physics script
            if (m_currentWeaponPhysics != null)
            {
                //Grab the weapon
                m_currentWeaponPhysics.GrabWeapon(gameObject, m_hand);

                //Notify that the weapon being held has changed
                if (OnWeaponHeldChange != null)
                {
                    OnWeaponHeldChange(m_currentWeaponGameObject, m_currentWeaponScript, m_currentWeaponPhysics, m_currentWeaponDataTracker);
                }
            }
        }
    }

    /*
    Description: Function to set all the variables related to a weapon to null, and ensure that the weapon
    is set to the Dropped status.
    Parameters: ref GameObject aWeaponGameObject-The variable that stores the weapon game object
                ref AWeapon aWeaponScript- The variable that stores the weapon script component. If this is not null, this will be
                                            used to set the weapon status as dropped
                 ref CSelectingMenu aSelectingMenu- The variable that stores the weapon selecting menu component
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, December 23th, 2016
    */
    public void DropWeapon()
    {
        //Set every variable related to the weapon to null
        m_currentWeaponGameObject = null;

        //If there is a weapon script
        if (m_currentWeaponScript != null)
        {
            //Set the weapon as dropped
            m_currentWeaponScript.PWeaponPhysics.DropWeapon();
            m_currentWeaponScript = null;
        }

        m_currentWeaponPhysics = null;

        if (OnWeaponHeldChange != null)
        {
            OnWeaponHeldChange(m_currentWeaponGameObject, m_currentWeaponScript, m_currentWeaponPhysics, m_currentWeaponDataTracker);
        }
    }

    /*
    Description: Helper function to get the current weapon componetns from the player hand
    Parameters: ref GameObject aWeaponGameObjectHolder - The gameobject that will store the weapon game object
                ref AWeapon aWeaponScriptHolder - The script that will store the weapon script
                ref CWeaponPhysics aWeaponPhysicsHolder - The script that will store the weapon physics script
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2016
    */
    public void GetCurrentWeaponComponents(ref GameObject aWeaponGameObjectHolder, ref AWeapon aWeaponScriptHolder, ref CWeaponPhysics aWeaponPhysicsHolder, ref CWeaponDataTracker aWeaponDataTracker)
    {
        aWeaponGameObjectHolder = m_currentWeaponGameObject;
        aWeaponScriptHolder = m_currentWeaponScript;
        aWeaponPhysicsHolder = m_currentWeaponPhysics;
        aWeaponDataTracker = m_currentWeaponDataTracker;
    }
}
