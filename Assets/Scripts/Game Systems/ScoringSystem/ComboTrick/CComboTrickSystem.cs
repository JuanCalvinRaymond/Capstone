using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: Combo trick sytem which add trick to the list, update the lifetimer and check if a combo is performed
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2016
*/
public class CComboTrickSystem : MonoBehaviour
{
    //Scoring system script
    private CScoringSystem m_scoringSystem;

    //Trick perform tracker script
    private CTrickPerformTracker m_trickPerformTracker;
    
    //Timer to check combo
    private float m_comboCheckTimer;

    //List of AComboTrick
    public List<AComboTrick> m_listOfComboTrick;

    //Variable to tweak in inspector
    public float m_comboCheckDuration = 2.0f;

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    private void Awake()
    {
        m_scoringSystem = GetComponent<CScoringSystem>();
        m_trickPerformTracker = GetComponent<CTrickPerformTracker>();

        m_comboCheckTimer = 0.0f;

        //Iterate all the AComboTrick and set scoring system variable
        if (m_scoringSystem != null)
        {
            foreach (AComboTrick combo in m_listOfComboTrick)
            {
                if (combo != null)
                {
                    combo.PScoringSystem = m_scoringSystem;
                }
            }
        }

    }

    /*
    Description: Update life timer on the Trick list, and check if combo is performed
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    void Update()
    {
        //Update combo check timer
        m_comboCheckTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        //If timer is finished and the list is not null
        if (m_comboCheckTimer > m_comboCheckDuration && m_listOfComboTrick != null)
        {
            //Iterate through all the AComboTrick
            foreach (AComboTrick combo in m_listOfComboTrick)
            {
                if (combo != null)
                {
                    //Check if combo is performed
                    if (combo.ComboCheck(m_trickPerformTracker.PLeftWeaponTrickList, m_trickPerformTracker.PRightWeaponTrickList))
                    {
                        //Call ChangeListEvent function
                        m_trickPerformTracker.ChangeListEvent();
                    }

                }
            }
            //Reset timer
            m_comboCheckTimer = 0.0f;
        }
    }
    
}
