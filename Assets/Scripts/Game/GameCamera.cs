using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
    private Transform m_target;

    void Update()
    {
        if (m_target)
        {
            transform.position = new Vector3(m_target.position.x, transform.position.y, m_target.position.z - 12.5f);
        }
    }

    public void setTarget(Transform pTarget)
    {
        m_target = pTarget;
    }
}