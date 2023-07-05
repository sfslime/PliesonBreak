using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

/*
Photonに関する、接続等を監視するスクリプト
 */

public class ConectObservation : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("誰かが退出したときに呼ばれる処理")] UnityEvent LeftPlayer;
    [SerializeField, Tooltip("サーバーから切断されたときに呼ばれる処理")] UnityEvent DiscnnectServer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
        DiscnnectServer.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}が退出しました");
        LeftPlayer.Invoke();
    }
}
