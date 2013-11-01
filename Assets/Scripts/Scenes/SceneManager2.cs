using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneManager2 : MonoBehaviour
{
    private const float m_speed = 50;
    private const float m_sceenHeight = 90;

    public Action e_end;

    public exSoftClip m_background;

    private float m_height;
    private List<Scene> m_sceneList;

    public void play(List<Scene> pSceneList)
    {
        m_background.height = 0;
        m_height = 0;

        m_sceneList = pSceneList;

        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        while (m_height < m_sceenHeight)
        {
            m_height += Time.deltaTime * m_speed;
            m_background.height = m_height;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        m_background.height = m_sceenHeight;

        if (e_end != null) e_end();
    }
}
