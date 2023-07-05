using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using ConstList;

public class EndGameManager : MonoBehaviour
{
    [SerializeField, Tooltip("終了後のテキスト")] Text Endtext;

    //一定時間は飛ばせないようにする
    private float Timer;
    const float WaitTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.GameResult)
        {
            Endtext.text = "GAME CLEAR!!";
        }
        else
        {
            Endtext.text = "GAME OVER..";
        }
        Debug.Log(PhotonNetwork.LocalPlayer.GetGameStatus());
        PhotonNetwork.Disconnect();
        BGMManager.Instance.SetBGM(BGMid.ENDING);
        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            if (Timer < WaitTime) return;
            SceneManager.LoadScene(SceanNames.STARTTITLE.ToString());
        }
    }
}
