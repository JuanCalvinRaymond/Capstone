using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
Description: End of Screen Stats class
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 11-2-2016
Extra Notes:
*/
public class CEndGameStats : MonoBehaviour
{
    //All the text that need to update
    public Text m_scoreText;
    public Text m_shotFireText;
    public Text m_shotHitText;
    public Text m_longestStreakText;
    public Text m_tricksText;
    public Text m_combosText;

    /*
    Description: Update all the value
    Creator: Juan Calvin Raymond
    Creation Date: 11-2-2016
    */
    public void UpdateText()
    {
        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the game manager has a scoring system
            if (CGameManager.PInstanceGameManager.PScoringSystem != null)
            {
                //Score text
                CUtilitySetters.SetText2DText(ref m_scoreText,
                    "Score: " + CGameManager.PInstanceGameManager.PScoringSystem.PTotalScore.ToString());

                //Shots fired text
                CUtilitySetters.SetText2DText(ref m_shotFireText,
                    "Shot Fired: " + CGameManager.PInstanceGameManager.PScoringSystem.PShotFired.ToString());

                //Shots hit text
                CUtilitySetters.SetText2DText(ref m_shotHitText,
                    "Shot Hit: " + CGameManager.PInstanceGameManager.PScoringSystem.PShotHit.ToString());

                //Streak text
                CUtilitySetters.SetText2DText(ref m_longestStreakText,
                    "Longest Streak: " + CGameManager.PInstanceGameManager.PScoringSystem.PLongestStreak.ToString());

                //Tricks text
                CUtilitySetters.SetText2DText(ref m_tricksText,
                    "Tricks: " + CGameManager.PInstanceGameManager.PScoringSystem.PNumberTricks.ToString());

                //Combos text
                CUtilitySetters.SetText2DText(ref m_combosText,
                    "Combos: " + CGameManager.PInstanceGameManager.PScoringSystem.PNumberCombos.ToString());
            }
        }
    }
}
