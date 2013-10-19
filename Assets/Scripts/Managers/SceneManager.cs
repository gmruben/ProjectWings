using UnityEngine;
using System;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public Action e_sceneFinished;

    //SCENES
    public Scene m_sceneGKPunch;
    public Scene m_sceneVolleyShot;

    private static SceneManager _instance;

    #region PUBLIC FUNCTIONS

    public void play(string pSceneID)
    {
        Scene scene = (GameObject.Instantiate(m_sceneVolleyShot) as GameObject).GetComponent<Scene>();
        scene.play();
        scene.e_end += sceneFinished;
    }

    #endregion

    private void sceneFinished()
    {
        if (e_sceneFinished != null) e_sceneFinished();
    }

    #region PROPERTIES

    public static SceneManager instance
    {
        get
        {
            if (_instance == null)
            {
                //Find game object
                GameObject sceneManager = GameObject.Find("SceneManager");

                if (sceneManager == null)
                {
                    Debug.LogError("Couldn't find SceneManager instance");
                }

                //Get Custom Network component
                _instance = sceneManager.GetComponent<SceneManager>();
            }

            return _instance;
        }
    }

    #endregion
}

public class SceneID
{
    public const string GKPunch = "GKPunch";
    public const string GKCatch = "GKCatch";
}