using UnityEngine;
using System.Collections;

using System;


/*
Description: Helper class to switch a mesh according to the distance an object has from the main camera. 
This is done for optimization purposes.
Creator: Alvaro Chavez Mixco
Extra Notes: The class makes use of the game manager
*/
[RequireComponent(typeof(Renderer), typeof(MeshFilter))]
public class CLODMeshMaterial : ALOD
{
    /*
    Description: Simple struct to contain the mesh,  and 
    the percentage from 0 to 1  at with which it should switch
    Creator: Alvaro Chavez Mixco
    */
    [Serializable]
    public struct SLODMeshData
    {
        [SerializeField]
        public Mesh m_mesh;

        [SerializeField]
        public Material m_material;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        public float m_displayPercentage;
    }

    private MeshFilter m_meshFilter;
    private Renderer m_renderer;

    [Tooltip("States should be ordered from most detailed (lowest distance and percentage) to least detailed" +
        " (highest distance and percentage). The starting mesh of the objest should be set to its lowest detail.")]
    public SLODMeshData[] m_meshLOD;//Array of structs of meshes and when they shoul switch

    /*
    Description: Get the mesh filter, and if there are no meshes in the array disable this component. 
    Creator: Alvaro Chavez Mixco
    */
    protected override void SetLODElements()
    {
        //Get the mesh filter
        m_meshFilter = GetComponent<MeshFilter>();
        m_renderer = GetComponent<MeshRenderer>();

        //If the array is empty
        if (m_meshLOD == null)
        {
            //Disable the component
            enabled = false;
        }
    }

    /*
    Description: Check how far from the camera this object is as a percentage of the max camera distance property in this
    class. This percentage then will be used to determined if the mesh should be switched or not.
    Parameters: float aPercentageDistanceFromCamera - How far is the current object from the main game camera, as a
                                                      percent according to its max distance from camera value.
    Creator: Alvaro Chavez Mixco
    */
    protected override void CheckLODChanges(float aPercentageDistanceFromCamera)
    {

        //If the array is valid
        if (m_meshLOD != null)
        {
            //Go through all the meshes
            foreach (SLODMeshData meshData in m_meshLOD)
            {
                //If the element has a valid mesh
                if (meshData.m_mesh != null)
                {
                    //If the target is closer than the desired amount
                    if (aPercentageDistanceFromCamera < meshData.m_displayPercentage)
                    {
                        //Change the mesh
                        m_meshFilter.mesh = meshData.m_mesh;

                        //Change the material
                        m_renderer.material = meshData.m_material;

                        //Ensure the renderer is enabled
                        m_renderer.enabled = true;//Disable the renderer

                        //Exit the loop
                        return;
                    }
                    else//If no value was found, set lowest one.
                    {
                        //Set the lowest possible mesh
                        m_meshFilter.mesh = m_meshLOD[m_meshLOD.Length - 1].m_mesh;
                        m_renderer.material = m_meshLOD[m_meshLOD.Length - 1].m_material;
                    }

                }
                else//If the element is null
                {
                    m_renderer.enabled = false;//Disable the renderer
                }
            }
        }
    }
}
