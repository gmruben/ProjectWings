using UnityEngine;
using System.Collections;

public class ApplicationFactory : MonoBehaviour
{
    //ENTITY PREFABS
    public GameObject m_cursorPrefab;
    public GameObject m_arrow;
    public GameObject m_emptyExSpritePrefab;

    private static ApplicationFactory _instance;

    public MessageBus m_messageBus;
    public EntityCreator m_entityCreator;
    public LanguageManager m_languageManager;

    [HideInInspector]
    public FXManager m_fxManager;

    #region PROPERTIES

    public static ApplicationFactory instance
    {
        get
        {
            if (_instance == null)
            {
                //Find game object
                GameObject applicationFactory = GameObject.Find("ApplicationFactory");

                if (applicationFactory == null)
                {
                    Debug.LogError("Couldn't find ApplicationFactory instance");
                }

                //Get Custom Network component
                _instance = applicationFactory.GetComponent<ApplicationFactory>();

                //Initialize instance
                _instance.init();
            }

            return _instance;
        }
    }

    #endregion

    private void init()
    {
        m_messageBus = new MessageBus();
        m_entityCreator = new EntityCreator();
        m_languageManager = new LanguageManager();

        m_fxManager = GameObject.Find("FXManager").GetComponent<FXManager>();
    }
}
