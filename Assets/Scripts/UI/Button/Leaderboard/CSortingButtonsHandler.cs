using UnityEngine;
using System.Collections;


/*
Description: Class used to store all the sorting buttons that affect a leaderboard.
To work this class must be the parent of object of all the sorting buttons.
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, January 29, 2017
*/
public class CSortingButtonsHandler : MonoBehaviour
{
    private CButtonLeaderboardSorting[] m_sortingButtons;

    [Tooltip("The leaderboard  handler that stores the leaderboard that will be sorted by all the sorting buttons.")]
    public CLeaderboardHandler m_leaderboardHandler;

    /*
    Description: Gets all the sorting buttons in the children of this object, and set all of them
    to sort the same leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 29, 2017
    */
    void Start()
    {
        //Get all the sorting buttons in the children of the object
        m_sortingButtons = GetComponentsInChildren<CButtonLeaderboardSorting>();

        //Set the leaderboard that will be sorted in all the children
        SetLeaderboardToSort();
    }

    /*
    Description: Go through all the stored sorting buttons, and set all of them to sort
    the same leaderboard, the one stored in this class.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 29, 2017
    */
    private void SetLeaderboardToSort()
    {
        //If this object leaderboard handler is valid, and there are sorting buttons
        if (m_leaderboardHandler != null && m_sortingButtons != null)
        {
            //Go through all the sorting buttons
            for (int i = 0; i < m_sortingButtons.Length; i++)
            {
                //If the sorting button is valid
                 if(m_sortingButtons[i]!=null)
                {
                    //Set the leaderboard to sort for that sorting button
                    m_sortingButtons[i].PLeaderboardHandler = m_leaderboardHandler;
                }
            }

        }
    }

}
