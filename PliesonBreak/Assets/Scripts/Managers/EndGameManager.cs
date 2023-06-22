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
        if(PhotonNetwork.LocalPlayer.GetGameStatus() == (int)GAMESTATUS.ENDGAME_WIN)
        {
            Endtext.text = "GAME CLEAR!!";
        }
        else
        {
            Endtext.text = "GAME OVER..";
        }
        PhotonNetwork.Disconnect();
        BGMManager.Instance.SetBGM(BGMid.ENDING);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceanNames.STARTTITLE.ToString());
        }
    }
}
