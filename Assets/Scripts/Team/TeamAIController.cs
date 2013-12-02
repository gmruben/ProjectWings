using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamAIController : TeamController
{
    private TeamAI m_teamAI;

    public override void init(Game pGame, GameCamera pGameCamera, Board pBoard, Team pTeam)
    {
        m_game = pGame;
        m_gameCamera = pGameCamera;
        m_board = pBoard;
        m_team = pTeam;

        //m_teamAI.init();
    }

    public override void startTurn()
    {
        List<Vector2> list = new List<Vector2>();
        list.Add(new Vector2(0, 0));
        list.Add(new Vector2(1, 0));
        list.Add(new Vector2(1, 1));
        m_team.m_playerList[5].move(list);
    }
}
