using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public const float c_tileSize = 1;

    public const int SIZEX = 19;
    public const int SIZEY = 12;

    public exAtlas atlas;
    public GameObject m_tilePrefab;
    public GameObject m_tile;

    private int[][] m_boardData;

    private List<GameObject> m_tileList;

    private Ball m_ball;
    private List<Player> m_playerList;

    public void init(Ball pBall)
    {
        m_ball = pBall;

        //Init player list
        m_playerList = new List<Player>();

        //Get board data
        BoardData boardData = BoardDataBase.getBoardData("Board_01");

        m_boardData = new int[boardData.m_sizeX][];
        for (int i = 0; i < boardData.m_sizeX; i++)
        {
            m_boardData[i] = new int[boardData.m_sizeY];
            for (int j = 0; j < boardData.m_sizeY; j++)
            {
                GameObject go = GameObject.Instantiate(m_tile) as GameObject;
                go.transform.position = new Vector3(i, 0, j);
                go.transform.parent = transform;

                int index = atlas.GetIndexByName(boardData.m_boardTileData[i][j]);
                go.GetComponent<exSprite>().SetSprite(atlas, index);

                m_boardData[i][j] = BoardTileData.EMPTY;
            }
        }
    }

    public bool isPlayerOnTile(Vector2 index)
    {
        return m_boardData[(int)index.x][(int)index.y] == BoardTileData.PLAYER;
    }

    public void updateTile(Vector2 pIndex, int pValue)
    {
        m_boardData[(int)pIndex.x][(int)pIndex.y] = pValue;
    }

    public void drawTileRadius(Vector2 pTileIndex, int pRadius)
    {
        m_tileList = new List<GameObject>();

        for (int i = (int)pTileIndex.x - pRadius; i < (int)pTileIndex.x + pRadius + 1; i++)
        {
            for (int j = (int)pTileIndex.y - pRadius; j < (int)pTileIndex.y + pRadius + 1; j++)
            {
                if (Mathf.Abs(pTileIndex.x - i) +  Mathf.Abs(pTileIndex.y - j) <= pRadius &&
                   (i != pTileIndex.x || j != pTileIndex.y) &&
                   (i >= 0 && i < SIZEX && j >= 0 && j < SIZEY))
                {
                    drawTile(i, j);
                }
            }
        }
    }

    private void drawTile(float pX, float pY)
    {
        GameObject go = GameObject.Instantiate(m_tilePrefab) as GameObject;
        go.transform.position = new Vector3(pX, 0.05f, pY);

        m_tileList.Add(go);
    }

    public void drawShoot(Vector2 pIndex)
    {
        m_tileList = new List<GameObject>();

        //Tile index of the goal
        Vector2 goalIndex = new Vector2(18, 6);

        for (int i = (int)pIndex.x + 1; i < SIZEX; i++)
        {
            Vector2 start = new Vector2(i, 0);
            Vector2 end = new Vector2(i, 11);

            Debug.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y), Color.white, 500);
            Debug.DrawLine(new Vector3(pIndex.x, 0, pIndex.y), new Vector3(goalIndex.x, 0, goalIndex.y), Color.white, 500);

            Vector2 point = MathHelper.lineIntersectionPoint(start, end, pIndex, goalIndex);
            Vector2 index = tileIndexAtPoint(point);

            drawTile(index.x, index.y);
        }
    }

    public void clearTileRadius()
    {
        for (int i = 0; i < m_tileList.Count; i++)
        {
            Destroy(m_tileList[i]);
        }

        m_tileList.Clear();
    }

    public void ballToTile(Vector2 pIndex)
    {
        //If there was a player in that tile, give the ball to him
        if (m_boardData[(int)pIndex.x][(int)pIndex.y] == BoardTileData.PLAYER)
        {
            Player player = getPlayerAtIndex(pIndex);
            player.setBall(m_ball);
        }
        else
        {
            m_boardData[(int)pIndex.x][(int)pIndex.y] = BoardTileData.BALL;
        }
    }

    public void playerToTile(Player pPlayer)
    {
        //If the ball was in that tile, give it to the player
        if (m_boardData[(int)pPlayer.Index.x][(int)pPlayer.Index.y] == BoardTileData.BALL)
        {
            pPlayer.setBall(m_ball);
        }

        //Change board tile data
        m_boardData[(int)pPlayer.Index.x][(int)pPlayer.Index.y] = BoardTileData.PLAYER;
    }

    /// <summary>
    /// Adds a player to the board
    /// </summary>
    /// <param name="pPlayer">The player that is gonna be added</param>
    public void addPlayer(Player pPlayer, Vector2 pIndex)
    {
        //Set player index
        pPlayer.Index = pIndex;

        //Add player to the list and update board data
        m_playerList.Add(pPlayer);
        m_boardData[(int)pIndex.x][(int)pIndex.y] = BoardTileData.PLAYER;
    }

    public Player getPlayerAtIndex(Vector2 pIndex)
    {
        foreach (Player player in m_playerList)
        {
            if (player.Index == pIndex)
            {
                return player;
            }
        }

        return null;
    }

    public Vector2 tileIndexAtPoint(Vector2 pPoint)
    {
        int x = (int)((pPoint.x + (c_tileSize / 2)) / c_tileSize);
        int y = (int)((pPoint.y + (c_tileSize / 2)) / c_tileSize);

        return new Vector2(x, y);
    }

    #region PROPERTIES

    public int[][] data
    {
        get { return m_boardData; }
    }

    #endregion
}