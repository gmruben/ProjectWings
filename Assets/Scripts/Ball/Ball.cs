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

    private float m_shotSpeed = 5.0f;
    private bool m_inShot = false;

    private float m_accY;
    private float m_speedY;
    private float m_gravity = 2;

    private Board m_board;

    private BallAnimation m_ballAnimation;

    private Player m_player;

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

        /*if (m_inShot)
        {
            transform.position += new Vector3(m_direction.x * m_shotSpeed, m_speedY, m_direction.y * m_shotSpeed) * Time.deltaTime;

            //Check if we are on the goal tile
            if (transform.position.x > (Board.SIZEX - 1) * Board.c_tileSize)
            {
                m_player.team.opponentTeam.GK.showContextualMenu();

                m_inShot = false;
            }
        }*/
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
    /// <param name="pPlayer">The player who shot</param>
    /// <param name="pIndex">Tile index</param>
    public void shootTo(Player pPlayer, Vector2 pIndex)
    {
        //Remove parent
        transform.parent = null;
        transform.position = new Vector3(m_index.x, 0.15f, m_index.y);

        m_player = pPlayer;

        m_direction = (pIndex - m_index).normalized;

        //Set animatino
        m_ballAnimation.playAnimation("01_" + BallAnimationIds.SHOOT);

        StartCoroutine(updateShot());
    }

    private IEnumerator updateShot()
    {
        while (true)
        {
            transform.position += new Vector3(m_direction.x, 0, m_direction.y) * 2.5f * Time.deltaTime;

            int x = Mathf.FloorToInt(transform.position.x / Board.c_tileSize);
            if (x != Index.x)
            {
                Index = new Vector2(x, Index.y);
                if (checkPlayersAround())
                {
                    break;
                }
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private bool checkPlayersAround()
    {
        for (int x = (int)Index.x - 1; x < (int)Index.x + 2; x++)
        {
            for (int y = (int)Index.y - 1; y < (int)Index.y + 2; y++)
            {
                //Check also the current square
                if (Mathf.Abs(Index.x - x) + Mathf.Abs(Index.y - y) <= 1 && (x >= 0 && x < Board.SIZEX && y >= 0 && y < Board.SIZEY))
                {
                    if (m_board.isPlayerOnTile(new Vector2(x, y)))
                    {
                        Player player = m_board.getPlayerAtIndex(new Vector2(x, y));
                        if (player != null && !player.m_hasReacted && player.team.m_user != m_player.team.m_user)
                        {
                            ApplicationFactory.instance.m_messageBus.dispatchTackleBattleStart();
                            
                            FX02 fx = ApplicationFactory.instance.m_fxManager.createFX02(player.transform.position);
                            fx.init();

                            if (player.isGK)
                            {
                                fx.e_end += startCatch;
                                player.catchTo();
                            }
                            else
                            {
                                fx.e_end += startBlock;
                                player.blockTo();
                            }

                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void startCatch()
    {
        SceneManager.instance.playCatch_Goal(User.P1);
        SceneManager.instance.e_sceneFinished += endCatch;
    }

    private void endCatch()
    {
        SceneManager.instance.e_sceneFinished -= endCatch;
    }

    private void startBlock()
    {
        SceneManager.instance.playTackle02(User.P1, User.P2);
        SceneManager.instance.e_sceneFinished += blockEnd;
    }

    private void blockEnd()
    {
        SceneManager.instance.e_sceneFinished -= blockEnd;
        StartCoroutine(updateShot());
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
