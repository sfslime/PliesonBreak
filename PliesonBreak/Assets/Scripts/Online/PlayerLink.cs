using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

/*
プレイヤーの位置を共有し、画像・アニメーションを表示させる
必ずPlayerの子(スプライトに付ける)として生成する
PlayerBaseでは
AnimLink
を呼び出し、アニメーションの状態を共有する
作成社：飛田
*/

public class PlayerLink : MonoBehaviourPun, IPunObservable
{
    [SerializeField,Tooltip("自分が操作しているプレイヤー(Set不要)")] GameObject OriginObject;
    [SerializeField, Tooltip("アニメーションスクリプト")] PlayerAnimation PlayerAnimation;

    [SerializeField,Header("移動共有用変数"),Tooltip("滑らかな移動のための速度")] float LerpSpeed;
    [SerializeField,Tooltip("接続済みかどうか")] bool isJoin = false;
    [SerializeField, Tooltip("画像とプレイヤーのずれ修正")] Vector3 Offset;
    Vector3 PlayerPosition;
    public Vector3 Scale;

    // Start is called before the first frame update
    void Start()
    {
        Scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //移動計算用ポジションに変換
        if (photonView.IsMine && isJoin)
        {
            PlayerPosition = OriginObject.transform.position;
        }
    }

    private void FixedUpdate()
    {
        //プレイヤーに追従
        if (!photonView.IsMine && !isJoin) return;
        PlayerPosition += Offset;
        transform.position = Vector3.Lerp(transform.position, PlayerPosition, LerpSpeed * Time.fixedDeltaTime);
        transform.localScale = Scale;
        if(OriginObject.transform.localScale.x < 0)
        {
            TurnSprite(false);
        }
        else if (OriginObject.transform.localScale.x > 0)
        {
            TurnSprite(true);
        }
    }

    /// <summary>
    /// 操作している元のプレイヤーをセットし、接続を確認する
    /// </summary>
    /// <param name="OriginPlayer"></param>
    public void SetOrigin(GameObject OriginPlayer)
    {
        OriginObject = OriginPlayer;
        isJoin = true;
    }

    public void TurnSprite(bool isRight)
    {
        if (isRight)
        {
            if(Scale.x < 0)
            {
                Scale.x *= -1;
            }
        }
        else
        {
            if(Scale.x > 0)
            {
                Scale.x *= 1;
            }
        }
        transform.localScale = Scale;
    }

    public GameObject GetOrigin()
    {
        return OriginObject;
    }

    /// <summary>
    /// 他プレイヤーにアニメーションの状態を共有する
    /// 引数でアニメーションIDを取る
    /// </summary>
    /// <param name="anim"></param>
    public void AnimLink(AnimCode anim)
    {
        //photonView.RPC(nameof(RPCAnimLink), RpcTarget.Others, anim);
    }

    /// <summary>
    /// アニメーションを共有するRPC
    /// AnimLinkで呼び出す
    /// </summary>
    /// <param name="anim"></param>
    [PunRPC]
    void RPCAnimLink(AnimCode anim)
    {
        PlayerAnimation.SetAnim(anim);

    }

    /// <summary>
    /// 位置情報をクローンした他オブジェクトと共有する
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // プレイヤーオブジェクトの位置情報を送信
            stream.SendNext(PlayerPosition);
            stream.SendNext(Scale);
        }
        else
        {
            // プレイヤーオブジェクトの位置情報を受信
            PlayerPosition = (Vector3)stream.ReceiveNext();
            Scale = (Vector3)stream.ReceiveNext();
        }
    }
}
