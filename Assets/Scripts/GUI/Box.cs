using UnityEngine;
using System.Collections;
using System;

public class Box : MonoBehaviour
{
    private const float COUNTER_TIME = 0.10f;

    public event Action<int> optionSelectedEvent;
    public event Action closedEvent;

    /*public exSpriteFont m_option1;
    public exSpriteFont m_option2;
    public exSpriteFont m_option3;
    public exSpriteFont m_option4;*/

    public BoxText[] m_optionList;

    public Transform m_cursor;

    public int m_currentOptionIndex;

    private float counter;

    public void init(Player pPlayer)
    {
        counter = 0;

        m_currentOptionIndex = 0;

        m_optionList[0].text = "Move";
        m_optionList[0].isActive = !pPlayer.m_hasMoved;

        m_optionList[1].text = "Pass";
        m_optionList[1].isActive = !pPlayer.m_hasPerformedAction && pPlayer.m_ball != null;
        m_optionList[2].text = "Shoot";
        m_optionList[2].isActive = !pPlayer.m_hasPerformedAction && pPlayer.m_ball != null;

        m_optionList[3].text = "End";
        m_optionList[3].isActive = true;
    }

    void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            move((int)-Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetKeyDown(KeyCode.Z) && m_optionList[m_currentOptionIndex].isActive)
        {
            if (optionSelectedEvent != null)
            {
                optionSelectedEvent(m_currentOptionIndex);
            }
        }
    }

    private void move(int direction)
    {
        if (direction != 0)
        {
            m_currentOptionIndex += direction;
            m_currentOptionIndex = Mathf.Max(m_currentOptionIndex, 0);
            m_currentOptionIndex = Mathf.Min(m_currentOptionIndex, 3);

            m_cursor.localPosition = new Vector3(m_cursor.localPosition.x, 3 - m_currentOptionIndex * 2, m_cursor.localPosition.z);

            counter = COUNTER_TIME;
        }
    }
}
