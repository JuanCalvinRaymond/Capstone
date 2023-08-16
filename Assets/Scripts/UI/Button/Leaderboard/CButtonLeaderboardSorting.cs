using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

/*
Description: Class that inherits from the AButtonFunctionality class. This class is made in order to change the 
sorting method of the desired leaderboard. The class is limited to just changing between 2 corresponding (highest-lowest) 
sorting methods.
Creator: Alvaro Chavez Mixco
Creation Date:  Monday, Novemeber 14, 2016
Extra Notes: This class makes use of ELeaderboardSortingMethods to do enum "math". So the order of the members in this enum
may affect how this class works. This class is generally made to switch between a highest and lowest sorting method, therefore in
the enum this two values must be declared one after each other. Also ideally you would place the sorting method to start as the "highest" variant
in the enum
*/
public class CButtonLeaderboardSorting : AButtonFunctionality
{
    private bool m_isOnStartingValue = true;//Just used as a toggle
    private List<CLeaderboard> m_leaderboardsToSort;

    //Sorting members
    [Tooltip("The leaderboard  handler that stores the leaderboard that will be sorted.")]
    public CLeaderboardHandler m_leaderboardHandler;
    [Tooltip("In which method will the leaderboard be sorted. For this to work properly, since it uses enum math, " +
        "you would place the sorting method to start as the highest variant in the enum." +
        "In that way the button will switch between the highest and lowest sorting variants")]
    public ELeaderboardSortingMethods m_sortingMethod;

    public CLeaderboardHandler PLeaderboardHandler
    {
        set
        {
            m_leaderboardHandler = value;
        }
    }

    /*
    Description: Obtain the CLeaderboard object stored in the CLeaderboardHandler m_leaderboardHandler
    variable.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 10, 2017
    */
    protected override void Awake()
    {
        //Call the base awake
        base.Awake();

        //Create the list of leaderboards to sort
        m_leaderboardsToSort = new List<CLeaderboard>();

        //If the leaderboard handler is valid
        if (m_leaderboardHandler != null)
        {
            //Get the leaderboard object it stores
            m_leaderboardsToSort.Add(m_leaderboardHandler.PLeaderboard);
        }
    }

    /*
    Description: Change how the leaderboard will be sorted, and sort it using the new sorting method
    Creator: Alvaro Chavez Mixco
    Creation Date:  Monday, Novemeber 14, 2016
    */
    public void ChangeLeaderboardSorting()
    {
        //If the list of leaderboards to sort is valid
        if (m_leaderboardsToSort != null)
        {
            //Go through all the leaderboards in the list
            foreach (CLeaderboard leaderboard in m_leaderboardsToSort)
            {
                //If there is a leaderboard
                if (leaderboard != null)
                {
                    //Change the sorting method
                    leaderboard.PLeaderboardSortingMethod = m_sortingMethod;

                    //Sort the leaderboard
                    leaderboard.SortLeaderboard();
                }
            }
        }
    }

    /*
    Description: Adds a leaderboard to the list of leaderboards being sorted.
    Parameters: CLeaderboard aLeaderboard - The leaderboard we want to add to the list of leaderboards
                                            being sorted.
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    */
    public void AddLeaderboardToSort(CLeaderboard aLeaderboard)
    {
        //If the list of leaderboards is  valid
        if (m_leaderboardsToSort != null)
        {
            //Add the leaderboard to the list
            m_leaderboardsToSort.Add(aLeaderboard);
        }
    }

    /*
    Description: Override of the AButtonFunctionality OnExecution method. This function will toggle between the starting sorting method, and the one
    declared next to it on the ELeaderboardSortingMethods enum.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Monday, Novemeber 14, 2016
    Extra Notes: Ideally, depending on the order of the ELeaderboardSortingMethods member declaration, the starting value should
    be the "highest" variant 
    */
    public override void OnButtonExecution()
    {
        //If the sorting method is on the starting value
        if (m_isOnStartingValue == true)
        {
            //If the sorting method is valid in the enum (bigger than 0, and not the last one in the enum)
            if ((int)m_sortingMethod < Enum.GetNames(typeof(ELeaderboardSortingMethods)).Length && m_sortingMethod >= 0)
            {
                m_sortingMethod += 1;//Go to the next member in the enum
                m_isOnStartingValue = false;//Set that we are no longer in the starting value

                //Sort the leaderboard
                ChangeLeaderboardSorting();
            }
        }
        else//If the sorting method is not on the starting value
        {
            //If the sorting method enum is valid
            if (m_sortingMethod > 0)
            {
                m_sortingMethod -= 1;//Go to the previous sorting method
                m_isOnStartingValue = true; //Set that we are in the starting value

                //Sort the leaderboard
                ChangeLeaderboardSorting();
            }
        }
    }

}
