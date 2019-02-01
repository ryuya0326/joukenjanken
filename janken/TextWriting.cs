using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextWriting : MonoBehaviour {
    int score = janken.score;
    string s = "";

    //スコアをscoretext.txtに書き込み
    public void WriteScore () {
        score = gameover.scoreEND;
        s = score.ToString();
        StreamWriter sw = new StreamWriter("scoretext.txt", false);//true 追記　false 上書き
        sw.WriteLine(s);
        sw.Flush();
        sw.Close();
	}
}
