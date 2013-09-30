using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder
{
    private int mapWidth = 10;
    private int mapHeight = 10;

    private BinaryHeap<Node> m_openList;
    private List<Vector2> m_closedList;

    private int[][] m_boardData;

    private bool[][] isOnOpenList;
    private Vector2[][] parent;
    private int[][] Gcost;
    
    public bool isTherePath;

    public Vector2[] findPath(Vector2 start, Vector2 target, int[][] pBoardData)
    {
        mapWidth = pBoardData.Length;
        mapHeight = pBoardData[0].Length;

        m_boardData = pBoardData;
        isTherePath = false;

        //Initialize lists
        m_openList = new BinaryHeap<Node>(mapWidth * mapHeight + 2);
        m_closedList = new List<Vector2>();

        //Initialize arrays
        isOnOpenList = new bool[mapWidth + 1][];
        parent = new Vector2[mapWidth + 1][];
        Gcost = new int[mapWidth + 1][];
        
        for (int i = 0; i < mapWidth + 1; i++)
        {
            isOnOpenList[i] = new bool[mapHeight + 1];
            parent[i] = new Vector2[mapWidth + 1];
            Gcost[i] = new int[mapWidth + 1];
        }

        int parentXval = 0, parentYval = 0, a = 0, b = 0, addedGCost = 0, tempGcost = 0;

        //If the target position is the same as the start position
        if (start == target)
        {
            Debug.Log("NO PATH");
            return null;
        }

        Gcost[(int)start.x][(int)start.y] = 0; //reset starting square's G value to 0

        //Add the starting location to the open list of squares to be checked.
        isOnOpenList[(int)start.x][(int)start.y] = true;
        m_openList.add(new Node(start, 0, 0));

        //5.Do the following until a path is found or deemed nonexistent.
        do
        {
            //If the open list is not empty, take the first cell off of the list. This is the lowest F cost cell on the open list.

            if (m_openList.count != 0)
            {
                //Pop the first item off the open list.
                Node node = m_openList.remove();
                m_closedList.Add(node.index);

                parentXval = (int)node.index.x;
                parentYval = (int)node.index.y;

                //7.Check the adjacent squares. (Its "children" -- these path children
                //	are similar, conceptually, to the binary heap children mentioned
                //	above, but don't confuse them. They are different. Path children
                //	are portrayed in Demo 1 with grey pointers pointing toward
                //	their parents.) Add these adjacent child squares to the open list
                //	for later consideration if appropriate (see various if statements
                //	below).
                for (b = parentYval - 1; b <= parentYval + 1; b++)
                {
                    for (a = parentXval - 1; a <= parentXval + 1; a++)
                    {
                        //	If not off the map (do this first to avoid array out-of-bounds errors)
                        if (a != -1 && b != -1 && a != mapWidth && b != mapHeight)
                        {
                            //If not already on the closed list (items on the closed list have already been considered and can now be ignored)
                            if (!m_closedList.Contains(new Vector2(a, b)))
                            {
                                //If not a wall/obstacle square.
                                if (m_boardData[a][b] == BoardTileData.EMPTY)
                                {
                                    //Don't cut across corners
                                    bool isCorner = false;
                                    if (a == parentXval - 1)
                                    {
                                        if (b == parentYval - 1)
                                        {
                                            if (m_boardData[parentXval - 1][parentYval] != BoardTileData.EMPTY || m_boardData[parentXval][parentYval - 1] != BoardTileData.EMPTY)
                                            {
                                                isCorner = true;
                                            }
                                        }
                                        else if (b == parentYval + 1)
                                        {
                                            if (m_boardData[parentXval][parentYval + 1] != BoardTileData.EMPTY || m_boardData[parentXval - 1][parentYval] != BoardTileData.EMPTY)
                                                isCorner = true;
                                        }
                                    }
                                    else if (a == parentXval + 1)
                                    {
                                        if (b == parentYval - 1)
                                        {
                                            if (m_boardData[parentXval][parentYval - 1] != BoardTileData.EMPTY || m_boardData[parentXval + 1][parentYval] != BoardTileData.EMPTY)
                                            {
                                                isCorner = true;
                                            }
                                        }
                                        else if (b == parentYval + 1)
                                        {
                                            if (m_boardData[parentXval + 1][parentYval] != BoardTileData.EMPTY || m_boardData[parentXval][parentYval + 1] != BoardTileData.EMPTY)
                                            {
                                                isCorner = true;
                                            }
                                        }
                                    }
                                    if (isCorner == false)
                                    {
                                        //If not already on the open list, add it to the open list.			
                                        if (!isOnOpenList[a][b])
                                        {
                                            //Figure out its G cost
                                            if (Mathf.Abs(a - parentXval) == 1 && Mathf.Abs(b - parentYval) == 1)
                                            {
                                                //Cost of going to diagonal squares
                                                addedGCost = 14;
                                            }
                                            else
                                            {
                                                //Cost of going to non-diagonal squares
                                                addedGCost = 10;
                                            }
                                            Gcost[a][b] = Gcost[parentXval][parentYval] + addedGCost;

                                            //Figure out its H and F costs and parent
                                            parent[a][b] = new Vector2(parentXval, parentYval);

                                            //Change whichList to show that the new item is on the open list.
                                            isOnOpenList[a][b] = true;

                                            int hcost = 10 * (Mathf.Abs(a - (int)target.x) + Mathf.Abs(b - (int)target.y));
                                            int fcost = Gcost[a][b] + hcost;

                                            //Add the node to the open list
                                            m_openList.add(new Node(new Vector2(a, b), fcost, hcost));
                                        }
                                        //8.If adjacent cell is already on the open list, check to see if this 
                                        //	path to that cell from the starting location is a better one. 
                                        //	If so, change the parent of the cell and its G and F costs.	
                                        else
                                        {
                                            //Figure out the G cost of this possible new path
                                            if (Mathf.Abs(a - parentXval) == 1 && Mathf.Abs(b - parentYval) == 1)
                                            {
                                                //Cost of going to diagonal tiles
                                                addedGCost = 14;
                                            }
                                            else
                                            {
                                                //Cost of going to non-diagonal tiles
                                                addedGCost = 10;
                                            }
                                            tempGcost = Gcost[parentXval][parentYval] + addedGCost;

                                            //If this path is shorter (G cost is lower) then change the parent cell, G cost and F cost. 		
                                            if (tempGcost < Gcost[a][b]) //if G cost is less,
                                            {
                                                parent[a][b] = new Vector2(parentXval, parentYval); //change the square's parent
                                                Gcost[a][b] = tempGcost;//change the G cost			

                                                //Because changing the G cost also changes the F cost, if
                                                //the item is on the open list we need to change the item's
                                                //recorded F cost and its position on the open list to make
                                                //sure that we maintain a properly ordered open list.
                                                for (int x = 1; x <= m_openList.count; x++)
                                                {
                                                    //If it is the current node
                                                    if (m_openList.items[x].index.x == a && m_openList.items[x].index.y == b)
                                                    {
                                                        //Change the F cost
                                                        m_openList.items[x].FCost = Gcost[a][b] + m_openList.items[x].HCost;
                                                        m_openList.updateItemAtIndex(x);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //If open list is empty then there is no path.	
            else
            {
                isTherePath = false;
                break;
            }

            //If target is added to open list then path has been found.
            if (isOnOpenList[(int)target.x][(int)target.y])
            {
                isTherePath = true;
                break;
            }

        }
        while (true);//Do until path is found or deemed nonexistent

        //If there is a path, save it
        if (isTherePath)
        {
            Vector2 node = target;
            List<Vector2> nodeList = new List<Vector2>();

            //Working backwards from the target to the starting location by checking each cell's parent, figure out the length of the path.
            do
            {
                nodeList.Add(node);
                node = parent[(int)node.x][(int)node.y];
            }
            while (node != start);

            //Create the array
            int index = 1;
            Vector2[] path = new Vector2[nodeList.Count + 1];
            path[0] = start;
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                path[index] = nodeList[i];
                index++;
            }

            return path;
        }
        else
        {
            return null;
        }
    }
}