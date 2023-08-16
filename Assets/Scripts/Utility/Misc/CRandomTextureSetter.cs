using UnityEngine;
using System.Collections;

using System;

/*
Description: Class for setting a random texture of an object
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 17th, 2017
*/
[RequireComponent(typeof(MeshRenderer))]
public class CRandomTextureSetter : MonoBehaviour
{

    [Tooltip("The textures that will be set on the material")]
    public Texture[] m_textures;

    /*
    Description: At awake, set the desired texture for the material.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void Awake()
    {
        //If there are textures to set
        if (m_textures != null)
        {
            //Get the rendererer component
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            //Get the material from the renderer
            Material material = renderer.material;

            //Get a random texture
            Texture randomTexture = m_textures[UnityEngine.Random.Range(0, m_textures.Length)];

            //Set teh texture for the material
            CUtilitySetters.SetMaterialTexture(ref material, randomTexture);
        }
    }
}