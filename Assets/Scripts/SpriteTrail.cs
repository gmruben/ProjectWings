using UnityEngine;
using System.Collections;

public class SpriteTrail : MonoBehaviour
{
    private Transform m_transform;

    private const int c_numSprites = 25;
    private SpriteTrailSprite[] sprites;

    private float m_spawnTime;
    private int m_currentIndex;

    private bool m_isActive;

    public void init(Transform pTransform)
    {
        m_transform = pTransform;

        sprites = new SpriteTrailSprite[c_numSprites];

        for (int i = 0; i < c_numSprites; i++)
        {
            sprites[i] = ApplicationFactory.instance.m_entityCreator.createSpriteTrailSprite(GetComponent<exSprite>());
        }
    }

    public void setActive(bool pIsActive)
    {
        m_isActive = pIsActive;

        if (pIsActive)
        {
            spawnSprite();
        }
        else
        {
            for (int i = 0; i < c_numSprites; i++)
            {
                sprites[i].m_life = 0;
                sprites[i].renderer.enabled = false;
            }
        }
    }

    void Update()
    {
        if (m_isActive)
        {
            if (Time.time > m_spawnTime)
            {
                spawnSprite();
            }

            //Update sprites
            for (int i = 0; i < c_numSprites; i++)
            {
                sprites[i].updateSprite();
            }
        }
    }

    private void spawnSprite()
    {
        if (m_currentIndex >= c_numSprites) m_currentIndex = 0;
        sprites[m_currentIndex].revive(m_transform.position);

        m_currentIndex++;
        m_spawnTime = Time.time + 0.05f;
    }
}