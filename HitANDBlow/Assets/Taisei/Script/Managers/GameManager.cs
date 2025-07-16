using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private NumberManager NumManager;
    private DifficultResultManager DiffResuManager;
    private TitleManager titleManager;
    private GuideManager guideManager;
    private VideoManager videoManager;

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

    private string nowSceneName = "";

    private enum MANAGER_MODE
    {
        none,
        title,
        game,
    }
    private MANAGER_MODE nowMode = MANAGER_MODE.none;

    void Start()
    {
        //現在のシーン名を取得
        nowSceneName = SceneManager.GetActiveScene().name;

        switch (nowSceneName)
        {
            default:
                nowMode = MANAGER_MODE.none;
                break;

            //タイトルシーンの時
            case "Title":
                nowMode = MANAGER_MODE.title;
                titleManager = TitleManager.Instance;
                guideManager = GuideManager.Instance;
                videoManager = VideoManager.Instance;
                break;

            //ゲームシーンの時
            case "Game":
                nowMode = MANAGER_MODE.game;

                NumManager = NumberManager.Instance;
                DiffResuManager = DifficultResultManager.Instance;

                ResultObj.SetActive(false);
                break;
        }
    }

    void Update()
    {
        switch (nowMode)
        {
            default:
                break;

            case MANAGER_MODE.title:
                //タイトルのアップデート処理
                titleManager.TitleUpdate();
                //ルール説明画面が表示状態の時
                if (guideManager.CheckGuideOnOff())
                {
                    //ガイドの更新処理
                    guideManager.GuideUpdata();

                    //何かしらの操作があったため、動画再生までの時間をリセット
                    videoManager.ResetVideo();
                }
                //ルール説明画面が非表示状態の時
                else
                {
                    //ビデオの更新処理
                    videoManager.VideoUpdate();
                }
                break;

            case MANAGER_MODE.game:
                //ゲームの更新処理
                GameUpdate();
                break;
        }

    }

    /// <summary>
    /// ゲームシーンのアップデート
    /// </summary>
    private void GameUpdate()
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

}
