using UnityEngine;
using System.Collections;

public class TeamController
{
    protected Board m_board;
    protected Team m_team;

    public virtual void init(Board pBoard, Team pTeam) { }
    public virtual void startPhase() { }
}
