using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    [SerializeField] private GameObject GuidePanel;
    [SerializeField] private RubyText guideText;
    [SerializeField] private Image GuideImage;
    [SerializeField] private Text Page;

    [System.NonSerialized]public int nowGuidePage = 0;
    [System.NonSerialized] public int maxGuidePage;
    private GuideTextList textList;

    private List<GuideInput> guideInputs = new List<GuideInput>();

    [System.NonSerialized]public bool isPageChange = false;

    [SerializeField] private Image LeftButton;
    [SerializeField] private Image RightButton;


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
        for(int i = 0; i < guideInputs.Count; i++)
        {
            guideInputs[i].GuideButtonInput();
        }

        if (GuidePanel.activeSelf)
        {
            if (isPageChange)
            {
                Guide();
            }
        }

    }

    /// <summary>
    /// ページ更新
    /// </summary>
    private void PageTextUpdata()
    {
        Page.text = (nowGuidePage + 1) + "/" + maxGuidePage;
    }

    private void Guide()
    {
        guideText.text = textList.guideTextList[nowGuidePage].Text;
        GuideImage.sprite = textList.guideTextList[nowGuidePage].guideSprite;

        //左矢印ボタンの表示非表示
        if (nowGuidePage <= 0 && LeftButton.enabled == true)
        {
            LeftButton.enabled = false;
        }
        else if (nowGuidePage > 0 && LeftButton.enabled == false)
        {
            LeftButton.enabled = true;
        }

        //右矢印ボタンの表示非表示
        if (nowGuidePage >= maxGuidePage - 1 && RightButton.enabled == true)
        {
            RightButton.enabled = false;
        }
        else if (nowGuidePage < maxGuidePage - 1 && RightButton.enabled == false)
        {
            RightButton.enabled = true;
        }

        PageTextUpdata();
        isPageChange = false;
    }

    public void ChangeGuideActive(bool _trigger)
    {
        GuidePanel.SetActive(_trigger);
    }

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
