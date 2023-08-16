using UnityEngine;
using System.Collections;

/*
Description: Class to simply make a "splash screen". Once the time is over on this script, or
if the user skips it, this script would load up a new scene.
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, January 28, 2017
*/
class CCreditsDisplay : MonoBehaviour
{
    private Vector3 m_startingLocalPosition;

    private float m_timerCreditsRoll = 0.0f;
    private float m_percentScrolled = 0.0f;

    [Header("Position of Credits")]
    public Vector3 m_offsetStartingLocalPosition = Vector3.zero;
    public Vector3 m_offsetEndingLocalPosition = Vector3.zero;

    [Header("Credits Display")]
    public float m_scrollTime = 5.0f;
    public bool m_skippable = true;

    [Tooltip("Object to enable when the credits are over.")]
    public GameObject m_objectToShowAtEnd = null;

    /*
     Description: Save the starting position of the credits.
     Creator: Alvaro Chavez Mixco
     Creation Date: Saturday, January 28, 2017
     */
    private void Awake()
    {
        //Save the starting local position
        m_startingLocalPosition = transform.localPosition + m_offsetStartingLocalPosition;
    }

    /*
    Description: Scroll the credits according to the percentage of time completed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void Update()
    {
        //If the credits are skipable
        if (m_skippable == true)
        {
            //If any key was pressed
            if (CGameManager.PInstanceGameManager.PPlayerController.GetInterruptKeyPressed() == true)
            {
                //End the credits
                EndCredits();
            }
        }

        //Increase the timer
        m_timerCreditsRoll += CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        //Get the percentage of scroll completed
        m_percentScrolled = m_timerCreditsRoll / m_scrollTime;

        //Scroll (move) the credits
        ScrollCredits();

        //If the percent scrolled is beyond 100%
        if (m_percentScrolled >= 1.0f)
        {
            //End the credits
            EndCredits();
        }
    }

    /*
    Description: Scroll the credits according to the percentage of time completed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void ScrollCredits()
    {
        //Move the credits object local postion according to the percent completed, in relation to the starting and ending positoon
        transform.localPosition = Vector3.Lerp(m_offsetStartingLocalPosition, m_offsetEndingLocalPosition, m_percentScrolled);
    }

    /*
    Description: Prepare the credtis so that they can begin scrolling
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void ResetCredits()
    {
        //Reset timers and percents
        m_timerCreditsRoll = 0.0f;
        m_percentScrolled = 0.0f;

        //Move the object to the starting positon, plus it starting offset
        transform.localPosition = m_startingLocalPosition;
    }

    /*
     Description: Restart the credits if the object is enabled
     Creator: Alvaro Chavez Mixco
     Creation Date: Saturday, January 28, 2017
     */
    private void OnEnable()
    {
        //Restart the credits
        ResetCredits();
    }

    /*
    Description: Disable the credits, and if applicable show another object.
    Creator: Alvaro Chavez Mixco
    Creation Date: Saturday, January 28, 2017
    */
    private void EndCredits()
    {
        //Disable the credits game object
        gameObject.SetActive(false);

        //If the object we want to show is valid
        if (m_objectToShowAtEnd != null)
        {
            //Check if the object has an IMenu interface
            IMenu menuObject = m_objectToShowAtEnd.GetComponent<IMenu>();

            //If it has an IMenu interface
            if (menuObject != null)
            {
                //Call its activate fucntion
                menuObject.Activate();
            }
            else //If the object doesn't have an IMenu interface
            {
                //Enable it
                m_objectToShowAtEnd.SetActive(true);
            }
        }
    }
}
