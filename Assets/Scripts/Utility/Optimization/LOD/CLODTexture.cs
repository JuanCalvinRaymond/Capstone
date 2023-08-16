using UnityEngine;
using System.Collections;

using System;

/*
Description: Helper class to switch a texture according to the distance an object has from the main camera. 
This is done for optimization purposes.
Creator: Alvaro Chavez Mixco
Extra Notes: The class makes use of the game manager
*/
[RequireComponent(typeof(Renderer))]
public class CLODTexture : ALOD
{
    /*
    Description: //Simple struct to contain the texture,  and 
    the percentage from 0 to 1  at with which it should switch
    Creator: Alvaro Chavez Mixco
    */
    [Serializable]
    public struct SLODTextureData
    {
        [SerializeField]
        public Texture m_texture;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        public float m_displayPercentage;
    }

    private Renderer m_renderer;

    [Tooltip("States should be ordered from most detailed (lowest distance and percentage) to least detailed" +
    " (highest distance and percentage). The starting mesh of the objest should be set to its lowest detail.")]
    public SLODTextureData[] m_textureLOD;

    /*
    Description: Get the mesh Renderer, and if there are no textures in the array disable this component.
    Creator: Alvaro Chavez Mixco
    */
    protected override void SetLODElements()
    {
        m_renderer = GetComponent<Renderer>();

        //If the array is empty
        if (m_textureLOD == null)
        {
            //Disable the component
            enabled = false;
        }
    }

    /*
    Description: Check how far from the camera this object is as a percentage of the max camera distance property in this
    class. This percentage then will be used to determined if the texture should be switched or not.
    Parameters: float aPercentageDistanceFromCamera - How far is the current object from the main game camera, as a
                                                      percent according to its max distance from camera value.
    Creator: Alvaro Chavez Mixco
    */
    protected override void CheckLODChanges(float aPercentageDistanceFromCamera)
    {
        //If the array is valid
        if (m_textureLOD != null)
        {
            //Go through all the textures
            foreach (SLODTextureData textureData in m_textureLOD)
            {
                //If the element has a valid textures
                if (textureData.m_texture != null)
                {
                    //If the target is closer than the desired amount
                    if (textureData.m_displayPercentage > aPercentageDistanceFromCamera)
                    {
                        //Change the  texture in the maaterial
                        m_renderer.material.mainTexture = textureData.m_texture;

                        //Exit the loop
                        return;
                    }
                    else//If no value was found, set lowest one.
                    {
                        //Set the lowest possible mesh
                        m_renderer.material.mainTexture = m_textureLOD[m_textureLOD.Length - 1].m_texture;
                    }
                }
            }
        }
    }
}