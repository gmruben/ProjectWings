using UnityEngine;
using System;

public class MessageBus
{
    public Action TackleBattleStart;
    public Action PlayerMoveEnded;

    public Action CurrentSceneEnded;

    public Action<Player> PlayerMovedToTile;
    public Action<Ball> BallMovedToTile;

    public void dispatchTackleBattleStart()
    {
        if (TackleBattleStart != null) TackleBattleStart();
    }

    public void dispatchPlayerMoveEnded()
    {
        if (PlayerMoveEnded != null) PlayerMoveEnded();
    }

    public void dispatchCurrentSceneEnded()
    {
        if (CurrentSceneEnded != null) CurrentSceneEnded();
    }

    public void dispatchPlayerMovedToTile(Player pPlayer)
    {
        if (PlayerMovedToTile != null) PlayerMovedToTile(pPlayer);
    }

    public void dispatchBallMovedToTile(Ball pBall)
    {
        if (BallMovedToTile != null) BallMovedToTile(pBall);
    }

    public void cleanAllActions()
    {
        TackleBattleStart = null;
        PlayerMoveEnded = null;

        CurrentSceneEnded = null;

        PlayerMovedToTile = null;
        BallMovedToTile = null;
    }
}