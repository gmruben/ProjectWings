using UnityEngine;
using System;
using System.Collections;

public class Cursor : MonoBehaviour
{
    private const float COUNTER_TIME = 0.10f;

    private GameObject m_cachedGameObject;

    private Game m_game;
    private Board board;
    private Vector2 m_index;

    private float counter;

    //EVENTS
    public event Action<Vector2> e_move;
    public event Action<Vector2> e_end;
    public event Action e_cancel;

    public void init()
    {
        m_cachedGameObject = gameObject;

        m_index = Vector2.zero;
        counter = 0;

        setActive(false);
    }

    public void setActive(bool pIsActive)
    {
        gameObject.SetActiveRecursively(pIsActive);
    }

    public void setIndex(Vector2 pIndex)
    {
        m_index = pIndex;
        transform.position = new Vector3(m_index.x, 0.05f, m_index.y);
    }

    void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (e_end != null) e_end(m_index);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (e_cancel != null) e_cancel();
        }
    }

    private void move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            m_index += direction;

            m_index.x = Mathf.Max(0, m_index.x);
            m_index.x = Mathf.Min(Board.SIZEX - 1, m_index.x);
            m_index.y = Mathf.Max(0, m_index.y);
            m_index.y = Mathf.Min(Board.SIZEY - 1, m_index.y);

            transform.position = new Vector3(m_index.x, 0.05f, m_index.y);

            counter = COUNTER_TIME;

            if (e_move != null) e_move(m_index);
        }
    }
}
