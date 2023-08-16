using UnityEngine;
using System.Collections;

/*
Description: Script used to destroy a gameobject when the target compoent in its parent object is dying.
Creator: Alvaro Chavez Mixco
Creation Date: Wedsnesday, March 22nd, 2017
Extra Notes: This is intended to work with issues having objects show up once the target has shattered.
*/
public class CDestroyOnTargetDying : MonoBehaviour
{
    private ITarget m_target;

    /*
    Description: Get the target component and suscribe to its event
    Creator: Alvaro Chavez Mixco
    Creation Date: Wedsnesday, March 22nd, 2017
    Extra Notes: Done in awake since the object can very likely be created, and set to dying, at the same time.
    */
    private void Awake ()
    {
        //Get the target component in the parent
        m_target = GetComponentInParent<ITarget>();

        //If it has a target component
        if(m_target!=null)
        {
            //Suscribe to target event
            m_target.OnTargetDying += DestroyThisObject;

            //If when called the target has 0 health, assume that it is dead or dying
            if(m_target.PHealth<=0)
            {
                //Destroy this object
                DestroyThisObject(0.0f);
            }
        }
        else//If there are no target component
        {
            //Disable this component
            enabled = false;
        }
    }

    /*
    Description: Unsuscrbie from target event
    Creator: Alvaro Chavez Mixco
    Creation Date: Wedsnesday, March 22nd, 2017
    */
    private void OnDestroy()
    {
        //If it has a target component
        if (m_target != null)
        {
            m_target.OnTargetDying -= DestroyThisObject;
        }
    }

    /*
    Description: Destroy this gameobject.
    Parameters: loat aDyingTimer - Not used.In place so taht it can suscribe to target OnTargetDying event
    Creator: Alvaro Chavez Mixco
    Creation Date: Wedsnesday, March 22nd, 2017
    */
    private void DestroyThisObject(float aDyingTimer)
    {
        Destroy(gameObject);
    }
}
