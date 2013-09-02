using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class BoardDataBase : MonoBehaviour
{
    private static Hashtable m_boardList;

    public static BoardData getBoardData(string ID)
    {
        //If the list is empty, load the data
        if (m_boardList == null)
        {
            loadData();
        }

        if (m_boardList.ContainsKey(ID))
        {
            return m_boardList[ID] as BoardData;
        }
        else
        {
            Debug.LogError("The board " + ID + " doesn't exist");
            return null;
        }
    }

    public static void loadData()
    {
        //Initialize team list
        m_boardList = new Hashtable();

        TextAsset boardListData = Resources.Load("Data/Boards") as TextAsset;

        XmlDocument xDoc = new XmlDocument();

        using (XmlReader reader = XmlReader.Create(new StringReader(boardListData.text)))
        {
            xDoc.Load(reader);

            XmlNodeList nodeList = xDoc.GetElementsByTagName("Board");

            foreach (XmlNode node in nodeList)
            {
                BoardData boardData = new BoardData();
                boardData.m_name = node.SelectSingleNode("Name").InnerText;
                boardData.m_sizeX = Convert.ToInt32(node.SelectSingleNode("SizeX").InnerText);
                boardData.m_sizeY = Convert.ToInt32(node.SelectSingleNode("SizeY").InnerText);
                
                //Create array
                boardData.m_boardTileData = new string[boardData.m_sizeX][];
                for (int i = 0; i < boardData.m_sizeX; i++)
                {
                    boardData.m_boardTileData[i] = new string[boardData.m_sizeY];
                }

                XmlNodeList tileNodeList = node.SelectNodes("Tile");
                foreach (XmlNode tileNode in tileNodeList)
                {
                    string name = tileNode.SelectSingleNode("Name").InnerText;
                    int tileX = Convert.ToInt32(tileNode.SelectSingleNode("X").InnerText);
                    int tileY = Convert.ToInt32(tileNode.SelectSingleNode("Y").InnerText);

                    //Add index to the array
                    boardData.m_boardTileData[tileX][tileY] = name;
                }

                m_boardList.Add(boardData.m_name, boardData);
            }
        }
    }
}


public class BoardData
{
    public string m_name;
    public int m_sizeX;
    public int m_sizeY;
    public string[][] m_boardTileData;
}