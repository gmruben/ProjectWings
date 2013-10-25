using System;

public class MessageBus
{
    public Action QuitGame;
    public Action StartBackswing;
    public Action ReadyForShot;

    public void cleanAllActions()
    {
        QuitGame = null;
        StartBackswing = null;
        ReadyForShot = null;
    }
}