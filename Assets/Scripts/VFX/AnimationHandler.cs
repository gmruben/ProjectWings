using UnityEngine;
using System;
using System.Collections;

public class AnimationHandler : MonoBehaviour
{
    public Action e_animationEnd;

    private void OnAnimationEnd()
    {
        if (e_animationEnd != null) e_animationEnd();
    }
}
