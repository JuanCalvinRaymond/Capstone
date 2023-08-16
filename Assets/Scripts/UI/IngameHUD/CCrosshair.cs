using UnityEngine;
using System.Collections;

/*
Description:Class to display a texture in the screen that will serve as a reticle.
Creator: Alvaro Chavez Mixco
Creation Date: Wednesday, October 19th, 2016
Extra Notes:
*/
public class CCrosshair : MonoBehaviour
{
    private Vector2 m_reticleTextureSize;
    private Vector2 m_reticlePosition;//Lower left corner position
    private IController m_playerController;
    private bool m_showReticle = true;

    public Texture2D m_reticleTexture;

    public bool m_isReticleCentered = true;
    public bool m_showMouseCursor = false;

    /*
    Description:Initialising function used to get the player controller (used for input) and display or hide the cursor
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    Extra Notes:
    */
    void Start()
    {
        Cursor.visible = m_showMouseCursor;//Hide the mouse cursor

        if (CGameManager.PInstanceGameManager != null)
        {
            m_playerController = CGameManager.PInstanceGameManager.PPlayerController;//Get player controller
        }

        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Suscribe to the event when we change the showing aiming aids
            CSettingsStorer.PInstanceSettingsStorer.OnIsShowingAimingAidsChange += HideCrosshair;

            //Hide or show the lasers according to initial state
            HideCrosshair(CSettingsStorer.PInstanceSettingsStorer.PIsShowingAimingAids);
        }
    }

    /*
    Description: Unsuscribe from the setting events
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, November 21, 2016
    */
    private void OnDestroy()
    {
        //If there is a setting storer
        if (CSettingsStorer.PInstanceSettingsStorer != null)
        {
            //Unsuscribe to the input sensitivty change event
            CSettingsStorer.PInstanceSettingsStorer.OnIsShowingAimingAidsChange -= HideCrosshair;
        }
    }

    /*
    Description: Function to hide or show the crosshair
    Parameters(Optional):bool aShowStatus-Whether we want to show or hide the crosshair or not
    Creator: Alvaro Chavez Mixco
    Extra Notes(Optional): This function gets called because it suscribe to the OnShowAimingAids
    event in the settings storer.
    */
    private void HideCrosshair(bool aShowStatus)
    {
        m_showReticle = aShowStatus;
    }

    /*
    Description:Draws the reticle texture on the screen
    Creator: Alvaro Chavez Mixco
    Creation Date: Wednesday, October 19th, 2016
    Extra Notes:
    */
    private void OnGUI()
    {
        //If there is a reticle texture and we don't want to hide the crosshair
        if (m_reticleTexture != null && m_showReticle == true)
        {
            //Get the size of the texture, in case it has to be used for other purposes
            m_reticleTextureSize.x = m_reticleTexture.width;
            m_reticleTextureSize.y = m_reticleTexture.height;

            Vector2 halfReticleTextureSize = m_reticleTextureSize / 2.0f;//Saved for calculations

            //If the reticle is going to be placed according to the look input
            if (m_isReticleCentered == false && m_playerController != null)
            {
                m_reticlePosition.x = m_playerController.GetLookInput().x - halfReticleTextureSize.x;
                m_reticlePosition.y = (Screen.height - m_playerController.GetLookInput().y) - halfReticleTextureSize.y;

                //Ensure the reticle appears on the screen
                m_reticlePosition.x = Mathf.Clamp(m_reticlePosition.x, -halfReticleTextureSize.x, Screen.width - halfReticleTextureSize.x);
                m_reticlePosition.y = Mathf.Clamp(m_reticlePosition.y, -halfReticleTextureSize.y, Screen.height - halfReticleTextureSize.y);
            }
            else//If the reticle is going to be on center of screen
            {
                m_reticlePosition.x = (Screen.width / 2.0f) - halfReticleTextureSize.x;
                m_reticlePosition.y = (Screen.height / 2.0f) - halfReticleTextureSize.y;
            }

            //Draw the reticle
            GUI.DrawTexture(new Rect(m_reticlePosition, m_reticleTextureSize), m_reticleTexture);
        }
    }

}
