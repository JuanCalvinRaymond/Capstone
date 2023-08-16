using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


/*
Description: Script which subscribe to weapon's OnFire event and will update text component
Creator: Juan Calvin Raymond
Creation Date: 30 Jan 2017
*/
[RequireComponent(typeof(MeshRenderer))]
public class CAmmoHUD : MonoBehaviour
{
    /*
    Description: Simple struct that contain color and percent threshold
    Creator: Juan Calvin Raymond
    Creation Date: 20 Mar 2017
    */
    [Serializable]
    public class CAmmoThreshold
    {
        [SerializeField]
        public Color m_HUDColor;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        public float m_minimalAmmoLeftToChangeColor;

    }

    //Weapon script
    private AWeapon m_weapon;

    //Text component
    private TextMesh m_text;

    //List of ammo threshold
    public List<CAmmoThreshold> m_listOfAmmoThreshold;

    // This object's text renderer.
    private MeshRenderer m_meshRenderer;
    
    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    private void Awake()
    {
        m_text = GetComponent<TextMesh>();
        m_weapon = GetComponentInParent<AWeapon>();
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    /*
    Description: Subscribe to weapon OnFire's event and initialize the text
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    private void Start()
    {
        if(m_weapon != null)
        {
            m_weapon.OnFire += OnFire;
            m_weapon.OnEndReload += OnReload;
            m_weapon.OnStartReload += OnReload;
            CUtilitySetters.SetTextMeshText(ref m_text, m_weapon.PCurrentAmmo.ToString());
            m_meshRenderer.material.color = m_listOfAmmoThreshold[0].m_HUDColor;
        }
    }

    /*
    Description: Unsubscribe to weapon OnFire's event
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    private void OnDestroy()
    {
        if (m_weapon != null)
        {
            m_weapon.OnFire -= OnFire;
            m_weapon.OnStartReload -= OnReload;
            m_weapon.OnEndReload -= OnReload;
        }
    }

    /*
    Description: Call change color function
    Parameter: aCurrentAmmo : weapon's current ammo
               aWeaponHand : hand that hold the weapon
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    private void OnFire(int aCurrentAmmo, EWeaponHand aWeaponHand)
    {
        ChangeColor(aCurrentAmmo);
    }

    /*
    Description: Call change color function
    Parameter: aCurrentAmmo : weapon's current ammo
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    private void OnReload(int aCurrentAmmo)
    {
        ChangeColor(aCurrentAmmo);
    }

    /*
    Description: Update the text and color based on weapon's current ammo
    Parameter: aCurrentAmmo : weapon's current ammo
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    private void ChangeColor(int aCurrentAmmo)
    {
        m_text.text = aCurrentAmmo.ToString();

        float ammoPercent = (float)aCurrentAmmo / (float)m_weapon.m_maxAmmo;

        foreach (var threshold in m_listOfAmmoThreshold)
        {
            if(ammoPercent <= threshold.m_minimalAmmoLeftToChangeColor)
            {
                m_text.color = threshold.m_HUDColor;
                m_meshRenderer.material.color = threshold.m_HUDColor;
            }
        }
    }
}
