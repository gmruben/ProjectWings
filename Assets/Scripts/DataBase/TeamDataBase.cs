using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class TeamDataBase : MonoBehaviour
{
    private static Hashtable m_teamList;

    public static TeamData getTeamData(string ID)
    {
        //If the list is empty, load the data
        if (m_teamList == null)
        {
            loadData();
        }

        if (m_teamList.ContainsKey(ID))
        {
            return m_teamList[ID] as TeamData;
        }
        else
        {
            Debug.LogError("The team " + ID + " doesn't exist");
            return null;
        }
    }

    public static void loadData()
    {
        //Initialize team list
        m_teamList = new Hashtable();

        TextAsset playerListData = Resources.Load("Data/Teams") as TextAsset;

        XmlDocument xDoc = new XmlDocument();

        using (XmlReader reader = XmlReader.Create(new StringReader(playerListData.text)))
        {
            xDoc.Load(reader);

            XmlNodeList nodeList = xDoc.GetElementsByTagName("Team");

            foreach (XmlNode node in nodeList)
            {
                TeamData teamData = new TeamData();
                teamData.m_name = node.SelectSingleNode("Name").InnerText;
                
                XmlNodeList playerNodeList = node.SelectNodes("Player");
                foreach (XmlNode playerNode in playerNodeList)
                {
                    PlayerTeamData playerTeamData = new PlayerTeamData();

                    playerTeamData.m_name = playerNode.SelectSingleNode("Name").InnerText;
                    playerTeamData.m_tileX = Convert.ToInt32(playerNode.SelectSingleNode("TileX").InnerText);
                    playerTeamData.m_tileY = Convert.ToInt32(playerNode.SelectSingleNode("TileY").InnerText);

                    teamData.m_playerList.Add(playerTeamData);
                }

                m_teamList.Add(teamData.m_name, teamData);
            }
        }
    }
}


public class TeamData
{
    public string m_name;
    public List<PlayerTeamData> m_playerList = new List<PlayerTeamData>();
}

public class PlayerTeamData
{
    public string m_name;
    public int m_tileX;
    public int m_tileY;
}