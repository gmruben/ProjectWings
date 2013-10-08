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
    public const string Idle = "idle";
    public const string Run = "run";
    public const string Shoot = "shoot";
    public const string Catch = "catch";
    public const string Hurt = "hurt";
    public const string Jump = "jump";
    public const string Pass = "pass";
    public const string Tackle = "tackle";
}