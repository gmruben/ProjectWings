using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    private float m_passSpeed = 2.5f;
    private const float m_shotSpeed = 5.0f;

    private Vector2 m_index;
    private Vector2 m_direction;
    private Vector2 m_targetTileIndex;

    private List<Vector2> m_passTileList;
    
    private bool m_isShot = false;

    private float m_accY;
    private float m_speedY;
    private float m_gravity = 2;

    private Board m_board;

    private BallAnimation m_ballAnimation;

    public Player m_player;

    //EVENTS
    public event Action moveFinishedEvent;

    public void init(Board pBoard)
    {
        m_board = pBoard;

        m_ballAnimation = GetComponent<BallAnimation>();
        m_ballAnimation.init();
        m_ballAnimation.playAnimation(BallAnimationIds.IDLE);
    }

    /// <summary>
    /// Pass the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    public void passTo(Player pPlayer, Vector2 pIndex, List<Vector2> pTileList)
    {
        //Remove parent
        transform.parent = null;
        transform.position = new Vector3(m_index.x, 0.15f, m_index.y);

        m_player = pPlayer;
        m_passTileList = pTileList;

        m_direction = pIndex - m_index;
        m_targetTileIndex = pIndex;

        //m_accY = -m_gravity;
        //m_speedY = 1.5f * m_gravity;

        m_isShot = false;
        StartCoroutine(updatePass());
    }

    /// <summary>
    /// Shot the ball to a tile
    /// </summary>
    /// <param name="pPlayer">The player who shot</param>
    /// <param name="pIndex">Tile index</param>
    public void shootTo(Player pPlayer, Vector2 pIndex)
    {
        //Remove parent
        transform.parent = null;
        transform.position = new Vector3(m_index.x, 0.15f, m_index.y);

        m_player = pPlayer;

        m_direction = (pIndex - m_index).normalized;

        //Set animation
        m_ballAnimation.playAnimation("01_" + BallAnimationIds.SHOOT);

        m_isShot = true;
        StartCoroutine(updateShot());
    }

    public void moveToNextSquare()
    {
        if (m_isShot) StartCoroutine(updateShot());
        else StartCoroutine(updatePass());
    }

    private IEnumerator updateShot()
    {
        float nextTilePosX = (Index.x + 1) * Board.c_tileSize;
        while (Mathf.Abs(transform.position.x - nextTilePosX) > 0.05f)
        {
            transform.position += new Vector3(m_direction.x, 0, m_direction.y) * m_shotSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        Index = new Vector2(Index.x + 1, Index.y);
        ApplicationFactory.instance.m_messageBus.dispatchBallMovedToTile(this);
    }

    private IEnumerator updatePass()
    {
        Vector3 v = new Vector3(m_direction.x, 0, m_direction.y).normalized;
        Vector2 nextSquareIndex = m_passTileList[0];

        while (!(Mathf.Abs(transform.position.x - nextSquareIndex.x) < 0.05f && m_direction.x != 0) && !(Mathf.Abs(transform.position.z - nextSquareIndex.y) < 0.05f && m_direction.y != 0))
        {
            transform.position += v * m_passSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        m_passTileList.RemoveAt(0);
        Index = nextSquareIndex;

        if (Index == m_targetTileIndex) ApplicationFactory.instance.m_messageBus.dispatchBallPassEnded(this);
        else ApplicationFactory.instance.m_messageBus.dispatchBallMovedToTile(this);
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
