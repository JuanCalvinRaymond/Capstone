using UnityEngine;
using System.Collections;
using System;

/*
Description: OnFireSystem subscribe to OnTrickDetected from ScoringSystem to fill the meter, once the meter is above certain threshold player gain score multiplier and speeds platform up
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 12 Jan 2016
Extra Notes:
*/
public class COnFireSystem : MonoBehaviour
{
    /*
    Description: Simple struct that contain threshold and multiplier
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    Extra Notes:
    */
    [Serializable]
    public struct SMeterThreshold
    {
        [SerializeField]
        [Range(0, 1)]
        public float m_threshold;

        [SerializeField]
        public int m_multiplier;
    }

    //Moving platform animation
    private const string M_PLATFORM_MOVING_SPEED_VARIABLE_NAME = "m_movementSpeed";

    //Min value for style meter
    private const float M_MIN_STYLE_METER = 0.0f;

    //Scoring system script
    private CScoringSystem m_scoringSystem;

    //Animator script
    private Animator m_platformAnimator;

    //Current multiplier
    private int m_currentMultiplier;

    //Style meter variable
    private float m_styleMeter;

    //Timer before the meter goes deplete
    private float m_depleteTimer;

    //Speed that increase based on style meter
    private float m_platformSpeedMultiplier;

    //Bool to lock increment of meter
    private bool m_freezeStyle;

    //How much a trick will fill Style meter
    public float m_singleTrickIncreaseAmount = 0.02f;
    public float m_comboTrickIncreaseAmount = 0.06f;

    // Reference to the game manager's style glow component.
    private CStyleGlow m_styleGlowComponent = null;

    //Percentage amount that will decay over time
    public float m_meterDecayAmount = 0.05f;

    //Grace duration for player to not doing trick before meter starts depleting
    public float m_noTrickDurationBeforeDepletes = 1.5f;

    //Max value for style meter
    public float m_maxPlatformSpeed = 1.2f;

    //Min value for style meter
    public float m_minPlatformSpeed = 0.75f;

    //Max value for style meter
    public float m_maxStyleMeter = 1.0f;

    //List of meter threshold
    public SMeterThreshold[] m_listOfMeterThreshold;

    //Event variable
    public delegate void delegateOnStyleChanged(float aStyleAmount);
    public event delegateOnStyleChanged OnStyleChanged;

    public delegate void delegateOnSpeedChanged(float aSpeedMultiplier);
    public event delegateOnSpeedChanged OnSpeedChanged;

    public int PMultiplier
    {
        get
        {
            return m_currentMultiplier;
        }
    }

    public float PPlatformSpeedMultiplier
    {
        get
        {
            return m_platformSpeedMultiplier;
        }
    }

    public float PCurrentStyleMeter
    {
        get
        {
            return m_styleMeter;
        }
    }

    public bool PFreezeStyle
    {
        get
        {
            return m_freezeStyle;
        }
        set
        {
            m_freezeStyle = value;
        }
    }

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    */
    private void Awake()
    {
        m_styleMeter = M_MIN_STYLE_METER;
        m_currentMultiplier = 1;
    }

    /*
    Description: Subscribe to scoring system's OnTrickDetected event
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    */
    private void Start()
    {
        if (CGameManager.PInstanceGameManager != null)
        {
            if (CGameManager.PInstanceGameManager.PScoringSystem != null)
            {
                m_scoringSystem = CGameManager.PInstanceGameManager.PScoringSystem;

                m_scoringSystem.OnTrickDetected += OnTrickDetected;
                m_scoringSystem.OnComboDetected += OnComboDetected;
            }

            //set the platform animation to currently active platform animator
            if (CGameManager.PInstanceGameManager.PMovingPlatform != null)
            {
                m_platformAnimator = CGameManager.PInstanceGameManager.PMovingPlatform.PAnimator;
                m_styleGlowComponent = GetComponent<CStyleGlow>();

                if (m_styleGlowComponent == null)
                {
                    Debug.LogError("Game Manager could not initialize CStyleGlow component.");
                }
            }
        }

        OnStyleChanged += CheckThreshold;

        //Call OnSpeedChange to initalize platform's speed
        ChangePlatformSpeed();

    }


    /*
    Description: Unsubscribe to scoring system's OnTrickDetected event
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    */
    private void OnDestroy()
    {
        if (m_scoringSystem != null)
        {
            m_scoringSystem.OnTrickDetected -= OnTrickDetected;
            m_scoringSystem.OnComboDetected -= OnComboDetected;

        }
        OnStyleChanged -= CheckThreshold;

    }

    /*
    Description: Decrease the meter gauge by decay amount / frame and constantly check if the meter is above certain threshold
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    */
    private void Update()
    {
        //If style is above the minimum
        if (m_styleMeter > M_MIN_STYLE_METER)
        {
            //Decrease deplete timer
            m_depleteTimer -= CGameManager.PInstanceGameManager.GetScaledDeltaTime();

            //If timer is finished
            if (m_depleteTimer < 0.0f)
            {
                ChangeStyleMeter(-m_meterDecayAmount * CGameManager.PInstanceGameManager.GetScaledDeltaTime());
            }
        }

        
    }

    /*
    Description: Increase or decrease the meter gauge by the score modifier fill amount, and call OnSpeedChanged
    Parameters: aStyleAmount : How much style increase or decrease
    Creator: Juan Calvin Raymond
    Creation Date: 30 Jan 2017
    */
    public void ChangeStyleMeter(float aStyleAmount)
    {
        //Increase or decrease the meter, will not change if freeze is on
        m_styleMeter += aStyleAmount * (m_freezeStyle ? 0.0f : 1.0f);

        //Clamp the style meter 0 to 1
        m_styleMeter = Mathf.Clamp01(m_styleMeter);

        if(aStyleAmount > 0)
        {
            //Start Deplete Timer
            m_depleteTimer = m_noTrickDurationBeforeDepletes;
        }

        if (OnStyleChanged != null)
        {
            OnStyleChanged(m_styleMeter);
        }

        //Call ChangePlatformSpeed function
        ChangePlatformSpeed();
    }

    /*
    Description: Call OnStyleChanged and start deplete timer
    Parameters: ATrickScoreModifiers aScoreModifier - The trick that was done.
                aHandThatHeldWeapon : Hand that held weapon
                int aTotalCalculation - The current score value of the player.
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    */
    private void OnTrickDetected(ATrickScoreModifiers aScoreModifier, EWeaponHand aHandThatHeldWeapon)
    {
        ChangeStyleMeter(m_singleTrickIncreaseAmount);

    }

    /*
    Description: Call OnStyleChanged and start deplete timer
    Parameters: ATrickScoreModifiers aScoreModifier - The trick that was done.
                aWeight : How much does the combo weight
                int aTotalCalculation - The current score value of the player.
    Creator: Juan Calvin Raymond
    Creation Date: 12 Jan 2016
    */
    private void OnComboDetected(AComboTrick aComboModifier, int aWeight)
    {
     
        ChangeStyleMeter(m_comboTrickIncreaseAmount * aWeight);
        
    }

    /*
    Description: Calculate and change the platform's speed
    Creator: Juan Calvin Raymond
    Creation Date: 19 Jan 2016
    */
    private void ChangePlatformSpeed()
    {
        //Calculate platform speed
        m_platformSpeedMultiplier = m_minPlatformSpeed + ((m_maxPlatformSpeed - m_minPlatformSpeed) * (m_maxStyleMeter - M_MIN_STYLE_METER) * m_styleMeter);

        if(m_styleMeter <= 0.0f)
        {
            m_platformSpeedMultiplier = 0.0f;
        }

        //Update platform speed
        if (m_platformAnimator != null)
        {
            //Get the current speed of the platform, and affect it by the speed multiplier
            float platformSpeed = m_platformAnimator.GetFloat(M_PLATFORM_MOVING_SPEED_VARIABLE_NAME) * m_platformSpeedMultiplier;

            //Change the speed of the animation
            m_platformAnimator.SetFloat(M_PLATFORM_MOVING_SPEED_VARIABLE_NAME, platformSpeed);
        }

        if (OnSpeedChanged != null)
        {
            OnSpeedChanged(m_platformSpeedMultiplier);
        }
    }

    /*
    Description: Check if style meter is beyond certain threshold and change style meter accordingly
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void CheckThreshold(float aCurrentStyleMeter)
    {
        //Iterate through all the threshold
        foreach (SMeterThreshold threshold in m_listOfMeterThreshold)
        {
            //If meter is above the threshold
            if (m_styleMeter >= threshold.m_threshold)
            {
                //Change the multiplier
                m_currentMultiplier = threshold.m_multiplier;
            }
        }

        // Clamp style 0-1 and update the glow effects with the current style value.
        m_styleGlowComponent.PStyleValue = m_styleMeter;
    }

}
