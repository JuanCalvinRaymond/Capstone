using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Class that inherit from AComboTrick, it will check if both weapon performed Blind Fire -> Peripheral -> Blind Fire or vice versa
Parameters: 
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2017
Extra Notes:
*/
public class CComboTrickTheGrammation : AComboTrick
{
    //Const int total trick need to performed
    private const int M_NUMBER_OF_TRICK_NEED_TO_PERFORMED = 3;

    //Previous trick that's performed
    private ATrickScoreModifiers m_previousTrickPerformed;

    //List of index that need to be deleted
    private List<int> m_indexToDelete;

    /*
    Description: Instantiate variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
    */
    private void Awake()
    {
        m_indexToDelete = new List<int>();
        m_trickName = "The Grammation";
    }

    /*
    Description: Check if both weapon performed Blind Fire -> Peripheral -> Blind Fire or vice versa
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
    */
    protected override bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList)
    {
        //If there's item in the list
        if (aListToCheck.Count > 0)
        {
            //Reset all variable
            m_previousTrickPerformed = null;
            m_indexToDelete.Clear();

            //Iterate through list
            for (int i = 0; i < aListToCheck.Count; i++)
            {
                //If the trick is Blind Fire or Peripheral
                if (aListToCheck[i].m_scoreModifier.GetType() == typeof(CTrickBlindFire) || aListToCheck[i].m_scoreModifier.GetType() == typeof(CTrickPeripheral))
                {
                    //If previous trick is not set
                    if(m_previousTrickPerformed == null)
                    {
                        //Set previous trick to current trick
                        m_previousTrickPerformed = aListToCheck[i].m_scoreModifier;

                        //Add current index to the list
                        aIndexList.Add(i);
                    }
                    //If the trick is not the same as previous one
                    else if(aListToCheck[i].m_scoreModifier != m_previousTrickPerformed)
                    {
                        //Add current index to the list
                        aIndexList.Add(i);

                        //Change preious trick to current trick
                        m_previousTrickPerformed = aListToCheck[i].m_scoreModifier;

                    }
                }

                //If the index is more than 3
                if (aIndexList.Count >= M_NUMBER_OF_TRICK_NEED_TO_PERFORMED)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
