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
    [Header("ボタンの種類選択")] public ButtonProperty numButton = ButtonProperty.None;

    // 触れたときに分かりやすくするために表示する
    [Header("選択中に表示するオブジェクト"), SerializeField]
    GameObject nowSelectObj;

    // アニメーションの設定
    [SerializeField] Animator anim;

    //ボタンに設定する数字の値
    [SerializeField, Range(0, 9)] private int number = 0;

    //エンターが押されたかどうか インスペクターではいじれない
    [System.NonSerialized] public bool isEnter = false;

    [System.NonSerialized] public bool isCancel = false;

    //使ってる数字かどうか
    private bool isUseNum = false;

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

    private void Awake()
    {
        NumberManager numberManager = GameObject.Find("GameCanvas").GetComponent<NumberManager>();
        numberManager.GetNumberInputScript(this);
    }

    private void Start()
    {
        //数字ボタンの時
        if(numButton == ButtonProperty.Number)
        {
            //数字ボタンのImageを取得し、画像を変更
            Image buttonImage = this.GetComponent<Image>();
            buttonImage.sprite = SpriteData.SpriteEntity.NumberSprite[number];
        }

    }

    /// <summary>
    /// 入力関数
    /// </summary>
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

    /// <summary>
    /// 使う数字ボタンを表示、使わない数字ボタンを表示にする
    /// </summary>
    public void SetNumberButton(int _maxNum)
    {
        if(number >= _maxNum)
        {
            ChangeObjectActive(false);
        }
    }

    /// <summary>
    /// ボタンの表示非表示を変える
    /// </summary>
    /// <param name="_isActive">false=非表示 / true=表示</param>
    public void ChangeObjectActive(bool _isActive)
    {
        //現在の状態と変えたいときの状態が違うとき
        if(_isActive != this.gameObject.activeSelf)
        {
            nowSelectObj.SetActive(false);
            this.gameObject.SetActive(_isActive);
        }
    }

    /// <summary>
    /// 使われた数値を使われてない状態に戻す
    /// </summary>
    /// <param name="_num">使われてる数値数値</param>
    public void NumCancel(int _num)
    {
        if (number == _num)
        {
            isUseNum = false;
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
        //Debug.Log(number + " が入力されました");

        //使われた数字じゃないとき
        if (!isUseNum)
        {
            if (NumberData.InputNumberEntity.inputNum < NumberData.ELEMNT_NUM)
            {
                NumberData.InputNumberEntity.inputNumbers[NumberData.InputNumberEntity.inputNum] = number;
                NumberData.InputNumberEntity.inputNum++;
                NumberData.InputNumberEntity.saveNum = number;
                isUseNum = true;
            }
        }
    }

    private void Enter()
    {
        //Debug.Log("決定します");

        Debug.Log("入力した数値" + string.Join(" , ", NumberData.InputNumberEntity.inputNumbers));
        NumberData.InputNumberEntity.inputCount++;
        isEnter = true; 
    }

    private void Cancel()
    {
        //Debug.Log("1文字消します");
        
        //数値を入力した回数が0回より大きいとき
        if(NumberData.InputNumberEntity.inputNum > 0)
        {
            NumberData.InputNumberEntity.saveNum = NumberData.InputNumberEntity.inputNumbers[NumberData.InputNumberEntity.inputNum - 1];
            NumberData.InputNumberEntity.inputNumbers[NumberData.InputNumberEntity.inputNum - 1] = -1;
            NumberData.InputNumberEntity.inputNum--;
            isCancel = true;
        }
    }
    #endregion

}
