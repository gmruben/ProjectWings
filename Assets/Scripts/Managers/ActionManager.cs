using UnityEngine;
using System.Collections;

/// <summary>
/// This class calculates the outcome of any action in the game
/// </summary>
public class ActionManager : MonoBehaviour
{
    /// <summary>
    /// Calculates if a player can dribble
    /// </summary>
    /// <param name="pPlayer1">The player who is dribbling</param>
    /// <param name="pPlayer2">The player who is tackling</param>
    /// <returns>Returns true if player 1 can dribble player 2</returns>
    public static bool canDribble(Player pPlayer1, Player pPlayer2)
    {
        float dribbleValue = pPlayer1.m_stats.m_tackle * Random.Range(0.5f, 1.5f);
        float tackleValue = pPlayer1.m_stats.m_tackle * Random.Range(0.5f, 1.5f);
        
        return dribbleValue > tackleValue;
    }
}
