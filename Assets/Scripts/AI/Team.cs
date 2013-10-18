using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
    //Team GK
    private Player m_gk;
    //List of all the player in the team
    private List<Player> m_playerList;

    //Player with the ball
    private Player m_playerWithTheBall;

    //Opponent team
    private Team m_opponentTeam;

    public void tacklePlayer(Player opponent)
    {
        float bestScore = 0;
        Player bestPlayer = null;

        //Select who is the best player to tackle the opponent
        for (int i = 0; i < m_playerList.Count; i++)
        {
            //Get the score for the current player
            float score = m_playerList[i].m_AI.getTacklePlayerScore(opponent);
            //If it is better than the best
            if (score >= bestScore)
            {
                bestScore = score;
                bestPlayer = m_playerList[i];
            }
        }

        //Set the goal for the player
        //bestPlayer.m_AI.addGoal(new Goal_MoveToAndTackle(bestPlayer, pSquareList, opponent);
    }

    #region PROPERTIES

    public Team opponentTeam
    {
        get { return m_opponentTeam; }
    }

    public Player playerWithTheBall
    {
        get { return m_playerWithTheBall; }
    }

    public Player GK
    {
        get { return m_gk; }
    }

    #endregion
}
