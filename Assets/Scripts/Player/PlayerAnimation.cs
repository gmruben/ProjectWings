using UnityEngine;
using System;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    private exSpriteAnimation m_animation;

    //EVENTS
    public event Action animationFinished;

    public void init()
    {
        m_animation = GetComponent<exSpriteAnimation>();
	}

    public void playAnimation(string pAnimationName)
    {
        m_animation.Play(pAnimationName);
    }

    private void onAnimationFinished()
    {
        if (animationFinished != null)
        {
            animationFinished();
        }
    }

    public void addAnimation(exSpriteAnimClip animation)
    {
        m_animation.animations.Add(animation);
    }
}

public class PlayerAnimationIds
{
    public const string IDLE = "idle";
    public const string RUN = "run";
    public const string SHOOT = "shoot";
    public const string CATCH = "catch";
    public const string HURT = "hurt";
    public const string JUMP = "jump";
    public const string PASS = "pass";
    public const string TACKLE = "tackle";
}