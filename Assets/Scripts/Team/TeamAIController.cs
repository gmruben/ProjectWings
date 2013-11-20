using UnityEngine;
using System.Collections;

public class TeamAIController : TeamController
{
    private TeamAI m_teamAI;

    public override void init(Board pBoard, Team pTeam)
    {
        m_teamAI.init();
    }

    public override void startPhase()
    {
        
    }
}
