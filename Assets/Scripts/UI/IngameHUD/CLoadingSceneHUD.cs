using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Text;

/*
 Description: Class used to control the HUD that is shown in the loading screen
 Creator: Alvaro Chavez Mixco
 Creation Date: Friday, February 3rd, 2017
 */
public class CLoadingSceneHUD : MonoBehaviour
{
    private const string M_PERCENT_SIGN = "%";
    private const string M_DOT = ".";

    private StringBuilder m_animatedDotsStringBuilder;
    private float m_currentFrame = 0.0f;

    private float m_timeForNextFrame = 0.0f;
    private float m_timerForFrameChange = 0.0f;

    [Header("HUD elements")]
    public Text m_percentLoadedText;
    public Text m_movingLoadingText;
    public RawImage m_loadingImage;

    [Header("Loading screen images")]
    [Tooltip("Image should match their element number. Menus:0 | Beginner:1 | Advanced:2 | Practice: 3")]
    public Texture[] m_loadingScreenTextures;

    [Header("Animation")]
    public bool m_loopingAnimation;
    public int m_currentAnimationPose = 0;
    public int m_maxAnimationPose = 3;
    public float m_framesPerSecond = 30.0f;

    /*
     Description: Initialize the string builder
     Creator: Alvaro Chavez Mixco
     Creation Date: Friday, February 3rd, 2017
     */
    private void Awake()
    {
        //Initialize the string builder
        m_animatedDotsStringBuilder = new StringBuilder();
    }

    /*
    Description: Update the loading percent text
    Parameters: float aPercentLoaded - A 0.0 to 1.0 percent of how much the new scene has been loaded
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void UpdatePercentDisplay(float aPercentLoaded)
    {
        //Set the text for the percent loaded
        CUtilitySetters.SetText2DText(ref m_percentLoadedText, Mathf.Round((aPercentLoaded * 100.0f)).ToString() + M_PERCENT_SIGN);
    }

    /*
    Description: Set the correspponding image according to the scene type being loaded.
    Parameters: ELevelState aSceneBeingLoadedType - The type of scene that is being loaded.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    public void SetLoadingImage(ELevelState aSceneBeingLoadedType)
    {
        //If there are textures to set
        if (m_loadingScreenTextures != null)
        {
            //If the index is within the array valid values
            if (aSceneBeingLoadedType >= 0 && (int)aSceneBeingLoadedType < m_loadingScreenTextures.Length)
            {
                //Set the loading image according to the level
                CUtilitySetters.SetRawImageTexture(ref m_loadingImage, m_loadingScreenTextures[(int)aSceneBeingLoadedType]);

                m_timeForNextFrame = 1.0f / m_framesPerSecond;
            }
        }
    }

    /*
    Description: Update the dots animation
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void Update()
    {

        //Save the current pose of the animation
        float previousPose = m_currentAnimationPose;

        //Calculate which animation pose should be displayed
        CalculatePose();

        //If the previous pose is not the same as the current one
        if (previousPose != m_currentAnimationPose)
        {
            //Update the text in the dots
            SetDotsText(m_currentAnimationPose);
        }
    }

    /*
    Description: Calculate which "pose" or frame should the animation be in.
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void CalculatePose()
    {
        //Increase the time
        m_timerForFrameChange += Time.unscaledDeltaTime;

        //PENDING
        if (m_timerForFrameChange > m_timeForNextFrame)
        {
            m_timerForFrameChange = 0.0f;
            m_currentFrame++;

            //If the current frame is bigger than the frame for the next animation
            if (m_currentFrame > m_currentAnimationPose* m_framesPerSecond)
            {
                //Go to the next pose
                m_currentAnimationPose++;

                //If we are looping hte animation and is beyond the max number of poses
                if (m_currentAnimationPose > m_maxAnimationPose && m_loopingAnimation == true)
                {
                    //Reset the animation pose and frames
                    m_currentAnimationPose = 0;
                    m_currentFrame = 0;
                }
            }
        }
    }

    /*
    Description: Set the text for the dots animation
    Creator: Alvaro Chavez Mixco
    Creation Date: Friday, February 3rd, 2017
    */
    private void SetDotsText(int aNumberOfDots)
    {
        //If there is text to animate
        if (m_movingLoadingText != null)
        {
            //Clear the string builder
            m_animatedDotsStringBuilder.Length = 0;

            //For the number of dots to show
            for (int i = 0; i < aNumberOfDots; i++)
            {
                //Attach a dot to the string builder
                m_animatedDotsStringBuilder.Append(M_DOT);
            }

            //Set the new dots in the text
            CUtilitySetters.SetText2DText(ref m_movingLoadingText, m_animatedDotsStringBuilder.ToString());
        }
    }
}
