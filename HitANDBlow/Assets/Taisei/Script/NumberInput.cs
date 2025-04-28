using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

public class NumberInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ボタンの種類を設定
    public enum ButtonProperty
    {
        None,         // なしの状態
        Number,
        Enter,
        Cancel,

    }
    // 基本的にはNoneで設定して使うときに変更一種類のみに設定すること
    [Header("ボタンの種類選択"), SerializeField] ButtonProperty numButton = ButtonProperty.None;

    // 触れたときに分かりやすくするために表示する
    [Header("選択中に表示するオブジェクト"), SerializeField]
    GameObject nowSelectObj;

    // アニメーションの設定
    [SerializeField] Animator anim;

    //ボタンに設定する数字の値
    [SerializeField, Range(0, 9)] private int number = 0;

    #region UIOnOff
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

    private void Start()
    {
        //数字ボタンの時
        if(numButton == ButtonProperty.Number)
        {
            //数字ボタンのImageを取得し、画像を変更
            Image buttonImage = this.GetComponent<Image>();
            buttonImage.sprite = SpriteData.SpriteEntity.NumberSprite[number];
        }

        NumberManager numberManager = GameObject.Find("GameCanvas").GetComponent<NumberManager>();
        numberManager.GetNumberInputScript(this);
    }

    public void NumberButtonInput()
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
    private void Number()
    {
        Debug.Log(number + " が入力されました");
    }

    private void Enter()
    {
        Debug.Log("決定します");
    }

    private void Cancel()
    {
        
        Debug.Log("1文字消します");
    }
    #endregion

}
