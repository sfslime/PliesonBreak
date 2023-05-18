using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
�T���|�C���g���I�����C��������X�N���v�g
�T���|�C���g�ŃI�����C��������K�v������̂�
�E�T�����I���A�A�C�e�����o������
�E�I������T���𒆎~���A�j�󂷂�
�E��̃A�C�e���̏ꍇ�A�ݒ�ɂ���ċ�̂܂܎c���i�Z�p�I�Ȗ��ŃA�C�e��������ꍇ�͂��̃|�C���g��j�󂷂�K�v������j
�ł���A�T���|�C���g�ł͒T���I������
EndIntearact�֐����Ă�
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
    /// RPC�Ăяo���p�i�f�o�b�O�j
    /// </summary>
    /// <param name="RPCname"></param>
    public void CallRPC(string RPCname)
    {
        photonView.RPC(nameof(RPCname), RpcTarget.All);
    }

    /// <summary>
    /// �T�����I�������Ƃ��ɑ��̃|�C���g�ɒʒm����
    /// ��̃A�C�e����T�������Ƃ��A�u��̂܂܎c���v���u���ł����邩�v�����
    /// ��łȂ��Ƃ��͏��true�ɂ���
    /// �����FisDestroy>���̃I�u�W�F�N�g���܂߁A���̃|�C���g��j�󂷂邩�ǂ���
    /// </summary>
    public void EndInteract(bool isDestroy)
    {
        if (isDestroy)
        {
            //���̃v���C���[�ŃA�C�e�����o��������i�T�����ɃA�C�e��������ꍇ��ɂ�����Ăԁj
            photonView.RPC(nameof(RPCDropItem), RpcTarget.Others);
            //���ׂẴv���C���[�œ����|�C���g��j�󂷂�
            photonView.RPC(nameof(RPCDestroy), RpcTarget.Others);
        }
        //�j�󂵂Ȃ������ł͑��v���C���[�ɑ��M������̂͂Ȃ��A�T�������v���C���[�ł̂ݔj�󂳂��
        //��false�̏ꍇ�A���g��NullDrop�ł��邱�Ƃ��m�肵�Ă���
    }

    /// <summary>
    /// Destroy�P�̂ł͑��̃I�u�W�F�N�g���j�󂳂�Ȃ��̂�RPC������
    /// </summary>
    [PunRPC]
    void RPCDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ���̃��[�U�[�̃|�C���g�ł��o��������
    /// </summary>
    [PunRPC]
    void RPCDropItem()
    {
        //�T�����f
        OriginSearchPoint.StopSearch();
        //�A�C�e���o��
        OriginSearchPoint.InstantiateItem();
    }

}
