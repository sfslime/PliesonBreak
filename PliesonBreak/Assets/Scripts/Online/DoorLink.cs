using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorLink : MonoBehaviourPunCallbacks
{
    //リンクさせるオブジェクト
    [SerializeField] DoorBase originObject;
    //ゲームマネージャーのインスタンス
    GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        originObject = GetComponent<DoorBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ドアが開かれたときに他のプレイヤーのドアも開ける
    /// 引数で開いたかどうかを確認する
    /// </summary>
    /// <param name="isOpen"></param>
    /// <param name="isColition"></param>
    public void DoorStateLink(bool isOpen)
    {
        photonView.RPC(nameof(RPCDoorLink), RpcTarget.Others, isOpen);
    }

    [PunRPC]
    void RPCDoorLink(bool isOpen)
    {
        originObject.DoorOpen(isOpen);
    }
}
