using UnityEngine;
using System.Collections;

public class EntityCreator
{
    public Cursor createCursor()
    {
        Cursor cursor = (GameObject.Instantiate(ApplicationFactory.instance.m_cursor) as GameObject).GetComponent<Cursor>();
        return cursor;
    }

    public SpriteTrailSprite createSpriteTrailSprite(exSprite pExSprite)
    {
        SpriteTrailSprite sprite = (GameObject.Instantiate(ApplicationFactory.instance.m_emptyExSpritePrefab) as GameObject).GetComponent<SpriteTrailSprite>();
        sprite.init(pExSprite);

        return sprite;
    }
}