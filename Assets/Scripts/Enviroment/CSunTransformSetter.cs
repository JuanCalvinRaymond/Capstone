using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

// Used to set the transform property of a SunShafts script
// Charlotte Brown
[RequireComponent(typeof(SunShafts))]
public class CSunTransformSetter : MonoBehaviour
{
    public string m_sunTag = "MainSunPosition";

    private GameObject m_sun;

    void Start()
    {
        // Get the SunShafts component.
        SunShafts sunShafts = GetComponent<SunShafts>();

        // Get the main sun.
        m_sun = GameObject.FindGameObjectWithTag(m_sunTag);


        // If the sun is null, disable sun shafts and return.
        if(m_sun == null)
        {
            sunShafts.enabled = false;
            return;
        }
        
        // Set the SunShaft's sun transform to the sun's transform.
        sunShafts.sunTransform = m_sun.transform;
    }
}
