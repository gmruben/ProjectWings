using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
    //List of all the player in the team
    private List<Player> m_playerList;

    //Player with the ball
    private Player m_playerWithTheBall;
    
    //Opponent team
    private Team m_opponentTeam;

    #region PROPERTIES

    public Team opponentTeam
    {
        get { return m_opponentTeam; }
    }

    public Player playerWithTheBall
    {
        get { return m_playerWithTheBall; }
    }

    #endregion
}
