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

        //Add listeners
        ApplicationFactory.instance.m_messageBus.PlayerMovedToTile += currentPlayerMovedToTile;
        ApplicationFactory.instance.m_messageBus.BallMovedToTile += ballMovedToTile;
    }

    private void currentPlayerMovedToTile(Player pPlayer)
    {
        Player tacklePlayer;
        if (isPlayerTackled(pPlayer, out tacklePlayer))
        {
            ApplicationFactory.instance.m_messageBus.dispatchTackleBattleStart();

            FX02 fx = ApplicationFactory.instance.m_fxManager.createFX02(tacklePlayer.transform.position);
            fx.init();

            bool isDribble = UnityEngine.Random.RandomRange(0.0f, 1.0f) > 0.5f;

            if (isDribble) fx.e_end += tackleDribbleStart;
            else fx.e_end += tackleNoDribbleStart;

            m_tackleInfo = new TackleInfo();
            m_tackleInfo.m_isDribble = isDribble;
            m_tackleInfo.m_jumpPlayer = pPlayer;
            m_tackleInfo.m_tacklePlayer = tacklePlayer;
            m_tackleInfo.m_jumpToIndex = pPlayer.jumpToIndex;
            m_tackleInfo.m_tackleToIndex = pPlayer.Index;
        }
        else
        {
            pPlayer.moveToNextSquare();
        }
    }

    private void ballMovedToTile(Ball pBall)
    {
        Player cutPlayer;
        if (isBallCut(pBall, out cutPlayer))
        {
            ApplicationFactory.instance.m_messageBus.dispatchTackleBattleStart();

            FX02 fx = ApplicationFactory.instance.m_fxManager.createFX02(cutPlayer.transform.position);
            fx.init();

            if (cutPlayer.isGK)
            {
                bool isGoal = UnityEngine.Random.RandomRange(0.0f, 1.0f) > 0.5f;
                
                //if (isGoal) fx.e_end += startCatchGoal;
                //else fx.e_end += startCatchNoGoal;

                //player.catchTo();
            }
            else
            {
                bool isPass = UnityEngine.Random.RandomRange(0.0f, 1.0f) > 0.5f;
                
                //if (isPass) fx.e_end += cutPassStart;
                //else fx.e_end += cutNoPassStart;
                
                //player.blockTo();
            }
        }
        else
        {
            pBall.moveToNextSquare();
        }
    }

    private bool isPlayerTackled(Player pPlayer, out Player pTacklePlayer)
    {
        for (int x = (int)pPlayer.Index.x - 1; x < (int)pPlayer.Index.x + 2; x++)
        {
            for (int y = (int)pPlayer.Index.y - 1; y < (int)pPlayer.Index.y + 2; y++)
            {
                if (Mathf.Abs(pPlayer.Index.x - x) + Mathf.Abs(pPlayer.Index.y - y) <= 1 && (x != pPlayer.Index.x || y != pPlayer.Index.y) && (x >= 0 && x < Board.SIZEX && y >= 0 && y < Board.SIZEY))
                {
                    if (m_board.isPlayerOnTile(new Vector2(x, y)))
                    {
                        Player player = m_board.getPlayerAtIndex(new Vector2(x, y));
                        if (player != null && !player.m_hasReacted && player.team.m_user != pPlayer.team.m_user && player.isGonnaTackle())
                        {
                            pTacklePlayer = player;
                            return true;
                        }
                    }
                }
            }
        }

        pTacklePlayer = null;
        return false;
    }

    private bool isBallCut(Ball pBall, out Player pCutPlayer)
    {
        for (int x = (int)pBall.Index.x - 1; x < (int)pBall.Index.x + 2; x++)
        {
            for (int y = (int)pBall.Index.y - 1; y < (int)pBall.Index.y + 2; y++)
            {
                //Check also the current square
                if (Mathf.Abs(pBall.Index.x - x) + Mathf.Abs(pBall.Index.y - y) <= 1 && (x >= 0 && x < Board.SIZEX && y >= 0 && y < Board.SIZEY))
                {
                    if (m_board.isPlayerOnTile(new Vector2(x, y)))
                    {
                        Player player = m_board.getPlayerAtIndex(new Vector2(x, y));
                        if (player != null && !player.m_hasReacted && player.team.m_user != pBall.m_player.team.m_user)
                        {
                            pCutPlayer = player;
                            return true;
                        }
                    }
                }
            }
        }

        pCutPlayer = null;
        return false;
    }

    private TackleInfo m_tackleInfo;

    private void tackleDribbleStart()
    {
        SceneManager.instance.playTackle_Dribble(User.P1, User.P2);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += tackleEnd;
    }

    private void tackleNoDribbleStart()
    {
        SceneManager.instance.playTackle_NoDribble(User.P1, User.P2);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += tackleEnd;
    }

    private void tackleEnd()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= tackleEnd;

        m_tackleInfo.m_tacklePlayer.performTackle(m_tackleInfo.m_tackleToIndex);
        m_tackleInfo.m_jumpPlayer.performJump(m_tackleInfo.m_jumpToIndex);

        if (!m_tackleInfo.m_isDribble)
        {
            m_tackleInfo.m_jumpPlayer.takeBall();
            m_tackleInfo.m_tacklePlayer.setBall(m_ball);

            m_tackleInfo.m_jumpPlayer.jumpEnd += jumpNoDribbleEnd;
        }
        else
        {
            m_tackleInfo.m_jumpPlayer.jumpEnd += jumpDribbleEnd;
        }
    }

    private void jumpNoDribbleEnd()
    {
        m_tackleInfo.m_jumpPlayer.jumpEnd -= jumpNoDribbleEnd;

        PlayerTurnAnimation playerTurnAnimation = GUIManager.instance.createPlayerTurnAnimation();
        playerTurnAnimation.init();
        playerTurnAnimation.e_end += startPlayerTurn;
    }

    private void jumpDribbleEnd()
    {
        m_tackleInfo.m_jumpPlayer.jumpEnd -= jumpDribbleEnd;

        m_tackleInfo.m_jumpPlayer.moveToNextSquare();
    }

    /// <summary>
    /// Starts next player turn
    /// </summary>
    private void startPlayerTurn()
    {
        m_currentTeam.startTurn();
    }

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

    /*private void tackleTo(Vector2 pIndex)
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
    }*/

    /*private void tackleEnd()
    {
        //Remove listeners
        m_currentPlayer.e_actionEnd -= tackleEnd;

        //Clear board
        m_board.clearTileRadius();

        //showPlayerMenu();
    }*/
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