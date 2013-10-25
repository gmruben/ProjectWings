using UnityEngine;
using System;
using System.Collections;

public class Scene_GKCatch : Scene
{
    public exSoftClip m_background;

    public SceneAnimation m_background2;
    public SceneAnimation m_playerAnimation;
    public SceneAnimation m_ballAnimation;

    private float width;

    public override void play()
    {
        m_background.width = 0;
        width = 0;

        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        while (width < 128)
        {
            width += Time.deltaTime * 500;
            m_background.width = width;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        width = 128;
        m_background.width = width;

        m_ballAnimation.playAnimation();
        m_ballAnimation.e_animationEnd += ballAnimationEnd;
    }

    private void ballAnimationEnd()
    {
        m_playerAnimation.playSpriteAnimation("GKCatch_GKAnimation");

        m_ballAnimation.e_animationEnd -= ballAnimationEnd;
        m_playerAnimation.e_animationEnd += playerAnimationFinished;
    }

    private void playerAnimationFinished()
    {
        m_background2.gameObject.active = true;
        m_background2.playSpriteAnimation("BG");

        m_playerAnimation.e_animationEnd -= playerAnimationFinished;
        m_background2.e_animationEnd += backgroundAnimationFinished;
    }

    private void backgroundAnimationFinished()
    {
        //m_background2.gameObject.active = false;
        //m_ballAnimation.gameObject.active = false;

        m_background2.e_animationEnd -= backgroundAnimationFinished;

        //The animation has ended
        if (e_end != null) e_end();
    }
}
