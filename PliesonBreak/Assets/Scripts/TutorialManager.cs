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

    [SerializeField, Tooltip("矢印の位置の親")] GameObject ArrowRoot;
    [SerializeField, Tooltip("次のチュートリアルトリガーの親")] GameObject TrrigerPointRoot;
    #endregion

    [SerializeField,Tooltip("現在のチュートリアル段階")] int Faze;

    [SerializeField, Tooltip("目的表示テキストの表示先")] Text TutorialTextObject;
    [SerializeField, Tooltip("吹き出しの表示先")] Text TutorialMessageObject;

    [SerializeField, Tooltip("終了時のマスク")] Image EndMask;
    [SerializeField, Tooltip("真っ暗になるまでのレート"),Range(0.01f,0.2f)] float MaskRate;

    public static TutorialManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Faze = -1;

        for(int i = 0; i < ArrowRoot.transform.childCount; i++)
        {
            ArrowRoot.transform.GetChild(i).gameObject.SetActive(false);
            TrrigerPointRoot.transform.GetChild(i).gameObject.SetActive(false);
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
            TutorialTextObject.text = TutorialTexts[Faze];
            TutorialMessageObject.text = TutorialMessage[Faze];
            ArrowRoot.transform.GetChild(Faze).gameObject.SetActive(true);
            TrrigerPointRoot.transform.GetChild(Faze).gameObject.SetActive(true);
            if(Faze != 0)
            {
                ArrowRoot.transform.GetChild(Faze-1).gameObject.SetActive(true);
                TrrigerPointRoot.transform.GetChild(Faze-1).gameObject.SetActive(true);
            }
        }

        if(Faze == TutorialTexts.Count-1)
        {
            StartCoroutine(EndTutorial());
        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(3f);

        float ClearLance = 0;
        while (true)
        {
            //画面が暗くなる演出
            if(EndMask.color.a >= 0.8f)
            {
                break;
            }
            ClearLance += MaskRate;
            EndMask.color = new Color32(1, 1, 1, (byte)ClearLance);
            yield return null;
        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceanNames.LOBBY.ToString());
    }

}
