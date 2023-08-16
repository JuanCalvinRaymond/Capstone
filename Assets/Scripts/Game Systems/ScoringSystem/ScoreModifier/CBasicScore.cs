using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
Description: Calculating score based the target score value
Creator: Juan Calvin Raymond
Creation Date: 25 Oct 2016
*/
public class CBasicScore : MonoBehaviour
{
    /*
    Description: Calculating all the target value
    Parameters: aTargetList : List of target that get hit
                          aTotalCalculation : total score from the previous calculation
    Creator: Juan Calvin Raymond
    Creation Date: 25 Oct 2016
    Extra Notes:
    */
    public void CalculateTotalScore(List<GameObject> aTargetList, ref int aTotalCalculation)
    {
        //iterate all the target in the list
        foreach (GameObject target in aTargetList)
        {
            //Need to cast it
            ITarget tempTargetInterface = (ITarget)target.GetComponent(typeof(ITarget));

            if(tempTargetInterface != null)
            {
                //Add the score value
                aTotalCalculation += tempTargetInterface.PScoreValue;
            }
        }
    }
}
