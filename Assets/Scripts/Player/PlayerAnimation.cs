using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimation : MonoBehaviour
{
    private exSpriteAnimation m_animation;
    private List<string> m_animationNameList;

    //EVENTS
    public event Action animationFinished;

    public void init()
    {
        m_animation = GetComponent<exSpriteAnimation>();
        m_animationNameList = new List<string>();

        //Add listeners
        ApplicationFactory.instance.m_messageBus.TackleBattleStart += tackleBattleStart;
	}

    public void playAnimation(string pAnimationName)
    {
        m_animationNameList.Clear();
        m_animation.Play(pAnimationName);
    }

    public void playAnimation(List<string> pAnimationNameList)
    {
        m_animationNameList = pAnimationNameList;
        m_animation.Play(m_animationNameList[0]);
        m_animationNameList.RemoveAt(0);
    }

    private void onAnimationFinished(string pAnimationName)
    {
        if (animationFinished != null)
        {
            animationFinished();
        }

        if (m_animationNameList.Count > 0)
        {
            playAnimation(m_animationNameList);
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