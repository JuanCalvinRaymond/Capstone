using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
Description: Class used to hide objects when a move slider objecct moves an object.
Creator: Alvaro Chavez Mixco
Creation Date: Friday, March 3rd, 2017
*/
public class CMoverSliderObjectHider : MonoBehaviour
{
    /*
    Description: Enum to marks the type of cutoff
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    public enum ETypesOfCutoff
    {
        Disable
    };

    public CSliderObjectMover m_slider;

    public float m_squaredDistanceCutOff = 25.0f;

    [Tooltip("If this is false it will be assume the object is a vertical slider")]
    public bool m_horizontalSlider = true;

    [Tooltip("Offset in case we don't want to compare from directly the slider position")]
    public Vector3 m_sliderPositionOffset = Vector3.zero;

    public ETypesOfCutoff m_typeOfCuttoff;

    private delegate void delegCutOff(GameObject aObjectBeingCut, float aSquaredDistanceToObject);
    private delegCutOff m_cutOffFunction;
    private delegCutOff m_restoreFucntion;

    /*
    Description: Suscribe to the slider event
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void Awake()
    {
        //If there is a slider
        if (m_slider != null)
        {
            //Suscribe to event
            m_slider.OnSliderMovedObject += OnObjectMoved;
        }
    }

    /*
    Description: Get all the objects taht are being
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void Start()
    {
        //Set the desired cut off function
        SetCutoffFunctions(m_typeOfCuttoff);

        //If there is no slider
        if (m_slider == null)
        {
            //Disabel the component
            enabled = false;
        }
        else
        {
            //Set initial values
            OnObjectMoved(0.0f, m_slider.transform.position, gameObject.transform.position);
        }
    }

    /*
    Description: Get all the objects taht are being
    Parameters : ETypesOfCutoff aTypeOfCutoff - The desired type of cutoff
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */

    private void SetCutoffFunctions(ETypesOfCutoff aTypeOfCutoff)
    {
        switch (aTypeOfCutoff)
        {
            case ETypesOfCutoff.Disable:
                m_cutOffFunction = CutOffFDisable;
                m_restoreFucntion = RestoreEnable;
                break;
            default:
                break;
        }
    }

    /*
    Description: Disable the object.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void CutOffFDisable(GameObject aObjectBeingCut, float aSquaredDistanceToObject)
    {
        aObjectBeingCut.SetActive(false);
    }

    /*
    Description: Enable the object.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void RestoreEnable(GameObject aObjectBeingRestored, float aSquaredDistanceToObject)
    {
        aObjectBeingRestored.SetActive(true);
    }

    /*
    Description: Disable the object.
    Parameters: float aSliderPercent - The current percent of the slider
                Vector3 aSliderWorldPosition - The world postion of the slider
                Vector3 aObjectMovedPosition - The world position of the object being moved by the slider
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, March 3rd, 2017
    */
    private void OnObjectMoved(float aSliderPercent, Vector3 aSliderWorldPosition, Vector3 aObjectMovedPosition)
    {
        //If the slider cutoff and restore fuctions are valid
        if (m_cutOffFunction != null && m_restoreFucntion != null)
        {
            if (transform.childCount > 0)
            {
                GameObject[] childObjectsBeingMoved = CUtilityGame.GetChildrenGameObjectFromParent(transform.gameObject);

                //Get the slider position
                Vector3 sliderCenter = aSliderWorldPosition;

                //According to if the slider is horizontal or vertical offset its position,
                //IF slider is horizontal
                if (m_horizontalSlider == true)
                {
                    //Ignore the slider y position
                    sliderCenter.y = aObjectMovedPosition.y;
                }
                else//if the slider is vertical
                {
                    //Ignore the slider x position
                    sliderCenter.x = aObjectMovedPosition.x;
                }

                //Add the offset
                sliderCenter += m_sliderPositionOffset;

                float squaredDistanceToSlider = 0.0f;

                //Go through every object being moved by the slider
                for (int i = 0; i < childObjectsBeingMoved.Length; i++)
                {
                    //If the object is valid
                    if (childObjectsBeingMoved[i] != null)
                    {
                        //Calculate its ditance to the slider
                        squaredDistanceToSlider = (childObjectsBeingMoved[i].transform.position - sliderCenter).sqrMagnitude;

                        //If the distance is bigger than the cutoff range
                        if (squaredDistanceToSlider > m_squaredDistanceCutOff)
                        {
                            //Call the cutoff function
                            m_cutOffFunction(childObjectsBeingMoved[i], squaredDistanceToSlider);
                        }
                        else//If the distance is less than the cutoff range
                        {
                            //Call the restore function
                            m_restoreFucntion(childObjectsBeingMoved[i], squaredDistanceToSlider);
                        }
                    }
                }
            }
        }
    }
}
