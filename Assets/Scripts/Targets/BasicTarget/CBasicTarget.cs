using UnityEngine;
using System.Collections;

/*
Description: A simple static target, without AI, that will have a "life" point system to indicate when it will be destroyed. 
Creator: Alvaro Chavez Mixco
Creation Date: October 8, 2016
*/
public class CBasicTarget : ACoreTarget
{
    [Header("Scale based on max health")]
    [Tooltip("The scale of the object will be scaled according to its current max health and the base max heealth parameter")]
    public bool m_scaleObjectBasedOnMaxHealth = true;
    public int m_baseMaxHealth = 200;

    /*
    Description: Call the ACoreTarger awake function, and if desired, scale 
                 the target according to its max health. 
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    */
    protected override void Awake()
    {
        //Call the ACore Target awake
        base.Awake();

        //If the target will be scaled based on max health
        if (m_scaleObjectBasedOnMaxHealth == true)
        {
            //Get the initial scale of the object
            Vector3 initialScale = transform.localScale;

            //Scale the object according to its max health and the base max health
            transform.localScale = initialScale * ((float)m_maxHealth / m_baseMaxHealth);
        }
    }
}
