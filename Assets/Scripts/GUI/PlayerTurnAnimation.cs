using UnityEngine;
using System;
using System.Collections;

public class PlayerTurnAnimation : MonoBehaviour
{
    public Action e_end;

    public AnimationHandler m_bgAnimation;
    public AnimationHandler m_textAnimation;

    public void init()
    {
        m_bgAnimation.init();
        m_textAnimation.init();

        m_bgAnimation.playAnimation("PlayerTurnBG_Animation");
        m_textAnimation.playAnimation("PlayerTurnText_Animation");

        m_textAnimation.e_animationEnd += animationEnd;
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