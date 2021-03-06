using UnityEngine;
using System;
using System.Collections;

public class Scene_GKCatch_Goal : Scene
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
        while (width < 128)
        {
            width += Time.deltaTime * 500;
            m_background.width = width;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        width = 128;
        m_background.width = width;

        m_ballAnimation.playAnimation("GKCatchGoal_Ball01Animation");
        m_ballAnimation.e_animationEnd += ball01AnimationEnd;
    }

    private void ball01AnimationEnd()
    {
        m_playerAnimation.playAnimation("GKCatchGoal_GK01Animation");
        m_playerAnimation.playSpriteAnimation("GKCatchGoal_GKAnimation");

        m_playerAnimation.e_animationEnd += playerAnimationFinished;
        m_ballAnimation.e_animationEnd -= ball01AnimationEnd;
    }

    private void playerAnimationFinished()
    {
        m_ballAnimation.playAnimation("GKCatchGoal_Ball02Animation");

        m_ballAnimation.e_animationEnd += ball02AnimationEnd;
        m_playerAnimation.e_animationEnd -= playerAnimationFinished;
    }

    private void ball02AnimationEnd()
    {
        m_ballAnimation.e_animationEnd -= ball02AnimationEnd;

        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}
