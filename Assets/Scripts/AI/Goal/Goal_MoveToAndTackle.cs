using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal_MoveToAndTackle : GoalComposite
{
    private Player m_player;
    private List<Vector2> m_squareList;

    public Goal_MoveToAndTackle(Player pPlayer, List<Vector2> pSquareList, Player pOpponent)
    {
        //Make sure the subgoal list is clear
        removeAllSubgoals();

        m_player = pPlayer;
        m_squareList = pSquareList;
    }

    public override void activate()
    {
        m_status = GoalStatus.active;

        addSubgoal(new Goal_MoveTo(m_player, m_squareList));
    }

    public override int process()
    {
        return m_status;
    }

    public override void terminate()
    {

    }

}
