using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    //PREFABS
    public GameObject m_playerPrefab;
    public GameObject m_ballPrefab;
    public GameObject m_boxPrefab;
    public GameObject m_arrowPrefab;

    private Board m_board;
    private Cursor m_cursor;
    private Arrow m_arrow;
    private Ball m_ball;

    private Box m_box;

    private GameCamera m_camera;

    private Player m_currentPlayer;

	void Start ()
    {
        configurateMatch();
	}
	

	void Update ()
    {

	}

    private void configurateMatch()
    {
        m_ball = (GameObject.Instantiate(m_ballPrefab) as GameObject).GetComponent<Ball>();
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        m_cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();

        m_board.init(m_ball);
        m_ball.init(m_board);
        m_cursor.init(this);

        m_arrow = (GameObject.Instantiate(m_arrowPrefab) as GameObject).GetComponent<Arrow>();
        m_arrow.gameObject.SetActiveRecursively(false);

        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameCamera>();
        m_camera.setTarget(m_cursor.transform);

        //P1 Team
        TeamData team1Data = TeamDataBase.getTeamData("Nankatsu");
        foreach (PlayerTeamData player in team1Data.m_playerList)
        {
            //Get the player data
            PlayerData playerData = PlayerDataBase.getPlayerData(player.m_name);

            Player p = (GameObject.Instantiate(m_playerPrefab) as GameObject).GetComponent<Player>();
            p.init(m_board, "01", playerData.m_position == PlayerPosition.GK);
            m_board.addPlayer(p, new Vector2(player.m_tileX, player.m_tileY));

            if (player.m_tileX == 8 && player.m_tileY == 7)
            {
                p.setBall(m_ball);
            }
        }

        //P2 Team
        TeamData team2Data = TeamDataBase.getTeamData("Nankatsu");
        foreach (PlayerTeamData player in team2Data.m_playerList)
        {
            //Get the player data
            PlayerData playerData = PlayerDataBase.getPlayerData(player.m_name);

            Player p = (GameObject.Instantiate(m_playerPrefab) as GameObject).GetComponent<Player>();
            p.init(m_board, "02", playerData.m_position == PlayerPosition.GK);
            p.isFliped = true;

            m_board.addPlayer(p, new Vector2((Board.SIZEX - 1) - player.m_tileX, (Board.SIZEY - 1) - player.m_tileY));

        }

        //Add listeners
        m_cursor.moveFinishedEvent += selectPlayer;
    }

    public void selectPlayer(Vector2 index)
    {
        //If there is a player on the tile
        if (m_board.isPlayerOnTile(index))
        {
            //Get the player on the tile
            m_currentPlayer = m_board.getPlayerAtIndex(index);

            //If player hasn't ended his turn yet
            if (!m_currentPlayer.m_hasEndedTurn)
            {
                //Remove listeners
                m_cursor.moveFinishedEvent -= selectPlayer;

                m_cursor.gameObject.SetActiveRecursively(false);
                openBox();
            }
        }
    }

    public void openBox()
    {
        m_box = (GameObject.Instantiate(m_boxPrefab) as GameObject).GetComponent<Box>();
        m_box.init(m_currentPlayer); //"Move", "Pass", "Shoot", "Cancel");

        m_box.optionSelectedEvent += optionSelected;
        m_box.closedEvent += boxClosed;
    }

    public void optionSelected(int optionId)
    {
        if (optionId == 0)
        {
            //board.drawTileRadius(m_currentPlayer.getIndex(), 3);

            m_arrow.gameObject.SetActiveRecursively(true);
            m_arrow.init(m_currentPlayer.Index, (m_currentPlayer.isFliped ? -1 : 1));

            m_arrow.moveFinishedEvent += moveFinished;
        }
        else if (optionId == 1)
        {
            m_cursor.gameObject.SetActiveRecursively(true);
            m_camera.setTarget(m_cursor.transform);

            //Add listeners
            m_cursor.moveFinishedEvent += passTo;
        }
        else if (optionId == 2)
        {
            m_cursor.gameObject.SetActiveRecursively(true);
            m_camera.setTarget(m_cursor.transform);

            //Add listeners
            m_cursor.moveFinishedEvent += shootTo;
        }
        else if (optionId == 3)
        {
            //Finish current player turn
            m_currentPlayer.m_hasEndedTurn = true;
            m_currentPlayer.renderer.material = Resources.Load("Textures/Atlas/AtlasTintColor") as Material;

            m_cursor.gameObject.SetActiveRecursively(true);
            m_cursor.setIndex(m_currentPlayer.Index);
            m_camera.setTarget(m_cursor.transform);

            //Add listeners
            m_cursor.moveFinishedEvent += selectPlayer;
        }

        m_box.optionSelectedEvent -= optionSelected;
        m_box.closedEvent -= boxClosed;

        Destroy(m_box.gameObject);
    }

    public void boxClosed()
    {
        m_box.optionSelectedEvent -= optionSelected;
        m_box.closedEvent -= boxClosed;

        m_box.gameObject.SetActiveRecursively(false);
    }

    private void moveFinished(List<Vector2> pTileIndexList)
    {
        m_arrow.moveFinishedEvent -= moveFinished;

        m_currentPlayer.move(pTileIndexList);
        m_currentPlayer.moveFinishedEvent += currentPlayerMoveFinished;
    }

    private void currentPlayerMoveFinished()
    {
        //Remove listeners
        m_currentPlayer.moveFinishedEvent -= currentPlayerMoveFinished;

        openBox();
    }

    /// <summary>
    /// Pass the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    private void passTo(Vector2 pIndex)
    {
        //Remove listeners
        m_cursor.moveFinishedEvent -= passTo;

        //Hide the cursor
        m_cursor.gameObject.SetActiveRecursively(false);

        //Add listeners
        m_ball.moveFinishedEvent += passFinished;

        //Pass the ball
        m_currentPlayer.passTo(pIndex);
    }

    private void passFinished()
    {
        //Remove listeners
        m_ball.moveFinishedEvent -= passFinished;

        openBox();
    }

    /// <summary>
    /// Shoot the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    private void shootTo(Vector2 pIndex)
    {
        //Remove listeners
        m_cursor.moveFinishedEvent -= shootTo;

        //Hide the cursor
        m_cursor.gameObject.SetActiveRecursively(false);

        //Add listeners
        m_ball.moveFinishedEvent += passFinished;

        //Shoot the ball
        m_currentPlayer.shootTo(pIndex);
    }
}
