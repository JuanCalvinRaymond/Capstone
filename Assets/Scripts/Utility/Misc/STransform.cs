using UnityEngine;
using System.Collections;

/*
Description: Struct for easier handling of Unity Transforms
Creator: Alvaro Chavez Mixco
Creation Date:  Friday, February 3rd, 2017
*/
public struct STransform
{
    public Vector3 m_localPosition;
    public Vector3 m_worldPosition;
    public Quaternion m_localRotation;
    public Quaternion m_rotation;
    public Vector3 m_localScale;

    /*
    Description: Constructor to save the most pertinet values of a transform
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, February 3rd, 2017
    */
    public STransform(Transform aTransform)
    {
        m_localPosition = aTransform.localPosition;
        m_worldPosition = aTransform.position;
        m_localRotation = aTransform.localRotation;
        m_rotation = aTransform.rotation;
        m_localScale = aTransform.localScale;
    }
}
