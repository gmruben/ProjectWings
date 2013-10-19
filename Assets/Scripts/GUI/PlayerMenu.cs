using UnityEngine;
using System.Collections;
using System;

public class PlayerMenu : MonoBehaviour
{
    private const float COUNTER_TIME = 0.10f;

    public Action<int> e_selected;
    public Action e_cancel;

    public BoxText[] m_optionList;
    public int[] m_actionList;

    public Transform m_cursor;

    private int m_currentOptionIndex;
    private int m_numOptions;

    private float counter;

    YesNoMenu m_yesNoMenu;

    public void init(Player pPlayer)
    {
        counter = 0;

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
        m_optionList[3].text = "End Turn";
        m_optionList[3].isActive = true;
    }

    public void setGKData()
    {
        counter = 0;

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
            if (e_selected != null) e_selected(m_actionList[m_currentOptionIndex]);

            /*m_yesNoMenu = GUIManager.instance.showYesNoMenu();
            
            //Add listeners
            m_yesNoMenu.e_yes += confirm;
            m_yesNoMenu.e_no += cancel;
            m_yesNoMenu.e_cancel += cancel;*/
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (e_cancel != null) e_cancel();
        }
    }

    private void move(int direction)
    {
        if (direction != 0)
        {
            m_currentOptionIndex += direction;
            m_currentOptionIndex = Mathf.Clamp(m_currentOptionIndex, 0, 3);

            m_cursor.localPosition = new Vector3(m_cursor.localPosition.x, 3 - m_currentOptionIndex * 2, m_cursor.localPosition.z);

            counter = COUNTER_TIME;
        }
    }

    /*private void confirm()
    {
        //Remove listeners
        m_yesNoMenu.e_yes -= confirm;
        m_yesNoMenu.e_no -= cancel;
        m_yesNoMenu.e_cancel -= cancel;

        Destroy(m_yesNoMenu.gameObject);
        m_yesNoMenu = null;

        if (e_selected != null) e_selected(m_actionList[m_currentOptionIndex]);
    }

    private void cancel()
    {
        //Remove listeners
        m_yesNoMenu.e_yes -= confirm;
        m_yesNoMenu.e_no -= cancel;
        m_yesNoMenu.e_cancel -= cancel;

        Destroy(m_yesNoMenu.gameObject);
        m_yesNoMenu = null;
    }*/
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