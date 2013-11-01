using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerTackle : Scene
{
    public exSoftClip m_background;

    public SceneAnimation m_playerAnimation;
    public SceneAnimation m_ballAnimation;

    private float width;

    public override void play()
    {
        //m_background.width = 0;
        //width = 0;

        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        yield return new WaitForSeconds(0);

        /*while (width < 128)
        {
            width += Time.deltaTime * 500;
            m_background.width = width;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        width = 128;
        m_background.width = width;*/

        m_playerAnimation.playAnimation("PlayerTackle_Player01Animation");
        m_playerAnimation.playSpriteAnimation("PlayerTackle_Player02SpriteAnimation");

        //m_ballAnimation.playAnimation();
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
        m_playerAnimation.e_animationEnd -= playerAnimationFinished;
    }

    private void backgroundAnimationFinished()
    {
        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}
