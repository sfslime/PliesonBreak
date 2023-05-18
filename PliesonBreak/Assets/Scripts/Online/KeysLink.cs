using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
KeyItem�Ȃǂ��I�����C��������X�N���v�g
�z�肳���A�C�e���̃����N��
�E���̃A�C�e�����擾����A�ꎞ�I�ɔj�󂳂��
�E�擾���ꂽ�A�C�e���ƃv���C���[�������Ă���A�C�e�����؂�ւ��
�Ȃ̂ŁA���̃����N�X�N���v�g�ł͂��̓�_��Key�Ȃǂɗv������
Key�ł͂���ɉ�����
StateLink
���Ăяo��
 */


public class KeysLink : MonoBehaviourPunCallbacks
{
    //�����N������I�u�W�F�N�g
    [SerializeField] InteractObjectBase originObject;
    //�Q�[���}�l�[�W���[�̃C���X�^���X
    GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���}�l�[�W���[�C���X�^���X��ݒ�
        GameManager = GameManager.GameManagerInstance;
        if (GameManager == null) Debug.Log("GameManager not found");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �擾���ꂽ�Ƃ��ɌĂяo����A���̃A�C�e����j�󂷂�
    /// �\�Ȍ���A���̊֐��Ŕj�󂷂邱��
    /// �����FisOthes>���̃v���C���[�݂̂�j�󂷂邩�ǂ���
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
    /// �A�C�e����؂�ւ���ꍇ�̃I�[�o�[���[�h
    /// �V�����A�C�e�����������Ŏ��
    /// </summary>
    /// <param name="ObjID"></param>
    public void StateLink(InteractObjectBase.InteractObjs ObjID)
    {
        photonView.RPC(nameof(RPCChangeItem), RpcTarget.Others, ObjID);
    }

    /// <summary>
    /// �A�C�e�����擾�����Ƃ��ɑ��̃v���C���[�̉�ʂɂ���
    /// ���̃A�C�e����j�󂷂鏈��
    /// </summary>
    [PunRPC]
    public void RPCDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ���̃v���C���[�̉�ʂŃA�C�e�����؂�ւ��������
    /// ����ȊO�̃v���C���[�ł��ύX����
    /// </summary>
    /// <param name="ObjID"></param>
    [PunRPC]
    public void RPCChangeItem(InteractObjectBase.InteractObjs ObjID)
    {
        //�Q�[���}�l�[�W���[����X�v���C�g�擾
        var Sprite = GameManager.ReturnSprite(ObjID);

        //�摜�̐؂�ւ���v��
        //originObject
    }
}
