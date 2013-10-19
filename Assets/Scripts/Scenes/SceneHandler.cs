using UnityEngine;
using System.Collections;

public class SceneHandler : MonoBehaviour
{
    public Scene_GKPunch m_scene;

	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Z))
        {
            m_scene.play();
        }
	}
}
