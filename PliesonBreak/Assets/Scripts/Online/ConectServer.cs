using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

/*
�ڑ����s���}�l�[�W���[
�ڑ���̂��Ƃ͊֗^�����A�ڑ��݂̂��s��
 */

public class ConectServer : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class RoomPropertie
    {
        [Tooltip("�Q�����郋�[����")] public string RoomName;
        [Tooltip("�Q���\�l��")] public int MaxPlayer;
    }

    [SerializeField, Tooltip("�f�o�b�O�Ȃǂł̃I�t���C�����[�h")] bool isOffline;

    [Tooltip("�T�[�o�[�ڑ����ɌĂ΂��C�x���g")] public UnityEvent OnConect;
    [Tooltip("���[���Q�����ɌĂ΂��C�x���g")] public UnityEvent OnJoinde;
    [SerializeField, Tooltip("���[���Ɋւ�����")] public static RoomPropertie RoomProperties = new RoomPropertie();
    // Start is called before the first frame update
    void Start()
    {
        if (isOffline)
        {
            Debug.Log(PhotonNetwork.IsConnected);
            PhotonNetwork.OfflineMode = true;
        }
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        else PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        OnConect.Invoke();
        Debug.Log("OnConect");
        //�e�X�g
        //RoomProperties.MaxPlayer = 2;
        //RoomProperties.RoomName = "1";
        //SceneManager.LoadScene("WaitRoom");
        //�e�X�g
        //TryRoomJoin();
    }

    public void TryRoomJoin()
    {
        if(isOffline) { PhotonNetwork.JoinOrCreateRoom("5", new RoomOptions(), TypedLobby.Default); } else
        {
            //���[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
            PhotonNetwork.JoinOrCreateRoom(RoomProperties.RoomName, new RoomOptions(), TypedLobby.Default);
        }
        
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
}
