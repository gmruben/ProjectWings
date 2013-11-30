using UnityEngine;
using System;

public class MessageBus
{
    public Action TackleBattleStart;

    public Action CurrentSceneEnded;

    public Action<Player> PlayerMovedToTile;
    public Action<Player> PlayerMoveEnded;

    public Action<Ball> BallMovedToTile;
    public Action<Ball> BallPassEnded;

    public void dispatchTackleBattleStart()
    {
        if (TackleBattleStart != null) TackleBattleStart();
    }

    public void dispatchCurrentSceneEnded()
    {
        if (CurrentSceneEnded != null) CurrentSceneEnded();
    }

    public void dispatchPlayerMovedToTile(Player pPlayer)
    {
        if (PlayerMovedToTile != null) PlayerMovedToTile(pPlayer);
    }

    public void dispatchPlayerMoveEnded(Player pPlayer)
    {
        if (PlayerMoveEnded != null) PlayerMoveEnded(pPlayer);
    }

    public void dispatchBallMovedToTile(Ball pBall)
    {
        if (BallMovedToTile != null) BallMovedToTile(pBall);
    }

    public void dispatchBallPassEnded(Ball pBall)
    {
        if (BallPassEnded != null) BallPassEnded(pBall);
    }

    public void cleanAllActions()
    {
        TackleBattleStart = null;
        PlayerMoveEnded = null;

        CurrentSceneEnded = null;

        PlayerMovedToTile = null;
        BallMovedToTile = null;
        BallPassEnded = null;
    }
}