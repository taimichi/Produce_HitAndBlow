using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private NumberManager NumManager;
    private DifficultResultManager DiffResuManager;

    //ゲームが開始したかどうか
    private bool isGameStart = false;
    //ゲームが終了したかどうか
    private bool isGameFinish = false;

    //結果表示するときのオブジェクト
    [SerializeField] private GameObject ResultObj;
    //結果表示のテキスト
    [SerializeField] private Text ResultText;

    //一度プレイしたかどうか
    private bool isOnePlay = false;


    void Start()
    {
        GameObject.Find("GameCanvas").TryGetComponent<NumberManager>(out NumManager);
        GameObject.Find("Difficult_Result").TryGetComponent<DifficultResultManager>(out DiffResuManager);

        ResultObj.SetActive(false);
    }

    void Update()
    {
        //ゲーム開始前
        if (!isGameStart)
        {
            DiffResuManager.DifficultUpdate();

            //難易度が選択されたとき
            if (DiffResuManager.isSelect)
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
                isOnePlay = false;
            }
            //ゲーム終了時
            else
            {
                StartCoroutine(GameFinish());

                DiffResuManager.ResultUpdate();
            }

        }
    }

    private IEnumerator GameFinish()
    {

        yield return new WaitUntil(() => NumManager.CheckHide() == true);

        if (!isOnePlay)
        {
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

            isOnePlay = true;
        }

    }

    /// <summary>
    /// ボタンで起動
    /// 難易度選択から始める
    /// </summary>
    public void OneMoreGame()
    {
        isGameStart = false;
        isGameFinish = false;
        DiffResuManager.ResetDifficultPanel();
        NumManager.HistoryDelete();

        ResultObj.SetActive(false);
    }

    public void GameEnd()
    {
        GameEnd gameEnd = new GameEnd();
        gameEnd.EndButton();
    }
}
