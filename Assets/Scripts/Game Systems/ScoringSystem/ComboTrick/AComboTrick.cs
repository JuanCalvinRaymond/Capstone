using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
Description: Abstract combo trick class, It contain regular member and function
Parameters: 
Creator: Juan Calvin Raymond
Creation Date: 26 Jan 2016
Extra Notes:
*/
public abstract class AComboTrick : MonoBehaviour
{
    //Scoring system script
    protected CScoringSystem m_scoringSystem;

    //Name of individual trick
    protected string m_trickName;

    //List of Index to be delete once combo is performed
    protected List<int> m_listOfRightWeaponIndexToDelete;
    protected List<int> m_listOfLeftWeaponIndexToDelete;

    //Icon of Individual trick
    public Sprite m_trickIcon;

    //Variable to tweak in inspector
    public int m_value;
    public int m_weight = 1;

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

    public CScoringSystem PScoringSystem
    {
        set
        {
            m_scoringSystem = value;
        }
    }

    private void Start()
    {
        m_listOfRightWeaponIndexToDelete = new List<int>();
        m_listOfLeftWeaponIndexToDelete = new List<int>();
    }

    /*
    Description: Virtual function of how the iterate through list works
    Parameters: aLeftWeaponTrickList : Trick list on the left weapon
                aRightWeaponTrickList : Trick list on the right weapon
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    Extra Notes: it's difference between if checking single weapon or both weapon
    */
    public virtual bool ComboCheck(List<CTrickElement> aLeftWeaponTrickList, List<CTrickElement> aRightWeaponTrickList)
    {

        m_listOfRightWeaponIndexToDelete.Clear();
        m_listOfLeftWeaponIndexToDelete.Clear();

        //If both weapon passed IterateThroughList function
        if (IterateThroughList(aLeftWeaponTrickList, ref m_listOfLeftWeaponIndexToDelete) && IterateThroughList(aRightWeaponTrickList, ref m_listOfRightWeaponIndexToDelete))
        {
            if(m_scoringSystem != null)
            {
                //Call TrickDone function from scoring sytem script
                m_scoringSystem.ComboDone(this, m_weight);

                DeleteElementFromTheList(aLeftWeaponTrickList, aRightWeaponTrickList);
                return true;
            }
        }
        return false;
    }

    protected void DeleteElementFromTheList(List<CTrickElement> aLeftWeaponTrickList, List<CTrickElement> aRightWeaponTrickList)
    {

        if(m_listOfRightWeaponIndexToDelete.Count > 0)
        {
            for (int i = m_listOfRightWeaponIndexToDelete.Count - 1; i >= 0; i--)
            {
                aRightWeaponTrickList.RemoveAt(m_listOfRightWeaponIndexToDelete[i]);
            }
        }
        
        if(m_listOfLeftWeaponIndexToDelete.Count > 0)
        {
            for (int i = m_listOfLeftWeaponIndexToDelete.Count - 1; i >= 0; i--)
            {
                aLeftWeaponTrickList.RemoveAt(m_listOfLeftWeaponIndexToDelete[i]);
            }
        }


    }

    /*
    Description: Abstract function how each trick checks the list
    Parameters: aListToCheck : Trick list to check
    Creator: Juan Calvin Raymond
    Creation Date: 26 Jan 2016
    Extra Notes:
    */
    protected abstract bool IterateThroughList(List<CTrickElement> aListToCheck, ref List<int> aIndexList);
}
