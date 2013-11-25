using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerTackle_Dribble : Scene
{
    public exSoftClip m_background;

    public AnimationHandler m_playerAnimation;

    private float width;

    public override void play()
    {
        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        yield return new WaitForSeconds(0);

        m_playerAnimation.playAnimation("PlayerTackle02_Player01Animation");
        m_playerAnimation.playSpriteAnimation("PlayerTackle02_Player02SpriteAnimation");
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
