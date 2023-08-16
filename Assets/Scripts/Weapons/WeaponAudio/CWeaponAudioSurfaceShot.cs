using UnityEngine;
using System.Collections;

/*
Description: Plays a different audio and particle system according to the object that was hit. If a mesh collider was hit
              the class will sample the object specular map to know if the position hit is a windor or not.
Creator: Charlotte Brown
Creation Date: Thursday, March 16th, 2017
Extra Notes: This audio doesn't account for targets, since they call their own audio functions
*/
[RequireComponent(typeof(AWeapon))]
public class CWeaponAudioSurfaceShot : MonoBehaviour
{
    private const string M_UNIFORM_AUXILIARY = "u_auxMap";

    //Weapon script
    private AWeapon m_weapon;

    [Range(0.0f, 1.0f)]
    [Tooltip("The minimum value a pixel has to have in the specular map for it NOT to be considered glass.")]
    public float m_glassDetection = 0.1f;

    //Audio clip Variable
    [Header("Sounds")]
    public SAudioSourceSettings m_audioSourceSettings;

    [Space(20)]
    public AudioClip[] m_audioDefault;

    [Space(20)]
    public AudioClip[] m_audioWallHit;

    [Space(20)]
    public AudioClip[] m_audioGlassHit;

    [Header("Particle Systems")]
    [Tooltip("Particle system used if the object hit isn't a mesh collider, and therefore can't check its uvs")]
    public ParticleSystem m_defaultParticleSystem;
    public ParticleSystem m_wallParticleSystem;
    public ParticleSystem m_glassParticelSystem;

    /*
    Description: Initialize all variable
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void Awake()
    {
        //Get weapon script
        m_weapon = GetComponent<AWeapon>();
    }

    /*
    Description: Subscribe to weapon event
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void Start()
    {
        if (m_weapon != null)
        {
            m_weapon.OnNonTargetObjectsShot += PlaySurfaceHitAudio;
        }
    }

    /*
    Description: Unsubscribe to weapon event
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void OnDestroy()
    {
        if (m_weapon != null)
        {
            m_weapon.OnNonTargetObjectsShot -= PlaySurfaceHitAudio;
        }
    }


    /*
     * PENDING
    Description: Play wall hit audio in all wall that got hit by bullet
    Parameters: aAudioHitPosition : list of wall that got hit by bullet
    Creator: Juan Calvin Raymond
    Creation Date: 20 Dec 2016
    */
    private void PlaySurfaceHitAudio(GameObject aObjectShot, Vector3 aHitPosition, Vector3 aHitNormal,
        int aWeaponDamage, Vector2 aHitUV, Collider aCollider)
    {
        //If it a mesh collider, get the hitpoint using UVs
        if (aCollider.GetType() == typeof(MeshCollider))
        {
            //Get the object shot renderer
            Renderer objectRenderer = aObjectShot.GetComponent<Renderer>();
            if (objectRenderer == null)
            {
                return;
            }

            //Get the object shot material
            Material objectMaterial = objectRenderer.material;
            if (objectMaterial == null)
            {
                return;
            }

            //Get the axuiliary texture from the object shot
            if(objectMaterial.HasProperty(M_UNIFORM_AUXILIARY))
            {
                Texture2D auxiliaryTexture = objectMaterial.GetTexture(M_UNIFORM_AUXILIARY) as Texture2D;

                if (auxiliaryTexture == null)
                {
                    return;
                }

                //Scale the uvs
                aHitUV.x *= auxiliaryTexture.width;
                aHitUV.y *= auxiliaryTexture.height;

                //Read the desired pixel, according to the position where the object was shot
                Color hitPixelColor = auxiliaryTexture.GetPixel(Mathf.RoundToInt(aHitUV.x), Mathf.RoundToInt(aHitUV.y));


                //Use the axuiliary texture specular data to know if the object hit was glass or brick
                //If it is full specular it was glass, otherwise it was a brick
                if (hitPixelColor.r > m_glassDetection)
                {
                    //Break glass
                    CUtilityGame.PlayParticleSystemAtLocation(aHitPosition,
                        aHitNormal, m_glassParticelSystem);

                    CUtilitySound.PlayRandomSoundAtLocation(m_audioGlassHit, aHitPosition, m_audioSourceSettings);
                }
                else
                {
                    //Concrete
                    CUtilityGame.PlayParticleSystemAtLocation(aHitPosition,
                        aHitNormal, m_wallParticleSystem);
                    CUtilitySound.PlayRandomSoundAtLocation(m_audioWallHit, aHitPosition, m_audioSourceSettings);
                }
            }
            else//PENDING
            {
                //Default particle system
                CUtilityGame.PlayParticleSystemAtLocation(aHitPosition, aHitNormal, m_defaultParticleSystem);

                CUtilitySound.PlayRandomSoundAtLocation(m_audioDefault, aHitPosition, m_audioSourceSettings);
            }
        }
        else//If it is not a mesh collider
        {
            //Default particle system
            CUtilityGame.PlayParticleSystemAtLocation(aHitPosition, aHitNormal, m_defaultParticleSystem);

            CUtilitySound.PlayRandomSoundAtLocation(m_audioDefault, aHitPosition, m_audioSourceSettings);
        }
    }
}