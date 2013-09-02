using UnityEngine;
using System.Collections;

public class BallAnimation : MonoBehaviour
{
    private exSpriteAnimation m_animation;

    public void init()
    {
        m_animation = GetComponent<exSpriteAnimation>();
	}

    public void playAnimation(string pAnimationName)
    {
        m_animation.Play(pAnimationName);
    }
}

public class BallAnimationIds
{
    public const string IDLE = "ball_idle";
    public const string SHOOT = "ball_shoot";
}