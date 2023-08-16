using UnityEngine;
using System.Collections;

/*
Description: Destroy the gameobject after the life time is over
Parameters(Optional):
Creator: Juan Calvin Raymond
Creation Date: 9 Dec 2016
Extra Notes:
*/
public class CDestroyOverTime : MonoBehaviour
{
    private float m_lifeDuration;

    public float m_lifeTime = 0;


    /*
    Description: Initialize the timer
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 9 Dec 2016
    Extra Notes:
    */
    private void Start()
    {
        m_lifeDuration = m_lifeTime;
    }

    /*
    Description: Decrease the timer and destroy the gameobject when the timer is finished
    Parameters(Optional):
    Creator: Juan Calvin Raymond
    Creation Date: 9 Dec 2016
    Extra Notes:
    */
    private void Update()
    {
        m_lifeDuration -= CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        if(m_lifeDuration < 0)
        {
            Destroy(gameObject);
        }
    }
}
