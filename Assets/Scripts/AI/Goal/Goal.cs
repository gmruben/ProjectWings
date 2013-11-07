using UnityEngine;
using System.Collections;

public abstract class Goal
{
    //protected int m_player;
    protected int m_status;

    /// <summary>
    /// This will execute when the goal is activated
    /// </summary>
    public abstract void activate();

    /// <summary>
    /// This is called by the agent update function each update step
    /// </summary>
    public abstract int process();

    /// <summary>
    /// This will execute when the goal is terminated is exited
    /// </summary>
    public abstract void terminate();
}

public class GoalStatus
{
    public static int inactive = 0;
    public static int active = 1;
    public static int completed = 2;
    public static int failed = 3;
}