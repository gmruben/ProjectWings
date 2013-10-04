using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal_MoveTo : Goal
{
    private Player m_player;
    private List<Vector2> m_squareList;

    public Goal_MoveTo(Player pPlayer, List<Vector2> pSquareList)
    {
        m_player = pPlayer;
        m_squareList = pSquareList;
    }

    public override void activate()
    {
        m_status = GoalStatus.active;

        m_player.move(m_squareList);
    }

    public override int process()
    {
        return m_status;
    }

    public override void terminate()
    {

    }

}
