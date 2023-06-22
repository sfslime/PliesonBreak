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

public class PlayerLink : MonoBehaviourPun, IPunObservable
{
    [SerializeField,Tooltip("���������삵�Ă���v���C���[(Set�s�v)")] GameObject OriginObject;
    [SerializeField, Tooltip("�A�j���[�V�����X�N���v�g")] PlayerAnimation PlayerAnimation;

    [SerializeField,Header("�ړ����L�p�ϐ�"),Tooltip("���炩�Ȉړ��̂��߂̑��x")] float LerpSpeed;
    [SerializeField,Tooltip("�ڑ��ς݂��ǂ���")] bool isJoin = false;
    [SerializeField, Tooltip("�摜�ƃv���C���[�̂���C��")] Vector3 Offset;
    Vector3 PlayerPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�ړ��v�Z�p�|�W�V�����ɕϊ�
        if (photonView.IsMine && isJoin)
        {
            PlayerPosition = OriginObject.transform.position;
        }
    }

    private void FixedUpdate()
    {
        //�v���C���[�ɒǏ]
        if (!photonView.IsMine && !isJoin) return;
        PlayerPosition += Offset;
        transform.position = Vector3.Lerp(transform.position, PlayerPosition, LerpSpeed * Time.fixedDeltaTime);
        if (PlayerPosition.x - transform.position.x > 0 && transform.localScale.x < 0)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }else if(PlayerPosition.x - transform.position.x < 0 && transform.localScale.x > 0)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        
    }

    /// <summary>
    /// ���삵�Ă��錳�̃v���C���[���Z�b�g���A�ڑ����m�F����
    /// </summary>
    /// <param name="OriginPlayer"></param>
    public void SetOrigin(GameObject OriginPlayer)
    {
        OriginObject = OriginPlayer;
        isJoin = true;
    }

    public void SpriteMove()
    {
        
    }

    

    public GameObject GetOrigin()
    {
        return OriginObject;
    }

    /// <summary>
    /// ���v���C���[�ɃA�j���[�V�����̏�Ԃ����L����
    /// �����ŃA�j���[�V����ID�����
    /// </summary>
    /// <param name="anim"></param>
    public void AnimLink(AnimCode anim)
    {
        //photonView.RPC(nameof(RPCAnimLink), RpcTarget.Others, anim);
    }

    /// <summary>
    /// �A�j���[�V���������L����RPC
    /// AnimLink�ŌĂяo��
    /// </summary>
    /// <param name="anim"></param>
    [PunRPC]
    void RPCAnimLink(AnimCode anim)
    {
        PlayerAnimation.SetAnim(anim);

    }

    /// <summary>
    /// �ʒu�����N���[���������I�u�W�F�N�g�Ƌ��L����
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �v���C���[�I�u�W�F�N�g�̈ʒu���𑗐M
            stream.SendNext(PlayerPosition);
        }
        else
        {
            // �v���C���[�I�u�W�F�N�g�̈ʒu������M
            PlayerPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
