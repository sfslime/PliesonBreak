using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PrisonLink : MonoBehaviourPunCallbacks
{
    //リンクさせるオブジェクト
    [SerializeField] Prison originObject;
    //ゲームマネージャーのインスタンス
    GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        originObject = GetComponent<Prison>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 牢屋の状態を共有する
    /// 引数で開けたかどうかを取る
    /// </summary>
    public void StateLink(bool isopende)
    {
        photonView.RPC(nameof(RPCStateLink), RpcTarget.Others, isopende);
    }

    [PunRPC]
    void RPCStateLink(bool isopened)
    {
        originObject.PrisonOpen(isopened);
    }
}
