using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

/*
プレイヤーの位置を共有し、画像・アニメーションを表示させる
必ずPlayerの子として生成する
PlayerBaseでは
RPCAnimLink
を呼び出し、アニメーションの状態を共有する
作成社：飛田
*/

public class PlayerLink : MonoBehaviourPunCallbacks
{
    [SerializeField,Tooltip("自分が操作しているプレイヤー(Set不要)")] GameObject OriginObject;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            SetOrigin(transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine) transform.position = OriginObject.transform.position;
    }

    public void SetOrigin(GameObject OriginPlayer)
    {
        OriginObject = OriginPlayer;
    }

    [PunRPC]
    public void RPCAnimLink(AnimCode anim)
    {

    }
}
