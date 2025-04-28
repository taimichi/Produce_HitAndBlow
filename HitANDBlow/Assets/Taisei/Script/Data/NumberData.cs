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

    /// <summary>
    /// 当てる数字の桁数
    /// </summary>
    public const int ELEMNT_NUM = 3;

    /// <summary>
    /// 答えの数値を保存する配列
    /// </summary>
    public int[] answerNumbers = new int[ELEMNT_NUM];

    /// <summary>
    /// 入力された数値を保存する配列
    /// </summary>
    public int[] inputNumbers = new int[ELEMNT_NUM];

    /// <summary>
    /// 入力して決定をした回数
    /// </summary>
    public int inputCount = 0;

    /// <summary>
    /// 数値を入力した回数
    /// </summary>
    public int inputNum = 0;
}
