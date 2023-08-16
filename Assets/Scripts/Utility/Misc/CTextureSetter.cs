using UnityEngine;
using System.Collections;

/*
Description: Class for setting the texture of an object
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 17th, 2017
*/
[RequireComponent(typeof(MeshRenderer))]
public class CTextureSetter : MonoBehaviour
{
    [Tooltip("The texture that will be set on the material")]
    public Texture m_texture;

    /*
    Description: At awake, set the desired texture for the material.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    private void Awake()
    {
        //Get the rendererer component
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        //Get the material from the renderer
        Material material = renderer.material;

        //Set teh texture for the material
        CUtilitySetters.SetMaterialTexture(ref material, m_texture);
    }
}
