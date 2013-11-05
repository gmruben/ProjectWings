using UnityEngine;
using System.Collections;

public class SpriteTrailSprite : MonoBehaviour
{
    public Material m_alphaMaterial;

    internal float m_life;
    private exSprite exSprite;

    public void init(exSprite pExSprite)
    {
        //Cache and init ex sprite component
        exSprite = GetComponent<exSprite>();
        exSprite.SetSprite(pExSprite.atlas, pExSprite.index);

        renderer.enabled = false;
        renderer.material = m_alphaMaterial;
    }

    public void revive(Vector3 pPosition)
    {
        transform.position = new Vector3(pPosition.x, pPosition.y, pPosition.z + 0.1f);
        m_life = 1.0f;
        renderer.enabled = true;
    }

    public void updateSprite()
    {
        m_life -= Time.deltaTime * 2;

        if (m_life < 0)
        {
            renderer.enabled = false;
        }
        else
        {
            renderer.material.SetFloat("_Alpha", m_life);
        }
    }
}

