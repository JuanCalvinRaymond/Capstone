using UnityEngine;
using System.Collections.Generic;

/*
Description: Abstract class of score modifier type
Creator: Juan Calvin Raymond
Creation Date: 25 Oct 2016
*/
public abstract class ATrickScoreModifiers : MonoBehaviour
{
    //Scoring System script
    protected CScoringSystem m_scoringSystem;
    private AudioSource m_audioSource;
    
    //List of index to remove weapon data from the list if trick is performed
    protected List<int> m_checkIndexToDelete;
    protected List<int> m_compareIndexToDelete;

    //Name of individual trick
    protected string m_trickName;

    //Icon of Individual trick
    public Sprite m_trickIcon;
    
    //Audio when the trick is performed
    public AudioClip m_celebrationSound;

    //How much the trick worth
    public int m_value;
    
    public CScoringSystem PScoringSystem
    {
        set
        {
            m_scoringSystem = value;
        }
    }

    public string PTrickName
    {
        get
        {
            return m_trickName;
        }
    }

    public Sprite PTrickIcon
    {
        get
        {
            return m_trickIcon;
        }
    }

    /*
    Description: Initialize variable
    Parameters: 
    Creator: Juan Calvin Raymond
    Creation Date: 6 Feb 2016
    */
    protected virtual void Awake()
    {
        m_checkIndexToDelete = new List<int>();
        m_compareIndexToDelete = new List<int>();
        m_audioSource = GetComponentInParent<AudioSource>();
    }
    
    
    /*
    Description: It checks if the current modifier should play sound
    Creator: Juan Calvin Raymond
    Creation Date:25 Oct 2016
    */
    public bool PlayCelebrationSound()
    {
        if (CGameManager.PInstanceGameManager != null && m_celebrationSound != null)//If there is a game manager and a celebration sound
        {
            if (CGameManager.PInstanceGameManager.PMainCameraGameObject != null)//If there is a camera object
            {
                //Play sound
                CUtilitySound.PlaySoundOneShot(m_audioSource, m_celebrationSound);
                return true;
            }
        }
        return false;
    }

    /*
    Description: Virtual function that check if the trick is performed or not
    Creator: Juan Calvin Raymond
    Creation Date: 25 Oct 2016
    */
    public virtual void CalculateTotalScore(List<GameObject> aTargetList, EWeaponHand aWeaponHand, float aTimeWhenShot, List<SWeaponData> aListOfLeftWeaponData, List<SWeaponData> aListOfRightWeaponData)
    {
        if (aListOfLeftWeaponData != null && aListOfRightWeaponData != null)
        {
            //Reset all variable
            m_checkIndexToDelete.Clear();
            m_compareIndexToDelete.Clear();
            
            switch (aWeaponHand)
            {
                case EWeaponHand.None:
                    break;
                //If the weapon is on right hand
                case EWeaponHand.RightHand:
                    //Compare right weapon data to left weapon data, if true then step over
                    if (IterateThroughList(aListOfRightWeaponData, aListOfLeftWeaponData, aTimeWhenShot, aWeaponHand))
                    {
                        //If there are entries to remove
                        if (m_checkIndexToDelete.Count > 0)
                        {
                            //Remove all data that used from the list
                            for (int i = m_checkIndexToDelete.Count - 1; i >= 0; i--)
                            {
                                aListOfRightWeaponData.RemoveAt(m_checkIndexToDelete[i]);
                            }
                        }
                        if(m_compareIndexToDelete.Count > 0)
                        {
                            for (int i = m_compareIndexToDelete.Count - 1; i >= 0; i--)
                            {
                                aListOfLeftWeaponData.RemoveAt(m_compareIndexToDelete[i]);
                            }
                        } 

                        //Call TrickDone function from scoring system script
                        m_scoringSystem.TrickDone(this, aWeaponHand);
                    }
                    break;
                //If the weapon is on left hand
                case EWeaponHand.LeftHand:
                    //Compare left weapon data to right weapon data, if true then step over
                    if (IterateThroughList(aListOfLeftWeaponData, aListOfRightWeaponData, aTimeWhenShot, aWeaponHand))
                    {
                        if (m_checkIndexToDelete.Count > 0)
                        {
                            //Remove all data that used from the list
                            for (int i = m_checkIndexToDelete.Count - 1; i >= 0; i--)
                            {
                                aListOfLeftWeaponData.RemoveAt(m_checkIndexToDelete[i]);
                            }
                        }
                        if (m_compareIndexToDelete.Count > 0)
                        {
                            for (int i = m_compareIndexToDelete.Count - 1; i >= 0; i--)
                            {
                                aListOfRightWeaponData.RemoveAt(m_compareIndexToDelete[i]);
                            }
                        }

                        //Call TrickDone function from scoring system script
                        m_scoringSystem.TrickDone(this, aWeaponHand);

                    }
                    break;
                case EWeaponHand.BothHands:
                    break;
                default:
                    break;
            }
        }
    }

    /*
    Description: Abstract function that all child need to implement
    Creator: Juan Calvin Raymond
    Creation Date:25 Oct 2016
    */
    protected abstract bool IterateThroughList(List<SWeaponData> aWeaponDataToCheck, List<SWeaponData> aWeaponDataToCompare, float aTimeWhenShot, EWeaponHand aWeaponHand);
    
}
