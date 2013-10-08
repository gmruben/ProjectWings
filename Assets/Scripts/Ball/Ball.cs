using UnityEngine;
using System;
using System.Collections;

public class Ball : MonoBehaviour
{
    private float m_speed = 0.5f;

    private Vector2 m_index;
    private Vector2 m_direction;
    private Vector2 m_targetTileIndex;
    private bool m_isMoving = false;

    private float m_shotSpeed = 2.5f;
    private bool m_inShot = false;

    private float m_accY;
    private float m_speedY;
    private float m_gravity = 2;

    private Board m_board;

    private BallAnimation m_ballAnimation;

    //EVENTS
    public event Action moveFinishedEvent;

    public void init(Board pBoard)
    {
        m_board = pBoard;

        m_ballAnimation = GetComponent<BallAnimation>();
        m_ballAnimation.init();
        m_ballAnimation.playAnimation(BallAnimationIds.IDLE);
    }

	void Update ()
    {
        if (m_isMoving)
        {
            m_accY -= m_gravity * Time.deltaTime;
            m_speedY += m_accY * Time.deltaTime;

            transform.position += new Vector3(m_direction.x * m_speed, m_speedY, m_direction.y * m_speed) * Time.deltaTime;

            if ((new Vector2(transform.position.x, transform.position.z) - m_targetTileIndex).sqrMagnitude < 0.005f)
            {
                m_isMoving = false;
                transform.position = new Vector3(m_targetTileIndex.x, 0.15f, m_targetTileIndex.y);

                m_board.ballToTile(m_targetTileIndex);

                //Dispatch event
                moveFinishedEvent();
            }
        }

        if (m_inShot)
        {
            transform.position += new Vector3(m_direction.x * m_shotSpeed, m_speedY, m_direction.y * m_shotSpeed) * Time.deltaTime;

            /*if ((new Vector2(transform.position.x, transform.position.z) - m_targetTileIndex).sqrMagnitude < 0.005f)
            {
                //Dispatch event
                moveFinishedEvent();
            }*/
        }
	}

    /// <summary>
    /// Pass the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    public void passTo(Vector2 pIndex)
    {
        //Remove parent
        transform.parent = null;
        transform.position = new Vector3(m_index.x, 0.15f, m_index.y);

        m_direction = pIndex - m_index;
        m_targetTileIndex = pIndex;
        m_isMoving = true;

        m_accY = -m_gravity;
        m_speedY = 1.5f * m_gravity;
    }

    /// <summary>
    /// Shot the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    public void shootTo(Vector2 pIndex)
    {
        //Remove parent
        transform.parent = null;
        transform.position = new Vector3(m_index.x, 0.15f, m_index.y);

        m_direction = pIndex - m_index;
        m_inShot = true;

        //Set animatino
        m_ballAnimation.playAnimation("01_" + BallAnimationIds.SHOOT);
    }

    #region PROPERTIES

    public Vector2 Index
    {
        set
        {
            m_index = value;
        }

        get
        {
            return m_index;
        }
    }

    #endregion
}
