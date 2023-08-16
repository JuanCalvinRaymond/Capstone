using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/*
Description: Class that inherit from AComboTrick, will check if single weapon performed 3 same trick
Parameters: 
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2016
Extra Notes:
*/
public class CComboTrickTheParrot : AComboTrick
{
    //Trick which player performed
    private ATrickScoreModifiers m_trickThatPerformed;

    //Where the index start to call RemoveRange on the list
    private int m_startingIndexToDelete;

    //Counter to see how many consecutive trick player perform
    private int m_trickCounter = 0;

    //Variable to tweak in inspector
    public int m_maxConsecutiveTrick = 3;

    /*
    Description: Instantiate variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    private void Awake()
    {
        m_trickName = "The Parrot";
        m_trickThatPerformed = null;
    }

    /*
    Description: Override parent's function
    Parameters: aLeftWeaponTrickList : Trick list on left weapon
                aRightWeaponTrickList : Trick list on right weapon
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    public override bool ComboCheck(List<CTrickElement> aLeftWeaponTrickList, List<CTrickElement> aRightWeaponTrickList)
    {
        m_listOfRightWeaponIndexToDelete.Clear();
        m_listOfLeftWeaponIndexToDelete.Clear();
        
        //If any weapon pass IterateThroughList function
        if (IterateThroughList(aLeftWeaponTrickList, ref m_listOfLeftWeaponIndexToDelete) || IterateThroughList(aRightWeaponTrickList, ref m_listOfRightWeaponIndexToDelete))
        {
            if (m_scoringSystem != null)
            {
                //Call TrickDone from scoring system
                m_scoringSystem.ComboDone(this, m_weight);

                DeleteElementFromTheList(aLeftWeaponTrickList, aRightWeaponTrickList);
                return true;
            }
        }
        return false;

    }

    /*
    Description: Override parent's function
    Parameters: aListToCheck : Trick list to check
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    protected override bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList)
    {
        //If there's item in the list
        if(aListToCheck.Count > 0)
        {
            //Reset all variable
            m_trickCounter = 0;
            m_trickThatPerformed = null;

            //Iterate through list
            for (int i = 0; i < aListToCheck.Count; i++)
            {
                //If no trick have been performed
                if(m_trickThatPerformed == null)
                {
                    //Set the trick to the variable
                    m_trickThatPerformed = aListToCheck[i].m_scoreModifier;

                    //Add the counter
                    m_trickCounter += 1;

                    //Set the starting index to current index
                    aIndexList.Add(i);

                }
                //If there's trick that have been performed
                else
                {
                    //If the trick is the same as the last one performed
                    if(aListToCheck[i].m_scoreModifier == m_trickThatPerformed)
                    {
                        m_trickCounter += 1;

                        //Add the counter
                        aIndexList.Add(i);

                    }
                    //If the trick is different
                    else
                    {
                        m_trickCounter = 0;

                        //Reset the counter
                        aIndexList.Clear();

                        //Set the trick to the variable
                        m_trickThatPerformed = aListToCheck[i].m_scoreModifier;

                        //Set starting index to current index
                        aIndexList.Add(i);


                    }
                }

                //If counter is more than the max trick
                if(m_trickCounter >= m_maxConsecutiveTrick)
                {
                    ////Remove all the trick from the list
                    //aListToCheck.RemoveRange(m_startingIndexToDelete, m_maxConsecutiveTrick);
                    
                    return true;
                }
            }
        }
        return false;
    }
}
