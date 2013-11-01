using UnityEngine;
using System;
using System.Collections;

public class Scene_BallShot : Scene
{
    private const float m_time = 2.5f;

    public override void play()
    {
        StartCoroutine(playAnimation());
    }

    private IEnumerator playAnimation()
    {
        yield return new WaitForSeconds(m_time);

        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}
