using UnityEngine;
using System.Collections;

/*
 Description: Helper function used to calculate the linear and angular velocity of a game object
 Parameters(Optional):
 Creator: Alvaro Chavez Mixco
 Extra Notes: 
 */
public class CVelocityCalculator : MonoBehaviour
{
    //Variables to keep the previous frame conditions
    private Vector3 m_previousFramePosition;
    private Vector3 m_previousFrameRotation;

    //Velocities
    private Vector3 m_currentVelocity;
    private Vector3 m_currentAngularVelocity;

    // Used to store the velocity over the past few frames in order to smooth out unsteady
    // speeds caused by sampling from an animation.
    private Vector3[] m_recentVelocities;
    private Vector3[] m_recentAngularVelocities;

    // How many velocity samples the calculator should store.
    public ushort m_numberOfVelocitySamples = 5;
    
    public Vector3 PCurrentVelocity
    {
        get
        {
            return m_currentVelocity;
        }
    }

    public Vector3 PAngularVelocity
    {
        get
        {
            return m_currentAngularVelocity;
        }
    }

     /*
     Description: At start, set the intial values for the "previous" condtions
     Parameters(Optional): 
     (settings as traveling back)
     Creator: Alvaro Chavez Mixco
     Extra Notes: 
     */
    private void Start()
    {
        m_previousFramePosition = transform.position;
        m_previousFrameRotation = transform.rotation.eulerAngles;

        // Make sure the number of samples isn't 0.
        m_numberOfVelocitySamples = m_numberOfVelocitySamples > 0 ? m_numberOfVelocitySamples : (ushort)1;

        // Initialize the recent velocity lists.
        m_recentVelocities = new Vector3[m_numberOfVelocitySamples];
        m_recentAngularVelocities = new Vector3[m_numberOfVelocitySamples];
    }

    /*
    Description:Every frame calculate the linear velocity and the angular velocity of the object.
    Parameters(Optional): 
    (settings as traveling back)
    Creator: Alvaro Chavez Mixco
    Extra Notes: 
    */
    private void Update()
    {
        // If the two recent velocity arrays aren't null.
        if (m_recentAngularVelocities != null && m_recentVelocities != null)
        {
            // Add this frame's velocities to the recent velocities lists.
            SampleVelocity();

            //Save the current transform as the previous one for use in the next frame
            m_previousFramePosition = transform.position;
            m_previousFrameRotation = transform.rotation.eulerAngles;

            // Calculate the current smoothed out velocities.
            UpdateVelocities();
        }
    }

    /*
    Description: Get the velocity from this frame and add it into the recent velocities list.
    Creator: Charlotte Brown
    */
    private void SampleVelocity()
    {
        // Get this frame's velocities according to its current transform and the transform he had the last frame.
        Vector3 velocity = (transform.position - m_previousFramePosition) / CGameManager.PInstanceGameManager.GetScaledDeltaTime();
        Vector3 angularVelocity = (transform.rotation.eulerAngles - m_previousFrameRotation) / CGameManager.PInstanceGameManager.GetScaledDeltaTime();

        // Add the two velocities to the recent velocities lists.
        AddVelocitySample(ref m_recentVelocities, velocity);
        AddVelocitySample(ref m_recentAngularVelocities, angularVelocity);
    }

    /*
    Description: Add a given sample into the given array after shifting all array elements to the left
    by one. This allows for storing all velocities over the past few frames in order to smooth out the
    velocity jitter caused by sampling from an animation.
    Parameters: aSampleArray - The array to put the sample into | aSample - The sample to put into the array.
    Creator: Charlotte Brown 
    */
    private void AddVelocitySample(ref Vector3[] aSampleArray, Vector3 aSample)
    {
        // Shift all of the velocities in the array to the left by one. Be sure to not run off the end of the array.
        for(ushort i = 0; i < aSampleArray.Length - 1; i++)
        {
            aSampleArray[i] = aSampleArray[i + 1];
        }

        // Assign the given sample to the last (rightmost) element in the array.
        aSampleArray[aSampleArray.Length - 1] = aSample;
    }

    /*
    Description: Calculate the smoothed out/average velocities and save them to the class properties.
    Creator: Charlotte Brown
    */
    private void UpdateVelocities()
    {
        // Get the averaged velocities.
        m_currentVelocity = GetAverageValues(ref m_recentVelocities);
        m_currentAngularVelocity = GetAverageValues(ref m_recentAngularVelocities);
    }

    /*
    Description: Calculate the average of all vectors in the given array.
    Parameters: The array to calculate the average of.
    Creator: Charlotte Brown
    */
    private Vector3 GetAverageValues(ref Vector3[] aSampleArray)
    {
        // Set the average to zero.
        Vector3 average = Vector3.zero;

        foreach(Vector3 value in aSampleArray)
        {
            // Get the sum of all values in the array.
            average += value;
        }

        // Divide the sum by the total number of samples to get the average.
        return average / aSampleArray.Length;
    }
}
