using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class time : MonoBehaviour
{
    //タイム参照用
    public static float t = 0.0f;
    //インスペクターで変更用
    public float  starttime=0.0f;
    Text timeText;

    private void Start()
    {
        //タイムを初期化
        timeText = gameObject.GetComponent<Text>();
        t = starttime;
    }
    // Update is called once per frame
    void Update()
    {
        //waitがtrueじゃない時のみタイムを進める
        if (janken.wait == false)
        {
            timer();
        }
    }
    public void timer()
    {
        //マイナスにならないように0以下は0に
        timeText.text =t.ToString("F0");
        if (t > 0)
        {
            t -= Time.deltaTime;
        }
        else
        {
            t = 0;
        }
    }
}