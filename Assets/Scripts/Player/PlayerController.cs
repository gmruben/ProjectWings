using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    private Transform m_transform;

    private float m_speed = 2.5f;

    private List<Vector2> m_toMoveSquareList;

    private bool m_isMoving = false;
    private bool m_isInAMove = false;

    private bool m_isFliped = false;

    private Vector2 m_nextSquare;
    private Vector2 m_direction;
    private Vector2 m_index;

    private Player m_player;
    private exSprite m_sprite;
    private PlayerAnimation m_playerAnimation;

    //EVENTS
    public event Action moveFinishedEvent;

    public void init()
    {
        m_transform = transform;

        m_player = GetComponent<Player>();
        m_sprite = GetComponent<exSprite>();
        m_playerAnimation = GetComponent<PlayerAnimation>();

        //Set idle animation
        m_playerAnimation.playAnimation(m_player.m_teamID + (m_player.m_isGK ? "_gk_" : "_player_") + PlayerAnimationIds.IDLE);
    }

    void Update()
    {
        if (m_isMoving)
        {
            transform.position += new Vector3(m_direction.x, 0, m_direction.y) * m_speed * Time.deltaTime;

            if ((new Vector2(transform.position.x, transform.position.z) - m_nextSquare).sqrMagnitude < 0.005f)
            {
                Index = m_nextSquare;

                if (m_toMoveSquareList.Count > 0)
                {
                    moveToNextSquare();
                }
                else
                {
                    m_isMoving = false;
                    m_playerAnimation.playAnimation(m_player.m_teamID + (m_player.m_isGK ? "_gk_" : "_player_") + PlayerAnimationIds.IDLE);

                    //Dispatch event
                    moveFinishedEvent();
                }
            }
        }
    }

    public void move(List<Vector2> pToMoveSquareList)
    {
        m_playerAnimation.playAnimation(m_player.m_teamID + (m_player.m_isGK ? "_gk_" : "_player_") + PlayerAnimationIds.RUN);

        m_toMoveSquareList = pToMoveSquareList;

        m_isMoving = true;
        m_isInAMove = false;

        moveToNextSquare();
    }

    private void moveToNextSquare()
    {
        Vector2 squareIndex = m_toMoveSquareList[0];

        m_nextSquare = squareIndex;
        m_direction = squareIndex - m_player.Index;

        if (m_direction.x != 0)
        {
            isFliped = m_direction.x < 0;
        }
        
        m_toMoveSquareList.RemoveAt(0);
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
            m_sprite.scale = new Vector2(scaleX, m_sprite.scale.y);
        }

        get
        {
            return m_isFliped;
        }
    }

    #endregion
}