using UnityEngine;
using System.Collections;

using System;

/*
Description: Struct used to easily set in editor the parameters of an ease
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
[Serializable]
public struct SEaseSettings
{
    [SerializeField]
    public EEaseType m_easeType;

    [SerializeField]
    public EEaseMode m_easeMode;

    [SerializeField]
    public float m_duration;

    [SerializeField]
    public float m_extraParameter;
}

/*
Description: Struct used to easily set in editor the parameters of an easing function
             (No CEase wrapper object being used)
Creator: Alvaro Chavez Mixco
Creation Date : Thursday, February 23rd, 2017
*/
[Serializable]
public struct SEaseFunctionSettings
{
    [SerializeField]
    public EEaseType m_easeType;

    [SerializeField]
    public EEaseMode m_easeMode;

    [SerializeField]
    public float m_extraParameter;
}