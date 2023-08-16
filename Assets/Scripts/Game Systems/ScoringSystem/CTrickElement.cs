using UnityEngine;
using System.Collections;

/*
Description: Class that only have a life timer and ATrickScoreModifier script
Creator: Juan Calvin Raymond
Creation Date: 7 Feb 2017
Note : I have to make a new script so multiple script can have reference for it
*/

public class CTrickElement
{
    //Life timer
    public float m_lifeTimer;

    //Score modifier script
    public ATrickScoreModifiers m_scoreModifier;

    //Combo trick script
    public AComboTrick m_comboTrick;
    

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 7 Feb 2017
    */
    public void Init(ATrickScoreModifiers aScoreModifier)
    {
        m_scoreModifier = aScoreModifier;
        m_lifeTimer = 0.0f;
    }

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 7 Feb 2017
    */
    public void Init(AComboTrick aComboTrick)
    {
        m_comboTrick = aComboTrick;
        m_lifeTimer = 0.0f;
    }
}
