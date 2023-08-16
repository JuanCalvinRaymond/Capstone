using UnityEngine;
using System.Collections;

using System;

/*
Description: Helper class to switch a mesh and texture according to the distance an object has from the main camera. 
This is done for optimization purposes.
Creator: Alvaro Chavez Mixco
Extra Notes: The class makes use of the game manager.
*/
[RequireComponent(typeof(Renderer), typeof(MeshFilter))]
public class CLODMeshTexture : ALOD
{
    /*
    Description: Simple struct to contain the mesh and texture,  and 
    the percentage from 0 to 1  at with which it should switch
    Creator: Alvaro Chavez Mixco
    */
    [Serializable]
    public struct SLODMeshTextureData
    {
        [SerializeField]
        public Mesh m_mesh;

        [SerializeField]
        public Texture m_texture;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        public float m_displayPercentage;
    }

    private MeshFilter m_meshFilter;
    private Renderer m_renderer;

    [Tooltip("States should be ordered from most detailed (lowest distance and percentage) to least detailed" +
    " (highest distance and percentage). The starting mesh of the objest should be set to its lowest detail.")]
    public SLODMeshTextureData[] m_meshTextureLOD;
    /*
    Description: Get the mesh filter and the mesh renderer. If there are no elements in the array of structs, disable this component.
    Creator: Alvaro Chavez Mixco
    */
    protected override void SetLODElements()
    {
        //Get the required components
        m_meshFilter = GetComponent<MeshFilter>();
        m_renderer = GetComponent<Renderer>();

        //If the array is empty
        if (m_meshTextureLOD == null)
        {
            //Disable the component
            enabled = false;
        }
    }

    /*
    Description: Check how far from the camera this object is as a percentage of the max camera distance property in this
    class. This percentage then will be used to determined if the texture AND mesh should be switched or not.
    Parameters: float aPercentageDistanceFromCamera - How far is the current object from the main game camera, as a
                                                      percent according to its max distance from camera value.
    Creator: Alvaro Chavez Mixco
    Extra Notes: The mesh and texture are switched at the same unique point.
    */
    protected override void CheckLODChanges(float aPercentageDistanceFromCamera)
    {
        //If the array is valid
        if (m_meshTextureLOD != null)
        {
            //Go through all the meshes
            foreach (SLODMeshTextureData meshData in m_meshTextureLOD)
            {
                //If the element has a valid mesh
                if (meshData.m_mesh != null && meshData.m_texture)
                {
                    //If the target is closer than the desired amount
                    if (aPercentageDistanceFromCamera < meshData.m_displayPercentage)
                    {
                        //Enable the renderers
                        m_renderer.enabled = true;

                        //Change the texture and the mesh
                        m_meshFilter.mesh = meshData.m_mesh;
                        m_renderer.material.mainTexture = meshData.m_texture;

                        return;
                    }
                    else//If no value was found, set lowest one.
                    {
                        //Set the lowest possible mesh and texture
                        m_meshFilter.mesh = m_meshTextureLOD[m_meshTextureLOD.Length - 1].m_mesh;
                        m_renderer.material.mainTexture = m_meshTextureLOD[m_meshTextureLOD.Length - 1].m_texture;
                    }
                }
                else if (meshData.m_mesh == null)//If there is no mesh to set
                {
                    //Disable the renderer
                    m_renderer.enabled = false;
                }
            }
        }
    }
}