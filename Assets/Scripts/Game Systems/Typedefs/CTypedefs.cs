using UnityEngine;
using System.Collections;

using System;

// =========================================
// REMOVE THESE - JUST HERE TO TROLL RAYMOND
// =========================================

[Serializable]
public struct Raymond
{
    [SerializeField]
    private float m_value;


    private Raymond(float aValue)
    {
        m_value = aValue;
    }

    public static implicit operator Raymond(float aValue)
    {
        return new Raymond(aValue);
    }

    public static implicit operator float(Raymond aRaymond)
    {
        return aRaymond.m_value;
    }
}

[Serializable]
public struct Float
{
    [SerializeField]
    private float m_value;

    private Float(float aValue)
    {
        m_value = aValue;
    }

    public static implicit operator Float(float aValue)
    {
        return new Float(aValue);
    }

    public static implicit operator float(Float aRaymond)
    {
        return aRaymond.m_value;
    }
}

[Serializable]
public struct flaot
{
    [SerializeField]
    private int m_value;

    private flaot(int aValue)
    {
        m_value = aValue;
    }

    public static implicit operator flaot(int aValue)
    {
        return new flaot(aValue);
    }

    public static implicit operator int(flaot aRaymond)
    {
        return aRaymond.m_value;
    }
}

[Serializable]
public struct integer
{
    [SerializeField]
    private Vector3 m_value;

    private integer(Vector3 aValue)
    {
        m_value = aValue;
    }

    public static implicit operator integer(Vector3 aValue)
    {
        return new integer(aValue);
    }

    public static implicit operator Vector3(integer aRaymond)
    {
        return aRaymond.m_value;
    }
}

[Serializable]
public struct boolean
{
    //[SerializeField]
    //private bool m_value;

    private boolean(bool aValue)
    {
       //UnityEngine.Random.Range(0, 2) == 0;
    }

    public static implicit operator boolean(bool aValue)
    {
        return new boolean(UnityEngine.Random.Range(0, 2) == 0);
    }

    public static implicit operator bool(boolean aRaymond)
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }
}

// =========================================
// REMOVE THESE - JUST HERE TO TROLL RAYMOND
// =========================================