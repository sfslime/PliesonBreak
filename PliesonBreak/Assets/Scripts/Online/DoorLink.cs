using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorLink : MonoBehaviourPunCallbacks
{
    //�����N������I�u�W�F�N�g
    [SerializeField] DoorBase originObject;
    //�Q�[���}�l�[�W���[�̃C���X�^���X
    GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        originObject = GetComponent<DoorBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �h�A���J���ꂽ�Ƃ��ɑ��̃v���C���[�̃h�A���J����
    /// �����ŊJ�������ǂ������m�F����
    /// </summary>
    /// <param name="isOpen"></param>
    /// <param name="isColition"></param>
    public void DoorStateLink(bool isOpen)
    {
        photonView.RPC(nameof(RPCDoorLink), RpcTarget.Others, isOpen);
    }

    [PunRPC]
    void RPCDoorLink(bool isOpen)
    {
        originObject.DoorOpen(isOpen);
    }
}
