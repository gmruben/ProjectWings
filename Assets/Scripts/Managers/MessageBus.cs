using System;

public class MessageBus
{
    public Action QuitGame;
    public Action StartBackswing;
    public Action ReadyForShot;

    public Action PlayerMoveEnded;

    public void dispatchPlayerMoveEnded()
    {
        if (PlayerMoveEnded != null) PlayerMoveEnded();
    }

    public void cleanAllActions()
    {
        QuitGame = null;
        StartBackswing = null;
        ReadyForShot = null;
    }
}