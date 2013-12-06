using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    public Game m_game;
    public GameObject[] m_balls;

    private int numTurns;

    void Start()
    {
        numTurns = 5;

        //Add listeners
        ApplicationFactory.instance.m_messageBus.PlayerTurnEnded += endTurn;
        ApplicationFactory.instance.m_messageBus.UserPhaseEnded += startPhase;
    }

    private void startPhase(Team pTeam)
    {
        for (int i = 0; i < numTurns; i++)
        {
            m_balls[i].active = true;
        }
    }

    private void endTurn(int pTurnIndex)
    {
        for (int i = 0; i < numTurns; i++)
        {
            m_balls[i].active = (i <= (numTurns - pTurnIndex));
        }
    }
}