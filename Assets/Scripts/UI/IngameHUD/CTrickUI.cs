using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


/*
Description: Spawning trick name, trick icon and total score gain on the screen
Creator: Juan Calvin Raymond
Creation Date: 5 Dec 2016
*/
public class CTrickUI : MonoBehaviour
{

    //How many trick is showing on the screen
    private int m_trickShown;
    
    //Trick perform tracker script
    private CTrickPerformTracker m_trickPerformTracker;
    
    //Canvas script
    private Canvas m_canvas;

    //List of trick that player already performed
    private List<ATrickScoreModifiers> m_listOfSingleTrickPerformed;
    private List<AComboTrick> m_listOfComboTrickPerformed;

    //List of trick name that is showing
    private List<GameObject> m_listOfTrickUI;

    //List of UI's component
    private List<Image> m_listOfUIIconComponent;
    private List<Text> m_listOfUINameComponent;
    private List<Text> m_listOfUIMultiplierComponent;
    

    //Tweakable variable in inspector
    public int m_maxTrickUIToSpawn;
    public int m_textLifeDuration;
    public float m_textSpawnYAxisOffset;
    
    //Trick UI prefabs
    public GameObject m_trickIconPrefab;
    public GameObject m_trickNamePrefab;

    //Point where the trick UI start spawn
    public RectTransform m_trickIconSpawnPoint;
    public RectTransform m_trickNameSpawnPoint;
    public RectTransform m_trickMultiplierSpawnPoint;

    /*
    Description: Initializing Variable
    Creator: Juan Calvin Raymond
    Creation Date: 5 Dec 2016
    */
    private void Awake()
    {
        m_trickShown = 0;
        m_listOfTrickUI = new List<GameObject>();
        m_listOfSingleTrickPerformed = new List<ATrickScoreModifiers>();
        m_listOfComboTrickPerformed = new List<AComboTrick>();
        m_listOfUINameComponent = new List<Text>();
        m_listOfUIMultiplierComponent = new List<Text>();
        m_listOfUIIconComponent = new List<Image>();

        m_canvas = GetComponent<Canvas>();
    }

    /*
    Description: Setting and subscribing to trick perform tracker event, and Create UI on the screen
    Creator: Juan Calvin Raymond
    Creation Date: 5 Dec 2016
    */
    private void Start()
    {
        m_trickPerformTracker = CGameManager.PInstanceGameManager.PTrickPerformTracker;
        m_trickPerformTracker.OnListChange += CheckTrickPerformedList;
        

        //Call CreateUI function and add the component for trick name
        CreateUI(m_trickNamePrefab, m_trickNameSpawnPoint);
        if (m_listOfTrickUI.Count > 0)
        {
            //Get Text component from instantiated object
            foreach (GameObject UI in m_listOfTrickUI)
            {
                m_listOfUINameComponent.Add(UI.GetComponent<Text>());
            }
        }

        //Clear the list
        m_listOfTrickUI.Clear();

        //Call CreateUI function and add the component for trick icon
        CreateUI(m_trickIconPrefab, m_trickIconSpawnPoint);
        if (m_listOfTrickUI.Count > 0)
        {
            //Get Image component from instantiated object
            foreach (GameObject UI in m_listOfTrickUI)
            {
                m_listOfUIIconComponent.Add(UI.GetComponent<Image>());
            }
        }

        //Clear the list
        m_listOfTrickUI.Clear();

        //Call CreateUI function and add the component for trick multiplier
        CreateUI(m_trickNamePrefab, m_trickMultiplierSpawnPoint);
        if (m_listOfTrickUI.Count > 0)
        {
            //Get text component from instantiated object
            foreach (GameObject UI in m_listOfTrickUI)
            {
                m_listOfUIMultiplierComponent.Add(UI.GetComponent<Text>());
            }
        }

        ResetUI();
    }

    /*
    Description: Unsubscribe to trick perform tracker event
    Creator: Juan Calvin Raymond
    Creation Date: 5 Dec 2016
    */
    private void OnDestroy()
    {
        m_trickPerformTracker.OnListChange -= CheckTrickPerformedList;
        
    }

    /*
    Description: Create empty UI and add it to the list
    Parameters: aPrefab : Prefab to spawn
                aSpawnPoint : Point to start spawning prefab
    Creator: Juan Calvin Raymond
    Creation Date: 8 Dec 2016
    */
    private void CreateUI(GameObject aPrefab, RectTransform aSpawnPoint)
    {
        for (int i = 0; i < m_maxTrickUIToSpawn; i++)
        {
            //Calculate the offset
            Vector3 offset = new Vector3(0.0f, m_textSpawnYAxisOffset * i, 0.0f);
            offset = m_canvas.transform.TransformVector(offset);

            if (aPrefab != null && aSpawnPoint != null)
            {
                //Instantiate trick UI prefab
                GameObject clone = (GameObject)Instantiate(aPrefab,
                    aSpawnPoint.position + offset,
                    aSpawnPoint.rotation,
                    m_trickIconSpawnPoint.transform);

                //Add it to the list
                m_listOfTrickUI.Add(clone);
            }
        }
    }

    /*
    Description: Reset all trick ui component
    Parameters:
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    */
    private void ResetUI()
    {
        //Get all the UI from the UI image list
        foreach (Image UI in m_listOfUIIconComponent)
        {
            //Set the alpha to 0 and image to null
            UI.color = Color.clear;
            UI.sprite = null;
        }

        //Get all the UI from the UI name list
        foreach (Text UI in m_listOfUINameComponent)
        {
            //Set the text to empty
            UI.text = string.Empty;
        }

        //Get all the UI from the UI multiplier list
        foreach (Text UI in m_listOfUIMultiplierComponent)
        {
            //Set the text to empty
            UI.text = string.Empty;
        }
    }

    /*
    Description: Change UI text and sprite based on argument
    Parameters(Optional): aTrickName : Trick's name
                          aTrickIcon : Trick's icon
                          aTrickCounter : how many tricks player already performed
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes:
    */
    private void SpawnUI(string aTrickName, Sprite aTrickIcon, int aTrickCounter)
    {
        //If there still slot to show trick UI
        if (m_trickShown < m_maxTrickUIToSpawn)
        {
            if (m_listOfUINameComponent[m_trickShown] != null && aTrickName != null)
            {
                //Change the text to the name of the trick
                m_listOfUINameComponent[m_trickShown].text = aTrickName;
            }

            if (m_listOfUIIconComponent[m_trickShown] != null && aTrickIcon != null)
            {
                //Change the image to trick's icon and set the alpha to 1
                m_listOfUIIconComponent[m_trickShown].color = Color.white;
                m_listOfUIIconComponent[m_trickShown].sprite = aTrickIcon;
            }

            if (m_listOfUIMultiplierComponent[m_trickShown] != null && aTrickName != null)
            {
                //Change the text to the amount of counter
                m_listOfUIMultiplierComponent[m_trickShown].text = aTrickCounter.ToString() + "X";
            }

            //Increment counter
            m_trickShown++;
        }

        //InverseTrickDisplay();
    }

    //private void InverseTrickDisplay()
    //{
    //    for (int i = m_maxTrickUIToSpawn - 1; i > 0; i--)
    //    {
    //        if(m_listOfUINameComponent[i].text != string.Empty)
    //        {
    //            m_listOfUINameComponent[i - 1].text = m_listOfUINameComponent[i].text;
    //        }
    //    }
    //}


    /*
    Description: Check all the trick list and call SpawnUI function
    Parameters(Optional): aTrickName : Trick's name
                          aTrickIcon : Trick's icon
                          aTrickCounter : how many tricks player already performed
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes:
    */
    private void CheckTrickPerformedList()
    {
        //Reset all variable
        m_listOfSingleTrickPerformed.Clear();
        m_listOfComboTrickPerformed.Clear();
        m_trickShown = 0;
        ResetUI();


        IterateTroughList(m_trickPerformTracker.PLeftWeaponTrickList, m_trickPerformTracker.PRightWeaponTrickList);
        IterateTroughList(m_trickPerformTracker.PRightWeaponTrickList, m_trickPerformTracker.PLeftWeaponTrickList);
        IterateTroughList(m_trickPerformTracker.PComboTrickList);
    }


    /*
    Description: Iterate through single trick list and call SpawnUI function
    Parameters(Optional): aListOfTrickElement : List of first weapon's trick
                          aOtherList : List of other weapon's trick
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes:
    */
    private void IterateTroughList(List<CTrickElement> aListOfTrickElement, List<CTrickElement> aOtherList)
    {
        for (int i = aListOfTrickElement.Count - 1; i >=0 ; i--)
        {
            ATrickScoreModifiers tempScoreModifier = aListOfTrickElement[i].m_scoreModifier;
            if (!m_listOfSingleTrickPerformed.Contains(tempScoreModifier))
            {
                SpawnUI(tempScoreModifier.PTrickName,
                    tempScoreModifier.PTrickIcon,
                    aListOfTrickElement.FindAll((obj) => obj.m_scoreModifier == tempScoreModifier).Count +
                    aOtherList.FindAll((obj) => obj.m_scoreModifier == tempScoreModifier).Count);

                m_listOfSingleTrickPerformed.Add(tempScoreModifier);
            }
        }
    }

    /*
    Description: Iterate through combo trick list and call SpawnUI function
    Parameters(Optional): aListOfTrickElement : List of combo trick
    Creator: Juan Calvin Raymond
    Creation Date: 21 Mar 2017
    Extra Notes:
    */
    private void IterateTroughList(List<CTrickElement> aListOfTrickElement)
    {
        for (int i = aListOfTrickElement.Count - 1; i >= 0; i--)
        {
            AComboTrick tempScoreModifier = aListOfTrickElement[i].m_comboTrick;
            if (!m_listOfComboTrickPerformed.Contains(tempScoreModifier))
            {
                SpawnUI(tempScoreModifier.PTrickName,
                    tempScoreModifier.PTrickIcon,
                    aListOfTrickElement.FindAll((obj) => obj.m_comboTrick == tempScoreModifier).Count);

                m_listOfComboTrickPerformed.Add(tempScoreModifier);
            }
        }
    }

}
