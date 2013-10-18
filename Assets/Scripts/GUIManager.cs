using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public GameObject m_playerMenuPrefab;
    public GameObject m_yesNoMenuPrefab;

    private static GUIManager _instance;

    #region PROPERTIES

    public static GUIManager instance
    {
        get
        {
            if (_instance == null)
            {
                //Find game object
                GameObject guiManager = GameObject.Find("GUIManager");

                if (guiManager == null)
                {
                    Debug.LogError("Couldn't find GUIManager instance");
                }

                //Get Custom Network component
                _instance = guiManager.GetComponent<GUIManager>();
            }

            return _instance;
        }
    }

    #endregion

    public PlayerMenu showAtkMenu()
    {
        PlayerMenu menu = (GameObject.Instantiate(m_playerMenuPrefab) as GameObject).GetComponent<PlayerMenu>();
        //menu.init(m_currentPlayer);

        return menu;
    }

    public static void showDefMenu()
    {

    }

    public static void showPlayerContMenu()
    {

    }

    public static void showGKContMenu()
    {

    }

    public YesNoMenu showYesNoMenu()
    {
        YesNoMenu menu = (GameObject.Instantiate(m_yesNoMenuPrefab) as GameObject).GetComponent<YesNoMenu>();
        menu.init();

        return menu;
    }
}
