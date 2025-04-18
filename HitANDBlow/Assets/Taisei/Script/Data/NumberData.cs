using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputNumberData", menuName = "ScriptableObjects/InputNumberData")]
public class NumberData : ScriptableObject
{
    public const string PATH = "InputNumberData";
    private static NumberData _inputNumberEntity;
    public static NumberData InputNumberEntity
    {
        get
        {
            if (_inputNumberEntity == null)
            {
                _inputNumberEntity = Resources.Load<NumberData>(PATH);
                if (_inputNumberEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _inputNumberEntity;
        }
    }

    //���Ă鐔���̐�
    private const int ELEMNT_NUM = 3;

    /// <summary>
    /// �����̐��l��ۑ�����z��
    /// </summary>
    public int[] answerNumbers = new int[ELEMNT_NUM];

    /// <summary>
    /// ���͂��ꂽ���l��ۑ�����z��
    /// </summary>
    public int[] inputNumbers = new int[ELEMNT_NUM];

}
