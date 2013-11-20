using UnityEngine;
using System.Collections;

public class FX02 : MonoBehaviour
{
    public Animation m_animation;
    public AnimationHandler m_animationHandler;

    public void init()
    {
        m_animation.Play("FX02");
        m_animationHandler.e_animationEnd += animationEnd;
    }

    private void animationEnd()
    {
        Destroy(gameObject);
    }
}
