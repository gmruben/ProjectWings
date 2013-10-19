using UnityEngine;
using System;
using System.Collections;

public abstract class Scene : MonoBehaviour
{
    public Action e_end;

    public abstract void play();
}
