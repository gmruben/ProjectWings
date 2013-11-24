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

    public override void init(Board pBoard, Team pTeam)
    {
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
        m_cursor.setIndex(m_team.m_playerList[1].Index);

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

            //If player hasn't ended his turn yet and it is on my team
            if (!m_currentPlayer.m_hasEndedTurn && m_currentPlayer.team.m_user == m_team.m_user)
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
        m_currentMenu.e_cancel += cancelBox;
    }

    public void optionSelected(int optionId)
    {
        //Remove listeners
        m_currentMenu.e_selected -= optionSelected;
        m_currentMenu.e_cancel -= cancelBox;

        switch (optionId)
        {
            case PlayerAction.Move:
                m_board.drawTileRadius(m_currentPlayer.Index, 3);

                m_arrow.gameObject.SetActiveRecursively(true);
                m_arrow.init(m_currentPlayer.Index, (m_currentPlayer.isFliped ? -1 : 1));

                m_arrow.e_end += endMove;
                m_arrow.e_cancel += cancelMove;

                break;
            case PlayerAction.Pass:
                m_cursor.gameObject.SetActiveRecursively(true);
                //m_camera.setTarget(m_cursor.transform);

                //Add listeners
                //m_cursor.e_end += passTo;

                break;
            case PlayerAction.Shoot:
                m_cursor.setIndex(m_currentPlayer.Index);
                m_cursor.gameObject.SetActiveRecursively(true);

                //m_camera.setTarget(m_cursor.transform);

                //Draw board tiles
                m_board.drawShoot(m_currentPlayer.Index);

                //Add listeners
                m_cursor.e_end += startShot;
                //m_cursor.e_cancel += cancelShoot;

                break;
            case PlayerAction.EndTurn:
                //Finish current player turn
                m_currentPlayer.m_hasEndedTurn = true;
                m_currentPlayer.renderer.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f));

                m_cursor.setActive(true);
                //m_cursor.e_end += selectPlayer;

                //Dispatch event
                m_turnIndex--;
                if (m_turnIndex == 0)
                {
                    //m_currentPhase = (m_currentPhase == User.P1) ? User.P2 : User.P1;
                    //m_turnIndex = c_numberOfTurnsInAPhase;
                    //if (e_startPhase != null) e_startPhase(m_currentPhase);
                }
                else
                {
                    //if (e_endTurn != null) e_endTurn(m_turnIndex);
                }

                break;
            case PlayerAction.Tackle:
                //Paint the tiles where the player can tackle
                m_board.drawTileRadius(m_currentPlayer.Index, 1);

                //Set the cursor active
                m_cursor.setActive(true);
                //m_cursor.e_end += tackleTo;
                m_cursor.e_cancel += cancelMove;

                break;
        }

        GameObject.Destroy(m_currentMenu.gameObject);
    }

    private void startShot(Vector2 pIndex)
    {
        m_cursor.setActive(false);

        //Remove board tiles
        m_board.clearTileRadius();

        //Shoot the ball
        m_currentPlayer.shootTo();
    }

    private void cancelBox()
    {
        //Remove listeners
        m_currentMenu.e_selected -= optionSelected;
        m_currentMenu.e_cancel -= cancelBox;

        m_currentMenu.gameObject.SetActiveRecursively(false);

        //setCursorActive();
    }

    private void endMove(List<Vector2> pTileIndexList)
    {
        //Remove listeners
        m_arrow.e_end -= endMove;
        m_arrow.e_cancel -= cancelMove;

        m_currentPlayer.move(pTileIndexList);
        m_currentPlayer.moveFinishedEvent += currentPlayerMoveFinished;

        m_board.clearTileRadius();
    }

    private void cancelMove()
    {
        //Remove listeners
        m_arrow.e_end -= endMove;
        m_arrow.e_cancel -= cancelMove;

        //showPlayerMenu();
    }

    private void currentPlayerMoveFinished()
    {
        //Remove listeners
        m_currentPlayer.moveFinishedEvent -= currentPlayerMoveFinished;

        showPlayerMenu();
    }
}