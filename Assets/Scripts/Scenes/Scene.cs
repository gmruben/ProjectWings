using UnityEngine;
using System;
using System.Collections;

public class Scene : MonoBehaviour
{
    public Action e_end;

    public virtual void play() { }

    protected void cleanAllActions()
    {
        e_end = null;

        Destroy(gameObject);
    }
}