using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTrickPerformTracker : MonoBehaviour
{
    //Score system script
    private CScoringSystem m_scoringSystem;

    private CTrickElement m_trickElement;

    private List<CTrickElement> m_leftWeaponTrickList;
    private List<CTrickElement> m_rightWeaponTrickList;
    private List<CTrickElement> m_comboTrickList;

    public float m_trickLifeTimer = 3;

    public delegate void delegOnListChange();
    public event delegOnListChange OnListChange;

    public List<CTrickElement> PLeftWeaponTrickList
    {
        get
        {
            return m_leftWeaponTrickList;
        }
        set
        {
            m_leftWeaponTrickList = value;
        }
    }

    public List<CTrickElement> PRightWeaponTrickList
    {
        get
        {
            return m_rightWeaponTrickList;
        }
        set
        {
            m_rightWeaponTrickList = value;
        }
    }

    public List<CTrickElement> PComboTrickList
    {
        get
        {
            return m_comboTrickList;
        }
        set
        {
            m_comboTrickList = value;
        }
    }

    private void Awake()
    {
        m_rightWeaponTrickList = new List<CTrickElement>();
        m_leftWeaponTrickList = new List<CTrickElement>();
        m_comboTrickList = new List<CTrickElement>();
    }

    // Use this for initialization
    private void Start()
    {
        m_scoringSystem = CGameManager.PInstanceGameManager.PScoringSystem;

        m_scoringSystem.OnTrickDetected += AddSingleTrickToTheList;
        m_scoringSystem.OnComboDetected += AddComboTrickToTheList;
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_rightWeaponTrickList.Count > 0)
        {
            for (int i = 0; i < m_rightWeaponTrickList.Count; i++)
            {
                m_rightWeaponTrickList[i].m_lifeTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();
            }

            if (m_rightWeaponTrickList.RemoveAll((obj) => obj.m_lifeTimer > m_trickLifeTimer) > 0)
            {
                if (OnListChange != null)
                {
                    OnListChange();
                }
            }
        }


        if(m_leftWeaponTrickList.Count > 0)
        {
            for (int i = 0; i < m_leftWeaponTrickList.Count; i++)
            {
                m_leftWeaponTrickList[i].m_lifeTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();
                
            }

            if (m_leftWeaponTrickList.RemoveAll((obj) => obj.m_lifeTimer > m_trickLifeTimer) > 0)
            {
                if (OnListChange != null)
                {
                    OnListChange();
                }
            }
        }

        if (m_comboTrickList.Count > 0)
        {
            for (int i = 0; i < m_comboTrickList.Count; i++)
            {
                m_comboTrickList[i].m_lifeTimer += CGameManager.PInstanceGameManager.GetScaledDeltaTime();

            }

            if (m_comboTrickList.RemoveAll((obj) => obj.m_lifeTimer > m_trickLifeTimer) > 0)
            {
                if (OnListChange != null)
                {
                    OnListChange();
                }
            }
        }
    }

    private void AddSingleTrickToTheList(ATrickScoreModifiers aScoreModifier, EWeaponHand aHandThatHeldWeapon)
    {
        m_trickElement = new CTrickElement();
        m_trickElement.Init(aScoreModifier);

        switch (aHandThatHeldWeapon)
        {
            case EWeaponHand.RightHand:
                m_rightWeaponTrickList.Add(m_trickElement);

                if(OnListChange != null)
                {
                    OnListChange();
                }
                break;
            case EWeaponHand.LeftHand:
                m_leftWeaponTrickList.Add(m_trickElement);

                if (OnListChange != null)
                {
                    OnListChange();
                }
                break;
            default:
                break;
        }
    }

    private void AddComboTrickToTheList(AComboTrick aComboTrick, int aWeight)
    {
        m_trickElement = new CTrickElement();
        m_trickElement.Init(aComboTrick);

        m_comboTrickList.Add(m_trickElement);

        if (OnListChange != null)
        {
            OnListChange();
        }
    }

    public void ChangeListEvent()
    {
        if(OnListChange != null)
        {
            OnListChange();
        }
    }

}
