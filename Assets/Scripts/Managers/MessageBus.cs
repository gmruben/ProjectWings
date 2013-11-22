using System;

public class MessageBus
{
    public Action TackleBattleStart;
    public Action PlayerMoveEnded;

    public Action CurrentSceneEnded;

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

    public void cleanAllActions()
    {
        TackleBattleStart = null;
        PlayerMoveEnded = null;

        CurrentSceneEnded = null;
    }
}