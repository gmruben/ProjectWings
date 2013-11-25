using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour
{
    public GameObject m_scenePrefab;

    private Scene m_scene;
    private List<Scene> m_sceneList;

	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Z))
        {
            m_sceneList = new List<Scene>();

            Scene tackleScene = GUIManager.instance.createTackleDribbleScene();
            tackleScene.gameObject.SetActiveRecursively(false);
            m_sceneList.Add(tackleScene);

            Scene jumpScene = GUIManager.instance.createJumpScene();
            jumpScene.gameObject.SetActiveRecursively(false);
            m_sceneList.Add(jumpScene);

            play();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Scene scene = (GameObject.Instantiate(m_scenePrefab) as GameObject).GetComponent<Scene>();
            scene.play();
        }
	}

    private void play()
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
            play();
        }
    }
}