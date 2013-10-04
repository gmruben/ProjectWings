using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAI : MonoBehaviour
{
    //Position of the player
    Vector2 m_position;
    Player m_player;

    //List of goals
    private List<Goal> m_goalList = new List<Goal>();

    /// <summary>
    /// Adds a goal to the list
    /// </summary>
    /// <param name="pGoal">Goal to be added</param>
    public void addGoal(Goal pGoal)
    {
        m_goalList.Add(pGoal);
    }

    SupportSpot m_pBestSupportingSpot;

    public Vector2 determineBestSupportingPosition()
    {
        //reset the best supporting spot
        m_pBestSupportingSpot = null;

        float BestScoreSoFar = 0.0f;

        List<SupportSpot> squareList = new List<SupportSpot>();

        foreach (SupportSpot square in squareList)
        {
            //first remove any previous score. (the score is set to one so that
            //the viewer can see the positions of all the spots if he has the
            //aids turned on)
            square.m_dScore = 1.0f;

            //Test 1. is it possible to make a safe pass from the ball's position to this position?
            if (BigAI.isPassSafeFromAllOpponents(BigAI.m_playerWithBallPosition, square.m_vPos))
            {
                square.m_dScore += Prm.Spot_PassSafeStrength;
            }

            //Test 2. Determine if a goal can be scored from this position.
            if (canShoot(square.m_vPos))
            {
                square.m_dScore += Prm.Spot_CanScoreStrength;
            }

            //Test 3. calculate how far this spot is away from the controlling
            //player. The farther away, the higher the score. Any distances farther
            //away than OptimalDistance pixels do not receive a score.
            float OptimalDistance = 200.0f;
            float dist = Vector2.Distance(m_position, square.m_vPos);
            float temp = Mathf.Abs(OptimalDistance - dist);
            if (temp < OptimalDistance)
            {
                //normalize the distance and add it to the score
                square.m_dScore += Prm.Spot_DistFromControllingPlayerStrength * (OptimalDistance - temp) / OptimalDistance;
            }

            //check to see if this spot has the highest score so far
            if (square.m_dScore > BestScoreSoFar)
            {
                BestScoreSoFar = square.m_dScore;
                m_pBestSupportingSpot = square;
            }
        }

        return m_pBestSupportingSpot.m_vPos;
    }

    public bool isPassSafeFromOpponent(Vector2 from, Vector2 target, Player receiver, Player opp, double PassingForce)
    {
        return true;
    }

    public bool canShoot(Vector2 pPos)
    {
        return true;
    }

    public bool canPass(Player pPlayer)
    {
        return true;
    }

    public bool canTackle(Player pPlayer, out float tackleScore)
    {
        tackleScore = 1;

        return true;
    }

    public bool findPass(Player passer, Player receiver, Vector2 PassTarget, float power, float MinPassingDistance)
    {
        List<Player> teamPlayerList = new List<Player>();

        Vector2 opponentGoalPosition = Vector2.zero;
        float ClosestToGoalSoFar = 999999999.0f;
        Vector2 BallTarget = Vector2.zero;

        //Iterate through all this player's team members and calculate which one is in a position to be passed the ball
        foreach (Player player in teamPlayerList)
        {
            //make sure the potential receiver being examined is not this player and that it’s farther away than the minimum pass distance
            if (player != passer && (Vector2.Distance(passer.Index, player.Index) > MinPassingDistance * MinPassingDistance))
            {
                if (canPass(receiver))
                {
                    //if the pass target is the closest to the opponent's goal line found so far, keep a record of it
                    float Dist2Goal = Mathf.Abs(BallTarget.y - opponentGoalPosition.y);
                    if (Dist2Goal < ClosestToGoalSoFar)
                    {
                        ClosestToGoalSoFar = Dist2Goal;
                        //keep a record of this player
                        receiver = player;
                    }
                }
            }
        }

        if (receiver)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the score of the player for tackling an opponent
    /// </summary>
    /// <param name="opponent">The opponent to be tackled</param>
    /// <returns>The score of the player</returns>
    public float getTacklePlayerScore(Player opponent)
    {
        //Initialize score
        float score = 0;

        //If the opponent is in reach
        if (distanceTo(m_player.Index, opponent.Index) > 6)
        {
            score = -1;
        }

        return score;
    }

    public int distanceTo(Vector2 pPlayer1Index, Vector2 pPlayerIndex)
    {
        return 1;
    }
}

public class SupportSpot
{
    public Vector2 m_vPos;
    public float m_dScore;

    SupportSpot(Vector2 pPos, float pVal)
    {
        m_vPos = pPos;
        m_dScore = pVal;
    }
}

public class Prm
{
    public static float Spot_PassSafeStrength = 1;
    public static float Spot_CanScoreStrength = 1;
    public static float Spot_DistFromControllingPlayerStrength = 1;
}

public class BigAI
{
    public static Vector2 m_playerWithBallPosition;

    public static bool isPassSafeFromAllOpponents(Vector2 pPlayerWithBallPosition, Vector2 pSupportingPlayerPosition)
    {
        return true;
    }
}