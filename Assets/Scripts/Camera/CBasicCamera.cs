using UnityEngine;
using System.Collections;

/*
Description:Class to be added to a camera, this is to lerp  in lateUpdate the camera position and rotation to match the player head rotation
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
*/
public class CBasicCamera : MonoBehaviour
{
    public GameObject m_playerHeadGameObject = null;//It must be a child of the main gameobject

    public float m_moveEaseSpeed = 30.0f;
    public float m_rotationEaseSpeed = 20.0f;

    /*
    Description:lerp the camera position and rotation to match the player head rotation
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    */
    private void LateUpdate()
    {
        LateUpdateMovement();//Lerp positon
        LateUpdateRotation();//Lerp rotation
    }

    /*
     Description:Lerp the camera positon to the player positon
     Creator: Alvaro Chavez Mixco
     Creation Date: Wednesday, October 19th, 2016
     */
    private void LateUpdateMovement()
    {
        if (m_playerHeadGameObject != null)//If there is a player to follow
        {
            transform.position = Vector3.Lerp(transform.position, m_playerHeadGameObject.transform.position, Time.unscaledDeltaTime * m_moveEaseSpeed);//Ease this object positon to the player position
        }
    }

    /*
     Description:lerpe the camera rotation to match the player head rotation
     Creator: Alvaro Chavez Mixco
     Creation Date: Wednesday, October 19th, 2016
     */
    private void LateUpdateRotation()
    {
        if (m_playerHeadGameObject != null)//If there is a player to follow
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, m_playerHeadGameObject.transform.rotation, Time.unscaledDeltaTime * m_rotationEaseSpeed);
        }
    }

}
