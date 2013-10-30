using UnityEngine;
using System.Collections;
using System;

public class PlayerMenu : MonoBehaviour
{
    public Action<int> e_selected;
    public Action e_cancel;

    public BoxText[] m_optionList;
    public int[] m_actionList;

    private int m_currentOptionIndex;
    private int m_numOptions;

    YesNoMenu m_yesNoMenu;

    private bool m_hasFinishedAnimation;

    public void init(Player pPlayer)
    {
        transform.position = new Vector3(-(80 + (32 / 2)), -25, 0);

        m_hasFinishedAnimation = false;

        m_currentOptionIndex = 0;

        m_actionList = new int[4];

        m_actionList[0] = PlayerAction.Move;
        m_optionList[0].text = "Move";
        m_optionList[0].isActive = !pPlayer.m_hasMoved;

        if (pPlayer.hasBall)
        {
            m_actionList[1] = PlayerAction.Pass;
            m_optionList[1].text = "Pass";
            m_optionList[1].isActive = !pPlayer.m_hasPerformedAction;
            m_actionList[2] = PlayerAction.Shoot;
            m_optionList[2].text = "Shoot";
            m_optionList[2].isActive = !pPlayer.m_hasPerformedAction;
        }
        else
        {
            m_actionList[1] = PlayerAction.Tackle;
            m_optionList[1].text = "Tackle";
            m_optionList[1].isActive = !pPlayer.m_hasPerformedAction;
            m_optionList[2].text = "";
            m_optionList[2].isActive = false;
        }

        m_actionList[3] = PlayerAction.EndTurn;
        m_optionList[3].text = "End";
        m_optionList[3].isActive = true;

        //Highlight the first active option
        while(!m_optionList[m_currentOptionIndex].isActive)
        {
            m_currentOptionIndex++;
        }
        m_optionList[m_currentOptionIndex].isHighlighted = true;
    }

    public void setGKData()
    {
        m_currentOptionIndex = 0;
        m_numOptions = 2;

        m_actionList = new int[m_numOptions];

        m_actionList[0] = PlayerAction.Punch;
        m_optionList[0].text = "Punch";
        m_optionList[0].isActive = true;

        m_actionList[1] = PlayerAction.Catch;
        m_optionList[1].text = "Catch";
        m_optionList[1].isActive = true;

        m_optionList[2].enabled = false;
        m_optionList[3].enabled = false;
    }

    void Update()
    {
        if (!m_hasFinishedAnimation)
        {
            transform.position += new Vector3(Time.deltaTime * 150, 0, 0);
            if (transform.position.x > (32 / 2) - 82)
            {
                transform.position = new Vector3((32 / 2) - 82, -25, 0);
                m_hasFinishedAnimation = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                move(1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                move(-1);
            }

            if (Input.GetKeyDown(KeyCode.Z) && m_optionList[m_currentOptionIndex].isActive)
            {
                if (e_selected != null) e_selected(m_actionList[m_currentOptionIndex]);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (e_cancel != null) e_cancel();
            }
        }
    }

    private void move(int direction)
    {
        int index = m_currentOptionIndex;

        m_currentOptionIndex += direction;
        m_currentOptionIndex = Mathf.Clamp(m_currentOptionIndex, 0, 3);

        while (!m_optionList[m_currentOptionIndex].isActive)
        {
            m_currentOptionIndex += direction;

            if (m_currentOptionIndex < 0 || m_currentOptionIndex > 3)
            {
                m_currentOptionIndex = index;
                break;
            }
        }

        m_optionList[index].isHighlighted = false;
        m_optionList[m_currentOptionIndex].isHighlighted = true;
    }
}

public class PlayerAction
{
    public const int Move = 0;
    public const int Pass = 1;
    public const int Shoot = 2;
    public const int Tackle = 3;
    public const int EndTurn = 4;
    public const int Punch = 5;
    public const int Catch = 6;
}