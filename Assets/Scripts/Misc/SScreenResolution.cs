using UnityEngine;
using System.Collections;

/*
Description: Struct to easily handle the screen resolution setting.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public struct SScreenResolution
{
    public int m_width;
    public int m_height;
    public bool m_fullscreen;

    /*
    Description: Constructor to set all the values for the screen resolution
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public SScreenResolution(int aWidth, int aHeight, bool aFullScreen)
    {
        m_width = aWidth;
        m_height = aHeight;
        m_fullscreen = aFullScreen;
    }
}
