using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ConstList;

public class GoalLink : MonoBehaviourPunCallbacks
{
    [SerializeField] Goal OriginObject;
    // Start is called before the first frame update
    void Start()
    {
        OriginObject = GetComponent<Goal>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 他のプレイヤーとアイテム状態を共有する
    /// </summary>
    public void StateLink(InteractObjs Item)
    {
        photonView.RPC(nameof(RPCSetItem), RpcTarget.Others, Item);
    }

    [PunRPC]
    void RPCSetItem(InteractObjs Item)
    {
        OriginObject.SetEscapeItem(Item);
    }
}
