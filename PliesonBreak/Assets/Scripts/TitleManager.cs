using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class TitleManager : MonoBehaviourPunCallbacks
{
    const string RoomHead = "Room ";
    const string RoomMidle =  "\n参加人数\n";
    const string RoomOK = "参加可能";
    const string RoomNotOK = "参加不可";

    [SerializeField, Tooltip("1ルームの参加人数")] int MaxPlayer;
    [SerializeField, Tooltip("参加ボタンリスト")] CanvasGroup ButtonRoot;
    //ロビー参加済みか
    private bool isInLobby;

    private List<RoomInfo> RoomInfos;

    /// <summary>
    /// コネクトサーバーから呼ばれ、
    /// ロビーへの参加を試行する
    /// </summary>
    public void TryRobyJoin()
    {
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// ロビー接続時に呼ばれる
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        isInLobby = true;

        ButtonRoot.interactable = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        
        foreach(var room in roomList)
        {
            if (int.Parse(room.Name) < 0 || int.Parse(room.Name) > ButtonRoot.transform.childCount) continue;

            //ルーム番号に応じた情報を更新
            var button = ButtonRoot.transform.GetChild(int.Parse(room.Name));
            button.transform.GetChild(0).GetComponent<Text>().text = RoomHead+room.Name+RoomMidle + room.PlayerCount + "/" + room.MaxPlayers 
                                                                     + "\n"+ (room.IsOpen ? RoomOK : RoomNotOK);
            if (room.IsOpen)
            {
                button.GetComponent<Button>().interactable = true;
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>
    /// ボタンを押したら呼べるようにする
    /// </summary>
    public void JoinRoom(int RoomNm)
    {
        ButtonRoot.interactable = false;
        ConectServer.RoomProperties.RoomName = RoomNm.ToString();
        ConectServer.RoomProperties.MaxPlayer = MaxPlayer;
        SceneManager.LoadScene("WaitRoom");
    }

    // Start is called before the first frame update
    void Start()
    {
        isInLobby = false;
        //参加処理中かロビー参加前は押せなくする
        ButtonRoot.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
