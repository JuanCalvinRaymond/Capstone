using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
Description: Class used to display the leaderboard in a organized matter, it doesn't 
take care of actually making the leaderboard
Creator: Alvaro Chavez Mixco
Creation Date: Sunday, Novemeber 13, 2016
*/
public class CLeaderboardDisplay : MonoBehaviour
{
    //Variables making the entry display  and storing them
    private List<GameObject> m_leaderboardEntriesGameObject = new List<GameObject>();
    private List<CLeaderboardEntryDisplay> m_entryDisplays = new List<CLeaderboardEntryDisplay>();
    private List<CButtonLeaderboardSorting> m_sortingButtons;

    //Leaderboard scrolling
    private int m_currentPage = 0;//Index starts at 1, the current page of the leaderboard
    private int m_currentLeaderboardIndex = 0;

    //Leaderboards being stored
    private CLeaderboard m_localLeaderboard;
    private CLeaderboard m_onlineLeaderboard;
    public bool m_isShowingLocalLeaderboard = true;//If this is true, it is showing the local leaderboard, otherwise it is showing the online leaderboard

    //Properties of the leaderboard display
    [Header("Leaderboard Storage")]
    public ELevelState m_leaderboardLevel;
    public bool m_hasOnlineLeaderboard = true;
    public bool m_hasLocalLeaderboard = true;

    [Header("Leaderboard Display")]
    public GameObject m_entriesParentobject;
    public GameObject m_leaderboardEntryPrefab;//The formatting of each entry in the leaderboard
    public int m_numberOfEntriesToDisplay = 10;

    [Header("Leaderboard Entries")]
    public float m_yOffset;//The vertical distance between each entry in the leaderboard
    public Vector3 m_totalPlacementOffset = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 m_entriesRotation = new Vector3(0.0f, 180.0f, 0.0f);

    public GameObject m_sortingButtonsParents;

    public int PCurrentPage
    {
        get
        {
            return m_currentPage;
        }

        private set
        {
            m_currentPage = value;

            m_currentLeaderboardIndex = m_currentPage * m_numberOfEntriesToDisplay;
        }
    }

    public bool PIsShowingLocalLeaderboard
    {
        get
        {
            return m_isShowingLocalLeaderboard;
        }

        set
        {
            m_isShowingLocalLeaderboard = value;

            //If there is no local leaderboard
            if (m_hasLocalLeaderboard == false)
            {
                //Don't show a local leaderboard
                m_isShowingLocalLeaderboard = false;
            }
            else if (m_hasOnlineLeaderboard == false)
            {
                m_isShowingLocalLeaderboard = true;
            }
            //Reset the current leaderboard index when it switches between local and online
            m_currentLeaderboardIndex = 0;

            //If it showing the local leaderboard
            if (m_isShowingLocalLeaderboard == true && m_localLeaderboard != null)
            {
                //Update the display using the local leaderboard data
                UpdateDisplay(m_localLeaderboard.GetLeaderboardEntries(m_numberOfEntriesToDisplay, m_currentLeaderboardIndex));

                //Exit the function
                return;
            }
            else//If it is showing the online leaderboard
            {
                //If ther is an online manager
                if (COnlineManager.s_instanceOnlineManager != null)
                {
                    //If the online manager is connected to the server
                    if (COnlineManager.s_instanceOnlineManager.GetIsConnectedToLeaderboardServer() == true)
                    {
                        if (m_onlineLeaderboard == null)
                        {
                            m_onlineLeaderboard = new CLeaderboard(m_leaderboardLevel);
                        }

                        //Update the display using the online leaderboard data
                        UpdateDisplay(m_onlineLeaderboard.GetLeaderboardEntries(m_numberOfEntriesToDisplay, m_currentLeaderboardIndex));

                        //Exit the function
                        return;
                    }
                }
            }

            //If it reached this point it can't display the desired leaderboard, so show previous one
            PIsShowingLocalLeaderboard = !m_isShowingLocalLeaderboard;
            return;
        }
    }

    /*
    Description: Function used to get the leaderboard, make the text used to display the leaderboard, and
    sets the other properties required for displaying the leaderboard
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, Novemeber 13, 2016
    */
    private void Start()
    {
        //Get the local leaderboard
        GetLocalLeaderboard();

        //Suscribe to the OnConnect event of the online manager, if the display supports online
        SuscribeToOnlineManagersEvents();

        //Get the online leaderboard
        GetOnlineLeaderboard();

        //If is showing the local leaderboard
        if (m_isShowingLocalLeaderboard == true)
        {
            //Create each entry in the leaderboard using the local leaderboard
            CreateEntriesDisplays(m_localLeaderboard);
        }
        else//If it is showing the online leaderboard
        {
            //Create each entry in the leaderboard using the online leaderboard
            CreateEntriesDisplays(m_onlineLeaderboard);
        }

        //Update the leaderboard entries
        PIsShowingLocalLeaderboard = m_isShowingLocalLeaderboard;

        //Save the buttons that will be used to sort the leaderboard
        SetSortingButtons();
    }

    /*
    Description: Unsuscribe from the OnLeaderboardChange event
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, Novemeber 15, 2016
    */
    private void OnDestroy()
    {
        //If there is a local leaderboard
        if (m_localLeaderboard != null)
        {
            //Unsuscribe from event
            m_localLeaderboard.OnLeaderboardChange -= UpdateLocalLeaderboardDisplay;
        }

        //If there is a online leaderboard
        if (m_onlineLeaderboard != null)
        {
            //Unsuscribe from event
            m_onlineLeaderboard.OnLeaderboardChange -= UpdateOnlineLeaderboardDisplay;
        }

        //If there is an online manager instance
        if (COnlineManager.s_instanceOnlineManager != null)
        {
            //Unsuscribe from the connect event of the online manager
            COnlineManager.s_instanceOnlineManager.OnConnectToServerRequest -= GetOnlineLeaderboard;
        }
    }

    /*
    Description: Create the entries displays, and intially fill thems with the data of the leaderboard
    being passed
    Parameters: CLeaderboard aLeaderboardToDisplay - The leaderboard data where the initial state of the displays
                                                     will be obtained
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void CreateEntriesDisplays(CLeaderboard aLeaderboardToDisplay)
    {
        //If there is a leaderboard
        if (aLeaderboardToDisplay != null && m_leaderboardEntryPrefab != null)
        {

            GameObject parentObject = gameObject;

            //If there is a different parent object
            if (m_entriesParentobject != null)
            {
                parentObject = m_entriesParentobject;
            }

            //Get the current positon of the parent
            Vector3 entryPosition = m_leaderboardEntryPrefab.transform.position;
            entryPosition.y += m_yOffset;
            entryPosition += m_totalPlacementOffset;
            GameObject tempGameObject = null;

            //Create the entries according to the prefab
            for (int i = 0; i < m_numberOfEntriesToDisplay; i++)
            {
                //Create the game object and set its parent
                tempGameObject = (GameObject)Instantiate(m_leaderboardEntryPrefab, parentObject.transform);

                //Set entries position and rotation
                tempGameObject.transform.localPosition = entryPosition;
                tempGameObject.transform.rotation = Quaternion.Euler(m_entriesRotation);
                m_leaderboardEntriesGameObject.Add(tempGameObject);

                //Offset the Y
                entryPosition.y -= m_yOffset;
            }

            CLeaderboardEntryDisplay tempEntryDisplay;

            //Save the entries components
            foreach (GameObject entryGameObject in m_leaderboardEntriesGameObject)
            {
                //If the entry game object is valid
                if (entryGameObject != null)
                {
                    //Get the entry display component
                    tempEntryDisplay = entryGameObject.GetComponent<CLeaderboardEntryDisplay>();

                    //If it has the component
                    if (tempEntryDisplay != null)
                    {
                        //Add it to the list
                        m_entryDisplays.Add(tempEntryDisplay);
                    }
                }
            }
        }
    }

    /*
    Description: Get the local leaderboard from the game manager.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void GetLocalLeaderboard()
    {
        //If there is a game manager and the display will show local leaderboards
        if (CGameManager.PInstanceGameManager != null && m_hasLocalLeaderboard == true)
        {
            //Get the leaderboard
            m_localLeaderboard = CGameManager.PInstanceGameManager.GetLeaderboard(m_leaderboardLevel);

            //If there is a leaderboard
            if (m_localLeaderboard != null)
            {
                m_localLeaderboard.OnLeaderboardChange += UpdateLocalLeaderboardDisplay;//Suscribe to the  leaderboard change event
                UpdateDisplay(m_localLeaderboard.PCurrentLeaderboard);//Update the leaderboard for a first time
            }
        }
    }

    /*
   Description: Get the online leaderboard from the online manager. This may not be done immediately, so
    a leaderboard request will be send, that may or may not, eventually be filled out by the game manager.
   Creator: Alvaro Chavez Mixco
   Creation Date:  Sunday, January 22, 2017
   */
    private void GetOnlineLeaderboard()
    {
        //If the display will show online leaderboards
        if (m_hasOnlineLeaderboard == true)
        {
            //If there is no online leaderbaord object
            if (m_onlineLeaderboard == null)
            {
                //Make a new one
                m_onlineLeaderboard = new CLeaderboard(m_leaderboardLevel);
            }

            //Get the entries from the leaderboard
            GetEntriesFromOnlineLeaderboard();

            //If there is a leaderboard
            if (m_onlineLeaderboard != null)
            {
                m_onlineLeaderboard.OnLeaderboardChange += UpdateOnlineLeaderboardDisplay;//Suscribe to the  leaderboard change event
                UpdateDisplay(m_onlineLeaderboard.PCurrentLeaderboard);//Update the leaderboard for a first time
            }
        }
    }

    /*
    Description: Suscribe to the online manager OnConnectToServer request to know when to get the online leaderboard.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void SuscribeToOnlineManagersEvents()
    {
        //If there is an online manager and the class stores an online leaderboard
        if (COnlineManager.s_instanceOnlineManager != null && m_hasOnlineLeaderboard == true)
        {
            //Suscribe the connect event of the online manager to getting the online leaderboard
            COnlineManager.s_instanceOnlineManager.OnConnectToServerRequest += GetOnlineLeaderboard;
        }
    }

    /*
    Description: Saves all the sorting buttons components that are in the childrens of this gameobject
    Creator: Alvaro Chavez Mixco
    Creation Date:  Monday, Novemeber 14, 2016
    */
    private void SetSortingButtons()
    {
        //If the sorting buttons parent event is valid
        if (m_sortingButtonsParents != null)
        {
            //Get all the sorting buttons children of this object
            m_sortingButtons = new List<CButtonLeaderboardSorting>(
                m_sortingButtonsParents.GetComponentsInChildren<CButtonLeaderboardSorting>());

            //Go through all the sorting buttons                         
            foreach (CButtonLeaderboardSorting button in m_sortingButtons)
            {
                //If the button is valid
                if (button != null)
                {
                    //Set the leaderboards in the sorting button to match the one in this object
                    button.AddLeaderboardToSort(m_localLeaderboard);
                    button.AddLeaderboardToSort(m_onlineLeaderboard);
                }
            }
        }
    }

    /*
    Description: Update the leaderboard display using the entries from the local leaderboard
    Parameters: List<SPlayerEntry> aLeaderboardEntries - The entries to be displayed
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function gets called when the localLeaderboard OnLeaderboardChange event is called.
    */
    private void UpdateLocalLeaderboardDisplay(List<SPlayerEntry> aLeaderboardEntries)
    {
        //If it is showing the local leaderboard
        if (m_isShowingLocalLeaderboard == true)
        {
            //Update the entries
            UpdateDisplay(aLeaderboardEntries);
        }
    }

    /*
    Description: Update the leaderboard display using the entries from the local leaderboard
    Parameters: List<SPlayerEntry> aLeaderboardEntries - The entries to be displayed
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    Extra Notes: This function gets called when the onlineLeaderboard OnLeaderboardChange event is called.
    */
    private void UpdateOnlineLeaderboardDisplay(List<SPlayerEntry> aLeaderboardEntries)
    {
        //If it is showing the online leaderboard
        if (m_isShowingLocalLeaderboard == false)
        {
            //Update the entries
            UpdateDisplay(aLeaderboardEntries);
        }
    }

    /*
    Description: This function updates the text entreis so that they match the data being passed in the leaderboard
    Parameters:  List<SPlayerEntry> aLeaderboardEntries- The current leaderboard we want to display
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, Novemeber 13, 2016
    Extra Notes: This function is generally suscribed to the OnLeaderboardChange event, so that it gets automatically
    called whenever the leaderboard changes
    */
    private void UpdateDisplay(List<SPlayerEntry> aLeaderboardEntries)
    {
        //If the leaderboard is valid
        if (aLeaderboardEntries != null)
        {
            //For each entry in the leaderboard
            for (int i = 0; i < m_numberOfEntriesToDisplay; i++)
            {
                //If we have enough entry displays
                if (i < m_entryDisplays.Count && i < aLeaderboardEntries.Count)
                {
                    //If the entry display is not null
                    if (m_entryDisplays[i] != null)
                    {
                        //If the name is null, assume this entry is empty
                        if (aLeaderboardEntries[i].m_playerName == null)
                        {
                            //Write the entry as empty text
                            BlankLeaderboardDisplay(m_entryDisplays[i]);
                        }
                        else//If it is a valid entry
                        {
                            //Player name
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textName,
                                aLeaderboardEntries[i].m_playerName);

                            //Accuracy    
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textAccuracy,
                                CUtilityMath.RoundTo2Digits(aLeaderboardEntries[i].m_accuracy).ToString() + "%");

                            // Longest streak
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textLongestStreak,
                                aLeaderboardEntries[i].m_longestStreak.ToString());

                            //Number of tricks
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textNumberTricks,
                                aLeaderboardEntries[i].m_numberOfTricks.ToString());

                            //Number of combos
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textNumberCombos,
                                aLeaderboardEntries[i].m_numberOfCombos.ToString());

                            //Completion time
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textTime,
                                Mathf.Round(aLeaderboardEntries[i].m_completionTime).ToString());

                            //Player score
                            CUtilitySetters.SetText2DText(ref m_entryDisplays[i].m_textScore,
                                aLeaderboardEntries[i].m_score.ToString());
                        }
                    }
                }
            }
        }
    }


    /*
    Description: This function sets all the values in a leaderboard entry display as empty text
    Parameters:  CLeaderboardEntryDisplay aEntryDisplay - The leaderboard entry display to be blanked
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    private void BlankLeaderboardDisplay(CLeaderboardEntryDisplay aEntryDisplay)
    {
        //Player name
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textName,
            string.Empty);

        //Accuracy    
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textAccuracy,
            string.Empty);

        // Longest streak
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textLongestStreak,
             string.Empty);

        //Number of tricks
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textNumberTricks,
             string.Empty);

        //Number of combos
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textNumberCombos,
             string.Empty);

        //Completion time
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textTime,
            string.Empty);

        //Player score
        CUtilitySetters.SetText2DText(ref aEntryDisplay.m_textScore,
            string.Empty);
    }

    /*
    Description: Go to the next page of entries in the leaderboard
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    public void GoToNextPage()
    {
        //Increase the page count, this also increases the current leaderboard index
        PCurrentPage++;
        bool indexOutOfBounds = false;

        //If it is showing the local leaderboard and it has a local leaderboard
        if (m_isShowingLocalLeaderboard == true && m_hasLocalLeaderboard == true && m_localLeaderboard != null)
        {
            if (m_localLeaderboard != null)
            {
                //If the current index is bigger than the max number of entries in the local leaderboard
                if (m_currentLeaderboardIndex > m_localLeaderboard.PMaxNumberOfEntries)
                {
                    //Set that the leaderboard index is out of range
                    indexOutOfBounds = true;
                }
            }
        }
        //If it showing the online leaderboard and it has an online leaderboard
        else if (m_isShowingLocalLeaderboard == false && m_hasOnlineLeaderboard == true && m_onlineLeaderboard != null)
        {
            if (m_onlineLeaderboard != null)
            {
                //If the current index is bigger than the max number of entries in the online leaderboard
                if (m_currentLeaderboardIndex > m_onlineLeaderboard.PMaxNumberOfEntries)
                {
                    //Set that the leaderboard index is out of range
                    indexOutOfBounds = true;
                }
                else
                {
                    //Using the new index, get the entries from the online leaderboard
                    GetEntriesFromOnlineLeaderboard();
                }
            }
        }

        //If the index is within bounds
        if (indexOutOfBounds == true)
        {
            //Update the leaderboard displays
            PIsShowingLocalLeaderboard = PIsShowingLocalLeaderboard;
        }
        else//If the index is out of bounds
        {
            //Decrease the page, and the index
            PCurrentPage--;
        }
    }

    /*
    Description: Go to the previous page of entries in the leaderboard
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    public void GoToPreviousPage()
    {
        //Decrease the page count, this also decrease the current leaderboard index
        PCurrentPage--;

        //Ensure that the current page, and therefore the index, is never less than 0
        PCurrentPage = Mathf.Max(0, PCurrentPage);

        //Update the leaderboard displays
        PIsShowingLocalLeaderboard = PIsShowingLocalLeaderboard;
    }

    /*
    Description: Using the online manager, send a request to the online server for the entries, according
    to the leaderboard display onlineLeaderboard level, number of entries to display, current leaderboard index,
    and current leaderboard sorting method
    Creator: Alvaro Chavez Mixco
    Creation Date:  Sunday, January 22, 2017
    */
    public void GetEntriesFromOnlineLeaderboard()
    {
        //If the online manager is valid
        if (COnlineManager.s_instanceOnlineManager != null)
        {
            //If the online manager is connected to the server
            if (COnlineManager.s_instanceOnlineManager.GetIsConnectedToLeaderboardServer() == true)
            {
                //Get the new entries from the online leaderboard
                COnlineManager.s_instanceOnlineManager.GetOnlineLeaderboardEntries(ref m_onlineLeaderboard, m_leaderboardLevel,
                    m_numberOfEntriesToDisplay, m_currentLeaderboardIndex, m_onlineLeaderboard.PLeaderboardSortingMethod);
            }
        }
    }
}