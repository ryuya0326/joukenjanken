using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class gameStart : MonoBehaviour {
    //スタートする時に表示するオブジェクト
    [SerializeField]
    private GameObject _gameStartObj;
    [SerializeField]
    private Text _starttext;
    [SerializeField]
    private float Starttime = 3.5f;

    bool one = false;
    Sequence GOseq;
    //スタート時に流れるSEとメインBGM
    [SerializeField]
    private AudioSource BGM;
    [SerializeField]
    private AudioSource SE;
    [SerializeField]
    private AudioClip[] SEClip;


    //ゲームスタート時にポーズ
    //画面サイズを固定
    private void Start()
    {
        janken.wait = true;
        _gameStartObj.SetActive(true);
        Screen.SetResolution(1920, 1080, false, 60);
    }



    private void Update()
    {
        if(Starttime>=-0.5f)
        Starttime -= Time.deltaTime;
        //カウントダウンする
        if (Starttime >= 0.6f)
        {
            if (one == false)
            {
                StartCoroutine("texttween");
                one = true;
            }
                _starttext.text = Starttime.ToString("F0");
        }


        //GO
        else if(Starttime >= -0.5f)
        {
            if (one == true)
            {
                GO();
                one = false;
            }
            _starttext.text = "GO";
        }
        //表示終わったらゲーム開始（ポーズを解く）
        else if(one == false)
        {
            _gameStartObj.SetActive(false);
            janken.wait = false;
            one = true;
            BGM.mute = false; //メインBGMのミュートを外す
            BGM.time = 0f;    //メインBGMを最初に合わせる
        }
    }
    //カウントダウンをdotweenで動かす
    IEnumerator texttween()
    {
        for (int i = 0; i < 3; i++)
        {
            SE.PlayOneShot(SEClip[0]);
            _starttext.transform.DOScale(new Vector3(2, 2), 0.98f);
            yield return new WaitForSeconds(0.98f);
            _starttext.transform.localScale = new Vector3(0, 0);
        }
    }
    //カウントダウン後のGOの処理
    void GO()
    {
        SE.PlayOneShot(SEClip[1]);
        GOseq = DOTween.Sequence();
        GOseq.Append(_starttext.transform.DOScale(new Vector3(6, 6), 0.98f));
        GOseq.Join(_starttext.transform.DORotate(new Vector3(0, 0, -360), 0.98f, RotateMode.FastBeyond360));
    }
}