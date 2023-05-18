using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class ConectServer : MonoBehaviourPunCallbacks
{
    [Tooltip("サーバー接続時に呼ばれるイベント")] public UnityEvent OnConect;
    [Tooltip("ルーム参加時に呼ばれるイベント")] public UnityEvent OnJoinde;
    [SerializeField,Header("テスト")] string objname;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        OnConect.Invoke();
        Debug.Log("OnConect");
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        OnJoinde.Invoke();
        Debug.Log("Onjoin");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //テスト
    public void PopPlayer()
    {
        var Link = PhotonNetwork.Instantiate(objname, player.transform.position, Quaternion.identity);
        Link.GetComponent<PlayerLink>().SetOrigin(player);
    }
}
