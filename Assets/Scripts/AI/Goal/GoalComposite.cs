using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalComposite : Goal
{
    private List<Goal> m_subGoalList;

    /// <summary>
    /// This will execute when the goal is activated
    /// </summary>
    public override void activate()
    {

    }

    /// <summary>
    /// This is called by the agent update function each update step
    /// </summary>
    public override int process()
    {
        m_status = processSubgoals();

        return m_status;
    }

    /// <summary>
    /// This will execute when the goal is terminated is exited
    /// </summary>
    public override void terminate()
    {

    }

    public int processSubgoals()
    {
        //remove all completed and failed goals from the front of the subgoal list
        while (m_subGoalList.Count > 0) // && (m_subGoalList[0].isComplete() || (m_subGoalList[0].hasFailed()))
        {
            m_subGoalList[0].terminate();
            m_subGoalList.RemoveAt(0);
        }

        //If any subgoals remain, process the one at the front of the list
        if (m_subGoalList.Count > 0)
        {
            //Grab the status of the frontmost subgoal
            int statusOfSubGoals = m_subGoalList[0].process();

            //we have to test for the special case where the frontmost subgoal
            //reports "completed" and the subgoal list contains additional goals.
            //When this is the case, to ensure the parent keeps processing its
            //subgoal list,the "active" status is returned
            if (statusOfSubGoals == GoalStatus.completed && m_subGoalList.Count > 1)
            {
                return GoalStatus.active;
            }

            return statusOfSubGoals;
        }
        else
        {
            return GoalStatus.completed;
        }
    }

    public void addSubgoal(Goal pSubGoal)
    {
        m_subGoalList.Add(pSubGoal);
    }

    public void removeAllSubgoals()
    {
        for (int i = 0; i < m_subGoalList.Count; i++)
        {
            m_subGoalList[i].terminate();
            m_subGoalList.RemoveAt(0);
            m_subGoalList.Clear();
        }
    }
}
