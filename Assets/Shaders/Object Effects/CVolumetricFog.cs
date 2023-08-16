using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class CVolumetricFog : MonoBehaviour
{
	private MeshRenderer m_meshRenderer = null;
	//private Texture3D m_noise = null;

    [Tooltip("The texture to sample the fog from. Leave empty for solid fog.")]
    public Texture2D m_noiseTexture = null;

    [Tooltip("The fog shader to use.")]
    public Shader m_shader = null;

    [Tooltip("What colour should the fog be tinted?")]
    public Color m_colour = Color.white;

    [Tooltip("Should the fog uniforms be updated every tick or not?")]
    public bool m_updatePerTick = false;

    [Space(20)]

    [Tooltip("How many meters thick should the fog be?")]
	[Range(1, 500)]
	public float m_thickness = 50.0f;

    [Tooltip("How dense should the fog be per meter? (When density reaches 1, the fog is solid)")]
    [Range(0, 1)]
    public float m_densityPerMeter = 0.02f;

    [Tooltip("At what percent into the depth buffer should the fog start fading out?")]
    [Range(0, 1)]
    public float m_fogFadeOutStartPercent = 0.75f;

    [Space(20)]

    [Tooltip("How many samples the raymarch should take per fragment. Higher = better quality but more laggy.")]
    [Range(1, 100)]
    public int m_numberOfSamples = 10;

    [Tooltip("The minimum amount fog density can increase per sample.")]
    [Range(0, 1)]
	public float m_minDensityPerSample = 0.1f;

    [Tooltip("The maximum amount fog density can increase per sample.")]
    [Range(0, 1)]
	public float m_maxDensityPerSample = 0.9f;

    [Space(20)]

    [Tooltip("How many meters wide each repetition of fog should be.")]
    public float m_scale = 200.0f;

    [Tooltip("The exponent to apply to the fog at the top of the fog line.")]
    [Range(0, 100)]
	public float m_noiseExponentTop = 5.0f;

    [Tooltip("The exponent to apply to the fog at the bottom of the fog line.")]
    [Range(0, 100)]
    public float m_noiseExponentBottom = 1.0f;

    [Tooltip("How fast the fog should move in meters per second.")]
    public Vector2 m_fogMoveSpeed = new Vector2(0.1f, 0.1f);
    
	
	private void Awake()
	{		
		if(m_shader == null)
		{
			gameObject.SetActive(false);
			return;
		}
        
		m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.enabled = true;
		m_meshRenderer.material = new Material(m_shader);
        m_maxDensityPerSample = Mathf.Max(m_minDensityPerSample, m_maxDensityPerSample);
	}

	private void Start()
	{
		UpdateUniforms();
	}

	private void Update()
	{
		if(m_updatePerTick)
		{
			UpdateUniforms();
		}
	}

	private void UpdateUniforms()
	{
        if (m_noiseTexture != null)
        {
            m_meshRenderer.material.SetTexture("u_noiseTexture", m_noiseTexture);
        }

		m_meshRenderer.material.SetFloat("u_thickness", m_thickness);
		m_meshRenderer.material.SetInt("u_numberOfSamples", m_numberOfSamples);
		m_meshRenderer.material.SetFloat("u_densityPerMeter", m_densityPerMeter);
		m_meshRenderer.material.SetFloat("u_minDensityPerSample", m_minDensityPerSample);
		m_meshRenderer.material.SetFloat("u_maxDensityPerSample", m_maxDensityPerSample);
		m_meshRenderer.material.SetFloat("u_uvScale", m_scale);
		m_meshRenderer.material.SetFloat("u_moveSpeedX", m_fogMoveSpeed.x);
		m_meshRenderer.material.SetFloat("u_moveSpeedZ", m_fogMoveSpeed.y);
        m_meshRenderer.material.SetFloat("u_noiseExponentTop", m_noiseExponentTop);
        m_meshRenderer.material.SetFloat("u_noiseExponentBottom", m_noiseExponentBottom);
        m_meshRenderer.material.SetFloat("u_fogFadeOutStartPercent", m_fogFadeOutStartPercent);
        m_meshRenderer.material.SetColor("u_colour", m_colour);
	}
}
