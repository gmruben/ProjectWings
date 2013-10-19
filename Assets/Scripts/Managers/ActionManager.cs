using UnityEngine;
using System.Collections;

/// <summary>
/// This class calculates the outcome of any action in the game
/// </summary>
public class ActionManager : MonoBehaviour
{
    /// <summary>
    /// Calculates if a player can dogde a tackle
    /// </summary>
    /// <param name="pPlayer1">The player who is dodging the tackle</param>
    /// <param name="pPlayer2">The player who is performing the tackle</param>
    /// <returns>Returns true if player 1 can dodge player 2</returns>
    public static bool hasDogdedTackle(Player pPlayer1, Player pPlayer2)
    {
        return true;
    }
}
