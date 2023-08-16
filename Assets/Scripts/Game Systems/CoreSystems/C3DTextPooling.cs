using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/*
Description: Class that contain a list of inactive score text game object
Creator: Juan Calvin Raymond
Creation Date: 14 Mar 2017
*/
public class C3DTextPooling : MonoBehaviour
{
    //List of text mesh component
    private List<TextMesh> m_listOfInactiveScorePopUp;

    //Score text prefab
    public GameObject m_textPrefab;

    //Variable to tweak in inspector 
    public int m_amountOfText;

    public List<TextMesh> PInactiveList
    {
        get
        {
            return m_listOfInactiveScorePopUp;
        }
    }


    /*
    Description: Instantiate score text prefab and add it to the list
    Creator: Juan Calvin Raymond
    Creation Date: 14 Mar 2017
    */
    private void Awake()
    {
        m_listOfInactiveScorePopUp = new List<TextMesh>();

        for (int i = 0; i < m_amountOfText; i++)
        {
            GameObject textClone = (GameObject)Instantiate(m_textPrefab, transform);

            TextMesh tempTextMesh = textClone.GetComponent<TextMesh>();

            tempTextMesh.text = string.Empty;

            m_listOfInactiveScorePopUp.Add(tempTextMesh);
        }
    }
    
}
