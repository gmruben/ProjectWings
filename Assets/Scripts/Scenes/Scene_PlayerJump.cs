using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerJump : Scene
{
    public exSoftClip m_background;

    public SceneAnimation m_background2;
    public SceneAnimation m_playerAnimation;
    public SceneAnimation m_ballAnimation;

    SpriteTrail playerSpriteTrail;

    private float height;

    public override void play()
    {
        m_background.height = 0;
        height = 0;

        StartCoroutine(updateBG());

        m_background2.playAnimation("PlayerJump_BG01Animation");
        m_playerAnimation.playAnimation("PlayerJump_Player01Animation");
        m_ballAnimation.playAnimation("PlayerJump_Ball01Animation");

        m_playerAnimation.e_animationEnd += playerAnimationFinished;
    }

    private IEnumerator updateBG()
    {
        while (height < 128)
        {
            height += Time.deltaTime * 350;
            m_background.height = height;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        height = 128;
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
