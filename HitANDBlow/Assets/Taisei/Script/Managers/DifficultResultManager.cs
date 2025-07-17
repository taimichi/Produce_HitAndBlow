using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度と結果関連のマネージャースクリプト
/// </summary>
public class DifficultResultManager : MonoBehaviour
{
    private static DifficultResultManager instance;
    public static DifficultResultManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<DifficultResultManager>();
            }
            return instance;
        }
    }

    #region Difficult
    private List<DifficultInput> diffInputs = new List<DifficultInput>();

    [SerializeField] private CanvasGroup diffPanel;

    //難易度選択を開始したかどうか
    private bool isStartSelect = false;

    //難易度を選択したかどうか
    [System.NonSerialized] public bool isSelect = false;

    #endregion

    #region Result
    private List<ResultInput> resuInputs = new List<ResultInput>();
    #endregion

    private void Awake()
    {
        if(this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

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
    /// リザルト関連の更新関数
    /// </summary>
    public void ResultUpdate()
    {
        for(int i = 0; i < resuInputs.Count; i++)
        {
            resuInputs[i].ResultButtonInput();
        }
    }

    /// <summary>
    /// 難易度入力スクリプトを取得
    /// </summary>
    public void GetDifficultInput(DifficultInput _getDifficultInput)
    {
        diffInputs.Add(_getDifficultInput);
    }

    public void GetResultInput(ResultInput _getResultInput)
    {
        resuInputs.Add(_getResultInput);
    }

    /// <summary>
    /// 難易度選択画面を再度表示する
    /// </summary>
    public void ResetDifficultPanel()
    {
        DifficultyData.DifficultyEntity.nowDifficlt = DifficultyData.Difficult.none;
        isStartSelect = false;
        isSelect = false;

        diffPanel.alpha = 1;
        diffPanel.interactable = true;
        diffPanel.blocksRaycasts = true;
    }

    /// <summary>
    /// コルーチン　難易度選択
    /// </summary>
    private IEnumerator SelectDifficult()
    {
        if(DifficultyData.DifficultyEntity.nowDifficlt != DifficultyData.Difficult.none)
        {
            diffPanel.alpha = 0;
            diffPanel.interactable = false;
            diffPanel.blocksRaycasts = false;

            isSelect = true;
        }

        //難易度が選択され、パネルが非表示になったら、以降の処理に進む
        yield return new WaitUntil(() => diffPanel.alpha == 0);

        isStartSelect = true;

    }
}
