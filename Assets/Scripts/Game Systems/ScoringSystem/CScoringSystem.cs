using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
Description: Basic scoring system that iterate through all score modifier and calculate the total calculation
Creator: Juan Calvin Raymond
Creation Date: 25 Oct 2016
*/
public class CScoringSystem : MonoBehaviour
{
    //player Weapon Handler script
    private CPlayerWeaponHandler m_weaponHandler;

    //Basic Score script
    private CBasicScore m_basicScore;

    //Trick perform tracker script
    private CTrickPerformTracker m_trickPerformTracker;

    //On fire System script
    private COnFireSystem m_onFireSystem;

    //Player stats
    private int m_totalScore = 0;
    private int m_totalScoreCalculation = 0;
    private int m_shotsFired = 0;
    private int m_shotsHit = 0;
    private int m_streak = 0;
    private int m_longestStreak = 0;
    private int m_numberTricks = 0;
    private int m_numberCombos = 0;
    private float m_completionTime = 0.0f;

    private bool m_soundPlayed = false;

    //List of text to grab
    private List<TextMesh> m_listOfInactiveScore;
    
    //list of modifier
    public List<ATrickScoreModifiers> m_listOfScoreModifier;

    //Event variable
    public delegate void delegateTrickDetected(ATrickScoreModifiers aScoreModifier, EWeaponHand aHandThatHeldWeapon);
    public event delegateTrickDetected OnTrickDetected;

    public delegate void delegateComboDetected(AComboTrick aComboModifier, int aWeight);
    public event delegateComboDetected OnComboDetected;

    public delegate void delegScoreChange(int aScore);
    public event delegScoreChange OnScoreChange;
    

    public int PTotalScore
    {
        get
        {
            return m_totalScore;
        }
    }

    public int PShotFired
    {
        get
        {
            return m_shotsFired;
        }
        set
        {
            m_shotsFired = value;
        }
    }

    public int PShotHit
    {
        get
        {
            return m_shotsHit;
        }
        set
        {
            m_shotsHit = value;
        }
    }

    public int PLongestStreak
    {
        get
        {
            return m_longestStreak;
        }
    }

    public int PStreak
    {
        get
        {
            return m_streak;
        }
        set
        {
            m_streak = value;
        }
    }

    public int PNumberTricks
    {
        get
        {
            return m_numberTricks;
        }
    }

    public int PNumberCombos
    {
        get
        {
            return m_numberCombos;
        }
    }

    public float PCompletionTime
    {
        get
        {
            return m_completionTime;
        }
    }

    public bool PSoundPlayed
    {
        set
        {
            m_soundPlayed = value;
        }
    }
    
    /*
    Description: Instantiate variable
    Creator: Juan Calvin Raymond
    Creation Date: 25 Oct 2016
    */
    private void Awake()
    {
        m_basicScore = GetComponent<CBasicScore>();
        m_onFireSystem = GetComponentInParent<COnFireSystem>();
        m_trickPerformTracker = GetComponent<CTrickPerformTracker>();
    }

    /*
    Description: Setting this script reference to all the modifiers
    Creator: Juan Calvin Raymond
    Creation Date:25 Oct 2016
    */
    private void Start()
    {
        if (CGameManager.PInstanceGameManager != null)
        {
            if (CGameManager.PInstanceGameManager.PPlayerWeaponHandler != null)
            {
                //Get player from the game manager
                m_weaponHandler = CGameManager.PInstanceGameManager.PPlayerWeaponHandler;
            }
        }

        //Iterate through all modifier in the list and give this script reference to all the modifiers 
        if (m_listOfScoreModifier != null)
        {
            foreach (ATrickScoreModifiers modifier in m_listOfScoreModifier)
            {
                if (modifier != null)
                {
                    modifier.PScoringSystem = this;
                }
            }
        }

        //Set the 3d text object pool
        m_listOfInactiveScore = CGameManager.PInstanceGameManager.PListOfInactive3DText;

        //Subscribe to each weapon's OnFire event
        if (m_weaponHandler.PCurrentRightWeaponScript != null)
        {
            m_weaponHandler.PCurrentRightWeaponScript.OnTargetHit += CalculateScore;
        }

        if (m_weaponHandler.PCurrentLeftWeaponScript != null)
        {
            m_weaponHandler.PCurrentLeftWeaponScript.OnTargetHit += CalculateScore;
        }
    }


    /*
    Description: Unsubscribe from event
    Creator: Juan Calvin Raymond
    Creation Date : 28 Feb 2017
    */
    private void OnDestroy()
    {
        if (m_weaponHandler != null)
        {
            if (m_weaponHandler.PCurrentRightWeaponScript != null)
            {
                m_weaponHandler.PCurrentRightWeaponScript.OnTargetHit -= CalculateScore;
            }

            if (m_weaponHandler.PCurrentLeftWeaponScript != null)
            {
                m_weaponHandler.PCurrentLeftWeaponScript.OnTargetHit -= CalculateScore;
            }
        }
    }

    /*
    Description: Change the longest streak if player have longer streak, Check if player grab a new weapon, Register weapon data to a list, Check if player hit a target and calculate the score
    Creator: Juan Calvin Raymond
    Creation Date:2 Des 2016
    */
    private void Update()
    {
        //Check if the current streak is bigger than the longest streak, if yes then update longest streak
        if (m_streak > m_longestStreak)
        {
            m_longestStreak = m_streak;
        }

        //If there is a game manager
        if (CGameManager.PInstanceGameManager != null)
        {
            //If the user is playing
            if (CGameManager.PInstanceGameManager.PGameState == EGameStates.Play)
            {
                //Increase the completion time
                m_completionTime += CGameManager.PInstanceGameManager.GetScaledDeltaTime();

                
            }
        }
    }

    /*
    Description: Add the total score
    Parameters: aTotalCalculation : Total score after multiply by the trick
    Creator: Juan Calvin Raymond
    Creation Date:25 Oct 2016
    */
    public void AddCurrentScoreCalculationToTotal(int aTotalCalculation)
    {
        //Increase total score
        m_totalScore += aTotalCalculation;
        
        //Call event scores changed
        if (OnScoreChange != null)
        {
            OnScoreChange(m_totalScore);
        }
    }

    /*
    Description: Calculate total score gain by going through all the score modifier
    Parameter :  aCurrentAmmo : weapon's current ammo
                 aWeaponHand : hand that hold the weapon
                 aTimeWhenShot : time when weapon shoot
    Creator: Juan Calvin Raymond
    Creation Date: 2 Dec 2016
    */
    public void CalculateScore(List<GameObject> aListOfTargetHit, EWeaponHand aWeaponHand, float aTimeWhenShot)
    {
        //make sure calculation starts at 0
        m_totalScoreCalculation = 0;

        //Reset values
        m_soundPlayed = false;

        //Call CalculateTotalScore function from basicscore script
        m_basicScore.CalculateTotalScore(aListOfTargetHit, ref m_totalScoreCalculation);
        
        //Go through all the current modifiers
        foreach (ATrickScoreModifiers currentModifier in m_listOfScoreModifier)
        {
            if (currentModifier != null)
            {
                //Call CalculateTotalScore function from modifier
                currentModifier.CalculateTotalScore(aListOfTargetHit, aWeaponHand, aTimeWhenShot, m_weaponHandler.PListOfLeftWeaponData, m_weaponHandler.PListOfRightWeaponData);
            }
        }

        //Add total calculation to the total score
        m_shotsHit += aListOfTargetHit.Count;
        m_streak += aListOfTargetHit.Count;
        
        //Multiply the score with OnFire Multiplier
        m_totalScoreCalculation *= m_onFireSystem.PMultiplier;
        if (m_listOfInactiveScore.Count > 0)
        {
            foreach (GameObject target in aListOfTargetHit)
            {
                //Place 3D text to the target that got hit
                m_listOfInactiveScore[0].gameObject.GetComponent<CScorePopUp>().StartEasing(target.transform.position, CUtilityGame.LookAtPlayer(target), m_totalScoreCalculation);
                m_listOfInactiveScore.RemoveAt(0);
            }
        }
        //Add the total calculation to total score
        AddCurrentScoreCalculationToTotal(m_totalScoreCalculation);
    }

    /*
    Description: Call the event function
    Parameters: aScoreModifier : Score modifier script
                aHandThatHeldWeapon : Hand that held the gun
                aTotalCalculation : Total score after multiply by the trick
    Creator: Juan Calvin Raymond
    Creation Date: 4 Des 2016
    Extra Notes: I made this function so other script can call the event function.
    */
    public void TrickDone(ATrickScoreModifiers aScoreModifier, EWeaponHand aHandThatHeldWeapon)
    {     
        //Increase the number of tricks done
        m_numberTricks++;
        
        //Play celebration sound
        if (m_soundPlayed == false)//Ensure only 1 celebration sound is played
        {
            m_soundPlayed = aScoreModifier.PlayCelebrationSound();//Set that we have already played a sound if applicable
        }

        if (OnTrickDetected != null)
        {
            OnTrickDetected(aScoreModifier, aHandThatHeldWeapon);
        }

        //Check how many time the trick already performed
        int totalTrickPerformed = m_trickPerformTracker.PLeftWeaponTrickList.FindAll((obj) => obj.m_scoreModifier == aScoreModifier).Count +
                    m_trickPerformTracker.PRightWeaponTrickList.FindAll((obj) => obj.m_scoreModifier == aScoreModifier).Count;

        //Add trick value to total calculation
        m_totalScoreCalculation += aScoreModifier.m_value / (totalTrickPerformed > 1 ? totalTrickPerformed : 1);
    }

    /*
    Description: Call the event function
    Parameters: aComboModifer : Score modifier script
                aWeight : Weight of the modifier
                aTotalCalculation : Total score after multiply by the trick
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2017
    Extra Notes: I made this function so other script can call the event function.
    */
    public void ComboDone(AComboTrick aComboModifer, int aWeight)
    {
        //Increase the number of tricks done
        m_numberCombos++;
        
        if (OnComboDetected != null)
        {
            OnComboDetected(aComboModifer, aWeight);
        }

        //Check how many time the trick already performed
        int totalComboTrickPerformed = m_trickPerformTracker.PComboTrickList.FindAll((obj) => obj.m_comboTrick == aComboModifer).Count;

        //Add trick value to the total score
        AddCurrentScoreCalculationToTotal(aComboModifer.m_value / (totalComboTrickPerformed > 1 ? totalComboTrickPerformed : 1));
    }

    /*
    Description: adding score modifier to the modifier list
    Parameters: aModifier : Trick modifier script
    Creator: Juan Calvin Raymond
    Creation Date:25 Oct 2016
    Extra Notes:
    */
    public void AddScoreModifier(ATrickScoreModifiers aModifier)
    {
        if (m_listOfScoreModifier != null)//If the modifier list is valid
        {
            m_listOfScoreModifier.Add(aModifier);//Add the modifier
        }
    }

    /*
    Description: Return the accuracy percentage of the player
    Creator: Alvaro Chavez Mixco
    Creation Date:25 Oct 2016
    Extra Notes:
    */
    public float GetAccuracy()
    {
        //If no shot has been fired
        if (m_shotsFired <= 0)
        {
            return 0;//Return 0, to avoid divides by 0
        }

        //Return the percent of shots fire that have actually hit the target. Multiply by 100 so that is within the 0-100 range and not 0-1.
        return ((float)m_shotsHit / (float)m_shotsFired) * 100.0f;
    }

    /*
    Description: Function to easily fill a SPlayerEntry object with all the pertinent data of the player performance through the level.
    Parameters:  ref SPlayerEntry aPlayerEntry-The player entry object we want to fill with data. This is passed by reference.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, Novemeber 15, 2016
    Extra Notes: The name store in the player entry is NOT changed by this function
    */
    public void GetPlayerScoringData(ref SPlayerEntry aPlayerEntry)
    {
        //Fill the player entry being passed in with data
        aPlayerEntry.m_score = PTotalScore;
        aPlayerEntry.m_accuracy = GetAccuracy();
        aPlayerEntry.m_longestStreak = PLongestStreak;
        aPlayerEntry.m_numberOfTricks = PNumberTricks;
        aPlayerEntry.m_numberOfCombos = PNumberCombos;
        aPlayerEntry.m_shotsFired = PShotFired;
        aPlayerEntry.m_shotsHit = PShotHit;
        aPlayerEntry.m_completionTime = PCompletionTime;
    }
}
