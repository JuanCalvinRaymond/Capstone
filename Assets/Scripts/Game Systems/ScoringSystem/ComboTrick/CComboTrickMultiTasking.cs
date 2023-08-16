using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Class that inherit from AComboTrick, it will check if both weapon performed different trick
Parameters: 
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2017
Extra Notes:
*/
public class CComboTrickMultiTasking : AComboTrick
{
    //First trick performed
    private ATrickScoreModifiers m_firstTrickPerformed;

    //First trick index
    private int m_indexToDelete;

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017:
    */
    private void Awake()
    {
        m_trickName = "MultiTasking";
    }

    /*
    Description: Class that inherit from AComboTrick, it will check if both weapon performed different trick
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
    */
    protected override bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList)
    {
        
        //If there's an item in the list
        if (aListToCheck.Count > 0)
        {
            //Reset all variable
            m_firstTrickPerformed = null;

            //Iterate through list
            for (int i = 0; i < aListToCheck.Count; i++)
            {
                //If trick haven't been performed
                if (m_firstTrickPerformed == null)
                {
                    //Set first trick to current trick
                    m_firstTrickPerformed = aListToCheck[i].m_scoreModifier;

                    //Set index to current index
                    aIndexList.Add(i);
                }
                //If the trick is different from the first one
                else if(aListToCheck[i].m_scoreModifier != m_firstTrickPerformed)
                {
                    aIndexList.Add(i);
                    
                    return true;
                }
            }
        }
        return false;
    }
}
