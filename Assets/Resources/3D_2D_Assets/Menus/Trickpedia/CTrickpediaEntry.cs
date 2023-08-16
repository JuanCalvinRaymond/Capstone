using UnityEngine;
using System.Collections;

/*
Description: Class used to store and set the data to display a trickpedia entry
Creator: Alvaro Chavez Mixco
Creation Date: Friday, February 3rd, 2017
*/
public class CTrickpediaEntry : MonoBehaviour
{
    private Animator m_animator;

    //Ideally thic could be changed to read from a file rather than setting it in editor
    [Header("Trick Content")]
    public string m_trickName;

    public Texture m_trickIcon;

    [Tooltip("The name of the animation state to set.")]
    public string m_trickAnimation;

    [TextArea]
    public string m_trickInstructions;
    [TextArea]
    public string m_trickFlavor;
    public int m_scoreValue;

    [Header("Display Options")]
    public TextMesh m_nameText;
    public GameObject m_planeObjectForIcon;
    public TextMesh m_instructionsText;
    public TextMesh m_flavorText;
    public TextMesh m_scoreText;

    /*
    Description: Set the corresponding data for the trickpedia
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Awake()
    {
        //Get the animator component
        m_animator = GetComponentInChildren<Animator>();

        //If there is a player icon object
        if (m_planeObjectForIcon != null)
        {
            //Get the renderer of the icon object
            MeshRenderer renderer = m_planeObjectForIcon.GetComponent<MeshRenderer>();

            //If the renderer is valid
            if (renderer != null)
            {
                //Get the material of the renderer
                Material material = renderer.material;

                //Set the material of the icon
                CUtilitySetters.SetMaterialTexture(ref material, m_trickIcon);
            }
        }

        //Set all the trickpedia text
        CUtilitySetters.SetTextMeshText(ref m_nameText, m_trickName);
        CUtilitySetters.SetTextMeshText(ref m_instructionsText, m_trickInstructions);
        CUtilitySetters.SetTextMeshText(ref m_flavorText, m_trickFlavor);
        CUtilitySetters.SetTextMeshText(ref m_scoreText, m_scoreValue.ToString());
    }

    /*
    Description: Whenever the trickpedia is enabled reset the animation
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void OnEnable()
    {
        //Reset the animation
        if (m_animator != null)
        {
            //Play animation
            m_animator.Play(m_trickAnimation, 0, 0.0f);
        }
    }
}
