using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public Action tackleEnded;

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

    private TackleInfo m_tackleInfo;
    private CutInfo m_cutInfo;
    private CatchInfo m_catchInfo;

    #region BUILT-IN FUNCTIONS

    void Start()
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
        m_team1 = new Team(this, m_camera, m_board);
        m_team2 = new Team(this, m_camera, m_board);

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

        startUserPhase();

        //Add listeners
        ApplicationFactory.instance.m_messageBus.PlayerMovedToTile += currentPlayerMovedToTile;
        ApplicationFactory.instance.m_messageBus.BallMovedToTile += ballMovedToTile;
        ApplicationFactory.instance.m_messageBus.BallPassEnded += ballPassEnded;

        ApplicationFactory.instance.m_messageBus.PlayerTurnEnded += playerTurnEnd;
        ApplicationFactory.instance.m_messageBus.UserPhaseEnded += userPhaseEnd;
    }

    private void userPhaseEnd(Team pTeam)
    {
        if (m_currentTeam == m_team1) m_currentTeam = m_team2;
        else m_currentTeam = m_team1;

        startUserPhase();
    }

    private void playerTurnEnd(Player pPlayer)
    {

    }

    private void startUserPhase()
    {
        PlayerTurnAnimation playerTurnAnimation = GUIManager.instance.createPlayerTurnAnimation();
        playerTurnAnimation.init();
        playerTurnAnimation.e_end += startPlayerTurn;
    }

    private void startPlayerTurn()
    {
        m_currentTeam.startTurn();
    }

    private void currentPlayerMovedToTile(Player pPlayer)
    {
        Player tacklePlayer;
        if (isPlayerTackled(pPlayer, out tacklePlayer))
        {
            ApplicationFactory.instance.m_messageBus.dispatchTackleBattleStart();

            FX02 fx = ApplicationFactory.instance.m_fxManager.createFX02(tacklePlayer.transform.position);
            fx.init();
            fx.e_end += startPlayerTackleTo;

            bool isDribble = UnityEngine.Random.RandomRange(0.0f, 1.0f) > 0.5f;
            
            m_tackleInfo = new TackleInfo();
            m_tackleInfo.m_isDribble = isDribble;
            m_tackleInfo.m_jumpPlayer = pPlayer;
            m_tackleInfo.m_tacklePlayer = tacklePlayer;
            m_tackleInfo.m_tackleToIndex = pPlayer.Index;

            if (isDribble)
            {
                if (pPlayer.isMoveEnded) m_tackleInfo.m_jumpToIndex = tacklePlayer.Index;
                else m_tackleInfo.m_jumpToIndex = pPlayer.nextMoveTileIndex;
            }
            else
            {
                m_tackleInfo.m_jumpToIndex = tacklePlayer.Index;
            }
        }
        else
        {
            pPlayer.moveToNextSquare();
        }
    }

    private void startPlayerTackleTo()
    {
        playerTackleTo(m_tackleInfo);

        if (m_tackleInfo.m_isDribble) tackleEnded += tackleDribbleEnd;
        else tackleEnded += tackleNoDribbleEnd;
    }

    public void playerTackleTo(TackleInfo pTackleInfo)
    {
        m_tackleInfo = pTackleInfo;

        if (m_tackleInfo.m_isDribble) tackleDribbleStart();
        else tackleNoDribbleStart();
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

                if (isGoal) fx.e_end += catchGoalStart;
                else fx.e_end += catchNoGoalStart;

                m_catchInfo = new CatchInfo();
                m_catchInfo.m_isGoal = isGoal;
                m_catchInfo.m_catchPlayer = cutPlayer;
            }
            else
            {
                bool isPass = UnityEngine.Random.RandomRange(0.0f, 1.0f) > 0.5f;

                if (isPass) fx.e_end += cutPassStart;
                else fx.e_end += cutNoPassStart;

                m_cutInfo = new CutInfo();
                m_cutInfo.m_isPass = isPass;
                m_cutInfo.m_cutPlayer = cutPlayer;
            }
        }
        else
        {
            pBall.moveToNextSquare();
        }
    }

    private void ballPassEnded(Ball pBall)
    {
        //Set the ball to the player in the tile
        Player player = m_board.getPlayerAtIndex(pBall.Index);
        player.setBall(pBall);
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

    #region TACKLE

    private void tackleDribbleStart()
    {
        SceneManager.instance.playTackle_Dribble(User.P1, User.P2);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += tackleSceneEnd;
    }

    private void tackleNoDribbleStart()
    {
        SceneManager.instance.playTackle_NoDribble(User.P1, User.P2);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += tackleSceneEnd;
    }

    private void tackleSceneEnd()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= tackleSceneEnd;

        m_tackleInfo.m_tacklePlayer.performTackle(m_tackleInfo.m_tackleToIndex);
        m_tackleInfo.m_jumpPlayer.performJump(m_tackleInfo.m_jumpToIndex);

        if (!m_tackleInfo.m_isDribble)
        {
            m_tackleInfo.m_jumpPlayer.takeBall();
            m_tackleInfo.m_tacklePlayer.setBall(m_ball);
        }

        m_tackleInfo.m_jumpPlayer.jumpEnd += jumpEnd;
    }

    private void jumpEnd()
    {
        m_tackleInfo.m_jumpPlayer.jumpEnd -= jumpEnd;
        if (tackleEnded != null) tackleEnded();
    }

    private void tackleNoDribbleEnd()
    {
        tackleEnded -= tackleNoDribbleEnd;

        //Set new turn
        PlayerTurnAnimation playerTurnAnimation = GUIManager.instance.createPlayerTurnAnimation();
        playerTurnAnimation.init();
        playerTurnAnimation.e_end += startPlayerTurn;
    }

    private void tackleDribbleEnd()
    {
        tackleEnded -= tackleDribbleEnd;
        m_tackleInfo.m_jumpPlayer.moveToNextSquare();
    }

    #endregion

    #region CUT

    private void cutPassStart()
    {
        SceneManager.instance.playCut_Pass(User.P1);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += cutSceneEnd;
    }

    private void cutNoPassStart()
    {
        SceneManager.instance.playCut_NoPass(User.P1);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += cutSceneEnd;
    }

    private void cutSceneEnd()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= cutSceneEnd;

        m_cutInfo.m_cutPlayer.performCut();

        if (m_cutInfo.m_isPass)
        {
            m_cutInfo.m_cutPlayer.cutEnd += cutPassEnd;
        }
        else
        {
            m_cutInfo.m_cutPlayer.setBall(m_ball);
            m_cutInfo.m_cutPlayer.cutEnd += cutNoPassEnd;
        }
    }

    private void cutPassEnd()
    {
        m_cutInfo.m_cutPlayer.cutEnd -= cutPassEnd;
        m_ball.moveToNextSquare();
    }

    private void cutNoPassEnd()
    {
        m_cutInfo.m_cutPlayer.cutEnd -= cutNoPassEnd;

        //Set new turn
        PlayerTurnAnimation playerTurnAnimation = GUIManager.instance.createPlayerTurnAnimation();
        playerTurnAnimation.init();
        playerTurnAnimation.e_end += startPlayerTurn;
    }

    #endregion

    #region CATCH

    private void catchGoalStart()
    {
        SceneManager.instance.playCatch_Goal(User.P1);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += catchSceneEnd;
    }

    private void catchNoGoalStart()
    {
        SceneManager.instance.playCatch_NoGoal(User.P1);
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded += catchSceneEnd;
    }

    private void catchSceneEnd()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= catchSceneEnd;

        m_catchInfo.m_catchPlayer.performCatch();

        if (m_catchInfo.m_isGoal)
        {
            m_catchInfo.m_catchPlayer.jumpEnd += catchGoalEnd;
        }
        else
        {
            m_catchInfo.m_catchPlayer.setBall(m_ball);
            m_catchInfo.m_catchPlayer.jumpEnd += catchNoGoalEnd;
        }
    }

    private void catchGoalEnd()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= catchGoalEnd;
    }

    private void catchNoGoalEnd()
    {
        ApplicationFactory.instance.m_messageBus.CurrentSceneEnded -= catchNoGoalEnd;

        //Set new turn
        PlayerTurnAnimation playerTurnAnimation = GUIManager.instance.createPlayerTurnAnimation();
        playerTurnAnimation.init();
        playerTurnAnimation.e_end += startPlayerTurn;
    }

    #endregion

    private bool isPlayerSelectable()
    {
        return (m_gameMode == GameModes.P1VsP2 && m_currentPlayer.team.m_user == m_currentPhase);
    }

    private void setArrowActive()
    {
        m_arrow.gameObject.SetActiveRecursively(true);
        m_arrow.init(m_currentPlayer.Index, (m_currentPlayer.isFliped ? -1 : 1));
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