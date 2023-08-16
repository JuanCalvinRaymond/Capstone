using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
Description: Class that inherit from AComboTrick, it will check if both weapon performed Flip Shot -> Switcheroo
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2016
*/
public class CComboTrickTheJuggler : AComboTrick
{
    //Bool to see if player did a flip shot
    private bool m_didFlipShot;

    //Flip shot index
    private int m_indexToDelete;

    /*
    Description: Instantiate variable
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
    */
    private void Awake()
    {
        m_trickName = "The Juggler";
    }

    /*
    Description: Check if both weapon performed Flip Shot -> Switcheroo
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    */
    protected override bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList)
    {
        //If there's item in the list
        if (aListToCheck.Count > 0)
        {
            //Reset all variable
            m_didFlipShot = false;

            //Iterate through list
            for (int i = 0; i < aListToCheck.Count; i++)
            {
                //If the trick is Flip Shot
                if (aListToCheck[i].m_scoreModifier.GetType() == typeof(CTrickFlipShot))
                {
                    //Set bool to true
                    m_didFlipShot = true;

                    //Set the index to current index
                    aIndexList.Add(i);
                }

                //If the trick is Switcheroo and Flip Shot is already performed
                else if(aListToCheck[i].m_scoreModifier.GetType() == typeof(CTrickSwitcheroo) && m_didFlipShot)
                {
                    aIndexList.Add(i);
                    
                    return true;
                }

            }
        }
        return false;
    }
}
