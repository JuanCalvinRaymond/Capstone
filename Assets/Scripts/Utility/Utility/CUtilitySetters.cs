using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

/*
Description: Utility class used to easily set multiple Unity specific parameters.
Creator: Alvaro Chavez Mixco
*/
class CUtilitySetters
{
    private const string M_MATERIAL_TEXTURE_NAME = "_MainTex";

    /*
    Description: A helper function to set the text of a 3D Text object, or text mesh.
    Parameters: ref TextMesh aTextMesh - The 3D Text object that will be modified
                string aText - The new text that will be displayed,
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 13, 2017
    */
    public static void SetTextMeshText(ref TextMesh aTextMesh, string aText)
    {
        //If the 3D text object is valid
        if (aTextMesh != null)
        {
            //Set the text
            aTextMesh.text = aText;
        }
    }

    /*
    Description: A helper function to set the text of a 2D Text object
    Parameters: ref Text aTextObject - The 2D Text object that will be modified
                string aText - The new text that will be displayed,
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 13, 2017
    */
    public static void SetText2DText(ref Text aTextObject, string aText)
    {
        //If the 2D text object is valid
        if (aTextObject != null)
        {
            //Set the text
            aTextObject.text = aText;
        }
    }

    /*
    Description: A helper function to set the texture of a raw image
    Parameters: ref RawImage aRawImage - The RawImage component that will be modified
                Texture aTexture - The texture that will be assigned th the raw image
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetRawImageTexture(ref RawImage aRawImage, Texture aTexture)
    {
        //If the raw image object is valid and the texture is also valid
        if (aRawImage != null && aTexture != null)
        {
            //Set the texture
            aRawImage.texture = aTexture;
        }
    }

    /*
    Description: A helper function to set a bool parameter in an animator
    Parameters: ref Animator aAnimator - The animator component that will be modified
                string aParameterName - The parameter name in the animator that will be set
                bool aStatus - The value that will be assigned to the parameter
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetAnimatorBoolParameter(ref Animator aAnimator, string aParameterName, bool aStatus)
    {
        //If the animator is valid
        if (aAnimator != null)
        {
            //Set the bool in the animator
            aAnimator.SetBool(aParameterName, aStatus);
        }
    }

    /*
    Description: A helper function to set a float parameter in an animator
    Parameters: ref Animator aAnimator - The animator component that will be modified
                string aParameterName - The parameter name in the animator that will be set
                float aStatus - The value that will be assigned to the parameter
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetAnimatorFloatParameter(ref Animator aAnimator, string aParameterName, float aValue)
    {
        //If the animator is valid
        if (aAnimator != null)
        {
            //Set the float in the animator
            aAnimator.SetFloat(aParameterName, aValue);
        }
    }

    /*
    Description: A helper function to set a trigger parameter in an animator
    Parameters: ref Animator aAnimator - The animator component that will be modified
                string aParameterName - The parameter name in the animator that will be set
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetAnimatorTriggerParameter(ref Animator aAnimator, string aParameterName)
    {
        //If the animator is valid
        if (aAnimator != null)
        {
            //Set the bool in the animator
            aAnimator.SetTrigger(aParameterName);
        }
    }

    /*
    Description: A helper function to set the texture of a game object
    Parameters: ref GameObject aGameObject - The renderer which texture will be modified
                Texture aTexture - The new texture that will be applied to the material.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    Extra Notes: This only works if the shader in the material is using the shader named "_MainTex",
    as the uniform name for its texture.
    */
    public static void SetMaterialTexture(ref GameObject aGameObject, Texture aTexture)
    {
        //If the game object is valid
        if (aGameObject != null)
        {
            //Make a reference to the renderer
            MeshRenderer renderer = aGameObject.GetComponent<MeshRenderer>();

            //Set the renderer texture
            SetMaterialTexture(ref renderer, aTexture);
        }
    }

    /*
    Description: A helper function to set the texture of a renderer
    Parameters: ref MeshRenderer aRenderer - The renderer which material will be modified
                Texture aTexture - The new texture that will be applied to the material.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    Extra Notes: This only works if the shader in the material is using the shader named "_MainTex",
    as the uniform name for its texture.
    */
    public static void SetMaterialTexture(ref MeshRenderer aRenderer, Texture aTexture)
    {
        //If the renderer is valid
        if (aRenderer != null)
        {
            //Make a reference to the material
            Material rendererMaterial = aRenderer.material;

            //Set the material texture
            SetMaterialTexture(ref rendererMaterial, aTexture);
        }
    }

    /*
    Description: A helper function to set the texture of a material
    Parameters: ref Material aMaterial - The material that will be modified.
                Texture aTexture - The new texture that will be applied to the material.
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    Extra Notes: This only works if the shader in the material is using the shader named "_MainTex",
    as the uniform name for its texture.
    */
    public static void SetMaterialTexture(ref Material aMaterial, Texture aTexture)
    {
        //If the material and texture are valid
        if (aMaterial != null && aTexture != null)
        {
            //Set the texture in the material
            aMaterial.SetTexture(M_MATERIAL_TEXTURE_NAME, aTexture);
        }
    }

    /*
    Description: A helper function to set the texture of a material
    Parameters: GameObject aRootGameObject - The root game object that will be modified.
                Texture aTexture - The texture that will be applied to the object
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetTextureInObjectAndChildren(GameObject aRootGameObject, Texture aTexture)
    {
        //If the root game object is valid
        if (aTexture != null && aRootGameObject != null)
        {
            MeshRenderer[] tempRenderers = null;
            Material tempMaterial = null;

            //Get the mesh renderers in the children and the  parent
            tempRenderers = aRootGameObject.GetComponentsInChildren<MeshRenderer>();

            //If at least one mesh renderer was found
            if (tempRenderers.Length > 0)
            {
                //Go through all the mesh renderers
                for (int i = 0; i < tempRenderers.Length; i++)
                {
                    //If the  renderer is valid
                    if (tempRenderers[i] != null)
                    {
                        //Get the material from the renderer
                        tempMaterial = tempRenderers[i].material;

                        //Set the texture for iT
                        SetMaterialTexture(ref tempMaterial, aTexture);
                    }
                }
            }
        }
    }

    /*
    Description: A helper function to set the texture of a material for multiple objects
    Parameters: GameObject[] aGameObjects - The root game object that will be modified.
                Texture aTexture - The texture that will be applied to the object
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    Extra Notes: This function doesn't use the other functions of SetTexture (renderer, gameobject),
                 in this class, CUtilitySetters, for optimization purporses.
    */
    public static void SetTextureInObjects(GameObject[] aGameObjects, Texture aTexture)
    {
        //If the gameobject
        if (aGameObjects != null)
        {
            MeshRenderer tempRenderer = null;
            Material tempMaterial = null;

            //Go through every game object
            for (int i = 0; i < aGameObjects.Length; i++)
            {
                //If the object is valid
                if (aGameObjects[i] != null)
                {
                    //Get its renderer
                    tempRenderer = aGameObjects[i].GetComponent<MeshRenderer>();

                    //If the renderer is valid
                    if (tempRenderer != null)
                    {
                        //Save the material reference
                        tempMaterial = tempRenderer.material;

                        //Set the material texture
                        SetMaterialTexture(ref tempMaterial, aTexture);
                    }
                }
            }
        }
    }

    /*
    Description: A helper function to set the mesh of a mesh filter.
    Parameters: ref MeshFilter aMeshFilter - The mesh filter that mesh will be changed
                Mesh aMesh - The new mesh the mesh filter will have
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetMesh(ref MeshFilter aMeshFilter, Mesh aMesh)
    {
        //If the mesh filter and the mesh are valid
        if (aMeshFilter != null && aMesh != null)
        {
            aMeshFilter.mesh = aMesh;
        }
    }

    /*
    Description: A helper function to get the weapon type from a weapon game object
    Parameters: GameObject aWeaponGameObject - The gameobject that stores the weapon component
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static EWeaponTypes GetWeaponType(GameObject aWeaponGameObject)
    {
        //If the weapon game object is vlaid
        if (aWeaponGameObject != null)
        {
            //Get the weapon component
            AWeapon weapon = aWeaponGameObject.GetComponent<AWeapon>();

            //If there is a weapon component
            if (weapon != null)
            {
                //Get the weapon type
                return weapon.PWeaponType;
            }
        }

        return EWeaponTypes.None;
    }

    /*
    Description: A helper function to check for a null object and set it active
    Parameters: GameObject aParentObject - A object to set active
                bool aActiveStatus - The active status that will be set
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 27th, 2017
    */
    public static void SetActiveStatus(GameObject aGameObject, bool aActiveStatus)
    {
        //If the game object is valid
        if (aGameObject != null)
        {
            //Activate status
            aGameObject.SetActive(aActiveStatus);
        }
    }

    /*
    Description: A helper function to set an object at the desired index to active, while
                 setting all the other objects as inactive.
    Parameters: GameObject[] aGameObject - The array of objects to deactivate/activate
                int aIndexToActivate - The index of the object that will be activated.
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 27th, 2017
    */
    public static void SetActiveAndDeactivateOther(GameObject[] aGameObjects, int aIndexToActivate)
    {
        //If the game objects are valid
        if (aGameObjects != null)
        {
            //If the index is valid
            if (aIndexToActivate >= 0 && aIndexToActivate < aGameObjects.Length)
            {
                //Go through all the objects
                for (int i = 0; i < aGameObjects.Length; i++)
                {
                    //If it is the desire index
                    if (i == aIndexToActivate)
                    {
                        //Activate it
                        SetActiveStatus(aGameObjects[i], true);
                    }
                    else//If it is not the desired index
                    {
                        //Ensure that you deactivate the object you deactivate if it was already in the list
                        if (aGameObjects[i] != aGameObjects[aIndexToActivate])
                        {
                            //Deactivate it
                            SetActiveStatus(aGameObjects[i], false);
                        }
                    }
                }
            }
        }
    }

    /*
    Description: A helper function to set the active status of all the children of an object.
    Parameters: GameObject aParentObject - The parent gameobject from where the children to 
                                            activate/deactivate will be obtained
                bool aActiveStatus - The active status that will be set in all the children
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, January 25, 2017
    */
    public static void SetActiveStatusAllChildObjects(GameObject aParentObject, bool aActiveStatus)
    {
        //If the parent object is valid
        if (aParentObject != null)
        {
            //If the object has children
            if (aParentObject.transform.childCount > 0)
            {
                //Go through all the children
                for (int i = 0; i < aParentObject.transform.childCount; i++)
                {
                    //If the child object is valid
                    if (aParentObject.transform.GetChild(i) != null)
                    {
                        //Set the active status for the child
                        aParentObject.transform.GetChild(i).gameObject.SetActive(aActiveStatus);
                    }
                }
            }
        }
    }

    /*
    Description: A helper function to set the active status for the children, and all of their children, of a game object.
    Parameters: GameObject aParentObject - The parent gameobject from where the children to 
                                            activate/deactivate will be obtained
                bool aActiveStatus - The active status that will be set in all the children
    Creator: Alvaro Chavez Mixco
    Creation Date: Tuesday, March 21, 2017
    */
    public static void SetActiveRecursively(GameObject aParentObject, bool aActiveStatus)
    {
        //If the parent object is valid
        if (aParentObject != null)
        {
            //If the object has children
            if (aParentObject.transform.childCount > 0)
            {
                //Go through all the children
                for (int i = 0; i < aParentObject.transform.childCount; i++)
                {
                    //Set the active status for the child, and all the child children
                    SetActiveStatusAllChildObjects(aParentObject.transform.GetChild(i).gameObject, aActiveStatus);
                }
            }
        }
    }

    /*
    Description: Static function to copy the local values from one transform to another.
    Parameters: Transform aTransformToModify - The transform that will be modified.
                STransform aValuesToSet - The STransform which values will be copied into the transform.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Friday, February 3rd, 2017
    */
    public static void SetTransformLocalValues(Transform aTransformToModify, STransform aValuesToSet)
    {
        //Copy the local postion, rotation, and scale of the STransform to the transform
        aTransformToModify.localPosition = aValuesToSet.m_localPosition;
        aTransformToModify.localRotation = aValuesToSet.m_localRotation;
        aTransformToModify.localScale = aValuesToSet.m_localScale;
    }

    /*
    Description: A helper function to copy an array of transforms into a array of   
                STransform.
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 09th, 2017
    Extra Notes: This is primary used when you need to store the transform values of an object
                 at a certain point in a struct and not in a class. So that the values won't
                 get modified if the object is moved, rotated, etc.
    */
    public static STransform[] GetTransformsValues(Transform[] aTransformsToCopy)
    {
        //If the array is valid
        if (aTransformsToCopy != null)
        {
            //Initialize the array of saved transforms
            STransform[] savedTransforms = new STransform[aTransformsToCopy.Length];

            //Go through every transform in the array
            for (int i = 0; i < aTransformsToCopy.Length; i++)
            {
                //If transform is valid
                if (aTransformsToCopy != null)
                {
                    //Copy its values into array
                    savedTransforms[i] = new STransform(aTransformsToCopy[i]);
                }
            }

            return savedTransforms;
        }

        return null;
    }

    /*
    Description: A helper function to set copy most of the values of 1 transform to another
    Parameters: Transform aTransformToSet - The transform that will be modified
                Transform aTransformToCopy - The transform which values will be copied
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, January 13, 2017
    */
    public static void SetTransform(Transform aTransformToSet, Transform aTransformToCopy)
    {
        if (aTransformToSet != null && aTransformToCopy != null)
        {
            //Copy the parent
            aTransformToSet.SetParent(aTransformToCopy.parent);

            //Copy the word position and rotation
            aTransformToSet.position = aTransformToCopy.position;
            aTransformToSet.rotation = aTransformToCopy.rotation;

            //Copy the local positon, rotation and scale          
            aTransformToSet.localPosition = aTransformToCopy.localPosition;
            aTransformToSet.localRotation = aTransformToCopy.localRotation;
            aTransformToSet.localScale = aTransformToCopy.localScale;

            //Copy the tag
            aTransformToSet.tag = aTransformToCopy.tag;
        }
    }

    /*
    Description: A helper function to enable/disable a collider
    Parameters: ref Collider aCollider - The collider that will be enabled/disabled.
                bool aEnabledStatus - The enabled status that will be set in the colliders.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public static void SetEnabledCollider(ref Collider aCollider, bool aEnabledStatus)
    {
        //If the colider is valid
        if (aCollider != null)
        {
            //Set the enabled status
            aCollider.enabled = aEnabledStatus;
        }
    }

    /*
    Description: A helper function to enable/disable multiple colliders
    Parameters: Collider[] aColliders - The colliders that will be enabled/disabled.
                bool aEnabledStatus - The enabled status that will be set in the colliders.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public static void SetEnabledColliders(Collider[] aColliders, bool aEnabledStatus)
    {
        //If the colider array is valid
        if (aColliders != null)
        {
            //Go through every collider
            for (int i = 0; i < aColliders.Length; i++)
            {
                //Set its enabled status
                SetEnabledCollider(ref aColliders[i], aEnabledStatus);
            }
        }
    }

    /*
    Description: A helper function to enable/disable multiple renderers
    Parameters: Renderer[] aColliders - The renderers that will be enabled/disabled.
                bool aEnabledStatus - The enabled status that will be set in the colliders.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public static void SetEnabledRenderers(Renderer[] aRenderers, bool aEnabledStatus)
    {
        //If the render array is valid
        if (aRenderers != null)
        {
            //Go through every renderer
            for (int i = 0; i < aRenderers.Length; i++)
            {
                //If the renderer is valid
                if (aRenderers[i] != null)
                {
                    //Set the enabled status
                    aRenderers[i].enabled = aEnabledStatus;
                }
            }
        }
    }

    /*
    Description: A helper function to set the velocities of a rigid body. This function 
                 check that the angular velocity is a number (NaN has to be false)
    Parameters: ref Rigidbody aRigidBody - The rigid body that will be set
                Vector3 aLinearVelocity - The linear velocity that will be set
                Vector3 aAngularVelocity - The angular velocity that will be set
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 24th, 2017
    */
    public static void SetRigidBodyVelocities(ref Rigidbody aRigidBody, Vector3 aLinearVelocity, Vector3 aAngularVelocity)
    {
        aRigidBody.velocity = aLinearVelocity;

        //If the angular velocity is valid
        if (CUtilityMath.IsNaN(aAngularVelocity) == false)
        {
            //Set the rigid velocity of the object
            aRigidBody.angularVelocity = aAngularVelocity;
        }
    }

    /*
    Description: A helper function to reset the velocities of a rigid body back to 0.
    Parameters: ref Rigidbody aRigidBody - The rigid body that will be reset
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 17th, 2017
    */
    public static void ResetRigidBodyVelocities(ref Rigidbody aRigidBody)
    {
        //If the rigid body is valid
        if (aRigidBody != null)
        {
            //Reset linear velocity
            aRigidBody.velocity = Vector3.zero;

            //Reset angular velocity
            aRigidBody.angularVelocity = Vector3.zero;
        }
    }
}