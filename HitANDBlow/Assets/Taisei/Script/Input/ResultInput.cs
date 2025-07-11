using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResultInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ボタンの種類を設定
    public enum ButtonProperty
    {
        None,         // なしの状態
        OneMore,
        Exit,

    }
    // 基本的にはNoneで設定して使うときに変更一種類のみに設定すること
    [Header("ボタンの種類選択")] public ButtonProperty numButton = ButtonProperty.None;

    // 触れたときに分かりやすくするために表示する
    [Header("選択中に表示するオブジェクト"), SerializeField]
    GameObject nowSelectObj;

    private GameManager GM;

    private void Awake()
    {
        DifficultResultManager.Instance.GetResultInput(this);

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    #region UIOnOff
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 表示
        nowSelectObj.SetActive(true);
        //Debug.Log("UIに触れた");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 非表示
        nowSelectObj.SetActive(false);
        //Debug.Log("UIから離れた");
    }
    #endregion

    public void ResultButtonInput()
    {
        // 触れた状態で左クリックを押したら処理
        if (nowSelectObj.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (numButton != ButtonProperty.None)
                {
                    //anim.Play("UI_OnTheTouchMove");
                    // Enumの値ごとに対応する関数を実行
                    InvokeMatchingMethod(numButton);
                }
            }
        }
    }

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

    #region Function
    /// <summary>
    /// もう一度
    /// </summary>
    private void OneMore()
    {
        GM.OneMoreGame();
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    private void Exit()
    {
        SceneManager.LoadScene("Title");
    }
    #endregion

}
