using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: Cheat Tool to change platform movement speed, showing score to make it easier to debug
Creator: Juan Calvin Raymond
Creation Date: Wednesday, November 10th, 2016
Note : may add more function in the future, and it only works if the use have a keyboard.
*/
public class CCheatTool : MonoBehaviour
{
    //Moving platform animation
    private const string M_PLATFORM_MOVING_SPEED_VARIABLE_NAME = "m_movementSpeed";

    //Constant value of speed increment
    private const float M_SPEED_INCREMENT = 0.5f;
    private const float M_STYLE_INCREMENT = 0.5f;
    private const float M_PERCENTAGE_INCREMENT = 0.02F;

    //Platform animation name
    private string m_animationName = string.Empty;

    //The percentage of the animation to jump into 
    private float m_percentageOfPlatformAnimation;

    //Platform speed multiplier
    private float m_platformSpeedMultiplier;

    //Animator script
    private Animator m_platformAnimator;

    //Toggle to display cheat HUD
    private bool m_displayStats;

    //OnFireSystem script
    private COnFireSystem m_onFireSystem;

    public delegate void delegatePlatformSpeedChange(float aSpeed);
    public event delegatePlatformSpeedChange OnPlatformSpeedChange;

    public delegate void delegateDisplayStatsChange(bool aState);
    public event delegateDisplayStatsChange OnDisplayStatsChange;
    public event delegateDisplayStatsChange OnHideShowAllHUDForVideoTaking;

    public float PPercentagePlatformAnimation
    {
        get
        {
            return m_percentageOfPlatformAnimation;
        }
    }

    public bool PDisplayStats
    {
        get
        {
            return m_displayStats;
        }
    }

    public float PPlatformSpeed
    {
        get
        {
            return m_platformSpeedMultiplier;
        }
    }

    /*
    Description: Set the platform animation
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, November 17th, 2016
    Note :
    */
    private void Start()
    {
        //If any thing is not null, set the platform animation to currently active platform animator
        if (CGameManager.PInstanceGameManager != null)
        {
            if (CGameManager.PInstanceGameManager.PPlayerObject != null)
            {
                m_platformAnimator = CGameManager.PInstanceGameManager.PMovingPlatform.PAnimator;

            }

            if (CGameManager.PInstanceGameManager.POnFireSystem != null)
            {
                m_onFireSystem = CGameManager.PInstanceGameManager.POnFireSystem;

                //Subscribe to OnFireSystem OnSpeedChanged event
                m_onFireSystem.OnSpeedChanged += ChangePlatformSpeed;
            }
        }

        //Initialize platform speed multiplier to 1 to make sure platform run
        m_platformSpeedMultiplier = 1;
    }

    /*
    Description: Unsubscribe from even
    Creator: Juan Calvin Raymond
    Creation Date: 28 Feb 2017
    Note :
    */
    private void OnDestroy()
    {
        m_onFireSystem.OnSpeedChanged -= ChangePlatformSpeed;

    }

    /*
    Description: get input from keyboard and activate debug utility
    Creator: Juan Calvin Raymond
    Creation Date: Wednesday, November 17th, 2016
    Note :
    */
    private void Update()
    {
        //Constantly update percent completed
        if (m_platformAnimator != null)
        {
            m_percentageOfPlatformAnimation = m_platformAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        //If "]" is press increase the speed
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            IncreasePlatformSpeedMultiplier(M_SPEED_INCREMENT);
        }
        //If "[" is press decrease the speed
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            IncreasePlatformSpeedMultiplier(-M_SPEED_INCREMENT);
        }
        //If "O" is press toggle the display score variable 
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_displayStats = !m_displayStats;

            //Call display stats changed
            if (OnDisplayStatsChange != null)
            {
                //Display the stats
                OnDisplayStatsChange(m_displayStats);
            }
        }

        //If "L" is press toggle the all HUD 
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_displayStats = !m_displayStats;

            //Call display stats changed
            if (OnHideShowAllHUDForVideoTaking != null)
            {
                //Display the stats
                OnHideShowAllHUDForVideoTaking(m_displayStats);
            }
        }

        //If ";" is press Jump to certain frame in the animation
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            AddToAnimationPercent(-M_PERCENTAGE_INCREMENT);
        }

        //If "'" is press Jump to certain frame in the animation
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            AddToAnimationPercent(M_PERCENTAGE_INCREMENT);
        }

        //If "/" is pressed, Increase Style
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_onFireSystem.PFreezeStyle = !m_onFireSystem.PFreezeStyle;
        }

        //If "/" is pressed, Increase Style
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            AddStyleMeter(M_STYLE_INCREMENT);
        }

        //If "." is pressed, Decrease Style
        if (Input.GetKeyDown(KeyCode.Period))
        {
            AddStyleMeter(-M_STYLE_INCREMENT);
        }
    }

    /*
    Description: Increasing this script's platform speed multiplier
    Parameter : aSpeedIncrement : how much speed multiplier increase or decrease
    Creator: Juan Calvin Raymond
    Creation Date: 28 Feb 2017
    */
    private void IncreasePlatformSpeedMultiplier(float aSpeedIncrement)
    {
        m_platformSpeedMultiplier += aSpeedIncrement;

        //Call ChangePlatformSpeed function
        ChangePlatformSpeed(m_onFireSystem.PPlatformSpeedMultiplier);
    }


    /*
    Description: Updates the current platform speed values and calls the respective event
    Parameters: float aOnFireSpeedMultiplier : OnFireSystem's platform speed multiplier
    Creator: Alvaro Chavez Mixco, modified by Juan Calvin Raymond
    */
    private void ChangePlatformSpeed(float aOnFireSpeedMultiplier)
    {
        //If the platform has an animator
        if (m_platformAnimator != null)
        {
            //Calculate platform speed total
            float platformSpeed = m_platformSpeedMultiplier * aOnFireSpeedMultiplier;

            //Set the platform speed
            m_platformAnimator.SetFloat(M_PLATFORM_MOVING_SPEED_VARIABLE_NAME, platformSpeed);

            //Call the event
            if (OnPlatformSpeedChange != null)
            {
                OnPlatformSpeedChange(platformSpeed);
            }
        }
    }

    /*
    Description: Updates the current animation name in the cheat tool, and adds a percentage amount
    to the current percent amount it has
    Creator: Alvaro Chavez Mixco
    */
    private void AddToAnimationPercent(float aPercentageOfAnimation)
    {
        m_animationName = GetPlatformAnimationName();

        //Increment the current percentage
        m_percentageOfPlatformAnimation = m_platformAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + aPercentageOfAnimation;

        //Play the animation at the percentage
        m_platformAnimator.Play(m_animationName, 0, m_percentageOfPlatformAnimation);
    }

    /*
    Description: Updates the current style meter
    Creator:Juan Calvin Raymond
    */
    private void AddStyleMeter(float aStyleAmount)
    {
        m_onFireSystem.ChangeStyleMeter(aStyleAmount);
    }

    /*
    Description: Gets the name of the animation stored in the player, this is done through the game manager.
    Creator: Alvaro Chavez Mixco
    */
    private string GetPlatformAnimationName()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the game manager has a moving platform
            if (CGameManager.PInstanceGameManager.PMovingPlatform != null)
            {
                //Get the moving platform animation
                return CGameManager.PInstanceGameManager.PMovingPlatform.PAnimationName;
            }
        }

        return string.Empty;
    }
}
