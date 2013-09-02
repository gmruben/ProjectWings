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
}

public class PlayerAnimationIds
{
    public const string IDLE = "idle";
    public const string RUN = "run";
    public const string SHOOT = "shoot";
    public const string CATCH = "catch";
}