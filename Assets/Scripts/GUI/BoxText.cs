using UnityEngine;
using System.Collections;

public class BoxText : MonoBehaviour
{
    private exSpriteFont m_text;
    private bool m_isActive = true;

    void Awake()
    {
        m_text = GetComponent<exSpriteFont>();
    }

    #region PROPERTIES

    public bool isActive
    {
        set
        {
            if (value)
            {
                m_text.topColor = Color.white;
                m_text.botColor = Color.white;
            }
            else
            {
                m_text.topColor = Color.grey;
                m_text.botColor = Color.grey;
            }

            m_isActive = value;
        }

        get
        {
            return m_isActive;
        }
    }

    public string text
    {
        set
        {
            m_text.text = value;
        }

        get
        {
            return m_text.text;
        }
    }

    #endregion
}
