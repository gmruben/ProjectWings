using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    //CONSTANTS
    private const int c_numberOfTurnsInAPhase = 3;

    //PREFABS
    public GameObject m_playerPrefab;
    public GameObject m_ballPrefab;
    public GameObject m_boxPrefab;
    public GameObject m_arrowPrefab;

    private Board m_board;
    private Arrow m_arrow;
    private Ball m_ball;

    private PlayerMenu m_box;

    private GameCamera m_camera;

    private Team m_currentTeam;
    private Player m_currentPlayer;
    private int m_turnIndex;
    private int m_currentPhase;

    private int m_gameMode;

    private Team m_team1;
    private Team m_team2;

    //EVENTS
    public Action<int> e_startPhase;
    public Action<int> e_endTurn;

    #region BUILT-IN FUNCTIONS

    void Start ()
    {
        configurateMatch();
	}

    #endregion

    private void configurateMatch()
    {
        m_ball = (GameObject.Instantiate(m_ballPrefab) as GameObject).GetComponent<Ball>();
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        
        m_board.init(m_ball);
        m_ball.init(m_board);

        m_arrow = (GameObject.Instantiate(m_arrowPrefab) as GameObject).GetComponent<Arrow>();
        m_arrow.gameObject.SetActiveRecursively(false);

        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameCamera>();

        //Instantiate teams
        m_team1 = new Team(m_board);
        m_team2 = new Team(m_board);

        m_team1.m_opponentTeam = m_team2;
        m_team2.m_opponentTeam = m_team1;
        m_team1.m_user = User.P1;
        m_team2.m_user = User.P2;
        m_team1.ID = "01";
        m_team2.ID = "02";

        //P1 Team
        TeamData team1Data = TeamDataBase.getTeamData("Nankatsu");
        foreach (PlayerTeamData player in team1Data.m_playerList)
        {
            //Get the player data
            PlayerData playerData = PlayerDataBase.getPlayerData(player.m_name);

            Player p = (GameObject.Instantiate(m_playerPrefab) as GameObject).GetComponent<Player>();
            p.init(m_board, m_team1, playerData.m_position == PlayerPosition.GK);
            
            m_board.addPlayer(p, new Vector2(player.m_tileX, player.m_tileY));

            if (player.m_tileX == 8 && player.m_tileY == 7)
            {
                p.setBall(m_ball);
            }

            //Add player to the team
            m_team1.addPlayer(p);
        }

        //P2 Team
        TeamData team2Data = TeamDataBase.getTeamData("Nankatsu");
        foreach (PlayerTeamData player in team2Data.m_playerList)
        {
            //Get the player data
            PlayerData playerData = PlayerDataBase.getPlayerData(player.m_name);

            Player p = (GameObject.Instantiate(m_playerPrefab) as GameObject).GetComponent<Player>();
            p.init(m_board, m_team2, playerData.m_position == PlayerPosition.GK);
            p.isFliped = true;

            m_board.addPlayer(p, new Vector2((Board.SIZEX - 1) - player.m_tileX, (Board.SIZEY - 1) - player.m_tileY));

            //Add player to the team
            m_team2.addPlayer(p);
        }

        //P1 starts
        m_currentTeam = m_team1;
        m_currentPhase = User.P1;
        m_turnIndex = c_numberOfTurnsInAPhase;
        if (e_startPhase != null) e_startPhase(m_currentPhase);

        //Set game mode
        m_gameMode = GameModes.P1VsP2;

        PlayerTurnAnimation playerTurnAnimation = GUIManager.instance.createPlayerTurnAnimation();
        playerTurnAnimation.init();
        playerTurnAnimation.e_end += startPlayerTurn;
    }

    /// <summary>
    /// Starts next player turn
    /// </summary>
    private void startPlayerTurn()
    {
        m_currentTeam.startTurn();
        //SceneManager.instance.playTackle02();
    }

    /*public void selectPlayer(Vector2 index)
    {
        //If there is a player on the tile
        if (m_board.isPlayerOnTile(index))
        {
            //Get the player on the tile
            m_currentPlayer = m_board.getPlayerAtIndex(index);

            //If player hasn't ended his turn yet and is ours
            if (!m_currentPlayer.m_hasEndedTurn && isPlayerSelectable())
            {
                //Remove listeners
                m_cursor.e_end -= selectPlayer;

                m_cursor.gameObject.SetActiveRecursively(false);
                showPlayerMenu();
            }
        }
    }*/

    private bool isPlayerSelectable()
    {
        return (m_gameMode == GameModes.P1VsP2 && m_currentPlayer.team.m_user == m_currentPhase);
    }

    /*public void showPlayerMenu()
    {
        m_box = (GameObject.Instantiate(m_boxPrefab) as GameObject).GetComponent<PlayerMenu>();
        m_box.init(m_currentPlayer);

        m_box.e_selected += optionSelected;
        m_box.e_cancel += cancelBox;
    }*/

    /*public void optionSelected(int optionId)
    {
        //Remove listeners
        m_box.e_selected -= optionSelected;
        m_box.e_cancel -= cancelBox;

        if (optionId == PlayerAction.Move)
        {
            m_board.drawTileRadius(m_currentPlayer.Index, 3);
            setArrowActive();

            m_arrow.e_end += endMove;
            m_arrow.e_cancel += cancelMove;
        }
        else if (optionId == PlayerAction.Pass)
        {
            m_cursor.gameObject.SetActiveRecursively(true);
            m_camera.setTarget(m_cursor.transform);

            //Add listeners
            m_cursor.e_end += passTo;
        }
        else if (optionId == PlayerAction.Shoot)
        {
            m_cursor.setIndex(m_currentPlayer.Index);
            m_cursor.gameObject.SetActiveRecursively(true);

            m_camera.setTarget(m_cursor.transform);

            //Draw board tiles
            m_board.drawShoot(m_currentPlayer.Index);

            //Add listeners
            //m_cursor.e_end += shootTo;
            m_cursor.e_end += showShotScene;
            m_cursor.e_cancel += cancelShoot;
        }
        else if (optionId == PlayerAction.EndTurn)
        {
            //Finish current player turn
            m_currentPlayer.m_hasEndedTurn = true;
            m_currentPlayer.renderer.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f));

            setCursorActive();
            m_cursor.e_end += selectPlayer;

            //Dispatch event
            m_turnIndex--;
            if (m_turnIndex == 0)
            {
                m_currentPhase = (m_currentPhase == User.P1) ? User.P2 : User.P1;
                m_turnIndex = c_numberOfTurnsInAPhase;
                if (e_startPhase != null) e_startPhase(m_currentPhase);
            }
            else
            {
                if (e_endTurn != null) e_endTurn(m_turnIndex);
            }
        }
        else if (optionId == PlayerAction.Tackle)
        {
            //Paint the tiles where the player can tackle
            m_board.drawTileRadius(m_currentPlayer.Index, 1);
            
            //Set the cursor active
            setCursorActive();
            m_cursor.e_end += tackleTo;
            m_cursor.e_cancel += cancelMove;
        }

        Destroy(m_box.gameObject);
    }*/

    private void setArrowActive()
    {
        m_arrow.gameObject.SetActiveRecursively(true);
        m_arrow.init(m_currentPlayer.Index, (m_currentPlayer.isFliped ? -1 : 1));
    }
    
    private void setCursorActive()
    {
        //m_cursor.gameObject.SetActiveRecursively(true);
        //m_cursor.setIndex(m_currentPlayer.Index);
        //m_camera.setTarget(m_cursor.transform);
    }

    /*private void cancelBox()
    {
        //Remove listeners
        m_box.e_selected -= optionSelected;
        m_box.e_cancel -= cancelBox;

        m_box.gameObject.SetActiveRecursively(false);

        setCursorActive();
    }*/

    private void endMove(List<Vector2> pTileIndexList)
    {
        //Remove listeners
        m_arrow.e_end -= endMove;
        m_arrow.e_cancel -= cancelMove;

        m_currentPlayer.move(pTileIndexList);
        //m_currentPlayer.moveFinishedEvent += currentPlayerMoveFinished;

        m_board.clearTileRadius();
    }

    private void cancelMove()
    {
        //Remove listeners
        m_arrow.e_end -= endMove;
        m_arrow.e_cancel -= cancelMove;

        //showPlayerMenu();
    }

    /// <summary>
    /// Pass the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    private void passTo(Vector2 pIndex)
    {
        //Remove listeners
        //m_cursor.e_end -= passTo;

        //Hide the cursor
        //m_cursor.gameObject.SetActiveRecursively(false);

        //Add listeners
        m_ball.moveFinishedEvent += passFinished;

        //Pass the ball
        m_currentPlayer.passTo(pIndex);
    }

    private void passFinished()
    {
        //Remove listeners
        m_ball.moveFinishedEvent -= passFinished;

        //showPlayerMenu();
    }

    private void showShotScene(Vector2 pIndex)
    {
        //Remove listeners
        //m_cursor.e_end -= showShotScene;
        //m_cursor.e_cancel -= cancelShoot;

        Scene scene = GUIManager.instance.createVolleyShotScene();
        scene.play();
        scene.e_end += shootTo;
    }

    /// <summary>
    /// Shoot the ball to a tile
    /// </summary>
    /// <param name="pIndex">Tile index</param>
    private void shootTo() //Vector2 pIndex)
    {
        //Hide the cursor
        //m_cursor.gameObject.SetActiveRecursively(false);

        //Add listeners
        m_ball.moveFinishedEvent += passFinished;

        //Remove board tiles
        m_board.clearTileRadius();

        //Shoot the ball
        m_currentPlayer.shootTo();
    }

    private void cancelShoot()
    {
        //Remove listeners
        //m_cursor.e_end -= showShotScene;
        //m_cursor.e_cancel -= cancelShoot;
    }

    private void tackleTo(Vector2 pIndex)
    {
         //If there is a player on the tile
        if (m_board.isPlayerOnTile(pIndex))
        {
            //Get the player on the tile
            Player player = m_board.getPlayerAtIndex(pIndex);
            //If the player is not our team and has the ball
            if (player.team.m_user != m_currentPhase && player.hasBall)
            {
                //Remove listeners
                //m_cursor.e_end -= tackleTo;
                //m_cursor.e_cancel -= cancelMove;

                //Hide the cursor
                //m_cursor.gameObject.SetActiveRecursively(false);

                //Add listeners
                m_currentPlayer.e_actionEnd += tackleEnd;

                //Tackle
                player.hasBeenTackledFrom(m_currentPlayer.Index);
                //m_currentPlayer.tackleTo(pIndex);
            }
        }
    }

    private void tackleEnd()
    {
        //Remove listeners
        m_currentPlayer.e_actionEnd -= tackleEnd;

        //Clear board
        m_board.clearTileRadius();

        //showPlayerMenu();
    }
}

public class User
{
    public static int P1 = 0;
    public static int P2 = 1;
}

public class GameModes
{
    public static int P1VsCPU;
    public static int P1VsP2;
}