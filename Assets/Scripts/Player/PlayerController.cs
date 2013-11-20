using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    private Transform m_transform;
    private Board m_board;

    private float m_speed = 2.5f;

    private List<Vector2> m_toMoveSquareList;

    private bool m_isFliped = false;

    private Player m_player;
    
    private exSprite m_sprite;
    public exSprite m_spriteShadow;

    private PlayerAnimation m_playerAnimation;

    private Vector2 m_index;

    public void init(Player pPlayer, Transform pTransform, Board pBoard)
    {
        m_transform = pTransform;
        m_board = pBoard;

        m_player = pPlayer;

        m_sprite = GetComponent<exSprite>();
        
        m_playerAnimation = GetComponent<PlayerAnimation>();

        //Set idle animation
        m_playerAnimation.playAnimation(m_player.team.ID + (m_player.isGK ? "_gk_" : "_player_") + PlayerAnimationIds.Idle);
    }

    public void move(List<Vector2> pToMoveSquareList)
    {
        m_playerAnimation.playAnimation(m_player.team.ID + (m_player.isGK ? "_gk_" : "_player_") + PlayerAnimationIds.Run);
        m_toMoveSquareList = pToMoveSquareList;

        StartCoroutine(moveToNextSquare());
    }

    private IEnumerator moveToNextSquare()
    {
        Vector2 nextSquareIndex = m_toMoveSquareList[0];
        Vector2 direction = nextSquareIndex - m_player.Index;

        m_isFliped = direction.x != 0 && direction.x < 0;

        while ((new Vector2(transform.position.x, transform.position.z) - nextSquareIndex).sqrMagnitude > 0.005f)
        {
            transform.position += new Vector3(direction.x, 0, direction.y) * m_speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Index = nextSquareIndex;
        m_toMoveSquareList.RemoveAt(0);

        //Check if any player around is gonna tackle
        if (!checkPlayersAround())
        {
            if (m_toMoveSquareList.Count > 0)
            {
                StartCoroutine(moveToNextSquare());
            }
            else
            {
                m_playerAnimation.playAnimation(m_player.team.ID + (m_player.isGK ? "_gk_" : "_player_") + PlayerAnimationIds.Idle);
                ApplicationFactory.instance.m_messageBus.dispatchPlayerMoveEnded();
            }
        }
    }

    private bool checkPlayersAround()
    {
        for (int x = (int)Index.x - 1; x < (int)Index.x + 2; x++)
        {
            for (int y = (int)Index.y - 1; y < (int)Index.y + 2; y++)
            {
                if (Mathf.Abs(Index.x - x) +  Mathf.Abs(Index.y - y) <= 1 && (x != Index.x || y != Index.y) && (x >= 0 && x < Board.SIZEX && y >= 0 && y < Board.SIZEY))
                {
                    if (m_board.isPlayerOnTile(new Vector2(x, y)))
                    {
                        Player player = m_board.getPlayerAtIndex(new Vector2(x, y));
                        if (player.isGonnaTackle())
                        {
                            ApplicationFactory.instance.m_messageBus.dispatchTackleBattleStart();
                            ApplicationFactory.instance.m_fxManager.createFX02(player.transform.position);

                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void dribbleEnded()
    {
        if (m_toMoveSquareList.Count > 0)
        {
            StartCoroutine(moveToNextSquare());
        }
        else
        {
            m_playerAnimation.playAnimation(m_player.team.ID + (m_player.isGK ? "_gk_" : "_player_") + PlayerAnimationIds.Idle);
            ApplicationFactory.instance.m_messageBus.dispatchPlayerMoveEnded();
        }
    }

    #region PROPERTIES

    public Vector2 Index
    {
        set
        {
            transform.position = new Vector3(value.x, 0, value.y);

            m_index = value;

            //If we have the ball, set the ball index
            if (m_player.m_ball)
            {
                m_player.m_ball.Index = m_index;
            }
        }

        get
        {
            return m_index;
        }
    }

    public bool isFliped
    {
        set
        {
            //Set the value
            m_isFliped = value;

            //Set the sprite scale
            float scaleX = Mathf.Abs(m_sprite.scale.x) * (m_isFliped ? -1 : 1);
            float scaleY = m_sprite.scale.y;

            m_sprite.scale = new Vector2(scaleX, scaleY);
            m_spriteShadow.scale = new Vector2(scaleX, scaleY);
        }

        get
        {
            return m_isFliped;
        }
    }

    #endregion
}