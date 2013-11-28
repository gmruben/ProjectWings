using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerCut_NoPass : Scene
{
    public exSoftClip m_background;

    public AnimationHandler m_playerAnimation;
    public AnimationHandler m_ballAnimation;

    private float width;

    public override void play()
    {
        m_background.width = 0;
        width = 0;

        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        m_playerAnimation.playSpriteAnimation("PlayerCutNoPass_Player01SpriteAnimation");

        while (width < 128)
        {
            width += Time.deltaTime * 500;
            m_background.width = width;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        width = 128;
        m_background.width = width;

        m_ballAnimation.playAnimation("PlayerCutNoPass_Ball01Animation");
        m_ballAnimation.e_animationEnd += ballAnimationEnd;
    }

    private void ballAnimationEnd()
    {
        m_playerAnimation.playSpriteAnimation("PlayerCutNoPass_Player02SpriteAnimation");

        m_ballAnimation.e_animationEnd -= ballAnimationEnd;
        m_playerAnimation.e_animationEnd += playerAnimationFinished;
    }

    private void playerAnimationFinished()
    {
        m_playerAnimation.e_animationEnd -= playerAnimationFinished;

        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}