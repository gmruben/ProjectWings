using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    //SCENES
    public GameObject m_volleyShotScene;
    public GameObject m_tackleScene;

    public GameObject m_playerMenuPrefab;
    public GameObject m_yesNoMenuPrefab;

    private static GUIManager _instance;

    private PlayerMenu m_playerMenu;
    private YesNoMenu m_yesNoMenu;

    #region PUBLIC FUNCTIONS

    public PlayerMenu showAtkMenu()
    {
        PlayerMenu menu = (GameObject.Instantiate(m_playerMenuPrefab) as GameObject).GetComponent<PlayerMenu>();
        //menu.init(m_currentPlayer);

        return menu;
    }

    public void showDefMenu()
    {

    }

    public void showPlayerContMenu()
    {

    }

    public PlayerMenu showGKContMenu()
    {
        m_playerMenu = (GameObject.Instantiate(m_playerMenuPrefab) as GameObject).GetComponent<PlayerMenu>();
        m_playerMenu.setGKData();

        return m_playerMenu;
    }

    public YesNoMenu showYesNoMenu()
    {
        m_yesNoMenu = (GameObject.Instantiate(m_yesNoMenuPrefab) as GameObject).GetComponent<YesNoMenu>();
        m_yesNoMenu.init();

        return m_yesNoMenu;
    }

    public Scene createVolleyShotScene()
    {
        Scene scene = (GameObject.Instantiate(m_volleyShotScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createTackleScene()
    {
        Scene scene = (GameObject.Instantiate(m_tackleScene) as GameObject).GetComponent<Scene>();

        return scene;
    }

    #endregion

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
}
