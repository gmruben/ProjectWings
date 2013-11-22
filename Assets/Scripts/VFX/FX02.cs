using UnityEngine;
using System;
using System.Collections;

public class FX02 : MonoBehaviour
{
    public Action e_end;

    public AnimationHandler m_animation;

    public void init()
    {
        m_animation.init();
        m_animation.playAnimation("FX02_Animation");

        m_animation.e_animationEnd += animationEnd;
    }

    private void animationEnd()
    {
        if (e_end != null)
        {
            e_end();
            e_end = null;
        }

        GameObject.Destroy(gameObject);
    }
}
