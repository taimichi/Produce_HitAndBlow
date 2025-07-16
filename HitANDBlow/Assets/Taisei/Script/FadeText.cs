using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    //フェード用
    private float fadeSpeed = 0.02f;//フェードのスピード
    private bool isFade = false;    //フェードインかフェードアウトか　false=フェードアウト / true=フェードイン
    private float alpha;            //テキストのアルファ値

    private Text text;              //フェードしたいテキスト
    private Color textColor;        //テキストのカラー値



    void Start()
    {
        text = this.GetComponent<Text>();
        textColor = text.color;
    }


    void Update()
    {
        Fade();
    }

    /// <summary>
    /// フェード処理
    /// </summary>
    private void Fade()
    {
        if (!isFade)
        {
            //フェードアウト
            FadeOut();
        }
        else
        {
            //フェードイン
            FadeIn();
        }
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    private void FadeIn()
    {
        alpha -= fadeSpeed;
        textColor.a = alpha;

        if(alpha <= 0)
        {
            isFade = false;
        }
        ColorChange();
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    private void FadeOut()
    {
        alpha += fadeSpeed;
        textColor.a = alpha;

        if(alpha >= 1)
        {
            isFade = true;
        }
        ColorChange();
    }

    /// <summary>
    /// カラー変更
    /// </summary>
    private void ColorChange()
    {
        text.color = textColor;
    }
}
