using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ConstList;

/*
探索ポイントをオンライン化するスクリプト
探索ポイントでオンライン化する必要があるのは
・探索を終え、アイテムが出現する
・終わった探索を中止し、破壊する
・空のアイテムの場合、設定によって空のまま残す（技術的な問題でアイテムがある場合はこのポイントを破壊する必要がある）
であり、探索ポイントでは探索終了時に
EndIntearact関数を呼ぶ
 */

public class SearchPointLink : MonoBehaviourPunCallbacks
{
    [SerializeField] SearchPoint OriginSearchPoint;

    // Start is called before the first frame update
    void Start()
    {
        OriginSearchPoint = GetComponent<SearchPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// RPC呼び出し用（デバッグ）
    /// </summary>
    /// <param name="RPCname"></param>
    public void CallRPC(string RPCname)
    {
        photonView.RPC(nameof(RPCname), RpcTarget.All);
    }

    /// <summary>
    /// 探索が終了したときに他のポイントに通知する
    /// 空のアイテムを探索したとき、「空のまま残す」か「消滅させるか」を取る
    /// 空でないときは常にtrueにする
    /// 引数：isDestroy>このオブジェクトを含め、他のポイントを破壊するかどうか
    /// </summary>
    public void EndInteract(bool isDestroy)
    {
        if (isDestroy)
        {
            //すべてのプレイヤーで同じポイントを破壊する
            photonView.RPC(nameof(RPCDestroy), RpcTarget.Others);
        }
        //破壊しない処理では他プレイヤーに送信するものはなく、探索したプレイヤーでのみ破壊される
        //※falseの場合、中身がNullDropであることが確定している
    }

    public void SetDropItem(InteractObjs Obj)
    {
        photonView.RPC(nameof(RPCSetDropItem), RpcTarget.Others, Obj);
    }

    /// <summary>
    /// Destroy単体では他のオブジェクトが破壊されないのでRPC化する
    /// </summary>
    [PunRPC]
    void RPCDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 他のユーザーのポイントでも出現させる
    /// </summary>
    [PunRPC]
    void RPCDropItem()
    {
        //探索中断
        OriginSearchPoint.StopSearch();
        //アイテム出現
        OriginSearchPoint.InstantiateItem();
    }

    /// <summary>
    /// 他のプレイヤーにもセットする
    /// </summary>
    /// <param name="Obj"></param>
    [PunRPC]
    void RPCSetDropItem(InteractObjs Obj)
    {
        if(OriginSearchPoint == null) OriginSearchPoint = GetComponent<SearchPoint>();
        OriginSearchPoint.SetDropItem(Obj);
        Debug.Log("RPC ItemSet");
    }

}
