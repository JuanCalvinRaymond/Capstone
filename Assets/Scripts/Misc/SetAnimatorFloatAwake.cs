using UnityEngine;
using System.Collections;

/*
Description: Class to easily set the value of an animator durign awake.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, March 24th, 2017
*/
[RequireComponent(typeof(Animator))]
public class SetAnimatorFloatAwake : MonoBehaviour
{
    public string m_animatorParameterName = "m_speedMultiplier";
    public float m_valueToSet = 1.0f;

    /*
    Description: Set the desired float parameter for the animator.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    private void Awake()
    {
        //Get the animator component
        Animator animator = GetComponent<Animator>();

        //Set the animator float value
        CUtilitySetters.SetAnimatorFloatParameter(ref animator, m_animatorParameterName, m_valueToSet);
    }
}
