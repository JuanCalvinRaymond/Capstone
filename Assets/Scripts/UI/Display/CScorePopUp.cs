using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Description: Score will move to certain position using easing library
Creator: Juan Calvin Raymond
Creation Date: 9 Mar 2017
*/
public class CScorePopUp : MonoBehaviour
{
    //Text mesh component
    private TextMesh m_textMesh;

    //Easing script
    private CEaseVector3 m_ease;
    
    //Score starting position
    private Vector3 m_startingPosition;

    //Variable to tweak in editor
    public SEaseSettings m_easeSetting;
    public Vector3 m_targetEase;

    public float m_maxTextScale;
    public float m_minTextScale;
    public int m_maxScoreScale;
    public int m_minScoreScale;

    /*
    Description: Initialize variable
    Creator: Juan Calvin Raymond
    Creation Date: 9 Mar 2017
    */
    private void Awake()
    {
        m_textMesh = GetComponent<TextMesh>();
        m_ease = new CEaseVector3(m_easeSetting.m_easeType, m_easeSetting.m_easeMode);
    }
    

    /*
    Description: Update object position if ease script is running, and reset if the object reach it's destination
    Creator: Juan Calvin Raymond    
    Creation Date: 9 Mar 2017
    */
    private void Update()
    {
        //If ease scipt is still running
        if(m_ease.GetEasingTimer() <= m_easeSetting.m_duration)
        {
            //Update the local position
            transform.localPosition = m_ease.GetValue();
                
        }
        else
        {
            //Call ResetScorePopUp function
            ResetScorePopUp();
        }
    }

    /*
    Description: Set the text to target's score and run the ease script
    Creator: Juan Calvin Raymond    
    Creation Date: 9 Mar 2017
    */
    public void StartEasing(Vector3 aPositionToSpawn, Quaternion aRotationToSpawn, int aScoreValue)
    {
        //Initialize position and rotation of the text
        gameObject.SetActive(true);
        m_textMesh.text = aScoreValue.ToString();
        float scale = CUtilityMath.RescaleRangeClamp(aScoreValue, m_minScoreScale, m_maxScoreScale, m_minTextScale, m_maxTextScale);
        m_textMesh.transform.localScale = new Vector3(scale, scale, scale);
        transform.position = aPositionToSpawn;
        transform.rotation = aRotationToSpawn;
        
        //Grab the local position as starting value
        m_startingPosition = transform.localPosition;

        //Set easing parameter
        m_ease.SetEase(m_startingPosition, m_targetEase, m_easeSetting.m_duration, this, m_easeSetting.m_extraParameter);
        
        //Call run on ease script
        m_ease.Run();
    }
    
    /*
    Description: Put the object back to it's original position
    Creator: Juan Calvin Raymond    
    Creation Date: 9 Mar 2017
    */
    private void ResetScorePopUp()
    {
        //Stop ease script
        m_ease.Stop();

        //Reset the score and put it back to the list
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        CGameManager.PInstanceGameManager.PListOfInactive3DText.Add(m_textMesh);
    }
}
