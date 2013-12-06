using UnityEngine;
using System.Collections;
using System;

public class PlayerMenu : MonoBehaviour
{
    public Action<int> e_selected;
    public Action e_cancel;

    public BoxText[] m_optionList;

    [HideInInspector]
    public int[] m_actionList;

    private int m_currentOptionIndex;
    private int m_numOptions;

    YesNoMenu m_yesNoMenu;

    private bool m_hasFinishedAnimation;

    public void init(Player pPlayer)
    {
        transform.position = new Vector3(-(80 + (32 / 2)), 0, 0);

        m_hasFinishedAnimation = false;

        m_currentOptionIndex = 0;

        m_actionList = new int[4];

        m_actionList[0] = PlayerAction.Move;
        m_optionList[0].text = ApplicationFactory.instance.m_languageManager.getString("Move");
        m_optionList[0].isActive = !pPlayer.m_hasMoved;

        if (pPlayer.hasBall)
        {
            m_actionList[1] = PlayerAction.Dribble;
            m_optionList[1].text = ApplicationFactory.instance.m_languageManager.getString("Dribble");
            m_optionList[1].isActive = pPlayer.hasBall && !pPlayer.m_hasPerformedAction;
        }
        else
        {
            m_actionList[1] = PlayerAction.Tackle;
            m_optionList[1].text = ApplicationFactory.instance.m_languageManager.getString("Tackle");
            m_optionList[1].isActive = (pPlayer.team.m_playerWithTheBall == null);
        }

        m_actionList[2] = PlayerAction.Pass;
        m_optionList[2].text = ApplicationFactory.instance.m_languageManager.getString("Pass");
        m_optionList[2].isActive = pPlayer.hasBall && !pPlayer.m_hasPerformedAction;

        m_actionList[3] = PlayerAction.Shoot;
        m_optionList[3].text = ApplicationFactory.instance.m_languageManager.getString("Shoot");
        m_optionList[3].isActive = pPlayer.hasBall && !pPlayer.m_hasPerformedAction;

        //Highlight the first active option
        while (!m_optionList[m_currentOptionIndex].isActive)
        {
            m_currentOptionIndex++;
        }
        m_optionList[m_currentOptionIndex].isHighlighted = true;
    }

    void Update()
    {
        if (!m_hasFinishedAnimation)
        {
            transform.position += new Vector3(Time.deltaTime * 150, 0, 0);
            if (transform.position.x > (32 / 2) - 82)
            {
                transform.position = new Vector3((32 / 2) - 82, 0, 0);
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
    public const int Dribble = 1;
    public const int Tackle = 2;
    public const int Pass = 3;
    public const int Shoot = 4;
}