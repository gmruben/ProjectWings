using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    //SCENES
    public GameObject m_startScene;
    public GameObject m_volleyShotScene;
    public GameObject m_ballShotScene;
    public GameObject m_tackleNoDribbleScene;
    public GameObject m_tackleDribbleScene;
    public GameObject m_jumpScene;
    public GameObject m_gkCatchNoGoalScene;
    public GameObject m_gkCatchGoalScene;
    public GameObject m_goalScene;

    public GameObject m_playerMenuPrefab;
    public GameObject m_yesNoMenuPrefab;

    public GameObject m_playerTurnAnimationPrefab;

    private static GUIManager _instance;

    private PlayerMenu m_playerMenu;
    private YesNoMenu m_yesNoMenu;

    #region PUBLIC FUNCTIONS

    public PlayerTurnAnimation createPlayerTurnAnimation()
    {
        PlayerTurnAnimation playerTurnAnimation = (GameObject.Instantiate(m_playerTurnAnimationPrefab) as GameObject).GetComponent<PlayerTurnAnimation>();
        return playerTurnAnimation;
    }

    public PlayerMenu createPlayerMenu()
    {
        PlayerMenu menu = (GameObject.Instantiate(m_playerMenuPrefab) as GameObject).GetComponent<PlayerMenu>();
        return menu;
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

    public Scene createStartScene()
    {
        Scene scene = (GameObject.Instantiate(m_startScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createVolleyShotScene()
    {
        Scene scene = (GameObject.Instantiate(m_volleyShotScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createBallShotScene()
    {
        Scene scene = (GameObject.Instantiate(m_ballShotScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createTackleNoDribbleScene()
    {
        Scene scene = (GameObject.Instantiate(m_tackleNoDribbleScene) as GameObject).GetComponent<Scene>();

        return scene;
    }

    public Scene createTackleDribbleScene()
    {
        Scene scene = (GameObject.Instantiate(m_tackleDribbleScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createJumpScene()
    {
        Scene scene = (GameObject.Instantiate(m_jumpScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createGKCatchNoGoalScene()
    {
        Scene scene = (GameObject.Instantiate(m_gkCatchNoGoalScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createGKCatchGoalScene()
    {
        Scene scene = (GameObject.Instantiate(m_gkCatchGoalScene) as GameObject).GetComponent<Scene>();
        return scene;
    }

    public Scene createGoalScene()
    {
        Scene scene = (GameObject.Instantiate(m_goalScene) as GameObject).GetComponent<Scene>();
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
