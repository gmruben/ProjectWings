using UnityEngine;
using System;
using System.Collections;

public class SceneAnimation : MonoBehaviour
{
    public Action e_animationEnd;

    public void playAnimation()
    {
        animation.Play();
    }

    public void playSpriteAnimation(string pAnimationName)
    {
        GetComponent<exSpriteAnimation>().Play(pAnimationName);
    }

	public void OnAnimationEnd()
    {
	    if (e_animationEnd != null) e_animationEnd();
	}
}
