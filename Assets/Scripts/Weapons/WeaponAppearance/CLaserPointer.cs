using UnityEngine;
using System.Collections;

/*
 Description: Class to draw a line, "laser" in the forward direction of the object. This will detect objects and stop the laser at that point.
 Creator: Juan Calvin Raymond
 Creation Date: Wednesday, November 2, 2016
 */
[RequireComponent(typeof(LineRenderer))]
public class CLaserPointer : MonoBehaviour
{
    private AWeapon m_weapon;
    private LineRenderer m_lineRenderer;
    private Ray m_ray;
    private RaycastHit m_raycastHit;

    public float m_laserRange;

    /*
     Description: Get the line renderer at the start of the game
     Creator: Juan Calvin Raymond
     Creation Date: Wednesday, November 2, 2016
     */
    private void Awake()
    {
        //Save line renderer component
        m_lineRenderer = GetComponent<LineRenderer>();

        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Suscribe to the event when we change the showing aiming aids
            CSettingsStorer.PInstanceSettingsStorer.OnIsShowingAimingAidsChange += EnableLaser;

            //PENDING
            m_weapon = GetComponentInParent<AWeapon>();
            if (m_weapon != null)
            {
                if (m_weapon.PWeaponPhysics != null)
                {
                    m_weapon.PWeaponPhysics.OnWeaponDropped += HideLasers;
                    m_weapon.PWeaponPhysics.OnWeaponGrabbed += ShowLaser;
                }

                if (m_weapon.PWeaponPhysiscsState == EWeaponPhysicsState.Grabbed)
                {
                    ShowLaser();
                }
                else
                {
                    HideLasers();
                }
            }
        }
        else//PENDING
        {
            enabled = false;
        }


    }

    /*
    Description: Unsuscribe from the setting events
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, November 21, 2016
    */
    private void OnDestroy()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe to the is showing aiming aids event
            CSettingsStorer.PInstanceSettingsStorer.OnIsShowingAimingAidsChange -= EnableLaser;
        }

        if (m_weapon != null)
        {
            if (m_weapon.PWeaponPhysics != null)
            {
                m_weapon.PWeaponPhysics.OnWeaponDropped -= HideLasers;
                m_weapon.PWeaponPhysics.OnWeaponGrabbed -= ShowLaser;
            }
        }
    }

    private void HideLasers()
    {
        EnableLaser(false);
    }

    private void ShowLaser()
    {
        EnableLaser(true);
    }
    private void EnableLaser(bool aShowStatus)
    {
        bool showLaserPhysicsSettiings = m_weapon.PWeaponPhysiscsState == EWeaponPhysicsState.Grabbed;
        bool showLaserStorerSettings = CSettingsStorer.PInstanceSettingsStorer.PIsShowingAimingAids;

        //Hide and unhides the line renderer component according to its status
        m_lineRenderer.enabled = aShowStatus && showLaserPhysicsSettiings && showLaserStorerSettings;
    }


    /*
     Description: Do a raycast from the position of this object, the raycast will determine the distance of the line renderer end point
     Creator: Juan Calvin Raymond
     Creation Date: Wednesday, November 2, 2016
     */
    private void Update()
    {
        //If the line renderer is showing up
        if (m_lineRenderer.enabled == true)
        {
            m_ray = new Ray(transform.position, transform.forward);//Set a ray forward
            if (Physics.Raycast(m_ray, out m_raycastHit, m_laserRange))//Do a raycast forward
            {
                m_lineRenderer.SetPosition(1, new Vector3(0, 0, m_raycastHit.distance));//If the raycast hit anything, set the distance of the line renderer to that point
            }
            else//The raycast didn't hit anything
            {
                m_lineRenderer.SetPosition(1, new Vector3(0, 0, m_laserRange));//Set the raycast to be the max range

            }
        }
    }
}
