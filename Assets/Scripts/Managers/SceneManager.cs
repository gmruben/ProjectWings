using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
    public Action e_sceneFinished;

    //SCENES
    public Scene m_sceneGKPunch;
    public Scene m_sceneVolleyShot;

    private Scene m_scene;
    private List<Scene> m_sceneList;

    private static SceneManager _instance;

    #region PUBLIC FUNCTIONS

    public void play(string pSceneID)
    {
        Scene scene = (GameObject.Instantiate(m_sceneVolleyShot) as GameObject).GetComponent<Scene>();
        scene.play();
        scene.e_end += sceneFinished;
    }

    public void playTackle02(int pTeamUser1, int pTeamUser2)
    {
        m_sceneList = new List<Scene>();

        Scene startScene = GUIManager.instance.createStartScene();
        startScene.gameObject.SetActiveRecursively(false);
        m_sceneList.Add(startScene);

        Scene tackleScene = GUIManager.instance.createTackle02Scene();
        tackleScene.gameObject.SetActiveRecursively(false);
        m_sceneList.Add(tackleScene);

        Scene jumpScene = GUIManager.instance.createJumpScene();
        jumpScene.gameObject.SetActiveRecursively(false);
        m_sceneList.Add(jumpScene);

        playScene();
    }

    #endregion

    private void playScene()
    {
        m_scene = m_sceneList[0];
        m_sceneList.RemoveAt(0);

        m_scene.gameObject.SetActiveRecursively(true);
        m_scene.play();
        m_scene.e_end += sceneEnd;
    }

    private void sceneEnd()
    {
        m_scene.e_end -= sceneEnd;

        if (m_sceneList.Count > 0)
        {
            playScene();
        }
        else
        {
            sceneFinished();
        }
    }

    private void sceneFinished()
    {
        ApplicationFactory.instance.m_messageBus.dispatchCurrentSceneEnded();

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