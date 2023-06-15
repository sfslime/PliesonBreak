using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ConstList;

/*
マッチング中に待機するロビー内の管理マネージャー
現在のロビー状態の表示と、ロビー情報のコントロールを行う
マッチング後はゲーム開始に合わせてシーンの移動を行う
 */

public class WaitRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("情報を表示するメッセージ")] Text MessageText;
    [SerializeField, Tooltip("開始ボタン")] Button SceanMoveButton;

    //マスターかどうか
    private bool isMaster;
    //ルーム内で接続できているか
    private bool isInRoom;

    /// <summary>
    /// 現在のメンバーでゲームシーンに移動する
    /// </summary>
    public void MoveGameScean()
    {
        if (isMaster)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(/*"ProttypeSeacn");*/SceanNames.GAME.ToString());
        }
    }

    /// <summary>
    /// このシーン移動後に改めてルーム接続を試行する
    /// </summary>
    void TryRoomJoin()
    {
        //タイトルで確立した情報で接続
        PhotonNetwork.JoinOrCreateRoom(ConectServer.RoomProperties.RoomName, new RoomOptions(), TypedLobby.Default);
    }

    /// <summary>
    /// 接続時に呼ばれる関数
    /// マスターの場合、各種設定を行う
    /// </summary>
    public override void OnJoinedRoom()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        isInRoom = true;
        if (isMaster)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = (byte)ConectServer.RoomProperties.MaxPlayer;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("OnJoin");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        MessageText.text = message + " によって接続出来ません";
    }

    /// <summary>
    /// ルーム内の情報を更新し、表示する
    /// </summary>
    void RoomStatusUpDate()
    {
        if (!isMaster)
        {
            SceanMoveButton.interactable = false;
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        SceanMoveButton.transform.GetChild(0).gameObject.GetComponent<Text>().text
            = "開始(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")";

        //メンバリストを表示

    }

    // Start is called before the first frame update
    void Start()
    {
        isMaster = false;
        isInRoom = false;
        TryRoomJoin();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRoom)
        {
            RoomStatusUpDate();
        }
    }
}
