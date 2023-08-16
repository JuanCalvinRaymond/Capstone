using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Manages a bunch of points in space and a flocking simulation compute shader in order
 * to produce an effect that looks like birds (or other flying/aquatic animals) flying
 * togther in flocks, yet in an unpredictable manner.
 * 
 * See Birds.shader and Birds.compute for more info!
 * 
 * To use this:
 *  1) Add this script to an empty game object
 *  2) Give the component a reference to Birds.compute
 *  3) Create a material that uses Birds.shader
 *  4) Give the component a reference to the material
 *  5) Configure material and component properties!
 * 
 * Creator: Charlotte C. Brown
*/
public class CBirds : MonoBehaviour
{
	private struct SBirdData
	{
        // Where the bird currently is.
		public Vector3 m_position;

        // How fast the bird is moving.
		public Vector3 m_velocity;

        // How fast the bird was moving last frame (used for banking).
        public Vector3 m_previousVelocity;
	}
	
    // Names of shader uniforms that get called every frame - used for efficiency.
	private const string M_UNIFORM_BIRD_DATA = "u_birdData";
    private const string M_UNIFORM_BIRD_COUNT = "u_birdCount";
    private const string M_UNIFORM_DELTA_TIME = "u_deltaTime";
    private const string M_UNIFORM_BOUNDS_MIN = "u_boundingBoxMin";
    private const string M_UNIFORM_BOUNDS_MAX = "u_boundingBoxMax";
    private const string M_UNIFORM_BOUNDS_FORCE = "u_borderForce";
    private const string M_UNIFORM_SPEED = "u_speed";
    private const string M_UNIFORM_COHESION_RADIUS = "u_cohesionRadiusSquared";
    private const string M_UNIFORM_COHESION_FORCE = "u_cohesionForce";
    private const string M_UNIFORM_MAX_VELOCITY_CHANGE_PCT = "u_maxVelocityChangePercent";
    private const string M_UNIFORM_MIN_SEPARATION = "u_minimumSeparationSquared";
    private const string M_UNIFORM_ALIGNMENT_FORCE = "u_alignmentForce";
    private const string M_UNIFORM_AVOIDANCE_FORCE = "u_avoidanceForce";
    private const string M_UNIFORM_VERTICAL_DEVIATION_AMP = "u_verticalDeviationAmplitude";
    private const string M_UNIFORM_AVOIDANCE_RADIUS_SQUARED = "u_avoidanceRadiusSquared";
    private const string M_UNIFORM_WEAPON_1_POSITION = "u_weapon1Position";
    private const string M_UNIFORM_WEAPON_1_DIRECTION = "u_weapon1Direction";
    private const string M_UNIFORM_WEAPON_2_POSITION = "u_weapon2Position";
    private const string M_UNIFORM_WEAPON_2_DIRECTION = "u_weapon2Direction";

    // Storage variables for holding dynamically created objects.
    private int m_flockingKernel;
    private int m_birdCountBase = 8;
    private int m_birdCount;
    private IEnumerator m_flockingCoroutine;
    private float m_flockingSecondsPerUpdate;
    private Bounds m_boundingBox;

    // Data buffers used for tracking birds and communicating between the other two scripts
    private SBirdData[] m_birdData = null;
    private ComputeBuffer m_birdDataBuffer = null;
    private Mesh m_birdDataMesh = null;
    private List<Vector3> m_birdLocations;
    private List<Vector3> m_birdVelocities;
    private List<Vector4> m_birdPreviousVelocities;
    private int[] m_birdMeshIndices;

    // Data relating to the player's weapons. (0 = left, 1 = right).
    private GameObject[] m_weapons;

    // Player object.
    private GameObject m_player;

    // Largest size of valid flight area.
    private float m_largestDimensionSq;

    public ComputeShader m_flockingComputeShader; // Reference to the bird compute shader.
	public string m_flockingKernelName = "SimulateFlocking"; // The name of the kernel to use.
	public int m_birdCountMultiplier = 8; // How many times "the base value of 8" more birds to create.
	public float m_flockingUpdatesPerSecond = 60.0f; // The frame-rate of the flocking simulation.
	public Color m_gizmoColour = Color.red; // What colour to draw the boundary and bird gizmos in.
    public bool m_shouldIgnoreRangeTest = false; // If true, birds will update no matter how far they are from the player.
	public bool m_showGizmo = true; // If boundary and bird gizmos are currently active.
	public float m_birdGizmoSize = 0.1f; // How large, in meters, the bird gizmos should be.
	public Vector3 m_flockingAreaSize = Vector3.one; // How large, in meters, the area birds can fly in is.
	public float m_speed = 1.0f; // The speed that birds will always be traveling.
	public float m_borderForce = 100.0f; // How aggressively birds are pushed back in-bounds upon leaving them.
	public float m_maxVelocityChangePercent = 0.2f; // How much the bird tries to keep going in the same direction.
    public float m_verticalDeviationAmplitude = 0.1f; // How quickly the bird can climb or descend.
	public float m_cohesionForce = 2.0f; // How strong the force keeping flocks together is.
	public float m_preSimulationSeconds = 10.0f; // How many seconds to simulate on start-up.
	public float m_cohesionRadiusSquared = 36.0f; // How many meters apart birds can be considered in a flock.
	public float m_minimumSeparationSquared = 4.0f; // The closest birds should try to be.
	public float m_alignmentForce = 20.0f; // How strongly the birds try to match the velocity of flockmates.
    public float m_avoidanceForce = 30.0f; // How strongly the birds try to get out of the player's line of fire.
    public float m_avoidanceRadiusSquared = 900.0f; // How far the birds will try to stay from the line of fire.
	public Material m_birdMaterial; // Which material to draw birds with.

	private void Start()
	{
        // Get reference to the player.
        m_player = CGameManager.PInstanceGameManager.PPlayerObject;
        
        // Calculate the total number of birds and valid bounding box.
        m_birdCount = m_birdCountMultiplier * m_birdCountBase;
		m_boundingBox.SetMinMax(transform.position - m_flockingAreaSize / 2.0f, transform.position + m_flockingAreaSize / 2.0f);

        m_largestDimensionSq = Mathf.Max(m_flockingAreaSize.x / 2.0f, m_flockingAreaSize.y / 2.0f, m_flockingAreaSize.z / 2.0f);
        m_largestDimensionSq *= m_largestDimensionSq;

        // Create the bird data array, data buffer, and initialize flocking data.
        m_birdData = new SBirdData[m_birdCount];
		m_birdDataBuffer = new ComputeBuffer(m_birdCount, sizeof(float) * 3 * 3);//Size of a float * 3 * 3, because struct stores 3 Vector3 variables
		ResetFlockingData();

		// Create the bird data mesh and fill its data with default values.
		m_birdDataMesh = new Mesh();
		m_birdMeshIndices = new int[m_birdCount];
		m_birdLocations = new List<Vector3>();
		m_birdVelocities = new List<Vector3>();
		m_birdPreviousVelocities = new List<Vector4>();

		for(int i = 0; i < m_birdCount; i++)
		{
			m_birdLocations.Add(m_birdData[i].m_position);
			m_birdVelocities.Add(m_birdData[i].m_velocity);
			m_birdPreviousVelocities.Add(m_birdData[i].m_previousVelocity);
			m_birdMeshIndices[i] = i;
		}

		UpdateBirdDataMesh();

		// Get the flocking kernel index from the compute shader.
		m_flockingKernel = m_flockingComputeShader.FindKernel(m_flockingKernelName);

		// Calculate delta-time info (used by the shader and coroutine).
		m_flockingUpdatesPerSecond = Mathf.Clamp(m_flockingUpdatesPerSecond, 1.0f, float.MaxValue);
		m_flockingSecondsPerUpdate = 1.0f / m_flockingUpdatesPerSecond;

        // Get weapon data and initialize uniforms in case a weapon is null.
        m_weapons = GameObject.FindGameObjectsWithTag(CGlobalTags.M_TAG_WEAPON);
        m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_1_POSITION, Vector4.zero);
        m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_1_DIRECTION, Vector4.zero);
        m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_2_POSITION, Vector4.zero);
        m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_2_DIRECTION, Vector4.zero);

        // Start the update coroutine. Pre-simulate for a given amount of time.
        m_flockingCoroutine = UpdateSimulation();
		AdvanceSimulationBySeconds(m_preSimulationSeconds);
		RunSimulation();
	}

	private void OnApplicationQuit()
	{
		StopSimulation();
		m_birdDataBuffer.Dispose();
	}

	private void OnDestroy()
	{
		StopSimulation();
		m_birdDataBuffer.Dispose();
    }

	private void OnDisable()
	{
		StopSimulation();
	}

	private void OnEnable()
	{
		ResetFlockingData();
		RunSimulation();
	}

	private void OnDrawGizmos()
	{
		if (m_showGizmo)
		{
			Gizmos.color = m_gizmoColour;
			Gizmos.DrawWireCube(transform.position, m_flockingAreaSize);

			if (m_birdData != null && m_birdGizmoSize > 0.0f)
			{
				foreach (SBirdData birdData in m_birdData)
				{
					Gizmos.DrawCube(birdData.m_position, Vector3.one * m_birdGizmoSize);
				}
			}
		}
	}

	private void Update()
	{
		// Queue the mesh to be drawn.
		if (m_birdMaterial != null)
		{
			Graphics.DrawMesh(m_birdDataMesh, Matrix4x4.identity, m_birdMaterial, gameObject.layer);
		}
	}

	/*
	 * Resumes the bird flocking simulation with whatever data was present.
	 * Creator: Charlotte C. Brown
	*/
	public void RunSimulation()
	{
		if (m_flockingCoroutine != null)
		{
			StartCoroutine(m_flockingCoroutine);
		}
	}

	/*
	 * Pause the bird flocking simulation without changing any data.
	 * Creator: Charlotte C. Brown
	*/
	public void StopSimulation()
	{
		if (m_flockingCoroutine != null)
		{
			StopCoroutine(m_flockingCoroutine);
		}
	}

	/*
	 * Completely stop the bird flocking simulation and reset data to default.
	 * Creator: Charlotte C. Brown
	*/
	public void ResetSimulation()
	{
		if (m_flockingCoroutine != null)
		{
			StopCoroutine(m_flockingCoroutine);
			ResetFlockingData();
		}
	}

	/*
	 * Resets data and restarts the bird flocking simulation.
	 * Creator: Charlotte C. Brown
	*/
	public void RestartSimulation()
	{
		if (m_flockingCoroutine != null)
		{
			StopCoroutine(m_flockingCoroutine);
			ResetFlockingData();
			RunSimulation();
		}
	}

	/*
	 * Advances the simulation by the given number of seconds. This is primarily used
	 * for start-up to skip the simulation looking like a cloud.
     * Parameters:
     *              -float aSeconds - The number of seconds to advance the simulation.
	 * Creator: Charlotte C. Brown
	 */
	public void AdvanceSimulationBySeconds(float aSeconds)
	{
		float secondsToSimulate = aSeconds;

		// As long as there's time left to skip...
		while (secondsToSimulate > 0.0f)
		{
			// Advance the simulation by one tick and decrement the timer.
			TickSimulation();
			secondsToSimulate -= m_flockingSecondsPerUpdate;
		}
	}

	/*
	 * Coroutine used to update the bird flocking simulation at a reduced framerate for
	 * performance reasons. This simply runs the flocking shader and updates data as required.
	 * Creator: Charlotte C. Brown
	*/
	private IEnumerator UpdateSimulation()
	{
		while (true)
		{
            float distSqFromPlayer = Vector3.SqrMagnitude(m_player.transform.position - transform.position);

            if (distSqFromPlayer <= m_largestDimensionSq || m_shouldIgnoreRangeTest)
            {
                // Advance the simulation by one step.
                TickSimulation();
            }

			yield return new WaitForSeconds(m_flockingSecondsPerUpdate);
		}
	}

	/*
	 * Clears all existing flocking data and resets it to new default values.
	 * Creator: Charlotte C. Brown
	*/
	private void ResetFlockingData()
	{
		for(int i = 0; i < m_birdCount; i++)
		{
			// Calculate random start position and assign it.
			Vector3 spawnPosition;
			spawnPosition.x = Random.Range(m_boundingBox.min.x, m_boundingBox.max.x);
			spawnPosition.y = Random.Range(m_boundingBox.min.y, m_boundingBox.max.y);
			spawnPosition.z = Random.Range(m_boundingBox.min.z, m_boundingBox.max.z);
			m_birdData[i].m_position = spawnPosition;

			// Calculate a random start velocity.
			m_birdData[i].m_velocity = (Random.insideUnitSphere * m_speed).normalized;
		}
	}

	/*
	 * Advances the flocking simulation by one tick.
	 * Creator: Charlotte C. Brown
	*/
	private void TickSimulation()
	{
		if (m_birdData != null)
		{
            // Re-set each uniform every tick in order to allow for multiple systems to use the same shader.
            m_flockingComputeShader.SetInt(M_UNIFORM_BIRD_COUNT, m_birdCount);
            m_flockingComputeShader.SetFloat(M_UNIFORM_DELTA_TIME, m_flockingSecondsPerUpdate);
            m_flockingComputeShader.SetVector(M_UNIFORM_BOUNDS_MIN, new Vector4(m_boundingBox.min.x, m_boundingBox.min.y, m_boundingBox.min.z));
            m_flockingComputeShader.SetVector(M_UNIFORM_BOUNDS_MAX, new Vector4(m_boundingBox.max.x, m_boundingBox.max.y, m_boundingBox.max.z));
            m_flockingComputeShader.SetFloat(M_UNIFORM_BOUNDS_FORCE, m_borderForce);
            m_flockingComputeShader.SetFloat(M_UNIFORM_SPEED, m_speed);
            m_flockingComputeShader.SetFloat(M_UNIFORM_COHESION_RADIUS, m_cohesionRadiusSquared);
            m_flockingComputeShader.SetFloat(M_UNIFORM_COHESION_FORCE, m_cohesionForce);
            m_flockingComputeShader.SetFloat(M_UNIFORM_MAX_VELOCITY_CHANGE_PCT, m_maxVelocityChangePercent);
            m_flockingComputeShader.SetFloat(M_UNIFORM_MIN_SEPARATION, m_minimumSeparationSquared);
            m_flockingComputeShader.SetFloat(M_UNIFORM_ALIGNMENT_FORCE, m_alignmentForce);
            m_flockingComputeShader.SetFloat(M_UNIFORM_AVOIDANCE_FORCE, m_avoidanceForce);
            m_flockingComputeShader.SetFloat(M_UNIFORM_VERTICAL_DEVIATION_AMP, m_verticalDeviationAmplitude);
            m_flockingComputeShader.SetFloat(M_UNIFORM_AVOIDANCE_RADIUS_SQUARED, m_avoidanceRadiusSquared);

            if (m_weapons != null)
            {
                if (m_weapons.Length >= 1)
                {
                    m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_1_POSITION, m_weapons[0].transform.position);
                    m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_1_DIRECTION, m_weapons[0].transform.forward);
                }

                if (m_weapons.Length >= 2)
                {
                    m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_2_POSITION, m_weapons[1].transform.position);
                    m_flockingComputeShader.SetVector(M_UNIFORM_WEAPON_2_DIRECTION, m_weapons[1].transform.forward);
                }
            }

            // Fill the buffer with bird data. Run the compute shader and get data.
            m_birdDataBuffer.SetData(m_birdData);
			m_flockingComputeShader.SetBuffer(m_flockingKernel, M_UNIFORM_BIRD_DATA, m_birdDataBuffer);
            m_flockingComputeShader.Dispatch(m_flockingKernel, m_birdCountMultiplier, 1, 1);
            m_birdDataBuffer.GetData(m_birdData);

            // Update the mesh.
            UpdateBirdDataMesh();
		}
	}

	/*
	 * Sends up-to-date bird data to the mesh used for rendering the simulation.
	 * Creator: Charlotte C. Brown
	 */
	private void UpdateBirdDataMesh()
	{
		for (int i = 0; i < m_birdCount; i++)
		{
			m_birdLocations[i] = m_birdData[i].m_position;
			m_birdVelocities[i] = m_birdData[i].m_velocity;
			m_birdPreviousVelocities[i] = m_birdData[i].m_previousVelocity;
		}
		
		m_birdDataMesh.SetVertices(m_birdLocations);
		m_birdDataMesh.SetNormals(m_birdVelocities);
        m_birdDataMesh.SetTangents(m_birdPreviousVelocities);
		m_birdDataMesh.SetIndices(m_birdMeshIndices, MeshTopology.Points, 0);
	}
}
