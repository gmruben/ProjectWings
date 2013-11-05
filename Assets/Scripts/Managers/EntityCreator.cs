using UnityEngine;
using System.Collections;

public class EntityCreator
{
    public SpriteTrailSprite createSpriteTrailSprite(exSprite pExSprite)
    {
        SpriteTrailSprite sprite = (GameObject.Instantiate(ApplicationFactory.instance.m_emptyExSpritePrefab) as GameObject).GetComponent<SpriteTrailSprite>();
        sprite.init(pExSprite);

        return sprite;
    }
}