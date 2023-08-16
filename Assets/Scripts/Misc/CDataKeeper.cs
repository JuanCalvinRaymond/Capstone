using UnityEngine;
using System.Collections;

public class CDataKeeper : MonoBehaviour
{
    private static CDataKeeper s_instance;


    [Tooltip("The image sequences that the animated texture player can play")]
    public CAnimatedTexture[] m_imageSequences;


    private void Awake()
    {
        //Singleton instance
        //If the instance doesn't exist
        if (s_instance == null)
        {
            //Set this as the instance
            s_instance = this;

            //Suscribe to the on scene loaded event
            //SceneManager.sceneLoaded += OnSceneLoaded;

            //Ensure the game object is not destroyed when a new scene is loaded
            DontDestroyOnLoad(this);
        }
        else//If the instance already exists
        {
            //Destroy this object
            Destroy(gameObject);
        }
    }
    /*
    Description: Function to sort in editor alphanumerically all the frames/images of the image sequences 
    Creator: Alvaro Chavez Mixco
    Creation Date: Monday, March 20th, 2017
    Extra Notes: Context menu function accessible by right clicking the component.
    */
    [ContextMenu("Sort Image Sequence Alphanumerically")]
    private void SortImageSequences()
    {
        //If there are iamge sequences
        if (m_imageSequences != null)
        {
            //Go through every image sequence
            for (int i = 0; i < m_imageSequences.Length; i++)
            {
                //If the image sequence is valid
                if (m_imageSequences[i] != null)
                {
                    //Sort it alphanumerically
                    m_imageSequences[i].Sort();
                }
            }
        }
    }
}
