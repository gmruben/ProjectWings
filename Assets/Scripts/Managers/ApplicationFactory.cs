using UnityEngine;
using System.Collections;

public class ApplicationFactory : MonoBehaviour
{
    private static ApplicationFactory _instance;

    private MessageBus m_messageBus;

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
    }
}
