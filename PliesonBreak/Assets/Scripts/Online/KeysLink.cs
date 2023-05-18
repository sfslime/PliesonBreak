using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
KeyItemなどをオンライン化するスクリプト
想定されるアイテムのリンクは
・そのアイテムが取得され、一時的に破壊される
・取得されたアイテムとプレイヤーが持っているアイテムが切り替わる
なので、このリンクスクリプトではその二点をKeyなどに要求する
Keyではこれに応じて
StateLink
を呼び出す
 */


public class KeysLink : MonoBehaviourPunCallbacks
{
    //リンクさせるオブジェクト
    [SerializeField] InteractObjectBase originObject;
    //ゲームマネージャーのインスタンス
    GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        //ゲームマネージャーインスタンスを設定
        GameManager = GameManager.GameManagerInstance;
        if (GameManager == null) Debug.Log("GameManager not found");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 取得されたときに呼び出され、そのアイテムを破壊する
    /// 可能な限り、この関数で破壊すること
    /// 引数：isOthes>他のプレイヤーのみを破壊するかどうか
    /// </summary>
    /// <param name="isOthes"></param>
    public void StateLink(bool isOthes)
    {
        if (isOthes)
        {
            photonView.RPC(nameof(RPCDestroy), RpcTarget.Others);
        }
        else
        {
            photonView.RPC(nameof(RPCDestroy), RpcTarget.All);
        }
    }

    /// <summary>
    /// アイテムを切り替える場合のオーバーロード
    /// 新しいアイテム情報を引数で取る
    /// </summary>
    /// <param name="ObjID"></param>
    public void StateLink(InteractObjectBase.InteractObjs ObjID)
    {
        photonView.RPC(nameof(RPCChangeItem), RpcTarget.Others, ObjID);
    }

    /// <summary>
    /// アイテムを取得したときに他のプレイヤーの画面にある
    /// そのアイテムを破壊する処理
    /// </summary>
    [PunRPC]
    public void RPCDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 他のプレイヤーの画面でアイテムが切り替わった時に
    /// それ以外のプレイヤーでも変更する
    /// </summary>
    /// <param name="ObjID"></param>
    [PunRPC]
    public void RPCChangeItem(InteractObjectBase.InteractObjs ObjID)
    {
        //ゲームマネージャーからスプライト取得
        var Sprite = GameManager.ReturnSprite(ObjID);

        //画像の切り替えを要求
        //originObject
    }
}
