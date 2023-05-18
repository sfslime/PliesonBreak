using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

/*
�v���C���[�̈ʒu�����L���A�摜�E�A�j���[�V������\��������
�K��Player�̎q�Ƃ��Đ�������
PlayerBase�ł�
RPCAnimLink
���Ăяo���A�A�j���[�V�����̏�Ԃ����L����
�쐬�ЁF��c
*/

public class PlayerLink : MonoBehaviourPunCallbacks
{
    [SerializeField,Tooltip("���������삵�Ă���v���C���[(Set�s�v)")] GameObject OriginObject;
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
