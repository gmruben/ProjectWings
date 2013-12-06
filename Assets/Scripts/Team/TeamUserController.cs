using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamUserController : TeamController
{
    private int m_turnIndex;

    private Player m_currentPlayer;
    private Cursor m_cursor;
    private Arrow m_arrow;

    private PlayerMenu m_currentMenu;

    public override void init(Game pGame, GameCamera pGameCamera, Board pBoard, Team pTeam)
    {
        m_game = pGame;
        m_gameCamera = pGameCamera;
        m_board = pBoard;
        m_team = pTeam;

        m_turnIndex = 3;

        m_cursor = ApplicationFactory.instance.m_entityCreator.createCursor();
        m_cursor.init();

        m_arrow = ApplicationFactory.instance.m_entityCreator.createArrow();
    }

    public override void startTurn()
    {
        m_cursor.setActive(true);

        if (m_team.m_playerWithTheBall != null)
        {
            m_cursor.setIndex(m_team.m_playerWithTheBall.Index);
            m_gameCamera.moveTo(m_team.m_playerWithTheBall.Index);
        }

        //Add listener
        m_cursor.e_end += startPlayerTurn;
    }

    public void startPlayerTurn(Vector2 index)
    {
        //If there is a player on the tile
        if (m_board.isPlayerOnTile(index))
        {
            //Get the player on the tile
            m_currentPlayer = m_board.getPlayerAtIndex(index);

            //If the player is in my team
            if (m_currentPlayer.team.m_user == m_team.m_user)
            {
                //Remove listeners
                m_cursor.e_end -= startPlayerTurn;                

                m_cursor.gameObject.SetActiveRecursively(false);
                showPlayerMenu();
            }
        }
    }

    public void showPlayerMenu()
    {
        m_currentMenu = GUIManager.instance.createPlayerMenu();
        m_currentMenu.init(m_currentPlayer);

        m_currentMenu.e_selected += optionSelected;
        m_currentMenu.e_cancel += cancelPlayerMenu;
    }

    public void optionSelected(int optionId)
    {
        //Remove listeners
        m_currentMenu.e_selected -= optionSelected;
        m_currentMenu.e_cancel -= cancelPlayerMenu;

        switch (optionId)
        {
            case PlayerAction.Move:
                move();
                break;
            case PlayerAction.Pass:
                pass();
                break;
            case PlayerAction.Shoot:
                shoot();
                break;
            //case PlayerAction.EndTurn:
            //    endTurn();
            //    break;
            case PlayerAction.Tackle:
                tackle();
                break;
        }

        GameObject.Destroy(m_currentMenu.gameObject);
    }

    private void move()
    {
        m_board.drawTileRadius(m_currentPlayer.Index, 3);

        m_arrow.gameObject.SetActiveRecursively(true);
        m_arrow.init(m_currentPlayer.Index, (m_currentPlayer.isFliped ? -1 : 1));

        m_arrow.e_end += startMove;
        m_arrow.e_cancel += cancelMove;
    }

    private void startMove(List<Vector2> pTileIndexList)
    {
        //Remove listeners
        m_arrow.e_end -= startMove;
        m_arrow.e_cancel -= cancelMove;

        m_currentPlayer.move(pTileIndexList);

        ApplicationFactory.instance.m_messageBus.PlayerMoveEnded += playerMoveEnded;

        m_board.clearTileRadius();
    }

    private void cancelMove()
    {
        m_arrow.e_end -= startMove;
        m_arrow.e_cancel -= cancelMove;

        m_board.clearTileRadius();

        showPlayerMenu();
    }

    private void playerMoveEnded(Player pPlayer)
    {
        ApplicationFactory.instance.m_messageBus.PlayerMoveEnded -= playerMoveEnded;
        
        //showPlayerMenu();
        endTurn();
    }

    private void pass()
    {
        m_cursor.setIndex(m_currentPlayer.Index);
        m_cursor.gameObject.SetActiveRecursively(true);

        //Draw board tiles
        m_board.drawPass(m_currentPlayer.Index, m_currentPlayer.Index);

        //Add listeners
        m_cursor.e_move += drawPassTiles;
        m_cursor.e_end += startPass;
        m_arrow.e_cancel += cancelPass;
    }

    private void startPass(Vector2 pIndex)
    {
        m_cursor.e_move -= drawPassTiles;
        m_cursor.e_end -= startPass;
        m_arrow.e_cancel -= cancelMove;

        m_currentPlayer.passTo(pIndex, m_board.currentTileList);

        m_board.clearTileRadius();
        m_cursor.setActive(false);

        ApplicationFactory.instance.m_messageBus.BallPassEnded += ballPassEnded;
    }

    private void cancelPass()
    {
        m_cursor.e_move -= drawPassTiles;
        m_cursor.e_end -= startPass;
        m_arrow.e_cancel -= cancelPass;

        m_board.clearTileRadius();
        m_cursor.setActive(false);

        showPlayerMenu();
    }

    private void ballPassEnded(Ball pBall)
    {
        m_gameCamera.moveTo(m_currentPlayer.Index);

        ApplicationFactory.instance.m_messageBus.BallPassEnded -= ballPassEnded;
        m_gameCamera.CameraMovedToTargetEnded += ballPassEndCameraMoveEnded;
    }

    private void ballPassEndCameraMoveEnded()
    {
        m_gameCamera.CameraMovedToTargetEnded -= ballPassEndCameraMoveEnded;
        //showPlayerMenu();
        endTurn();
    }

    private void shoot()
    {
        m_cursor.setIndex(m_currentPlayer.Index);
        m_cursor.gameObject.SetActiveRecursively(true);

        //Draw board tiles
        m_board.drawShoot(m_currentPlayer.Index);

        //Add listeners
        m_cursor.e_end += startShot;
        m_cursor.e_cancel += cancelShot;
    }

    private void startShot(Vector2 pIndex)
    {
        m_cursor.setActive(false);

        //Remove board tiles
        m_board.clearTileRadius();

        //Shoot the ball
        m_currentPlayer.shootTo();
    }

    private void cancelShot()
    {
        m_arrow.e_end -= startMove;
        m_arrow.e_cancel -= cancelMove;

        showPlayerMenu();
        m_board.clearTileRadius();
    }

    private void shotEnded(Ball pBall)
    {
        //showPlayerMenu();
        endTurn();
    }

    private void drawPassTiles(Vector2 pIndex)
    {
        //Remove board tiles
        m_board.clearTileRadius();

        //Draw board tiles
        m_board.drawPass(m_currentPlayer.Index, pIndex);
    }

    private void tackle()
    {
        m_cursor.setIndex(m_currentPlayer.Index);
        m_cursor.setActive(true);

        m_board.drawTileRadius(m_currentPlayer.Index, 1);

        m_cursor.e_end += startTackle;
        m_cursor.e_cancel += cancelTackle;
    }

    private void startTackle(Vector2 pIndex)
    {
        m_cursor.e_end -= startTackle;
        m_cursor.e_cancel -= cancelTackle;

        Player playerToTackle = m_board.getPlayerAtIndex(pIndex);

        bool isDribble = UnityEngine.Random.RandomRange(0.0f, 1.0f) > 0.5f;

        TackleInfo tackleInfo = new TackleInfo();
        tackleInfo.m_isDribble = isDribble;
        tackleInfo.m_jumpPlayer = playerToTackle;
        tackleInfo.m_tacklePlayer = m_currentPlayer;
        tackleInfo.m_jumpToIndex = m_currentPlayer.Index;
        tackleInfo.m_tackleToIndex = pIndex;

        m_game.playerTackleTo(tackleInfo);
        m_game.tackleEnded += playerTackleEnded;

        m_board.clearTileRadius();
        m_cursor.setActive(false);
    }

    private void cancelTackle()
    {
        m_cursor.e_end -= startTackle;
        m_arrow.e_cancel -= cancelTackle;

        showPlayerMenu();

        m_board.clearTileRadius();
        m_cursor.setActive(false);
    }

    private void playerTackleEnded()
    {
        m_game.tackleEnded -= playerTackleEnded;
        //showPlayerMenu();
        endTurn();
    }

    private void cancelPlayerMenu()
    {
        m_currentMenu.e_selected -= optionSelected;
        m_currentMenu.e_cancel -= cancelPlayerMenu;

        GameObject.Destroy(m_currentMenu.gameObject);

        startTurn();
    }

    private void endTurn()
    {
        m_turnIndex--;
        if (m_turnIndex == 0)
        {
            endPhase();
        }
        else
        {
            startTurn();
            ApplicationFactory.instance.m_messageBus.dispatchPlayerTurnEnded(m_turnIndex);
        }
    }

    private void endPhase()
    {
        ApplicationFactory.instance.m_messageBus.dispatchUserPhaseEnded(m_team);
    }
}