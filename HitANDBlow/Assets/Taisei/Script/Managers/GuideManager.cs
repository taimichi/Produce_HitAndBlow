using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ルール説明用のマネージャースクリプト
/// </summary>
public class GuideManager : MonoBehaviour
{
    //シングルトン設定
    private static GuideManager instance;
    public static GuideManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GuideManager>();
            }
            return instance;
        }
    }

    //ルール説明用
    [SerializeField] private GameObject GuidePanel; //ルール説明画面のパネル
    [SerializeField] private RubyText guideText;    //ルール説明のテキスト
    [SerializeField] private Image GuideImage;      //ルール説明の画像
    [SerializeField] private Text Page;             //ルール説明のページ数

    //現在のページ数
    [System.NonSerialized]public int nowGuidePage = 0;
    //最大ページ数
    [System.NonSerialized] public int maxGuidePage;
    private GuideTextList textList;

    //ボタン用
    private List<GuideInput> guideInputs = new List<GuideInput>();

    //ページを変更したかどうか
    [System.NonSerialized]public bool isPageChange = false;

    [SerializeField] private Image LeftButton;      //左ボタン
    [SerializeField] private Image RightButton;     //右ボタン

    //放置時間計測用
    private float timer = 0f;
    //放置時間の限界値
    private const float MAX_LIMIT_TIME = 180f;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        textList = GuideTextList.GuideTextEntity;
        maxGuidePage = textList.guideTextList.Count;

        guideText.text = "<r=てすと>テスト</r>\n<r=てすと>テスト</r>";
        PageTextUpdata();

        ChangeGuideActive(false);
    }

    public void GuideUpdata()
    {
        //ボタンの更新処理
        for(int i = 0; i < guideInputs.Count; i++)
        {
            guideInputs[i].GuideButtonInput();
        }

        //ガイドが表示状態の時
        if (GuidePanel.activeSelf)
        {
            //ページが変更されたら
            if (isPageChange)
            {
                Guide();
                timer = 0f;
            }
            else
            {
                //放置時間が限界を超えたら
                if(timer >= MAX_LIMIT_TIME)
                {
                    //強制的にガイド画面を閉じる
                    CloseGuide();
                    timer = 0;
                }
                //超えてない場合
                else
                {
                    //放置状態の時間を計測
                    timer += Time.deltaTime;
                }
            }
        }

    }

    /// <summary>
    /// ページ数更新
    /// </summary>
    private void PageTextUpdata()
    {
        Page.text = (nowGuidePage + 1) + "/" + maxGuidePage;
    }

    /// <summary>
    /// ガイド画面の画像とテキスト、ページ数変更
    /// </summary>
    private void Guide()
    {
        //テキスト変更
        guideText.text = textList.guideTextList[nowGuidePage].Text;
        //画像変更
        GuideImage.sprite = textList.guideTextList[nowGuidePage].guideSprite;

        //左矢印ボタンの表示非表示
        if (nowGuidePage <= 0 && LeftButton.enabled == true)
        {
            LeftButton.enabled = false;

            GuideInput left = LeftButton.GetComponent<GuideInput>();
            left.SelectObjActive(false);
        }
        else if (nowGuidePage > 0 && LeftButton.enabled == false)
        {
            LeftButton.enabled = true;
        }

        //右矢印ボタンの表示非表示
        if (nowGuidePage >= maxGuidePage - 1 && RightButton.enabled == true)
        {
            RightButton.enabled = false;

            GuideInput right = RightButton.GetComponent<GuideInput>();
            right.SelectObjActive(false);
        }
        else if (nowGuidePage < maxGuidePage - 1 && RightButton.enabled == false)
        {
            RightButton.enabled = true;
        }

        PageTextUpdata();
        isPageChange = false;
    }

    /// <summary>
    /// ガイド画面を閉じる
    /// </summary>
    public void CloseGuide()
    {
        isPageChange = false;
        ChangeGuideActive(false);
    }

    /// <summary>
    /// ガイド画面の表示状態を変更
    /// </summary>
    /// <param name="_trigger">false=非表示 / true=表示</param>
    public void ChangeGuideActive(bool _trigger)
    {
        GuidePanel.SetActive(_trigger);
    }

    /// <summary>
    /// ガイド用のボタン用スクリプトを取得
    /// </summary>
    public void GetGuideInput(GuideInput _guideInput)
    {
        guideInputs.Add(_guideInput);
    }

    /// <summary>
    /// ルール説明画面が表示状態かどうか
    /// </summary>
    /// <returns>false=非表示 / true=表示</returns>
    public bool CheckGuideOnOff() => GuidePanel.activeSelf;
}
