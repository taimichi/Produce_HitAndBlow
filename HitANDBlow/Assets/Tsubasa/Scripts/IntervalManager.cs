using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalManager : MonoBehaviour
{
    private const float _TIME = 10.0f;   // 設定時間
    private const float _MIN = 60.0f;
    private float _time = _TIME*_MIN;

    private bool isCoolTime = false;

    // 時間制限処理用プロパティ
    public bool CoolTime
    {
        get { return isCoolTime; }
        set { isCoolTime = value; }
    }

    public float _Time
    {
        get { return _time; }
    }

    // 時間制限の計算
    public void Calculation()
    {
        if (!isCoolTime)
        {
            Debug.Log("計算してますからね");
            _time += Time.deltaTime;
            if (_time >= _TIME * _MIN)
            {
                isCoolTime = true;
                _time = _TIME * _MIN;
            }
        }
    }

    public void RESTART()
    {
        if (isCoolTime)
        {
            Debug.Log("計算してますからね");
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                isCoolTime = false;
                _time =0; // 再セット
            }
        }
    }

    #region Instance
    private static IntervalManager instance;

    public static IntervalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<IntervalManager>();
                if (instance == null)
                {
                    Debug.LogError("IntervalManager インスタンスが見つかりません。シーンに存在していますか？");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Singleton設定
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // シーン遷移でも破棄しない
        }
        else if (this != instance)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
