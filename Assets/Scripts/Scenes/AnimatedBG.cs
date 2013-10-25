using UnityEngine;
using System.Collections;

public class AnimatedBG : MonoBehaviour
{
    public float m_speedX = 0.1f;
    public float m_speedY = 0.0f;

    private float m_offsetX = 0;
    private float m_offsetY = 0;
	
	void Update ()
    {
        m_offsetX += Time.deltaTime * m_speedX;
        m_offsetY += Time.deltaTime * m_speedY;

        renderer.material.SetTextureOffset("_MainTex", new Vector2(m_offsetX, m_offsetY));
	}
}
