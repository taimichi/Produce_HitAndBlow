using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private NumberManager NumManager;
    private DifficultManager DiffManager;

    //ゲームが開始したかどうか
    private bool isGameStart = false;
    //ゲームが終了したかどうか
    private bool isGameFinish = false;

    [SerializeField] private GameObject ResultObj;
    [SerializeField] private Text ResultText;

    void Start()
    {
        GameObject.Find("GameCanvas").TryGetComponent<NumberManager>(out NumManager);
        GameObject.Find("Difficult_Result").TryGetComponent<DifficultManager>(out DiffManager);

        ResultObj.SetActive(false);
    }

    void Update()
    {
        //ゲーム開始前
        if (!isGameStart)
        {
            DiffManager.DifficultUpdate();

            //難易度が選択されたとき
            if (DiffManager.isSelect)
            {
                NumManager.GameStart();
                isGameStart = true;
            }
        }
        //ゲーム開始後
        else
        {
            NumManager.NumberUpdate();
            //ゲーム中の時
            if (!isGameFinish)
            {
                isGameFinish = NumManager.CheckGameNow();
            }
            //ゲーム終了時
            else
            {
                StartCoroutine(GameFinish());
            }

        }
    }

    private IEnumerator GameFinish()
    {

        yield return new WaitUntil(() => NumManager.CheckHide());

        ResultObj.SetActive(true);

        //ゲームオーバー
        if (!NumManager.CheckGameClear())
        {
            ResultText.text = "ゲームオーバー！";
        }
        //ゲームクリア
        else
        {
            ResultText.text = "ゲームクリア！";
        }

    }

    /// <summary>
    /// ボタンで起動
    /// 難易度選択から始める
    /// </summary>
    public void OnOneMoreGame()
    {
        isGameStart = false;
        isGameFinish = false;
        DiffManager.ResetDifficultPanel();
        NumManager.HistoryDelete();

        ResultObj.SetActive(false);
        //SceneManager.LoadScene("GameScene");
    }
}
