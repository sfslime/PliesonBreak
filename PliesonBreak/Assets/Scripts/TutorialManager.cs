using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using ConstList;

public class TutorialManager : MonoBehaviour
{
    #region チュートリアル指示定義

    List<string> TutorialTexts = new List<string>() { "扉を開けよう","鍵を探そう","牢屋を開けよう","探索をしよう","鍵を取ろう","扉を開けよう","先へ進もう","見つからないように進もう","脱出のパーツを探そう>残り2つ","修理しよう", "脱出のパーツを探そう>残り1つ","脱出だ！" };

    List<string> TutorialMessage = new List<string>() { "ここから脱出するべきだ", "鍵が必要だ...", "あの牢屋、外からな開けられるな", "あそこになにかありそうだ", "あの扉と同じ色。これだな", "先を急ごう", "誰かいるな...", "見つかったら終わりだな", "この船にはパーツが足りない", "一つしか持てない。一度持っていこう", "これで最後だな", "追われる前に逃げよう" };

    [SerializeField, Tooltip("矢印の位置")] List<GameObject> TutorialArrow = new List<GameObject>();
    #endregion

    [SerializeField,Tooltip("現在のチュートリアル段階")] int Faze;

    [SerializeField, Tooltip("目的表示テキストの表示先")] Text TutorialTextObject;
    [SerializeField, Tooltip("吹き出しの表示先")] Text TutorialMessageObject;
    // Start is called before the first frame update
    void Start()
    {
        Faze = -1;

        foreach(var Arrow in TutorialArrow)
        {
            Arrow.SetActive(false);
        }

        TutorialTrriger(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// プレイヤーがチュートリアルトリガーを踏んだとき
    /// チュートリアルを進行させる
    /// </summary>
    /// <param name="Trrigerfaze"></param>
    public void TutorialTrriger(int Trrigerfaze)
    {
        if(Faze + 1 == Trrigerfaze)
        {
            Faze++;
            TutorialMessageObject.text = TutorialTexts[Faze];
            TutorialMessageObject.text = TutorialMessage[Faze];
            TutorialArrow[Faze].SetActive(true);
        }

        if(Faze == TutorialTexts.Count-1)
        {

        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            //画面が暗くなる演出
            break;
        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceanNames.TITLE.ToString());
    }

}
