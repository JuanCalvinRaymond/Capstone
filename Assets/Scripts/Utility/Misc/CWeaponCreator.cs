using UnityEngine;
using System.Collections;

/*
Description: Script to create a weapon prefab according to the weapon type being passed.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CWeaponCreator : MonoBehaviour
{
    [Tooltip("The list of supported weapons for creations.")]
    public GameObject[] m_prefabs;

    [Tooltip("The prefab that will be returned if the weapon type being asked for was not found.")]
    public GameObject m_defaultWeapon;

    /*
    Description: Go through the list of weapon prefabs, and if there is one missing the requested
                 weapon type, return it.
    Paramters: EWeaponTypes aWeaponType - The type of weapon we wish to create.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public GameObject GetWeaponPrefab(EWeaponTypes aWeaponType)
    {
        AWeapon weaponComponent;

        //If the prefab is valid
        if (m_prefabs != null)
        {
            //Go through all the prefabs
            for (int i = 0; i < m_prefabs.Length; i++)
            {
                //If current prefab is null
                if (m_prefabs[i] != null)
                {
                    //Get their weapon component
                    weaponComponent = m_prefabs[i].GetComponent<AWeapon>();

                    //If the weapon component is valid
                    if (weaponComponent != null)
                    {
                        //If the weapon type matches the oe of the desired prefab
                        if (aWeaponType == weaponComponent.PWeaponType)
                        {
                            //Return the prefab
                            return m_prefabs[i];
                        }
                    }
                }
            }
        }

        return m_defaultWeapon;
    }
}
