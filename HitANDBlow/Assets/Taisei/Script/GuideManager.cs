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


    void Start()
    {
        textList = GuideTextList.GuideTextEntity;
        maxGuidePage = textList.guideTextList.Count;

        guideText.text = "<r=てすと>テスト</r>\n<r=てすと>テスト</r>";
    }

    public void GuideUpdata()
    {
        for(int i = 0; i < guideInputs.Count; i++)
        {
            guideInputs[i].GuideButtonInput();
        }
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
