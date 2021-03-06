using UnityEngine;
using System;
using System.Collections;

public class Scene_Goal : Scene
{
    public AnimationHandler m_ballAnimation;

    SpriteTrail playerSpriteTrail;

    private float width;

    public override void play()
    {
        StartCoroutine(updateBG());
    }

    private IEnumerator updateBG()
    {
        yield return new WaitForSeconds(0);

        m_ballAnimation.playSpriteAnimation("Goal_BallSpriteAnimation");
        m_ballAnimation.e_animationEnd += ballAnimationEnd;
    }

    private void ballAnimationEnd()
    {
        m_ballAnimation.e_animationEnd -= ballAnimationEnd;

        //The animation has ended
        if (e_end != null) e_end();

        //Clean all actions
        cleanAllActions();
    }
}
