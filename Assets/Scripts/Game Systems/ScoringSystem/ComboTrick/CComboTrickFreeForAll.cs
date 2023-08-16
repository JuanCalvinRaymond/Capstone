using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
Description: Class that inherit from AComboTrick, it will check if both weapon performed gunslinger and other trick
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2016
*/
public class CComboTrickFreeForAll : AComboTrick
{
    //Bool to check if player have performed gunslinger
    private bool m_didGunslinger;

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    private void Awake()
    {
        m_trickName = "Free For All";
    }

    /*
    Description: Check if both weapon performed gunslinger and one other trick
    Parameters: aListToCheck : Trick list to check
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    protected override bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList)
    {
        //If there's item on the list
        if(aListToCheck.Count > 0)
        {
            //Reset all variable
            m_didGunslinger = false;

            //Iterate through list
            for (int i = 0; i < aListToCheck.Count; i++)
            {
                //If the trick is gunslinger and weapon haven't performed it
                if (aListToCheck[i].m_scoreModifier.GetType() == typeof(CTrickGunSlinger) && m_didGunslinger == false)
                {
                    //Set bool to true
                    m_didGunslinger = true;

                    //Set gunslinger index to current index
                    aIndexList.Add(i);
                }
                //If the trick is not gunslinger and player already performed gunslinger
                else if (aListToCheck[i].m_scoreModifier.GetType() != typeof(CTrickGunSlinger) && m_didGunslinger == true )
                {
                    aIndexList.Add(i);
                    return true;
                }

            }

        }
        return false;
    }
}
