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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceanNames.STARTTITLE.ToString());
        }
    }
}
