using UnityEngine;
using System.Collections;
  
/*
Description: Function to make the player controller's rumble when he shoots his weapon.
Creator: Alvaro Chavez Mixco
Creation Date: Sunday, March 12th, 2017
*/
[RequireComponent(typeof(AWeapon))]
public class CWeaponShotRumble : MonoBehaviour
{
    protected AWeapon m_weapon;

    [Range(0, 3999)]
    public ushort m_shootingRumbleStrength = 500;
    public float m_shootingRumbleDuration = 0.25f;

    /*
    Description: Get the weapon component
    Creator: Alvaro Chavez Mixco
    Creation Date: 20 Dec 2016
    Extra Notes:
    */
    private void Awake()
    {
        //Get weapon script
        m_weapon = GetComponent<AWeapon>();
    }

    /*
    Description: Subscribe to weapon OnFire event
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, March 12th, 2017
    */
    protected virtual void Start()
    {
        m_weapon.OnFire += FireRumble;
    }

    /*
    Description: Unsubscribe to weapon OnFire event
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, March 12th, 2017
    */
    protected void OnDestroy()
    {
        m_weapon.OnFire -= FireRumble;
    }

    /*
    Description: When the weapon is shot make the controller in the hand that shot the weapon vibrate,
                 using the parameters stored in the weapon.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, March 12th, 2017
    */
    protected virtual void FireRumble(int aCurrentAmmo, EWeaponHand aWeaponHand)
    {
        //Make the controller rumble
        CUtilityGame.RumbleControl(aWeaponHand, m_shootingRumbleDuration, m_shootingRumbleStrength);
    }
}
