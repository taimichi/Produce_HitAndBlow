using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using System;
using UnityEngine.UI;
public partial class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region enumData
    // ボタンの種類を設定
    // タイトルシーンでのボタン
    public enum ButtonProperty
    {
        None,         // なしの状態
        VolumeUp,     // 音量上昇
        VolumeDown,   // 音量下降
        GoTitle,      // タイトルへ戻る
        OpenTutorial, // チュートリアルを開く(仮)
        Yes_Title,    // 中断する場合
        No_Title,     // 中断しない場合
    }
    // 基本的にはNoneで設定しておく
    [Header("ボタンの種類選択"), SerializeField] ButtonProperty button = ButtonProperty.None;
    #endregion

    // 触れたときに分かりやすくするために表示する
    [Header("選択中に表示するオブジェクト"), SerializeField]
    GameObject nowSelectObj;

    [Header("ゲーム中断するか確認するUIパネル"),SerializeField] GameObject GoTitlePanel;
    // アニメーションの設定
    [SerializeField] Animator anim;

    // UI接触のインターフェース処理
    #region InterFace
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 表示
        nowSelectObj.SetActive(true);
        Debug.Log("UIに触れた");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 非表示
        nowSelectObj.SetActive(false);
        Debug.Log("UIから離れた");
    }
    #endregion

    public void Update()
    {
        // 触れた状態で左クリックを押したら処理
        if (nowSelectObj.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (button != ButtonProperty.None)
                {
                    anim.Play("UI_OnTheTouchMove");
                    // Enumの値ごとに対応する関数を実行
                    InvokeMatchingMethod(button);
                }
            }
        }
    }

    // これは関数のマッチング
    // enumと同じ名前の関数を実行する
    #region MatchingMethod
    private void InvokeMatchingMethod(ButtonProperty kButton)
    {
        string methodName = kButton.ToString(); // Enumの名前を文字列化

        // メソッド情報を取得
        MethodInfo method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (method != null)
        {
            method.Invoke(this, null);
        }
        else
        {
            Debug.LogWarning($"メソッド '{methodName}' が見つかりません。");
        }
    }

    // 音量上げる機能
    private void VolumeUp()
    {
        Debug.Log("UP");
    }

    // 音量下げる機能
    private void VolumeDown()
    {
        Debug.Log("DOWN");
    }
    #endregion
}
