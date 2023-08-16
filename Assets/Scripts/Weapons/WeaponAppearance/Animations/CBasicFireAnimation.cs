using UnityEngine;
using System.Collections;

/*
Description: Class for basic ramming fire animation
Creator: Alvaro Chavez Mixco
Creation Date: Friday, December 23th, 2016
*/
public class CBasicFireAnimation : AWeaponAnimation
{
    //Const string of animation trigger name
    private const string M_GUN_RAMMING_VARIABLE_NAME = "m_isRamming";
    private const string M_GUN_ANIMATION_SPEED_VARIABLE_NAME = "m_speed";

    [Header("Ramming Animation")]
    public float m_gunAnimationSpeedMultiplier = 1.0f;

    /*
    Description: Sets the speed that the animation will have
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, December 23th, 2016
    Extra Notes: This is done in start and not just before the animation is played, because unless for testing purposes,
    the speed of the animation should never change midgame.
    */
    protected override void Start()
    {
        //Call the base start method
        base.Start();

        if (m_animatorGun != null)
        {
            //Set the speed of the animation
            m_animatorGun.SetFloat(M_GUN_ANIMATION_SPEED_VARIABLE_NAME, m_gunAnimationSpeedMultiplier);
        }
    }

    /*
    Description: Plays a gun ramming/recoil animation
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    protected override void FireAnimation()
    {
        if (m_animatorGun != null)
        {
            //Play weapon animation
            m_animatorGun.SetTrigger(M_GUN_RAMMING_VARIABLE_NAME);
        }
    }
}
