using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour
{
    private const float c_zoomSpeed = 10.0f;

    private Camera c_Camera;

    private Transform m_target;
    private Vector3 m_targetPosition;

    void Start()
    {
        //Cache components
        c_Camera = camera;

        //Add listeners
        ApplicationFactory.instance.m_messageBus.TackleBattleStart += tackleBattleStart;
    }

    void Update()
    {
        if (m_target)
        {
            transform.position = new Vector3(m_target.position.x, transform.position.y, -9.5f); //m_target.position.z - 12.5f);
        }
    }

    public void setTarget(Transform pTarget)
    {
        m_target = pTarget;
    }

    private void tackleBattleStart()
    {
        StartCoroutine(zoomIn());
    }

    private IEnumerator zoomIn()
    {
        while (c_Camera.fieldOfView > 20)
        {
            c_Camera.fieldOfView -= Time.deltaTime * c_zoomSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Scene scene = GUIManager.instance.createTackleScene();

        scene.play();
        //scene.e_end += dribbleEnded;
    }
}