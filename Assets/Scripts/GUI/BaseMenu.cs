using UnityEngine;
using System.Collections;

public class BaseMenu : MonoBehaviour
{
    private Color enabledColor = new Color(1.0f, 1.0f, 1.0f);
    private Color disabledColor = new Color(0.8f, 0.8f, 0.8f);
    private Color highlightedColor = new Color(1.0f, 1.0f, 0.0f);

    private bool m_hasFinishedAnimation;

    public BoxText[] m_optionList;
    public int[] m_actionList;

    private int m_currentOptionIndex;
    private int m_numOptions;

    void Update()
    {
        if (!m_hasFinishedAnimation)
        {
            transform.position += new Vector3(Time.deltaTime * 150, -25, 0);
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

            /*if (Input.GetKeyDown(KeyCode.Z) && m_optionList[m_currentOptionIndex].isActive)
            {
                if (e_selected != null) e_selected(m_actionList[m_currentOptionIndex]);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (e_cancel != null) e_cancel();
            }*/
        }
    }

    private void move(int direction)
    {
        if (direction != 0)
        {
            m_optionList[m_currentOptionIndex].isHighlighted = false;

            m_currentOptionIndex += direction;
            m_currentOptionIndex = Mathf.Clamp(m_currentOptionIndex, 0, 3);

            m_optionList[m_currentOptionIndex].isHighlighted = true;
        }
    }
}
