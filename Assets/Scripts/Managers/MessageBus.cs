using UnityEngine;
using System;

public class MessageBus
{
    public Action TackleBattleStart;

    public Action CurrentSceneEnded;

    public Action<Player> PlayerMovedToTile;
    public Action<Player> PlayerMoveEnded;
    public Action<Player> PlayerTackleEnded;

    public Action<Ball> BallMovedToTile;
    public Action<Ball> BallPassEnded;

    public Action<Player> PlayerTurnEnded;
    public Action<Team> UserPhaseEnded;

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

    public void dispatchPlayerTackleEnded(Player pPlayer)
    {
        if (PlayerTackleEnded != null) PlayerTackleEnded(pPlayer);
    }

    public void dispatchBallMovedToTile(Ball pBall)
    {
        if (BallMovedToTile != null) BallMovedToTile(pBall);
    }

    public void dispatchBallPassEnded(Ball pBall)
    {
        if (BallPassEnded != null) BallPassEnded(pBall);
    }

    public void dispatchPlayerTurnEnded(Player pPlayer)
    {
        if (PlayerTurnEnded != null) PlayerTurnEnded(pPlayer);
    }

    public void dispatchUserPhaseEnded(Team pTeam)
    {
        if (UserPhaseEnded != null) UserPhaseEnded(pTeam);
    }

    public void cleanAllActions()
    {
        TackleBattleStart = null;
        PlayerMoveEnded = null;

        CurrentSceneEnded = null;

        PlayerMovedToTile = null;
        BallMovedToTile = null;
        BallPassEnded = null;

        PlayerTurnEnded = null;
        UserPhaseEnded = null;
    }
}