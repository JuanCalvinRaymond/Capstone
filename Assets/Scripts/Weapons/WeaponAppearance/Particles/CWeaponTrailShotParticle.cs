using UnityEngine;
using System.Collections;

/*
Description: Class usedto control the particles emitted by a hit scan weapon
Creator: Alvaro Chavez Mixco
Creation Date: Thursday, March 16th, 2017
*/
[RequireComponent(typeof(CHitScanWeapon))]
public class CWeaponTrailShotParticle : CWeaponShotParticle
{
    private CHitScanWeapon m_hitScanWeapon;

	// The particles to emit.
	public ParticleSystem m_firingLineParticles;

	// How many particles to emit per meter of firing line length.
	[Range(0, 10)]
	public float m_tracerParticlesPerMeter = 0.2f;

	// What percent of the firing line to emit particles between. X = minimum percent, Y = maximum percent.
	public Vector2 m_firingLineBounds;

    /*
    Description: Get the hitscan weapon component
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    protected override void Awake()
    {
        base.Awake();

        //Get the hit scan weapon component
        m_hitScanWeapon = GetComponent<CHitScanWeapon>();
    }

    /*
    Description: Suscribe to hitscan weapon event
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    protected override void Start()
    {
        base.Start();

        //Suscribe to hitscan weapon onfire event
        //This is different from  onfire event in order to access raycast information
        m_hitScanWeapon.OnFireHitscanWeapon += OnHitScanWeaponFired;
    }

    /*
    Description: Unsuscribe from the hitscan weapon event
    Creator: Alvaro Chavez Mixco
    Creation Date: Thursday, March 16th, 2017
    */
    protected override void OnDestroy()
    {
        base.OnDestroy();

        //Unsuscribe to hit scan weapon onfire event
        m_hitScanWeapon.OnFireHitscanWeapon -= OnHitScanWeaponFired;
    }

    /*
    Description: Creates a line of tracer particles from the firing point along the firing line.
    Creator: Charlotte C. Brown
    */
    private void OnHitScanWeaponFired(Vector3 aShootPosition, Vector3 aHitPosition)
    {
		// Make sure particles aren't null.
		if (m_firingLineParticles == null)
		{
			return;
		}

        // Get the total distance between the two points and divide it. This is to keep particle density even at all ranges.
        int numberOfParticles = (int)(Vector3.Distance(aHitPosition, aShootPosition) * m_tracerParticlesPerMeter);

		// For each particle we want to spawn...
		for (int i = 0; i < numberOfParticles; i++)
		{
			// Pick a random position between the firing point and the end of the firing line.
			Vector3 emitPosition = Vector3.Lerp(aShootPosition, aHitPosition, Random.Range(m_firingLineBounds.x, m_firingLineBounds.y));

			// Set the position.
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
			emitParams.position = emitPosition;

			// Emit one particle.
			m_firingLineParticles.Emit(emitParams, 1);
		}
    }
}
