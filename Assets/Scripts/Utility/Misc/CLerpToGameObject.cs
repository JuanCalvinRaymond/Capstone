using UnityEngine;
using System.Collections;

/*
Description: Simple function to lerp a gameobject to another gameobject position and/or rotation with customizable speed and offsets
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 25, 2016
*/
public class CLerpToGameObject : MonoBehaviour
{
    public GameObject m_objectToLerpTo;//It must be a child of the main gameobject

    public Vector3 m_positionOffset = Vector3.zero;
    public Vector3 m_rotationOffset = Vector3.zero;
    public float m_positionEaseSpeed = 30.0f;
    public float m_rotationEaseSpeed = 20.0f;

    public bool m_lerpPosition = true;
    public bool m_lerpRotation = true;

    /*
    Description: If desired,lerps the position and/or rotation
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 25, 2016
    */
    private void Update()
    {
        if (m_lerpPosition == true) //If we want to lerp the position
        {
            UpdatePosition();//Lerp positon
        }

        if (m_lerpRotation == true)//If we want to lerp the rotation
        {
            UpdateRotation();//Lerp rotation
        }
    }

    /*
    Description: Lerps the position of this gameobject to the position , plus offset , of another gameobject with the desired speed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 25, 2016
    */
    private void UpdatePosition()
    {
        if (m_objectToLerpTo != null)//If there is a player to follow
        {
            transform.position = Vector3.Lerp(transform.position, 
                m_objectToLerpTo.transform.position + m_positionOffset,
                CGameManager.PInstanceGameManager.GetScaledDeltaTime() * m_positionEaseSpeed);//Ease this object positon to the player position
        }
    }

    /*
    Description: Lerps the rotation of this gameobject to the rotation , plus offset , of another gameobject with the desired speed.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 25, 2016
    */
    private void UpdateRotation()
    {
        if (m_objectToLerpTo != null)//If there is a player to follow
        {
            Vector3 targetObjectRotation = m_objectToLerpTo.transform.rotation.eulerAngles;///Get the target object rotation in euler angles
            Quaternion targetRotation = Quaternion.Euler(m_rotationOffset.x + targetObjectRotation.x, m_rotationOffset.y + targetObjectRotation.y, m_rotationOffset.z + targetObjectRotation.z);//Add to the target rotation the offset
            //we want for it; and convert it to Quaternions

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                CGameManager.PInstanceGameManager.GetScaledDeltaTime() * m_rotationEaseSpeed);//Lerp the rotation to the desired rotaiton
        }
    }
}