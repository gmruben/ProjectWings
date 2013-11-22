using UnityEngine;
using System.Collections;

public class EntityCreator
{
    public Cursor createCursor()
    {
        Cursor cursor = (GameObject.Instantiate(ApplicationFactory.instance.m_cursor) as GameObject).GetComponent<Cursor>();
        return cursor;
    }

    public Arrow createArrow()
    {
        Arrow arrow = (GameObject.Instantiate(ApplicationFactory.instance.m_arrow) as GameObject).GetComponent<Arrow>();
        return arrow;
    }

    public SpriteTrailSprite createSpriteTrailSprite(exSprite pExSprite)
    {
        SpriteTrailSprite sprite = (GameObject.Instantiate(ApplicationFactory.instance.m_emptyExSpritePrefab) as GameObject).GetComponent<SpriteTrailSprite>();
        sprite.init(pExSprite);

        return sprite;
    }
}