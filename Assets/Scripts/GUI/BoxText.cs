using UnityEngine;
using System.Collections;

public class BoxText : MonoBehaviour
{
    private Color activeColor = new Color(1.0f, 1.0f, 1.0f);
    private Color inactiveColor = new Color(0.8f, 0.8f, 0.8f);
    private Color highlightedColor = new Color(1.0f, 1.0f, 0.0f);

    private exSpriteFont m_text;
    
    private bool m_isActive = true;
    private bool m_isHighlighted= false;

    void Awake()
    {
        m_text = GetComponent<exSpriteFont>();
    }

    #region PROPERTIES

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

    public bool isActive
    {
        set
        {
            if (value)
            {
                m_text.topColor = activeColor;
                m_text.botColor = activeColor;
            }
            else
            {
                m_text.topColor = inactiveColor;
                m_text.botColor = inactiveColor;
            }

            m_isActive = value;
        }

        get
        {
            return m_isActive;
        }
    }

    public bool isHighlighted
    {
        set
        {
            if (value)
            {
                m_text.topColor = highlightedColor;
                m_text.botColor = highlightedColor;
            }
            else
            {
                if (m_isHighlighted)
                {
                    m_text.topColor = activeColor;
                    m_text.botColor = activeColor;
                }
                else
                {
                    m_text.topColor = inactiveColor;
                    m_text.botColor = inactiveColor;
                }
            }

            m_isHighlighted = value;
        }

        get
        {
            return m_isHighlighted;
        }
    }

    #endregion
}
