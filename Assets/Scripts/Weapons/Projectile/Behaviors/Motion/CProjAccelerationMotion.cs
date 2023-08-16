using UnityEngine;
using System.Collections;
using System;


/*
Description: Behavior of projectile that works based on a  starting acceleration through easing, until the
             projectile eventually deccelerates
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
public class CProjAccelerationMotion : MonoBehaviour, IMotionBehaviour
{
    private float m_propulsionTime;
    private float m_timerPropulsionTime;
    private float m_totalInitialLifeTime;
    private Vector3 m_velocity = Vector3.zero;
    private float m_currentExhaustSpeed;
    private Vector3 m_gravity;

    private CEase m_accelEase;
    private CEase m_deccelEase;

    [Header("Speed settings")]
    public float m_maxExhaustSpeed = 10.0f;
    public float m_rotationSpeed = 5.0f;
    public bool m_gravityOnlyWhenTimeOver = true;

    [Range(0.0f, 1.0f)]
    public float m_accelerationLifePercent = 0.5f;

    [Range(0.0f, 1.0f)]
    public float m_deccelerationLifePercent = 0.5f;

    [Range(0.0f, 1.0f)]
    public float m_propulsionTimePercent = 0.8f;

    [Header("Easing settings")]
    public SEaseFunctionSettings m_accelerationEaseSettings;
    public SEaseFunctionSettings m_deccelerationEaseSettings;

    public float m_accelerationEaseSpeed = 1.0f;
    public float m_deccelerationEaseSpeed = 1.0f;

    /*
    Description: Initialize eases
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    private void Awake()
    {
        //Create the eases
        m_accelEase = new CEase(m_accelerationEaseSettings.m_easeType, m_accelerationEaseSettings.m_easeMode);
        m_accelEase.SetEaseSpeed(m_accelerationEaseSpeed);

        m_deccelEase = new CEase(m_deccelerationEaseSettings.m_easeType, m_deccelerationEaseSettings.m_easeMode);
        m_deccelEase.SetEaseSpeed(m_deccelerationEaseSpeed);

        //Get the gravity from the physiscs settings
        m_gravity = Physics.gravity;
    }

    /*
    Description: Event called when a projectile is fired. It sets starting values for projectile motion.
    Parameters: CProjectile aProjectile - The projectile initializing this behavior
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void Init(CProjectile aProjectile)
    {
		//Set initial values
		m_velocity = Vector3.zero;//aProjectile.PVelocity;
        m_propulsionTime = aProjectile.PLifeTime * m_propulsionTimePercent;
        m_timerPropulsionTime = 0.0f;
        m_currentExhaustSpeed = aProjectile.PSpeed;

        //If the acceleration ease is not currently running
        if (m_accelEase.IsEasing() == false)
        {
            //Set it
            m_accelEase.SetEase(m_currentExhaustSpeed, m_maxExhaustSpeed - m_currentExhaustSpeed,
            m_propulsionTime * m_accelerationLifePercent - m_timerPropulsionTime, this, m_accelerationEaseSettings.m_extraParameter);

            //Run ease
            m_accelEase.Run();
        }
    }

    /*
    Description: Update the projectile motion using a rocket thrust equation, combined with easing
    Parameters: CProjectile aProjectile - The projectile updating this behavior
    Creator: Alvaro Chavez Mixco
    Creation Date : Thursday, February 23rd, 2017
    */
    public void UpdateMotion(CProjectile aProjectile)
    {
        //https://en.wikipedia.org/wiki/Tsiolkovsky_rocket_equation
        //Change of Velocity=exhaust velocity  * natural Logarithm function * (inital mass/final Mass)
        
        //If the projectile is currently accelerating
        if (m_timerPropulsionTime < m_propulsionTime * m_accelerationLifePercent)
        {
            //Set the speed to match the acceleration ease speed
            m_currentExhaustSpeed = m_accelEase.GetValue();

            if (m_gravityOnlyWhenTimeOver == false)
            {
                m_velocity += m_gravity;
            }

        }
        else if (m_timerPropulsionTime < m_propulsionTime * m_deccelerationLifePercent)
        {
            //Keep constant thrust speed
            //Except for gravity
            if (m_gravityOnlyWhenTimeOver == false)
            {
                m_velocity += m_gravity;
            }
        }
        else if (m_timerPropulsionTime < m_propulsionTime)//If the projectile is deccelerating
        {
            //If the deceleration ease is not running
            if (m_deccelEase.IsEasing() == false)
            {
                //Set ease
                m_deccelEase.SetEaseFinalValue(m_currentExhaustSpeed, 0.0f, (m_propulsionTime * m_deccelerationLifePercent), this);

                //Run ease
                m_deccelEase.Run();
            }

            //Make current speed based on deccel ease
            m_currentExhaustSpeed = m_deccelEase.GetValue();

            m_velocity += m_gravity * Time.deltaTime * m_rotationSpeed;
        }
        else//If the projectile is no longer propulsing
        {
            //Set exhaust speed to 0
            m_currentExhaustSpeed = 0.0f;

            //Apply gravity
            m_velocity += m_gravity * Time.deltaTime * m_rotationSpeed;
        }

        //Increase the velocity of the object base on current exhaust speed
        //Change of Velocity=exhaust velocity  * natural Logarithm function * (inital mass/final Mass)
        m_velocity += (transform.forward * m_currentExhaustSpeed) * (float)Math.Log(Math.E) * 1.0f;

        //If the object has velocity
        if (m_velocity != Vector3.zero)
        {
            //Slerp the object to the desired rotation (lookrotation) according to velocity
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(m_velocity),
                Time.deltaTime * m_rotationSpeed
            );
        }

        //Move projectile
        aProjectile.gameObject.transform.position += m_velocity * Time.deltaTime;

        //Update projectile speed
        aProjectile.PSpeed = m_velocity.magnitude * Time.deltaTime;

        //Increase life timer (this is only  used to know in which part of ease it is, not for actual killing projectile)
        m_timerPropulsionTime += Time.deltaTime;
    }
}
