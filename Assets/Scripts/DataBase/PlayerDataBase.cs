using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class PlayerDataBase : MonoBehaviour
{
    private static Hashtable m_playerList;

    public static PlayerData getPlayerData(string ID)
    {
        //If the list is empty, load the data
        if (m_playerList == null)
        {
            loadData();
        }

        if (m_playerList.ContainsKey(ID))
        {
            return m_playerList[ID] as PlayerData;
        }
        else
        {
            Debug.LogError("The player " + ID + " doesn't exist");
            return null;
        }
    }

    public static void loadData()
    {
        //Initialize player list
        m_playerList = new Hashtable();

        TextAsset playerListData = Resources.Load("Data/Players") as TextAsset;

        XmlDocument xDoc = new XmlDocument();

        using (XmlReader reader = XmlReader.Create(new StringReader(playerListData.text)))
        {
            xDoc.Load(reader);

            XmlNodeList nodeList = xDoc.GetElementsByTagName("Player");

            foreach (XmlNode node in nodeList)
            {
                PlayerData playerData = new PlayerData();

                playerData.m_name = node.SelectSingleNode("Name").InnerText;
                playerData.m_position = node.SelectSingleNode("Position").InnerText;

                m_playerList.Add(playerData.m_name, playerData);
            }
        }
    }
}

public class PlayerData
{
    public string m_name;
    public string m_position;
}

public class PlayerPosition
{
    public const string GK = "GK";
    public const string DF = "DF";
    public const string MF = "MF";
}