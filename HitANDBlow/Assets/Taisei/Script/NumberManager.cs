using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    private static NumberManager instance;
    public static NumberManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<NumberManager>();
            }
            return instance;
        }
    }

    //履歴用のプレハブ
    [SerializeField] private GameObject HistoryPre;
    //答え表示用のプレハブ
    [SerializeField] private GameObject AnswerPre;
    //履歴の親オブジェクト
    [SerializeField] private Transform HistoryParent;

    //ヒットブローが0の時表示するオブジェクト
    [SerializeField] private GameObject NothingObj;

    //指定の手数
    private int maxEffot = 0;

    //入力する数値の最小数
    private int minInputNumber = 0;
    //入力する数値の最大数
    private int maxInputNumber = 9;

    //履歴オブジェクトを保存する配列
    private GameObject[] Historys;
    //答えようオブジェクトを保存する変数
    private GameObject AnswerObj;

    //数値入力関連ボタンのスクリプトを入れるリスト
    private List<NumberInput> numberInputs = new List<NumberInput>();

    //エンターボタンのスクリプトがリストの何番目に入っているか
    private int enterButtonNum = 0;
    //1つ消すボタンのスクリプトがリストの何番目に入っているか
    private int cancelButtonNum = 0;

    //答えの数値生成用リスト
    private List<int> answerGenerate = new List<int>();

    //ヒットとブローの数
    private int hitCount = 0;
    private int blowCount = 0;

    //ゲームが終了したかどうか
    private bool isGameFinish = false;
    //ゲームクリアかどうか
    private bool isGameClear = false;

    private Image HideAnswer;
    private bool isHideAnswer = false;

    //答えの表示時間
    private const float ANSWER_DISPLAYTIME = 2f;
    private float answer_displayCount = 0f;

    private void Awake()
    {
        if(this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {

    }

    /// <summary>
    /// ゲームを開始するときに呼び出す
    /// </summary>
    public void GameStart()
    {        
        //入力、答えの数値初期化
        NumberAllReset();

        //履歴生成
        CreateHistoryArea();

        //答え生成
        AnswerGenerate();

        for(int i = 0; i < numberInputs.Count; i++)
        {
            if (numberInputs[i].numButton == NumberInput.ButtonProperty.Number)
            {
                numberInputs[i].NumCancel();
            }
        }
    }

    /// <summary>
    /// 数値関連の更新用関数
    /// </summary>
    public void NumberUpdate()
    {
        if (!isGameFinish)
        {
            //ボタンの入力処理
            for (int i = 0; i < numberInputs.Count; i++)
            {
                numberInputs[i].NumberButtonInput();
            }

            //入力した回数が当てる数字の桁数と同じになった時
            if (NumberData.InputNumberEntity.inputNum == NumberData.ELEMNT_NUM)
            {
                //決定ボタンを表示
                numberInputs[enterButtonNum].ChangeObjectActive(true);
            }
            //入力した回数が当てる数字の桁数じゃないとき
            else
            {
                //決定ボタンを非表示
                numberInputs[enterButtonNum].ChangeObjectActive(false);
            }

            //決定ボタンが押されたとき
            if (numberInputs[enterButtonNum].isEnter)
            {
                HBCheck();

                //正解のとき
                if (AnswerCheck())
                {
                    Debug.Log("正解！");
                    isGameFinish = true;
                    isGameClear = true;
                }
                //不正解の時
                else
                {
                    Debug.Log("不正解！");
                    //回数上限に行ってないとき
                    if (NumberData.InputNumberEntity.inputCount < maxEffot)
                    {
                        NextSet();
                        Debug.Log("次！");
                    }
                    //回数上限に行ったとき
                    else
                    {
                        isGameFinish = true;
                        isGameClear = false;
                        Debug.Log("終了！");
                    }
                }

                numberInputs[enterButtonNum].isEnter = false;
            }
            //決定が押されてないとき
            else
            {
                if (NumberData.InputNumberEntity.inputNum - 1 >= 0)
                {
                    int nowNum = NumberData.InputNumberEntity.inputNum - 1;
                    //数値が入力されたとき
                    if (NumberData.InputNumberEntity.inputNumbers[nowNum] != -1)
                    {
                        GameObject historyObj = GetHistoryObj(nowNum);
                        if (historyObj.TryGetComponent<Image>(out Image historyImage))
                        {
                            historyImage.sprite = SpriteData.SpriteEntity.NumberSprite[NumberData.InputNumberEntity.inputNumbers[nowNum]];
                        }
                    }
                }

            }

            //1つ消すボタンが押されたとき
            if (numberInputs[cancelButtonNum].isCancel)
            {
                for (int i = 0; i < numberInputs.Count; i++)
                {
                    if (numberInputs[i].numButton == NumberInput.ButtonProperty.Number)
                    {
                        //一番最後に押された数字のボタンを再び押せるようにする
                        numberInputs[i].NumCancel(NumberData.InputNumberEntity.saveNum);
                    }
                }

                GameObject historyObj = GetHistoryObj(NumberData.InputNumberEntity.inputNum);
                if (historyObj.TryGetComponent<Image>(out Image historyImage))
                {
                    historyImage.sprite = SpriteData.SpriteEntity.NoneNumberSprite;
                }
                numberInputs[cancelButtonNum].isCancel = false;
            }
        }
        else
        {
            //答えを隠しているオブジェクトを非表示に
            Color hideAnswerColor = HideAnswer.color;
            float a = hideAnswerColor.a;
            if (a >= 0)
            {
                a -= 0.01f;
                HideAnswer.color = new Color(hideAnswerColor.r, hideAnswerColor.g, hideAnswerColor.b, a);
            }
            else
            {
                if(answer_displayCount >= ANSWER_DISPLAYTIME)
                {
                    isHideAnswer = true;
                }
                answer_displayCount += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 履歴エリアの生成
    /// 手数分の履歴と、答えを一つ生成
    /// </summary>
    public  void CreateHistoryArea()
    {
        //手数と入力する数値の種類を設定
        switch (DifficultyData.DifficultyEntity.nowDifficlt)
        {
            case DifficultyData.Difficult.easy:
                maxEffot = 4;
                maxInputNumber = 5;
                break;

            case DifficultyData.Difficult.normal:
                maxEffot = 6;
                maxInputNumber = 7;
                break;

            case DifficultyData.Difficult.hard:
                maxEffot = 10;
                maxInputNumber = 10;
                break;
        }
        //最小値を設定
        minInputNumber = 0;

        for(int i = 0; i < numberInputs.Count; i++)
        {
            if(numberInputs[i].numButton == NumberInput.ButtonProperty.Number)
            {
                numberInputs[i].SetNumberButton(maxInputNumber);
            }
        }

        //履歴を表示するオブジェクトを生成
        Historys = new GameObject[maxEffot];
        for(int i = 0; i < maxEffot; i++)
        {
            Historys[i] = Instantiate(HistoryPre, HistoryParent);

            GameObject historyCountObj = Historys[i].transform.GetChild(0).gameObject;
            Text historyCountText = historyCountObj.GetComponent<Text>();
            historyCountText.text = (i + 1).ToString();
        }
        //答えを表示するオブジェクトを生成
        AnswerObj = Instantiate(AnswerPre, HistoryParent);
        isHideAnswer = false;
        answer_displayCount = 0;

        FrameSet();
    }

    /// <summary>
    /// 履歴と答えを表示するオブジェクトを削除
    /// </summary>
    public void HistoryDelete()
    {
        isGameClear = false;
        isGameFinish = false;
        if (Historys.Length > 0 || Historys != null)
        {
            for (int i = 0; i < Historys.Length; i++)
            {

                Destroy(Historys[i]);
            }
            Destroy(AnswerObj);
        }
    }

    /// <summary>
    /// 答えの判定
    /// </summary>
    /// <returns>false=不正解 / true=正解</returns>
    private bool AnswerCheck()
    {
        for(int i = 0; i < NumberData.InputNumberEntity.inputNumbers.Length; i++)
        {
            if (!NumberData.InputNumberEntity.inputNumbers[i].Equals(NumberData.InputNumberEntity.answerNumbers[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ヒットとブロー判定
    /// </summary>
    private void HBCheck()
    {
        for(int i_input = 0; i_input < NumberData.InputNumberEntity.inputNumbers.Length; i_input++)
        {
            for(int i_answer = 0; i_answer < NumberData.InputNumberEntity.answerNumbers.Length; i_answer++)
            {
                //数値が一致している時
                if(NumberData.InputNumberEntity.inputNumbers[i_input] == NumberData.InputNumberEntity.answerNumbers[i_answer])
                {
                    //場所も一致している時
                    if(i_input == i_answer)
                    {
                        hitCount++;
                    }
                    //場所は一致してないとき
                    else
                    {
                        blowCount++;
                    }

                }
            }
        }

        Debug.Log("H:" + hitCount + " B:" + blowCount);

        //現在の履歴のヒットブローを表示するオブジェクト取得
        GameObject HBParent = Historys[NumberData.InputNumberEntity.inputCount - 1].transform.GetChild(2).gameObject;
        //ヒットブローを表示するImageオブジェクトの配列
        Image[] HBGroup = new Image[NumberData.ELEMNT_NUM];
        for (int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            //Imageコンポーネントを取得し、配列に入れる
            HBGroup[i] = HBParent.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        int hbNum = 0;
        //ヒット表示
        for(int i = 0; i < hitCount; i++)
        {
            HBGroup[hbNum].sprite = SpriteData.SpriteEntity.HitBlowSprite[0];
            hbNum++;
        }
        //ブロー表示
        for(int i = 0; i < blowCount; i++)
        {
            HBGroup[hbNum].sprite = SpriteData.SpriteEntity.HitBlowSprite[1];
            hbNum++;
        }

        //ヒット、ブローが両方0の時
        if(hitCount == 0 && blowCount == 0)
        {
            Instantiate(NothingObj, HBParent.transform);
        }
    }

    /// <summary>
    /// 次の入力の準備
    /// </summary>
    private void NextSet()
    {
        for (int i = 0; i < numberInputs.Count; i++)
        {
            if (numberInputs[i].numButton == NumberInput.ButtonProperty.Number)
            {
                numberInputs[i].NumCancel();
            }
        }

        FrameSet();

        //入力リストに-1を入れて初期化
        for (int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            NumberData.InputNumberEntity.inputNumbers[i] = -1;
        }
        //入力回数をリセット
        NumberData.InputNumberEntity.inputNum = 0;
        //ヒットとブローの数をリセット
        hitCount = blowCount = 0;
        //最後に入力した数値をリセット
        NumberData.InputNumberEntity.saveNum = -1;
    }

    /// <summary>
    /// 現在の履歴の位置に枠を表示
    /// </summary>
    private void FrameSet()
    {
        //履歴から枠を取得
        GameObject Frame = Historys[NumberData.InputNumberEntity.inputCount].transform.GetChild(3).gameObject;
        //枠を表示
        Frame.SetActive(true);

        //一番最初じゃないとき
        if(NumberData.InputNumberEntity.inputCount > 0)
        {
            //前回の枠を取得
            GameObject beforeFrame = Historys[NumberData.InputNumberEntity.inputCount - 1].transform.GetChild(3).gameObject;
            //前回の枠を非表示
            beforeFrame.SetActive(false);
        }
    }

    /// <summary>
    /// 入力、答えの数値を初期化
    /// </summary>
    private void NumberAllReset()
    {
        //入力と答えのリストに-1を入れて初期化
        for(int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            NumberData.InputNumberEntity.inputNumbers[i] = -1;
            NumberData.InputNumberEntity.answerNumbers[i] = -1;
        }
        //入力回数と決定を押した回数をリセット
        NumberData.InputNumberEntity.inputNum = 0;
        NumberData.InputNumberEntity.inputCount = 0;
        //最後に入力した数値をリセット
        NumberData.InputNumberEntity.saveNum = -1;
        //ヒットとブローの数をリセット
        hitCount = blowCount = 0;

        isGameFinish = false;
        isGameClear = false;    

    }

    /// <summary>
    /// 履歴の数値を表示するオブジェクトを取得する
    /// </summary>
    /// <param name="_num">履歴の数値表示用オブジェクトの何番目か</param>
    /// <returns>履歴の数値表示用オブジェクト</returns>
    private GameObject GetHistoryObj(int _num)
    {
        GameObject plAnswerParent = Historys[NumberData.InputNumberEntity.inputCount].transform.GetChild(1).gameObject;
        GameObject[] plAnswer = new GameObject[NumberData.ELEMNT_NUM];
        for (int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            plAnswer[i] = plAnswerParent.transform.GetChild(i).gameObject;
        }

        return plAnswer[_num];

    }


    /// <summary>
    /// 答えの数値を生成
    /// </summary>
    private void AnswerGenerate()
    {
        //答えの数値生成用リストを初期化
        answerGenerate.Clear();
        for (int i = minInputNumber; i < maxInputNumber; i++)
        {
            answerGenerate.Add(i);
        }

        int count = 0;

        //答えの数値を選択
        //当てる数値の回数文繰り返す
        while(count < NumberData.ELEMNT_NUM)
        {
            //数値を抽選
            int indexNumber = Random.Range(minInputNumber, answerGenerate.Count);

            //抽選で選ばれた数値を答えのリストに保存
            NumberData.InputNumberEntity.answerNumbers[count] = answerGenerate[indexNumber];
            //選ばれた数値をリストから削除し、選ばれなくする
            answerGenerate.Remove(answerGenerate[indexNumber]);

            count++;
        }

        Debug.Log("答え表示\n" + string.Join(" , " , NumberData.InputNumberEntity.answerNumbers));

        //答えの画像を数値の画像に変更
        GameObject[] answers = new GameObject[NumberData.ELEMNT_NUM];
        GameObject answerPar = AnswerObj.transform.GetChild(0).gameObject;
        for(int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            answers[i] = answerPar.transform.GetChild(i).gameObject;
            if(answers[i].TryGetComponent<Image>(out Image answerImage))
            {
                //答えの画像を変更
                answerImage.sprite = SpriteData.SpriteEntity.NumberSprite[NumberData.InputNumberEntity.answerNumbers[i]];
            }

        }

        HideAnswer = AnswerObj.transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    /// <summary>
    /// 数値入力スクリプトを取得し、リストに保存
    /// </summary>
    public void GetNumberInputScript(NumberInput _numberInput)
    {
        numberInputs.Add(_numberInput);
        //取得したスクリプトのButtonPropertyがエンターのとき
        if(_numberInput.numButton == NumberInput.ButtonProperty.Enter)
        {
            _numberInput.ChangeObjectActive(false);
            enterButtonNum = numberInputs.Count - 1;
        }
        else if(_numberInput.numButton == NumberInput.ButtonProperty.Cancel)
        {
            cancelButtonNum = numberInputs.Count - 1;
        }
    }

    /// <summary>
    /// 現在ゲームが続いているか終わっているか
    /// </summary>
    /// <returns>false=ゲーム中 / true=ゲーム終了</returns>
    public bool CheckGameNow() => isGameFinish;

    /// <summary>
    /// ゲームをクリアしたかどうか
    /// </summary>
    /// <returns>false=ゲームオーバー / true=ゲームクリア</returns>
    public bool CheckGameClear() => isGameClear;

    /// <summary>
    /// 答えの表示が出来たかどうか
    /// </summary>
    /// <returns>false=表示途中 / true=表示終了</returns>
    public bool CheckHide() => isHideAnswer;


}
