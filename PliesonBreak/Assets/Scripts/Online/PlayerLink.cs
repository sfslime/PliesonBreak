using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

/*
�v���C���[�̈ʒu�����L���A�摜�E�A�j���[�V������\��������
�K��Player�̎q(�X�v���C�g�ɕt����)�Ƃ��Đ�������
PlayerBase�ł�
AnimLink
���Ăяo���A�A�j���[�V�����̏�Ԃ����L����
�쐬�ЁF��c
*/

public class PlayerLink : MonoBehaviourPunCallbacks
{
    [SerializeField,Tooltip("���������삵�Ă���v���C���[(Set�s�v)")] GameObject OriginObject;
    [SerializeField, Tooltip("�A�j���[�V�����X�N���v�g")] PlayerAnimation PlayerAnimation;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            SetOrigin(transform.parent.gameObject);
            PlayerAnimation = OriginObject.GetComponent<PlayerAnimation>();
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

    public void AnimLink(AnimCode anim)
    {
        photonView.RPC(nameof(RPCAnimLink), RpcTarget.Others, anim);
    }

    [PunRPC]
    void RPCAnimLink(AnimCode anim)
    {
        PlayerAnimation.SetAnim(anim);
    }
}
