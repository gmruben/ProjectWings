using UnityEngine;
using System;
using System.Collections;

public class Scene_PlayerTackle : Scene
{
    public exSoftClip m_background;

    public AnimationHandler m_playerAnimation;
    public AnimationHandler m_ballAnimation;

    SpriteTrail playerSpriteTrail;

    private float width;

    public override void play()
    {
        //playerSpriteTrail = m_playerAnimation.GetComponent<SpriteTrail>();
        //playerSpriteTrail.init(m_playerAnimation.transform);

        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        yield return new WaitForSeconds(0);

        m_playerAnimation.playAnimation("PlayerTackle_Player01Animation");
        m_playerAnimation.playSpriteAnimation("PlayerTackle_Player02SpriteAnimation");
        m_playerAnimation.e_animationEnd += playerAnimationFinished;

        m_ballAnimation.playSpriteAnimation("PlayerTackle_Ball01SpriteAnimation");

        //playerSpriteTrail.setActive(true);
    }

    private void playerAnimationFinished()
    {
        m_playerAnimation.e_animationEnd -= playerAnimationFinished;

        m_ballAnimation.playSpriteAnimation("PlayerTackle_Ball02SpriteAnimation");
        m_ballAnimation.e_animationEnd += ballAnimationFinished;

        //Set trail inactive
        //playerSpriteTrail.setActive(false);
    }

    private void ballAnimationFinished()
    {
        m_ballAnimation.e_animationEnd -= ballAnimationFinished;

        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}
