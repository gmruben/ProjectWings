using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    public Game m_game;

    public GameObject[] m_balls;

    void Start()
    {
        //Add listeners
        m_game.e_endTurn += endTurn;
        m_game.e_startPhase += startPhase;
    }

    private void startPhase(int pPhase)
    {
        for (int i = 0; i < m_balls.Length; i++)
        {
            m_balls[i].active = true;
        }
    }

    private void endTurn(int pTurnsLeft)
    {
        for (int i = 0; i < m_balls.Length; i++)
        {
            m_balls[i].active = i < pTurnsLeft;
        }
    }
}
