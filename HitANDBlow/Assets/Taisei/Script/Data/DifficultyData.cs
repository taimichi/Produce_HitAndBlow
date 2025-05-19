using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/DifficultyData")]
public class DifficultyData : ScriptableObject
{
    public const string PATH = "DifficultyData";
    private static DifficultyData _difficultyEntity;
    public static DifficultyData DifficultyEntity
    {
        get
        {
            if (_difficultyEntity == null)
            {
                _difficultyEntity = Resources.Load<DifficultyData>(PATH);
                if (_difficultyEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _difficultyEntity;
        }
    }

    /// <summary>
    /// 難易度の種類
    /// </summary>
    public enum Difficult
    {
        none,   //選択していない
        easy,   //簡単
        normal, //普通
        hard    //難しい
    }

    /// <summary>
    /// 現在の難易度
    /// </summary>
    public Difficult nowDifficlt = Difficult.none;

}
