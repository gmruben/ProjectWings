using UnityEngine;
using System;
using System.Collections;

public class SceneAnimation : MonoBehaviour
{
    public Action e_animationEnd;

    private Animation m_animation;
    private exSpriteAnimation m_spriteAnimation;

    public void init()
    {
        //Cache components
        m_animation = animation;
        m_spriteAnimation = GetComponent<exSpriteAnimation>();
    }

    /// <summary>
    /// Plays a Unity Animation
    /// </summary>
    /// <param name="pAnimationName">Animation Name</param>
    public void playAnimation(string pAnimationName)
    {
        animation.Play(pAnimationName);
    }

    /// <summary>
    /// Plays an EX Sprite Animation
    /// </summary>
    /// <param name="pAnimationName">Animation Name</param>
    public void playSpriteAnimation(string pAnimationName)
    {
        GetComponent<exSpriteAnimation>().Play(pAnimationName);
    }

	public void OnAnimationEnd()
    {
	    if (e_animationEnd != null) e_animationEnd();
	}
}
