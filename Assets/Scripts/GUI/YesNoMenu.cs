using UnityEngine;
using System.Collections;
using System;

public class YesNoMenu : MonoBehaviour
{
    private const float COUNTER_TIME = 0.10f;

    public Action e_yes;
    public Action e_no;
    public Action e_cancel;

    public BoxText[] m_optionList;

    public Transform m_cursor;

    public int m_currentOptionIndex;

    private float counter;

    public void init()
    {
        counter = 0;

        m_currentOptionIndex = 0;

        m_optionList[0].text = "Yes";
        m_optionList[0].isActive = true;

        m_optionList[1].text = "No";
        m_optionList[1].isActive = true;
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_currentOptionIndex == 0 && e_yes != null) e_yes();
            if (m_currentOptionIndex == 1 && e_no != null) e_no();
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
            m_currentOptionIndex = Mathf.Clamp(m_currentOptionIndex, 0, 1);

            m_cursor.localPosition = new Vector3(m_cursor.localPosition.x, 3 - m_currentOptionIndex * 2, m_cursor.localPosition.z);

            counter = COUNTER_TIME;
        }
    }
}