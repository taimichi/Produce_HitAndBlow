using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    //履歴用のプレハブ
    [SerializeField] private GameObject HistoryPre;
    //答え表示用のプレハブ
    [SerializeField] private GameObject AnswerPre;
    //履歴の親オブジェクト
    [SerializeField] private Transform HistoryParent;

    //指定の手数
    private int maxEffot = 0;

    //入力する数値の最小数
    private int minInputNumber = 0;
    //入力する数値の最大数
    private int maxInputNumber = 9;

    //履歴オブジェクトを保存する配列
    private GameObject[] Historys;
    //答えようオブジェクトを保存する変数
    private GameObject AnswerObj;

    //数値入力関連ボタンのスクリプトを入れるリスト
    private List<NumberInput> numberInputs = new List<NumberInput>();

    //答えの数値生成用リスト
    private List<int> answerGenerate = new List<int>();

    private void Start()
    {
        //入力、答えの数値初期化
        NumberReset();

        //履歴生成
        CreateHistoryArea();

        //答え生成
        AnswerGenerate();

    }

    /// <summary>
    /// 数値関連の更新用関数
    /// </summary>
    public void NumberUpdate()
    {
        for(int i= 0; i < numberInputs.Count; i++)
        {
            numberInputs[i].NumberButtonInput();
        }
    }

    /// <summary>
    /// 履歴エリアの生成
    /// 手数分の履歴と、答えを一つ生成
    /// </summary>
    public  void CreateHistoryArea()
    {
        switch (DifficultyData.DifficultyEntity.nowDifficlt)
        {
            case DifficultyData.Difficult.easy:
                maxEffot = 5;
                break;

            case DifficultyData.Difficult.normal:
                maxEffot = 7;
                break;

            case DifficultyData.Difficult.hard:
                maxEffot = 10;
                break;
        }
        //最大値、最小値を設定
        maxInputNumber = maxEffot;
        minInputNumber = 0;

        Historys = new GameObject[maxEffot];
        for(int i = 0; i < maxEffot; i++)
        {
            Historys[i] = Instantiate(HistoryPre, HistoryParent);
        }
        AnswerObj = Instantiate(AnswerPre, HistoryParent);
    }

    /// <summary>
    /// 入力、答えの数値を初期化
    /// </summary>
    private void NumberReset()
    {
        for(int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            NumberData.InputNumberEntity.inputNumbers[i] = -1;
            NumberData.InputNumberEntity.answerNumbers[i] = -1;
        }
    }

    /// <summary>
    /// 答えの数値を生成
    /// </summary>
    private void AnswerGenerate()
    {
        for(int i = minInputNumber; i < maxInputNumber; i++)
        {
            answerGenerate.Add(i);
        }

        int count = 0;
        while(count < NumberData.ELEMNT_NUM)
        {
            int indexNumber = Random.Range(minInputNumber, answerGenerate.Count);

            NumberData.InputNumberEntity.answerNumbers[count] = answerGenerate[indexNumber];
            answerGenerate.Remove(indexNumber);

            count++;
        }

        Debug.Log("答え表示" + string.Join(" , " , NumberData.InputNumberEntity.answerNumbers));
    }

    public void GetNumberInputScript(NumberInput _numberInput)
    {
        numberInputs.Add(_numberInput);
    }
}
