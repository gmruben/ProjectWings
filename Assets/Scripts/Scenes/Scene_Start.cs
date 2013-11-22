using UnityEngine;
using System;
using System.Collections;

public class Scene_Start : Scene
{
    private const float m_speed = 250;
    private const float m_sceenHeight = 90;

    public exSoftClip m_background;

    private float m_height;

    void Start()
    {
        play();
    }

    public void play()
    {
        m_background.height = 0;
        m_height = 0;

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

        //Clean all actions
        cleanAllActions();
    }
}
