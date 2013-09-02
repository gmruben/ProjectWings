using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Arrow : MonoBehaviour
{
    public event Action<List<Vector2>> moveFinishedEvent;

    private const float COUNTER_TIME = 0.10f;

    public exAtlas atlas;

    private Vector2 startIndex;

    public GameObject arrayPart;

    private List<GameObject> partList = new List<GameObject>();
    private List<Vector2> tileIndexList = new List<Vector2>();

    private float counter;
    private bool m_isActive = false;

    private GameCamera m_camera;
    private Board m_board;

    public GameObject m_directions;

    #region BUILT-IN FUNCTIONS

    void Update()
    {
        if (!m_isActive)
        {
            return;
        }

        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            moveFinishedEvent(tileIndexList);
            clear();
        }
    }

    #endregion

    #region PUBLIC FUNCTIONS

    public void init(Vector2 pStart, int direction)
    {
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameCamera>();

        partList.Clear();
        tileIndexList.Clear();
        
        m_isActive = true;
        startIndex = pStart;
        add(pStart + new Vector2(direction, 0));

        m_directions.SetActiveRecursively(false);
    }

    #endregion

    #region PRIVATE FUNCTIONS

    private void sort()
    {
        for (int i = 1; i < partList.Count - 1; i++)
        {
            //int index = atlas.GetIndexByName("arrow03");
            //partList[i].GetComponent<exSprite>().SetSprite(atlas, index);
        }
    }

    /// <summary>
    /// Adds a new tile to the arrow
    /// </summary>
    /// <param name="index">Index of the tile to be added</param>
    private void add(Vector2 index)
    {
        GameObject part = GameObject.Instantiate(arrayPart) as GameObject;
        part.transform.position = new Vector3(index.x, 0.05f, index.y);
        part.transform.parent = transform;
        part.GetComponent<ArrowPart>().index = index;

        m_camera.setTarget(part.transform);

        partList.Add(part);
        tileIndexList.Add(index);

        //Move the directions
        m_directions.transform.position = new Vector3(index.x, 0.05f, index.y);

        sort();
    }

    private void move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            //Don´t allow diagonals
            if (direction.x != 0 && direction.y != 0)
            {
                direction.y = 0;
            }

            Vector2 index = partList[partList.Count - 1].GetComponent<ArrowPart>().index + direction;

            foreach (GameObject part in partList)
            {
                if (part.GetComponent<ArrowPart>().index == index)
                {
                    return;
                }
            }

            //If the index is inside the board and the tile is empty, add it
            if ((index.x >= 0 && index.x < Board.SIZEX && index.y >= 0 && index.y < Board.SIZEY) && !m_board.isPlayerOnTile(index))
            {
                add(index);
            }

            counter = COUNTER_TIME;
        }
    }

    /// <summary>
    /// Clears all the parts of the arrow
    /// </summary>
    private void clear()
    {
        m_isActive = false;

        foreach (GameObject part in partList)
        {
            Destroy(part);
        }
    }

    #endregion
}