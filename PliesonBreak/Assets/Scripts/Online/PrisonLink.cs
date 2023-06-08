using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PrisonLink : MonoBehaviourPunCallbacks
{
    //�����N������I�u�W�F�N�g
    [SerializeField] Prison originObject;
    //�Q�[���}�l�[�W���[�̃C���X�^���X
    GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        originObject = GetComponent<Prison>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �S���̏�Ԃ����L����
    /// �����ŊJ�������ǂ��������
    /// </summary>
    public void StateLink(bool isopende)
    {
        photonView.RPC(nameof(RPCStateLink), RpcTarget.Others, isopende);
    }

    [PunRPC]
    void RPCStateLink(bool isopened)
    {
        originObject.PrisonOpen(isopened);
    }
}
