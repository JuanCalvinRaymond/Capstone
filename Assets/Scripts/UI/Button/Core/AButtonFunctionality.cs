using UnityEngine;
using System.Collections;

/*
Description: Base class intended to work with CButton events. This class
             servers merely as an abstract class so that the child classes
             can do certain functionality according to the timing of the CButton events.
Creator: Alvaro Chavez Mixco
Creation Date: Sunday, January 29th, 2017
*/
[RequireComponent((typeof(CButton)))]
public abstract class AButtonFunctionality : MonoBehaviour
{
    protected CButton m_button;

    /*
    Description: Get the button component.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected virtual void Awake()
    {
        //Get the button component
        m_button = GetComponent<CButton>();
    }

    /*
    Description: Suscribe to the button component events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected virtual void Start()
    {
        //Suscribe to the button events
        m_button.OnHoverEvent += OnButtonHover;
        m_button.OnUnHoverEvent += OnButtonUnHover;
        m_button.OnUnClickEvent += OnButtonUnClick;
        m_button.OnClickEvent += OnButtonClick;
        m_button.OnPressEvent += OnButtonPress;
        m_button.OnExecutionEvent += OnButtonExecution;
    }

    /*
    Description: Unsuscribe from the button component events.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    protected virtual void OnDestroy()
    {
        //Unsuscribe from the button events
        m_button.OnHoverEvent -= OnButtonHover;
        m_button.OnUnHoverEvent -= OnButtonUnHover;
        m_button.OnUnClickEvent -= OnButtonUnClick;
        m_button.OnClickEvent -= OnButtonClick;
        m_button.OnPressEvent -= OnButtonPress;
        m_button.OnExecutionEvent -= OnButtonExecution;
    }

    /*
    Description: Empty virtual function so that it can be overwritten by child classes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnButtonHover()
    {
    }

    /*
    Description: Empty virtual function so that it can be overwritten by child classes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnButtonUnHover()
    {
    }

    /*
    Description: Empty virtual function so that it can be overwritten by child classes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnButtonUnClick()
    {
    }

    /*
    Description: Empty virtual function so that it can be overwritten by child classes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnButtonClick()
    {
    }

    /*
    Description: Empty virtual function so that it can be overwritten by child classes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public virtual void OnButtonPress()
    {
    }

    /*
    Description: Abstract function so that it can be overwritten by child classes.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public abstract void OnButtonExecution();
}
