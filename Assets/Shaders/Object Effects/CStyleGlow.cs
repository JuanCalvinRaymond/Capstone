using UnityEngine;
using System.Collections;

/*
 * Tracks any number of materials and updates their emission values whenever the
 * Style system changes. This allows for weapons and other objects to glow brightly
 * while the player is performing well.
 * Creator: Charlotte C. Brown
 */
[ExecuteInEditMode]
public class CStyleGlow : MonoBehaviour
{
    // Set this to whatever the player's current Style percentage is.
    public float PStyleValue
    {
        get { return m_styleValue; }
        set { m_styleValue = value; }
    }

    // The brightest this object should glow (>1 = HDR).
    public float m_maxGlow = 2.0f;

    // Exponent used for scaling glow. 1 = linear gain, higher values = exponential gain.
    public float m_glowExponent = 3.0f;

    // A list of all materials that this effect should update.
    public Material[] m_materials;

    // A list of all particle and light systems that this effect should update.
    public ParticleSystem[] m_particleSystems;

    // A list of all lights that this effect should update.
    public Light[] m_lights;

    // The colour to make things glow.
    public Color m_glowColour = Color.red;

    // Above which style value should fire start?
    public float m_fireThreshold = 0.9f;

    // The tag to search for lights.
    public string m_styleLightTag = "StyleLight";

    // Whatever the player's Style value was last update.
    private float m_lastStyleValue;

    // The current total glow strength.
    private float m_glowMultiplier;

    // The current style value.
    private float m_styleValue;

    // Used to detect when the fire threshold is first crossed in order to only trigger events once.
    private bool m_fireHasStarted = false;

    /*
     * Set last style value to -1 in order to force a reset when the level restarts.
     * Charlotte C. Brown
     */
    private void Awake()
    {
        m_lastStyleValue = -1.0f;

        // Check that there are properties to set
        if(m_materials==null || m_lights ==null || m_particleSystems==null)
        {
            // If there aren't, disable the component
            enabled = false;
        }
    }

    private void Start()
    {
        GameObject[] lightObjects = GameObject.FindGameObjectsWithTag(m_styleLightTag);
        m_lights = new Light[lightObjects.Length];

        for (int i = 0; i < m_lights.Length; i++)
        {
            Light light = lightObjects[i].GetComponent<Light>();

            if (light == null)
            {
                Debug.LogError("Invalid light object {0}", lightObjects[i]);
                m_lights = null;
                break;
            }

            m_lights[i] = light;
        }
    }

    /*
     * Remove all glow when the application exits.
     * Charlotte C. Brown
     */
    private void OnApplicationQuit()
    {
        if (m_materials != null)
        {
            foreach (Material material in m_materials)
            {
                material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    /*
	 * Update the emission values of each linked material. Only updates if the style value has
	 * changed in order to save on performance.
	 * Creator: Charlotte C. Brown
	 */
    private void Update()
    {
        // Only set uniforms if the style value has changed.
        if (m_styleValue != m_lastStyleValue)
        {
            m_glowMultiplier = Mathf.Pow(m_styleValue * m_maxGlow, m_glowExponent);

            // Enable lights
            if (m_lights != null)
            {
                foreach (Light light in m_lights)
                {
                    light.intensity = m_styleValue * m_maxGlow;
                }
            }

            if (m_materials != null)
            {
                // Set the glow amount of each material.
                foreach (Material material in m_materials)
                {
                    material.SetColor("_EmissionColor", m_glowColour * m_glowMultiplier);
                }
            }

            // Check to make sure we only enable particle and lighting effects once.
            if (m_styleValue >= m_fireThreshold && !m_fireHasStarted)
            {
                m_fireHasStarted = true;

                // Start emitting particles.
                foreach (ParticleSystem system in m_particleSystems)
                {
                    system.Play();
                }
            }
            // Check to make sure we only disable particle and lighting effects once.
            else if (m_styleValue < m_fireThreshold && m_fireHasStarted)
            {
                m_fireHasStarted = false;

                // Stop emitting particles.
                foreach (ParticleSystem system in m_particleSystems)
                {
                    system.Stop();
                }
            }

            m_lastStyleValue = m_styleValue;
        }
    }
}
