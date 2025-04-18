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
    /// ��Փx�̎��
    /// </summary>
    public enum Difficult
    {
        none,   //�I�����Ă��Ȃ�
        easy,   //�ȒP
        normal, //����
        hard    //���
    }

    /// <summary>
    /// ���݂̓�Փx
    /// </summary>
    public Difficult nowDifficlt = Difficult.none;

}
