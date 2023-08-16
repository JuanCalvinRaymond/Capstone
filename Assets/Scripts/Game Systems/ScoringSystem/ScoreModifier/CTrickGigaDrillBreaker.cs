using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System;

/*
Description: Calculating score based on how many target player hit.
Creator: Juan Calvin Raymond
Creation Date: 25 Oct 2016
*/
public class CTrickGigaDrillBreaker : ATrickScoreModifiers
{

    /*
    Description: Set the modifier name
    Creator: Juan Calvin Raymond
    Creation Date: 22 Jan 2016
    */
    protected override void Awake()
    {
        base.Awake();
        m_trickName = "Collateral";
    }

    /*
    Description: just there because it's an abstract class
    Creator: Juan Calvin Raymond
    Creation Date:25 Oct 2016
    */
    protected override bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand)
    {
        return true;
    }

    /*
    Description: Cheking if player hit more than 1 target
    Parameters: aTargetList : List of target that get hit
                aTotalCalculation : total score from the previous calculation
    Creator: Juan Calvin Raymond
    Creation Date: 25 Oct 2016
    */
    public override void CalculateTotalScore(List<GameObject> aTargetList, EWeaponHand aWeaponHand, float aTimeWhenShot, List<SWeaponData> aListOfLeftWeaponData, List<SWeaponData> aListOfRightWeaponData)
    {
        //If player hit multiple target
        if (aTargetList.Count > 1)
        {
            //Call TrickDone function from scoring system script
            m_scoringSystem.TrickDone(this, aWeaponHand);
        }
    }
}
