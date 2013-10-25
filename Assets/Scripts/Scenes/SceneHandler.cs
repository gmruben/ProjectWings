using UnityEngine;
using System.Collections;

public class SceneHandler : MonoBehaviour
{
    public Scene m_scene;

	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Z))
        {
            m_scene.play();
        }
	}
}
