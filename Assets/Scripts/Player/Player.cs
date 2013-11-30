using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Player : MonoBehaviour
{
    public Transform m_ballPoint;

    internal Board m_board;

    private GameCamera m_camera;
    public Ball m_ball;

    public PlayerStats m_stats;
    private bool m_isGK;

    private PlayerAnimation playerAnimation;
    private PlayerController playerController;

    private Vector2 m_tileIndexToShoot;

    public bool m_hasMoved = false;
    public bool m_hasPerformedAction = false;
    public bool m_hasEndedTurn = false;
    public bool m_hasReacted = false;

    private Team m_team;

    //AI of the player
    public PlayerAI m_AI;

    private Vector2 m_tackleToIndex;

    //EVENTS
    public event Action moveFinishedEvent;
    public event Action e_actionEnd;

    public void init(Board pBoard, Team pTeam, bool pIsGK)
    {
        m_team = pTeam;
        m_board = pBoard;
        m_isGK = pIsGK;

        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameCamera>();

        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimation.init();

        playerController = GetComponent<PlayerController>();
        playerController.init(this, transform, m_board);

        //Stats
        m_stats = new PlayerStats();
        m_stats.m_dribble = 10;
        m_stats.m_tackle = 10;

        //Add listeners
        //ApplicationFactory.instance.m_messageBus.PlayerMoveEnded += playerControllerMoveFinished;
    }

    public void setIndex(Vector2 pIndex)
    {
        //Update board
        m_board.updateTile(playerController.Index, BoardTileData.EMPTY);
        m_board.updateTile(pIndex, BoardTileData.PLAYER);

        playerController.Index = pIndex;
    }

    public void move(List<Vector2> pTileIndexList)
    {
        //Update board tile
        m_board.updateTile(Index, BoardTileData.EMPTY);

        //Set camera target
        m_camera.setTarget(transform);
        playerController.move(pTileIndexList);

        //Set the player has already moved
        m_hasMoved = true;
    }

    public void setBall(Ball pBall)
    {
        m_ball = pBall;

        //Make it player's child and put it in position
        m_ball.transform.parent = transform;
        m_ball.transform.localPosition = m_ballPoint.localPosition;

        //Set ball index
        m_ball.Index = playerController.Index;
    }

    /// <summary>
    /// Pass the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    public void passTo(Vector2 pIndex, List<Vector2> pTileList)
    {
        m_ball.passTo(this, pIndex, pTileList);

        //Set camera target
        m_camera.setTarget(m_ball.transform);

        //Set the player has already performed an action
        m_hasPerformedAction = true;
    }

    public void shootTo()
    {
        SceneManager.instance.playShot(User.P1);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += performShot;
    }

    private void performShot()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= performShot;

        playerAnimation.playAnimation(m_team.ID + (m_isGK ? "_gk_" : "_player_") + PlayerAnimationIds.Shoot);
        playerAnimation.animationFinished += shootAnimationFinished;

        //Tile index of the goal
        Vector2 goalIndex = new Vector2(18, 6);
        //Store tile to shoot
        m_tileIndexToShoot = goalIndex;

        //Set the player has already performed an action
        m_hasPerformedAction = true;
    }

    public void blockTo()
    {
        m_hasReacted = true;
    }

    /// <summary>
    /// Shoot the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    /*private void playerControllerMoveFinished()
    {
        m_board.playerToTile(this);

        if (moveFinishedEvent != null) moveFinishedEvent();
    }*/

    private void shootAnimationFinished()
    {
        //Remove listeners
        playerAnimation.animationFinished -= shootAnimationFinished;

        m_ball.shootTo(this, m_tileIndexToShoot);

        //Set camera target
        m_camera.setTarget(m_ball.transform);
    }

    public bool isGonnaTackle()
    {
        return true; // UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;
    }

    public void moveToNextSquare()
    {
        StartCoroutine(playerController.moveToNextSquare());
    }

    //Player controller wrapper
    public void performTackle(Vector2 pTackleToIndex)
    {
        playerController.performTackle(pTackleToIndex);
    }

    //Player controller wrapper
    public void performJump(Vector2 pJumpToIndex)
    {
        playerController.performJump(pJumpToIndex);
    }

    //Player controller wrapper
    public void performCut()
    {
        m_hasReacted = true;
        playerController.performCut();
    }

    //Player controller wrapper
    public void performCatch()
    {
        m_hasReacted = true;
        playerController.performCatch();
    }

    public void takeBall()
    {
        m_ball.transform.parent = null;
        m_ball = null;
    }

    #region PROPERTIES

    public Vector2 Index
    {
        set { playerController.Index = value; }
        get { return playerController.Index; }
    }

    public bool isFliped
    {
        set { playerController.isFliped = value; }
        get { return playerController.isFliped; }
    }

    public Vector2 nextMoveTileIndex
    {
        get { return playerController.nextMoveTileIndex; }

    }
    public bool isMoveEnded
    {
        get { return playerController.isMoveEnded; }
    }

    public bool hasBall
    {
        get { return m_ball != null; }
    }

    public bool isGK
    {
        get { return m_isGK; }
    }

    public Team team
    {
        get { return m_team; }
    }

    public Action jumpEnd
    {
        get { return playerController.jumpEnd; }
        set { playerController.jumpEnd = value; }
    }

    public Action cutEnd
    {
        get { return playerController.cutEnd; }
        set { playerController.cutEnd = value; }
    }

    public Action catchEnd
    {
        get { return playerController.catchEnd; }
        set { playerController.catchEnd = value; }
    }

    #endregion
}
