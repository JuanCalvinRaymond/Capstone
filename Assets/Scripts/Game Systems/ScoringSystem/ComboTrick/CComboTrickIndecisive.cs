using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Class that inherit from AComboTrick, it will check if single weapon performed 3 different trick
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2017
*/
public class CComboTrickIndecisive : AComboTrick
{
    //List of different trick
    private List<ATrickScoreModifiers> m_listOfDifferentTrick;

    //List of Index to be delete once combo is performed
    private List<int> m_listOfIndexToDelete;

    //Variable to tweak in inspector
    public int m_maxDifferentTrick = 3;

    /*
    Description: Initialize all variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
     */
    private void Awake()
    {
        m_listOfDifferentTrick = new List<ATrickScoreModifiers>();
        m_listOfIndexToDelete = new List<int>();
        m_trickName = "Indecisive";
    }

    /*
    Description: Check if any weapon pass IterateThroughList function
    Parameters: aLeftWeaponTrickList : Trick list on left weapon
                aRightWeaponTrickList : Trick list on right weapon
    Creator: Juan Calvin Raymond
   
    */
    public override bool ComboCheck(List<CTrickElement> aLeftWeaponTrickList, List<CTrickElement> aRightWeaponTrickList)
    {
        m_listOfRightWeaponIndexToDelete.Clear();
        m_listOfLeftWeaponIndexToDelete.Clear();

        //Check if single weapon pass IterateThroughList
        if (IterateThroughList(aLeftWeaponTrickList, ref m_listOfLeftWeaponIndexToDelete) || IterateThroughList(aRightWeaponTrickList, ref m_listOfRightWeaponIndexToDelete))
        {
            if (m_scoringSystem != null)
            {
                //Call TrickDone on scoring system script
                m_scoringSystem.ComboDone(this, m_weight);
                
                DeleteElementFromTheList(aLeftWeaponTrickList, aRightWeaponTrickList);

                return true;
            }
        }

        return false;
    }

    /*
    Description: Check if any weapon performed 3 different tricks
    Parameters: aListToCheck : Trick list to check
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
    Extra Notes:
    */
    protected override bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList)
    {

        //If there's item 
        if(aListToCheck.Count > 0)
        {
            //Reset all variable
            m_listOfIndexToDelete.Clear();
            m_listOfDifferentTrick.Clear();

            //Iterate through list
            for (int i = 0; i < aListToCheck.Count; i++)
            {
                //If trick is not on the list
                if (!m_listOfDifferentTrick.Contains(aListToCheck[i].m_scoreModifier))
                {
                    //Add it to the list
                    m_listOfDifferentTrick.Add(aListToCheck[i].m_scoreModifier);

                    //Add the index to the list
                    aIndexList.Add(i);
                    
                }
                //If trick is more than max
                if (m_listOfDifferentTrick.Count >= m_maxDifferentTrick)
                {
                    
                    return true;
                }
            
            }
        }
        return false;
    }
}
