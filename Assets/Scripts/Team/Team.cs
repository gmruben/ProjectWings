using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team
{
    //Team GK
    private Player m_gk;
    //List of all the player in the team
    private List<Player> m_playerList;

    //Player with the ball
    private Player m_playerWithTheBall;

    //Opponent team
    public Team m_opponentTeam;
    private string m_ID;
    public int m_user;

    public Team()
    {
        m_playerList = new List<Player>();
    }

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

    public void addPlayer(Player pPlayer)
    {
        m_playerList.Add(pPlayer);

        //Check if the player is GK
        if (pPlayer.isGK)
        {
            m_gk = pPlayer;
        }

        //Check if the player has the ball
        if (pPlayer.hasBall)
        {
            m_playerWithTheBall = pPlayer;
        }
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

    public string ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    #endregion
}

//TEAM COLORS
//Color1Light = 235, 210, 0
//Color1Shadow = 120, 120, 15
//Color2Light = 50, 50, 50
//Color2Shadow = 255, 255, 255