using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteData", menuName = "ScriptableObjects/SpriteData")]
public class SpriteData : ScriptableObject
{
    public const string PATH = "SpriteData";
    private static SpriteData _spriteEntity;
    public static SpriteData SpriteEntity
    {
        get
        {
            if (_spriteEntity == null)
            {
                _spriteEntity = Resources.Load<SpriteData>(PATH);
                if (_spriteEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _spriteEntity;
        }
    }

    /// <summary>
    /// ”š‚Ì‰æ‘œ‚ğ“ü‚ê‚é”z—ñ
    /// </summary>
    public Sprite[] NumberSprite;
}
