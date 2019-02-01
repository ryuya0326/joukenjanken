using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class gameover : MonoBehaviour {
    //スコア書き込み用スクリプト
    [SerializeField]
    SSDataControl SSDataControl;

    //ゲームオーバーに表示させるオブジェクト、テキスト
    [SerializeField]
    private GameObject gameoverObj,gameover_text, gameover_score;
    [SerializeField]
    private Text scoretext;
    [SerializeField]
    private Text T_gameover_text;
    //一回のみ動く
    bool one = true;
    bool one2 = true;
    bool one3 = true;

    //ゲージがマックスになった時と、ゲームオーバーの処理が終わった時
    bool geageMax = false;
    bool gameoverEnd = false;

    //ゲームオーバーに使うSE;
    [SerializeField]
    private AudioSource SE;
    [SerializeField]
    private AudioClip SEClip;
    [SerializeField]
    private AudioClip HiscoreClip;
    [SerializeField]
    private AudioClip bombClip;
    //ゲームオーバーに使うＢＧＭ
    [SerializeField]
    private AudioSource MainBGM;
    [SerializeField]
    private AudioSource GameOverBGM;
    //ハイスコア消去用タイムを表示するテキスト
    [SerializeField]
    private Text delateText;
    //ハイスコア削除用のタイム
    [SerializeField]
    private float deletetime = 3.5f;
    //ハイスコア達成時に表示するオブジェクト
    [SerializeField]
    private GameObject hiscoreObj;


    float barnum = 0; //スライダーのバーの最大

    //スライダー
    [SerializeField]
    private Slider scoreSlider;
    [SerializeField]
    private Image sliderfill;
    
    //紙吹雪のエフェクト
    [SerializeField]
    private GameObject kamief;

    static public int scoreEND;

    private void Update()
    {
        //タイムがなくなったらゲームオーバー
        if (time.t <= 0 && one == true)
        {
            StartCoroutine(GameOver());
            StartCoroutine(geageUP());
        }  
        if(gameoverEnd == true)
        {
            //ZEROのキー長押し三秒でハイスコアを消す
            if (Input.GetKey(KeyCode.Z) &&
                Input.GetKey(KeyCode.E) &&
                Input.GetKey(KeyCode.R) &&
                Input.GetKey(KeyCode.O) &&
                deletetime > 0)
            {
                delateText.text = "" + deletetime.ToString("F0");
                deletetime -= Time.deltaTime;
                if (deletetime <= 0)
                {
                    delateText.text = "ハイスコア消去完了";
                    SE.PlayOneShot(bombClip);
                    deletetime = 0;
                    PlayerPrefs.DeleteAll();
                }
            }
            //長押しが解除されたら3秒にする
            else if (deletetime > 0)
            {
                deletetime = 3.5f;
                delateText.text = "";
            }
            //ゲームを閉じる処理
            if (Input.GetButtonDown("Start"))
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
        }
    }

    //ゲームオーバーの処理
    IEnumerator GameOver()
    {
        one = false;
        gameoverObj.SetActive(true);
        //gameoverに一回だけする処理
        if (one3 == true)
        {         
            //スコアのバーの最大値
            barnum = janken.score / 35.0f;
            //メインのBGMをミュートに
            MainBGM.mute = true;

            //ゲージの増加処理
            for (; ; )
            {
                scoreSlider.value += 0.001f;
                //スコアバーが最大値になったら
                if (scoreSlider.value >= barnum)
                {
                    scoreSlider.value = barnum;
                    geageMax = true;
                    if(scoreSlider.value * 35.0f <= 0)
                        T_gameover_text.text = "Worst";
                    break;
                }
                //1~10
                if (scoreSlider.value * 35.0f <= 10)
                {
                    T_gameover_text.text = "Worst";
                    sliderfill.color = new Color(155f / 255f, 0f / 255f, 255f / 255f, 255f / 255f);
                    scoreEND = 0;
                }
                //11~20
                else if (scoreSlider.value * 35.0f <= 20)
                {
                    T_gameover_text.text = "Bad";
                    sliderfill.color = new Color(0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f);
                    scoreEND = 1;
                }
                //21~25
                else if (scoreSlider.value * 35.0f <= 25)
                {
                    T_gameover_text.text = "Good";
                    sliderfill.color = new Color(40f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                    scoreEND = 2;
                }
                //26~34
                else if (scoreSlider.value * 35.0f <= 34)
                {
                    T_gameover_text.text = "Great";
                    sliderfill.color = new Color(255f / 255f, 69f / 255f, 0f / 255f, 255f / 255f);
                    scoreEND = 3;
                }
                //35~
                else if (scoreSlider.value * 35.0f <= 100000)
                {
                    T_gameover_text.text = "Excellent";
                    sliderfill.color = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                    scoreSlider.value = 1.0f;
                    geageMax = true;
                    scoreEND = 4;
                    break;
                }
                yield return null;
            }
            //BGMを変える処理
            GameOverBGM.mute = false;
            GameOverBGM.time = 0f;
            one3 = false;
        }

        int score = janken.score;
        gameover_score.SetActive(true);
        scoretext.text = ""+score;

        //スコアをテキストに起こす処理
        SSDataControl.SaveData(0, 1, scoreEND, 5);

        yield return new WaitForSeconds(1.5f);

        //ハイスコア更新されたとき
        if (janken.highScoreUP == true)
        {
            if (one2 ==true)
            {
                SE.PlayOneShot(HiscoreClip);
                one2 = false;
                kamief.SetActive(true);
            }
            hiscoreObj.SetActive(true);
        }

        gameoverEnd = true;
    }
    //ゲージアップ時のSE
    IEnumerator geageUP()
    {
        if (one3 == true) {
            while (geageMax==false) {
                SE.PlayOneShot(SEClip);
                yield return new WaitForSeconds(0.09f);
            }
        }
    }
}