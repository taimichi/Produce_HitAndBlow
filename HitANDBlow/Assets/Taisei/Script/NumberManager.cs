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

    //履歴オブジェクトを保存する配列
    private GameObject[] Historys;
    //答えようオブジェクトを保存する変数
    private GameObject AnswerObj;

    private void Start()
    {
        //履歴生成
        CreateHistoryArea();
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
                maxEffot = 4;
                break;

            case DifficultyData.Difficult.normal:
                maxEffot = 6;
                break;

            case DifficultyData.Difficult.hard:
                maxEffot = 10;
                break;
        }

        Historys = new GameObject[maxEffot];
        for(int i = 0; i < maxEffot; i++)
        {
            Historys[i] = Instantiate(HistoryPre, HistoryParent);
        }
        AnswerObj = Instantiate(AnswerPre, HistoryParent);
    }

    /// <summary>
    /// 答えの数値を生成
    /// </summary>
    private void AnswerGenerate()
    {

    }
}
