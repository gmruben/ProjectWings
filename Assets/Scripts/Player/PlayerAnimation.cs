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

        //Add listeners
        ApplicationFactory.instance.m_messageBus.TackleBattleStart += tackleBattleStart;
	}

    public void playAnimation(string pAnimationName)
    {
        m_animation.Play(pAnimationName);
    }

    private void onAnimationFinished(string pAnimationName)
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

    private void tackleBattleStart()
    {
        m_animation.Pause();
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