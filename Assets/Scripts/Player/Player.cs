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

    private PlayerStats m_stats;
    public string m_teamID;         //The ID of the player team
    public bool m_isGK;             //Whether the player

    private PlayerAnimation playerAnimation;
    private PlayerController playerController;

    private Vector2 m_tileIndexToShoot;

    public bool m_hasMoved = false;
    public bool m_hasPerformedAction = false;
    public bool m_hasEndedTurn = false;

    //EVENTS
    public event Action moveFinishedEvent;

    public void init(Board pBoard, string pTeamID, bool pIsGK)
    {
        m_board = pBoard;
        m_teamID = pTeamID;
        m_isGK = pIsGK;

        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameCamera>();

        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimation.init();


        //LOAD ANIMATIONS IN RUNTIME
        //Load atlas texture
        //renderer.material = Resources.Load("Textures/" + pTeamID + "/Team" + pTeamID + "AtlasMaterial") as Material;

        //Add animations
        /*DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Resources/Animations/" + pTeamID);
        FileInfo[] fileInfo = info.GetFiles("*.asset");
        foreach (FileInfo file in fileInfo)
        {
            string animationName = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
            exSpriteAnimClip animation = Resources.Load("Animations/" + pTeamID + "/" + animationName) as exSpriteAnimClip;
            playerAnimation.addAnimation(animation);
        }*/

        playerController = GetComponent<PlayerController>();
        playerController.init();

        //Add listeners
        playerController.moveFinishedEvent += playerControllerMoveFinished;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pBall"></param>
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
    public void passTo(Vector2 pIndex)
    {
        m_ball.passTo(pIndex);

        //Set camera target
        m_camera.setTarget(m_ball.transform);

        //Set the player has already performed an action
        m_hasPerformedAction = true;
    }

    public void shootTo(Vector2 pIndex)
    {
        //Set player animation
        playerAnimation.playAnimation(m_teamID + (m_isGK ? "_gk_" : "_player_") + PlayerAnimationIds.SHOOT);
        playerAnimation.animationFinished += shootAnimationFinished;

        //Store tile to shoot
        m_tileIndexToShoot = pIndex;

        //Set the player has already performed an action
        m_hasPerformedAction = true;
    }

    /// <summary>
    /// Shoot the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    private void playerControllerMoveFinished()
    {
        m_board.playerToTile(this);

        moveFinishedEvent();
    }

    private void shootAnimationFinished()
    {
        //Remove listeners
        playerAnimation.animationFinished -= shootAnimationFinished;

        m_ball.shootTo(m_tileIndexToShoot);

        //Set camera target
        m_camera.setTarget(m_ball.transform);
    }

    #region PROPERTIES

    public Vector2 Index
    {
        set
        {           
            playerController.Index = value;
        }

        get
        {
            return playerController.Index;
        }
    }

    public bool isFliped
    {
        set
        {
            playerController.isFliped = value;
        }

        get
        {
            return playerController.isFliped;
        }
    }

    #endregion
}
