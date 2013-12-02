using UnityEngine;
using System.Collections;

public class TeamController
{
    protected Game m_game;
    protected GameCamera m_gameCamera;
    protected Board m_board;
    protected Team m_team;

    public virtual void init(Game pGame, GameCamera pGameCamera, Board pBoard, Team pTeam) { }
    public virtual void startTurn() { }
}
