using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerVolleyShot : Scene
{
    public exSoftClip m_background;

    public AnimationHandler m_background2;
    public AnimationHandler m_playerAnimation;
    public AnimationHandler m_ballAnimation;

    private float width;

    public override void play()
    {
        m_background2.gameObject.active = false;

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

        m_ballAnimation.playAnimation("PlayerVolleyShot_Ball01Animation");
        m_ballAnimation.e_animationEnd += ballAnimationEnd;
    }

    private void ballAnimationEnd()
    {
        m_playerAnimation.playSpriteAnimation("Player");

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
        m_background2.e_animationEnd -= backgroundAnimationFinished;

        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}