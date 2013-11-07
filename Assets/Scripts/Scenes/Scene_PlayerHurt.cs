using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerHurt : Scene
{
    public SceneAnimation m_background;
    public SceneAnimation m_playerAnimation;

    private float width;

    public override void play()
    {
        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        yield return new WaitForSeconds(0);

        m_background.playSpriteAnimation("PlayerHurt_BGSpriteAnimation");
        m_background.e_animationEnd += backgroundAnimationEnd;
    }

    private void backgroundAnimationEnd()
    {
        m_background.e_animationEnd -= backgroundAnimationEnd;

        m_playerAnimation.playAnimation("PlayerHurt_PlayerAnimation");
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
