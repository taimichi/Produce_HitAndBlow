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
    /// 数字の画像を入れる配列
    /// </summary>
    public Sprite[] NumberSprite;

    /// <summary>
    /// 数値が設定されていないときの画像
    /// </summary>
    public Sprite NoneNumberSprite;

    /// <summary>
    /// ヒットアンドブローの画像を入れる配列
    /// </summary>
    public Sprite[] HitBlowSprite;

}
