using System;

public class MessageBus
{
    public Action TackleBattleStart;
    public Action PlayerMoveEnded;

    public void dispatchTackleBattleStart()
    {
        if (TackleBattleStart != null) TackleBattleStart();
    }

    public void dispatchPlayerMoveEnded()
    {
        if (PlayerMoveEnded != null) PlayerMoveEnded();
    }

    public void cleanAllActions()
    {
        TackleBattleStart = null;
        PlayerMoveEnded = null;
    }
}