using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//動画再生関連のマネージャースクリプト
public class VideoManager : MonoBehaviour
{
    //シングルトン設定
    private static VideoManager instance;
    public static VideoManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<VideoManager>();
            }
            return instance;
        }
    }

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject video;

    //動画を流すまで待機する時間(秒)
    [SerializeField, Tooltip("動画を流すまでの待機時間(秒)")] private float waitTime = 10f;
    //時間測定用タイマー
    private float timer = 0f;
    //動画が再生中かどうか
    private bool isPlayVideo = false;

    [SerializeField] private Fade fade;
    //フェードにかかる時間
    [SerializeField, Tooltip("フェードの時間")] private float fadeTime = 0.5f;

    //フェード処理が終わったかどうか
    private bool isFade = false;

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
        ResetVideo();
        video.SetActive(false);
    }

    /// <summary>
    /// 動画の再設定
    /// </summary>
    public void ResetVideo()
    {
        //最初のフレームに戻す処理
        videoPlayer.frame = 0;
        videoPlayer.Play();
        videoPlayer.Pause();

        timer = 0f;
        isPlayVideo = false;
        isFade = false;
    }

    /// <summary>
    /// ビデオ関連の更新処理
    /// </summary>
    public void VideoUpdate()
    {
        //再生されていないとき
        if (!isPlayVideo)
        {
            //待機時間を超えた時
            if (timer >= waitTime)
            {
                if (!video.activeSelf)
                {
                    //一度だけ　フェードインが開始されていないとき
                    if (!isFade)
                    {
                        //フェードイン開始
                        //フェードインが終わったらビデオを映すオブジェクトを表示
                        fade.FadeIn(fadeTime, () =>
                        {
                            video.SetActive(true);
                            videoPlayer.loopPointReached += LoopPointReached;
                            //フェードアウト開始
                            //フェードアウトが終わったらビデオを開始
                            fade.FadeOut(fadeTime, () => videoPlayer.Play());
                            isPlayVideo = true;

                        });
                        isFade = true;
                    }
                }
                else
                {
                }
            }
            else
            {
                //何もキーが押されていない時間を計測
                timer += Time.deltaTime;
            }

        }
        //再生中の時
        else
        {

            //何かしらのキー、マウスが押されたとき
            if (Input.anyKeyDown)
            {
                ResetVideo();
                //ビデオを映すオブジェクトを非表示に
                video.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 動画を最後まで再生しきった時の処理
    /// </summary>
    public void LoopPointReached(VideoPlayer vp)
    {
        isFade = false;
        if (video.activeSelf)
        {
            //一度だけ　フェードインが開始されていないとき
            if (!isFade)
            {
                //フェードイン開始
                //フェードインが終わったらビデオを映すオブジェクトを表示
                fade.FadeIn(fadeTime, () => 
                { 
                    video.SetActive(false);
                    fade.FadeOut(fadeTime);
                });
                isFade = true;
            }
        }

        ResetVideo();
    }

}
