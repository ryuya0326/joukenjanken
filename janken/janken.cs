using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class janken : MonoBehaviour {

    private int _playernum = 1;//1:グー,2:チョキ,3:パー
    private int _enemynum = 1;//1:グー,2:チョキ,3:パー
    private int _zyoukenNum = 1;//1:勝ち 2:負け 3:あいこ

    public static bool wait = false; //ポーズ処理時にtrueになる変数
    [SerializeField]
    private Image _enemyhand; //敵の手のイメージ
    [SerializeField]
    private Text _zyouken; //条件のテキスト
    [SerializeField]
    private Text _scoreText; //スコアのテキスト
    [SerializeField]
    private Text _HighScoreText; //ハイスコアのテキスト

    [SerializeField]
    private GameObject _ketteiImage; //自分の手が決定したときに動くイメージ
    [SerializeField]
    private GameObject[] _playerhandPos;//↑のposition
    [SerializeField]
    private Image[] _hand; //敵の手を変える時用のイメージ

    [SerializeField]
    private GameObject[] _hantei; //1.〇（正解）2.×(不正解)
    Vector3[] hanteipos=new Vector3[2];　//判定の初期位置
    Vector3[] hanteiscale = new Vector3[2];//判定の初期スケール

    [SerializeField]
    private AudioSource SE; //効果音のソース
    [SerializeField]
    private AudioClip[] SEclip;//1:正解 2:間違い
    [SerializeField]
    private Text timeText;//間違えた時のタイムの処理に使うtext

    public static bool highScoreUP = false; //ハイスコアが上回った時true
    public static int score = 0;//スコア
    private int highScore;//ハイスコア
    private string key = "HIGH SCORE";//ハイスコア保存用のkey
    private void Start()
    {
        //〇×判定の初期値を取得
        hanteipos[0] = _hantei[0].transform.position;
        hanteipos[1] = _hantei[1].transform.position;
        hanteiscale[0] = _hantei[0].transform.localScale;
        hanteiscale[1] = _hantei[1].transform.localScale;
        //ハイスコアのキー設定
        highScore = PlayerPrefs.GetInt(key, 0);
        _HighScoreText.text = highScore.ToString();
        //最初相手の手
        random();
        //スコアの初期設定
        score = 0;
        _scoreText.text =score.ToString();
        //自分の手が決定したときに動くイメージの初期位置
        _playernum = 1;

    }
    // Update is called once per frame
    void Update () {
        //weitがfalseになった時動けるようになる
        if (wait == false)
        {
            if (time.t > 0)
            {
                playermove();
            }
        }
        //スコアがハイスコアを上回った時
        if (score > highScore)
        {
            highScore = score; //ハイスコア更新
            PlayerPrefs.SetInt(key, highScore);//ハイスコアを保存
            _HighScoreText.text = highScore.ToString();//ハイスコアを表示

            highScoreUP = true;
        }

    }

    //playerの動き
    void playermove()
    {
        //グーの時
        if (Input.GetButtonDown("X")||Input.GetKeyDown(KeyCode.Z)) {
            _playernum = 1;
            wait = true;
            StartCoroutine("jankenkekka");
        }
        //チョキの時
        else if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.X))
        {
            _playernum = 2;
            wait = true;
            StartCoroutine("jankenkekka");
        }
        //パーの時
        else if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.C))
        {
            _playernum = 3;
            wait = true;
            StartCoroutine("jankenkekka");
        }
        //決定UIを移動（黄色いやつ）
        switch (_playernum)
        {
            case 1:
                _ketteiImage.transform.position = _playerhandPos[0].transform.position;
                break;
            case 2:
                _ketteiImage.transform.position = _playerhandPos[1].transform.position;
                break;
            case 3:
                _ketteiImage.transform.position = _playerhandPos[2].transform.position;
                break;
            default:
                break;
        }
    }

    //敵の手と条件を変える
   void random()
    {
        _enemynum = Random.Range(1, 4);
        _zyoukenNum = Random.Range(1, 4);
        switch (_enemynum)
        {
            case 1://相手グー
                _enemyhand.sprite = _hand[0].sprite;
                break;
            case 2://相手チョキ
                _enemyhand.sprite = _hand[1].sprite;
                break;
            case 3://相手パー
                _enemyhand.sprite = _hand[2].sprite;
                break;
        }
        //ランダムで条件を決める
        switch (_zyoukenNum)
        {
            case 1:
                _zyouken.text = "勝て";
                break;
            case 2:
                _zyouken.text = "負けろ";
                break;
            case 3:
                _zyouken.text = "あいこにしろ";
                break;
        }
    }

    IEnumerator jankenkekka()
    {
        switch (_playernum)
        {
            case 1://グーの時
                switch (_zyoukenNum)
                {
                    case 1://勝ち
                        if (_enemynum == 2)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                    case 2://まけ
                        if (_enemynum == 3)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                            break;            
                    case 3://あいこ
                        if (_enemynum == 1)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                }
                break;
            case 2://チョキの時
                switch (_zyoukenNum)
                {
                    case 1://勝ち
                        if (_enemynum == 3)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                    case 2://まけ
                        if (_enemynum == 1)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                    case 3://あいこ
                        if (_enemynum == 2)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                }
                break;
            case 3://パーの時
                switch (_zyoukenNum)
                {
                    case 1://勝ち
                        if (_enemynum == 1)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                    case 2://まけ
                        if (_enemynum == 2)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                    case 3://あいこ
                        if (_enemynum == 3)
                        {
                            OK();
                        }
                        else
                        {
                            NG();
                        }
                        break;
                }
                break;
        }
        _scoreText.text = score.ToString();//スコアをテキストに反映
        yield return new WaitForSeconds(0.5f);
        //〇と×を消す
        _hantei[0].SetActive(false);
        _hantei[1].SetActive(false);
        wait = false;
        random();
    }
    //正解の処理
    void OK()
    {
        score++;
        SE.PlayOneShot(SEclip[0]);
        //初期位置に移動
        _hantei[0].transform.position = hanteipos[0];
        _hantei[0].transform.localScale = hanteiscale[0];
        _hantei[0].SetActive(true);
        //dotweenでジャンプ
        _hantei[0].transform.DOLocalJump(Vector3.one, 250, 2, 1.5f).Restart();
    }
    //不正解の処理
    void NG()
    {
        SE.PlayOneShot(SEclip[1]);
        time.t -= 5;
        timeText.text =time.t.ToString("F0");
        //初期位置に移動
        _hantei[1].transform.position = hanteipos[1];
        _hantei[1].transform.localScale = hanteiscale[1];
        _hantei[1].SetActive(true);
        _hantei[1].transform.DOPunchScale(new Vector3(0.7f,0.7f),1.5f).Restart();
    }
}