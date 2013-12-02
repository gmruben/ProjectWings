using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour
{
    public Action CameraMovedToTargetEnded;

    private const float c_fieldOfView = 25.0f;
    private const float c_zoomSpeed = 10.0f;
    private const float c_speed = 2.5f;

    private Camera c_Camera;
    private Transform m_transform;

    private Transform m_target;
    private Vector3 m_targetPosition;

    void Start()
    {
        //Cache components
        m_transform = transform;
        c_Camera = camera;

        //Add listeners
        ApplicationFactory.instance.m_messageBus.TackleBattleStart += tackleBattleStart;
    }

    void Update()
    {
        if (m_target)
        {
            transform.position = new Vector3(m_target.position.x, transform.position.y, -9.5f);
        }
    }

    public void setTarget(Transform pTarget)
    {
        m_target = pTarget;
    }

    private void tackleBattleStart()
    {
        //StartCoroutine(zoomIn());
    }

    private IEnumerator zoomIn()
    {
        while (c_Camera.fieldOfView > 20)
        {
            c_Camera.fieldOfView -= Time.deltaTime * c_zoomSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void moveTo(Vector2 pTileIndex)
    {
        m_target = null;
        StartCoroutine(updateMoveTo(pTileIndex));
    }

    private IEnumerator updateMoveTo(Vector2 pTileIndex)
    {
        float tilex = pTileIndex.x * Board.c_tileSize;
        int sign = (int)(tilex - transform.position.x);

        while (Mathf.Abs(tilex - transform.position.x) > 0.25f)
        {
            m_transform.position += new Vector3(sign * c_speed * Time.deltaTime, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        m_transform.position = new Vector3(tilex, m_transform.position.y, m_transform.position.z);
        if (CameraMovedToTargetEnded != null) CameraMovedToTargetEnded();
    }
}