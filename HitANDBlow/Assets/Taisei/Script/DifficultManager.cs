using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultManager : MonoBehaviour
{
    private List<DifficultInput> diffInputs = new List<DifficultInput>();

    [SerializeField] private GameObject DifficultPanel;

    //難易度選択を開始したかどうか
    private bool isStartSelect = false;

    //難易度を選択したかどうか
    [System.NonSerialized] public bool isSelect = false;

    void Start()
    {
        DifficultyData.DifficultyEntity.nowDifficlt = DifficultyData.Difficult.none;
    }

    /// <summary>
    /// 難易度関連の更新関数
    /// </summary>
    public void DifficultUpdate()
    {
        for(int i = 0; i < diffInputs.Count; i++)
        {
            diffInputs[i].DifficultButtonInput();
        }

        //難易度が選択されていないとき
        if (!isStartSelect)
        {
            StartCoroutine(SelectDifficult());
        }
    }

    /// <summary>
    /// 難易度入力スクリプトを取得
    /// </summary>
    public void GetDifficultInput(DifficultInput _getDifficultInput)
    {
        diffInputs.Add(_getDifficultInput);
    }

    /// <summary>
    /// 難易度選択画面を再度表示する
    /// </summary>
    public void ResetDifficultPanel()
    {
        DifficultyData.DifficultyEntity.nowDifficlt = DifficultyData.Difficult.none;
        isStartSelect = false;
        isSelect = false;
        DifficultPanel.SetActive(true); 
    }

    /// <summary>
    /// コルーチン　難易度選択
    /// </summary>
    private IEnumerator SelectDifficult()
    {
        if(DifficultyData.DifficultyEntity.nowDifficlt != DifficultyData.Difficult.none)
        {
            DifficultPanel.SetActive(false);
            isSelect = true;
        }

        //難易度が選択され、パネルが非表示になったら、以降の処理に進む
        yield return new WaitUntil(() => DifficultPanel.activeSelf == false);

        isStartSelect = true;

    }
}
