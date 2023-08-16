using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

/*
Description: Class to show and control the Cheat UI
Creator: Alvaro Chavez Mixco
*/
public class CCheatHUD : MonoBehaviour
{
    //String constants used to display as "titles" in text
    private const string M_SCORE_TITLE = "Score: ";
    private const string M_NUMBER_TRICKS_TITLE = "Tricks: ";
    private const string M_ANIMATION_PERCENT_TITLE = "Animation %: ";
    private const string M_MOVE_SPEED_TITLE = "Move Speed: ";
    private const string M_RIGHT_WEAPON_TITLE = "Right Weapon: ";
    private const string M_LEFT_WEAPON_TITLE = "Left Weapon: ";
    private const string M_STYLE_PERCENT_TITLE = "Style %: ";
    private const string M_FPS_TITLE = "FPS: ";

    private CScoringSystem m_scoringSystem;
    private COnFireSystem m_onFireSystem;

    private CPlayerHand m_rightHand;
    private CPlayerHand m_leftHand;

    public List<GameObject> m_objectHider;
    public GameObject m_cheatObjectHider;
    public CCheatTool m_cheatTool;

    //Text game objects being modified
    public Text m_textScore;
    public Text m_textTricks;
    public Text m_textAnimationPercent;
    public Text m_textMoveSpeed;
    public Text m_textRightWeapon;
    public Text m_textLeftWeapon;
    public Text m_textStyle;
    public Text m_textFPS;

    /*
    Description: At start save the player object, and suscribe to the CheatTool change events
    Creator: Alvaro Chavez Mixco
    */
    void Start()
    {
        //If there is a cheat tool
        if (m_cheatTool != null)
        {
            //Subscribe to its change events
            m_cheatTool.OnDisplayStatsChange += HideCheatHUD;
            m_cheatTool.OnHideShowAllHUDForVideoTaking += HideHUD;
            m_cheatTool.OnPlatformSpeedChange += ChangePlatformSpeedText;

            //Initial states for events
            HideCheatHUD(m_cheatTool.PDisplayStats);
            ChangePlatformSpeedText(m_cheatTool.PPlatformSpeed);
        }

        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            m_rightHand = CGameManager.PInstanceGameManager.PPlayerWeaponHandler.PRightHand;
            m_leftHand = CGameManager.PInstanceGameManager.PPlayerWeaponHandler.PLeftHand;
            GameObject tempObject = null;
            AWeapon tempWeapon = null;
            CWeaponPhysics tempPhysics = null;
            CWeaponDataTracker tempDataTracker = null;

            if (m_rightHand != null)
            {
                m_rightHand.OnWeaponHeldChange += UpdateRightWeaponText;
                m_rightHand.GetCurrentWeaponComponents(ref tempObject, ref tempWeapon, ref tempPhysics, ref tempDataTracker);
                UpdateRightWeaponText(tempObject, tempWeapon, tempPhysics, tempDataTracker);

            }

            if (m_leftHand != null)
            {
                m_leftHand.OnWeaponHeldChange += UpdateLeftWeaponText;
                m_leftHand.GetCurrentWeaponComponents(ref tempObject, ref tempWeapon, ref tempPhysics, ref tempDataTracker);
                UpdateLeftWeaponText(tempObject, tempWeapon, tempPhysics, tempDataTracker);
            }

            m_scoringSystem = CGameManager.PInstanceGameManager.PScoringSystem;

            //If the scoring sytem is valid
            if (m_scoringSystem != null)
            {
                //Subscribe to events
                m_scoringSystem.OnScoreChange += ChangeScoreText;
                m_scoringSystem.OnTrickDetected += ChangeTricksText;

                //Initial state
                ChangeScoreText(m_scoringSystem.PTotalScore);
                ChangeTricksText(null, EWeaponHand.None);//Parameter doesn't matter in this function , they are only used to match event signature
            }

            //Get OnFireSystem from game manager
            m_onFireSystem = CGameManager.PInstanceGameManager.POnFireSystem;

            if (m_onFireSystem != null)
            {
                //Subscribe to events
                m_onFireSystem.OnStyleChanged += ChangeStyleText;

                ChangeStyleText(m_onFireSystem.PCurrentStyleMeter);
            }
        }
    }

    /*
    Description: On destroy unsuscribe from the respective events.
    Creator: Alvaro Chavez Mixco
    */
    private void OnDestroy()
    {
        //If there is a cheat tool
        if (m_cheatTool != null)
        {
            //Unsubscribe from events
            m_cheatTool.OnDisplayStatsChange -= HideCheatHUD;
            m_cheatTool.OnHideShowAllHUDForVideoTaking -= HideHUD;
            m_cheatTool.OnPlatformSpeedChange -= ChangePlatformSpeedText;
        }

        if (m_rightHand != null)
        {
            m_rightHand.OnWeaponHeldChange -= UpdateRightWeaponText;
        }

        if (m_leftHand != null)
        {
            m_leftHand.OnWeaponHeldChange -= UpdateLeftWeaponText;
        }

        //If the scoring sytem is valid
        if (m_scoringSystem != null)
        {
            //Unsubscribe from event
            m_scoringSystem.OnScoreChange -= ChangeScoreText;
            m_scoringSystem.OnTrickDetected -= ChangeTricksText;
        }

        if (m_onFireSystem != null)
        {
            //Unsubscribe to events
            m_onFireSystem.OnStyleChanged -= ChangeStyleText;
        }
    }

    /*
    Description: Update the player data and the cheat toll animation percent of completion.
    Creator: Alvaro Chavez Mixco
    Extra Notes: This is done in update and not through events because this values are constantly updated.
    */
    void Update()
    {
        //If the cheat tool is valid
        if (m_cheatTool != null)
        {
            //Set the text for platform animation percent
            CUtilitySetters.SetText2DText(ref m_textAnimationPercent, M_ANIMATION_PERCENT_TITLE +
                (CUtilityMath.RoundTo2Digits(m_cheatTool.PPercentagePlatformAnimation) * 100.0f).ToString());
        }

        CUtilitySetters.SetText2DText(ref m_textFPS, M_FPS_TITLE + 1 / Time.smoothDeltaTime);
    }

    /*
    Description: Hide and show the hider object.
    Parameters: bool aStatus- If we want to show or hide the Cheat UI
    Creator: Alvaro Chavez Mixco
    Extra Notes: This event is usually called through functions from CCheatTool. The object  should be the parent
    of all the cheat UI elements.
    */
    private void HideHUD(bool aStatus)
    {
        //Enable or show the hider object
        foreach (GameObject go in m_objectHider)
        {
            CUtilitySetters.SetActiveStatus(go,aStatus);
        }
    }

    /*
    Description: Hide and show the hider object.
    Parameters: bool aStatus- If we want to show or hide the Cheat UI
    Creator: Alvaro Chavez Mixco
    Extra Notes: This event is usually called through functions from CCheatTool. The object  should be the parent
    of all the cheat UI elements.
    */
    private void HideCheatHUD(bool aStatus)
    {
        m_cheatObjectHider.SetActive(aStatus);
    }

    /*
    Description: When the platform speed changes update the text
    Parameters: float aSpeed-The speed of the platform that will be displayed in the UI.
    Creator: Alvaro Chavez Mixco
    Extra Notes: This event is usually called through functions from CCheatTool.
    */
    private void ChangePlatformSpeedText(float aSpeed)
    {
        CUtilitySetters.SetText2DText(ref m_textMoveSpeed, M_MOVE_SPEED_TITLE + CUtilityMath.RoundTo2Digits(aSpeed).ToString());
    }

    /*
    Description: When the score changes update the text
    Parameters: int aTotalScore- The current total score of the player
    Creator: Alvaro Chavez Mixco
    Extra Notes: This event is usually called through functions from CScoringSystem.
    */
    private void ChangeScoreText(int aTotalScore)
    {
        CUtilitySetters.SetText2DText(ref m_textScore, M_SCORE_TITLE + aTotalScore.ToString());
    }

    /*
    Description: When the score changes update the text
    Parameters: int aTotalScore- The current total score of the player
    Creator: Alvaro Chavez Mixco
    Extra Notes: This event is usually called through functions from CScoringSystem.
    */
    private void ChangeStyleText(float aTotalStyle)
    {
        CUtilitySetters.SetText2DText(ref m_textStyle, M_STYLE_PERCENT_TITLE + (CUtilityMath.RoundTo2Digits(aTotalStyle) * 100.0f).ToString());
    }

    /*
    Description: When the number of tricks changes update the text
    Parameters: The parameters in this function are not used. They only exist
    to match thE TrickssDone event (from CScoring System) signature
    Creator: Alvaro Chavez Mixco
    Extra Notes: This event is usually called through functions from CScoringSystem.
    */
    private void ChangeTricksText(ATrickScoreModifiers aScoreModifier, EWeaponHand aHandThatHeldWeapon)
    {
        int totalTricks = 0;

        //If there is a scoring system
        if (m_scoringSystem != null)
        {
            //Get the curent number of tricks
            totalTricks = m_scoringSystem.PNumberTricks;
        }

        //Update the trick text
        CUtilitySetters.SetText2DText(ref m_textTricks, M_NUMBER_TRICKS_TITLE + totalTricks.ToString());
    }

    /*
    Description: Update the weapon text for the right weapon
    Parameters: GameObject aWeaponGameObject- The game object containing the weapon
                AWeapon aWeaponScript - The weapon script with the required informaiton
                CWeaponPhysics aWeaponPhysics - The physics script of the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 22nd, 2017
    */
    private void UpdateRightWeaponText(GameObject aWeaponGameObject, AWeapon aWeaponScript, CWeaponPhysics aWeaponPhysics, CWeaponDataTracker aWeaponDataTracker)
    {
        UpdateWeaponText(aWeaponScript, M_RIGHT_WEAPON_TITLE, ref m_textRightWeapon);
    }

    /*
    Description: Update the weapon text for the left weapon
    Parameters: GameObject aWeaponGameObject- The game object containing the weapon
                AWeapon aWeaponScript - The weapon script with the required informaiton
                CWeaponPhysics aWeaponPhysics - The physics script of the weapon
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 22nd, 2017
    */
    private void UpdateLeftWeaponText(GameObject aWeaponGameObject, AWeapon aWeaponScript, CWeaponPhysics aWeaponPhysics, CWeaponDataTracker aWeaponDataTracker)
    {
        UpdateWeaponText(aWeaponScript, M_LEFT_WEAPON_TITLE, ref m_textLeftWeapon);
    }

    /*
    Description: Update the desired text with the weapon type contained in the weapon script
    Parameters: AWeapon aWeaponScript - The weapon script
                string aTitle - The title that will precede the weapon type
                ref Text m_textToModify - The text that will display the title and weapon type
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, March 22nd, 2017
    */
    private void UpdateWeaponText(AWeapon aWeaponScript, string aTitle, ref Text m_textToModify)
    {
        EWeaponTypes currentWeaponType = EWeaponTypes.None;

        //If there is a valid weapon script
        if (aWeaponScript != null)
        {
            //Get its weapon type
            currentWeaponType = aWeaponScript.PWeaponType;
        }

        //Set the text in the UI
        CUtilitySetters.SetText2DText(ref m_textToModify, aTitle + currentWeaponType.ToString());
    }
}
