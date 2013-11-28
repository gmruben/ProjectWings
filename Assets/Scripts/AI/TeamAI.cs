using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamAI : MonoBehaviour
{
    //List of all the player in the team
    List<Player> m_playerList;

    Team opponentTeam;

    public void init()
    {
        //Initialize player list
        m_playerList = new List<Player>();
    }

    public void startTurn()
    {
        //TackleInfo info = calculateBestPlayerToTackle(opponentTeam.playerWithTheBall);

        PathFinder pathFinder = new PathFinder();
        //List<Vector2> moveList = pathFinder.findPath(info.m_positionToTackleFrom, info.m_positionToTackleTo);
        //info.m_player.move(moveList);
    }

    /// <summary>
    /// Calculates who is the best player to tackle the opponent player with the ball
    /// </summary>
    /*public TackleInfo calculateBestPlayerToTackle(Player opponent)
    {
        //Store best score and player
        float bestScore = 0;
        Player bestPlayer = null;

        //Iterate player list
        foreach (Player player in m_playerList)
        {
            float tackleScore;
            //If the current player can tackle the opponent
            if (player.m_AI.canTackle(opponent, out tackleScore))
            {
                if (tackleScore > bestScore)
                {
                    bestPlayer = player;
                }
            }
        }

        return new TackleInfo(bestPlayer, bestPlayer.Index, opponent.Index);
    }*/
}

/// <summary>
/// Stores the information needed to perform a tackle
/// </summary>
/*public class TackleInfo
{
    public Player m_player;
    public Vector2 m_positionToTackleFrom;
    public Vector2 m_positionToTackleTo;

    public TackleInfo(Player pPlayer, Vector2 pFrom, Vector2 pTo)
    {
        m_player = pPlayer;
        m_positionToTackleFrom = pFrom;
        m_positionToTackleTo = pTo;
    }
}*/