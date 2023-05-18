using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class ConectServer : MonoBehaviourPunCallbacks
{
    [Tooltip("�T�[�o�[�ڑ����ɌĂ΂��C�x���g")] public UnityEvent OnConect;
    [Tooltip("���[���Q�����ɌĂ΂��C�x���g")] public UnityEvent OnJoinde;
    [SerializeField,Header("�e�X�g")] string objname;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        OnConect.Invoke();
        Debug.Log("OnConect");
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        OnJoinde.Invoke();
        Debug.Log("Onjoin");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�e�X�g
    public void PopPlayer()
    {
        var Link = PhotonNetwork.Instantiate(objname, player.transform.position, Quaternion.identity);
        Link.GetComponent<PlayerLink>().SetOrigin(player);
    }
}
