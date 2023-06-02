using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ConstList;

/*
�}�b�`���O���ɑҋ@���郍�r�[���̊Ǘ��}�l�[�W���[
���݂̃��r�[��Ԃ̕\���ƁA���r�[���̃R���g���[�����s��
�}�b�`���O��̓Q�[���J�n�ɍ��킹�ăV�[���̈ړ����s��
 */

public class WaitRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("����\�����郁�b�Z�[�W")] Text MessageText;
    [SerializeField, Tooltip("�J�n�{�^��")] Button SceanMoveButton;

    //�}�X�^�[���ǂ���
    private bool isMaster;
    //���[�����Őڑ��ł��Ă��邩
    private bool isInRoom;

    /// <summary>
    /// ���݂̃����o�[�ŃQ�[���V�[���Ɉړ�����
    /// </summary>
    public void MoveGameScean()
    {
        if (isMaster)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(SceanNames.GAME.ToString());
        }
    }

    /// <summary>
    /// ���̃V�[���ړ���ɉ��߂ă��[���ڑ������s����
    /// </summary>
    void TryRoomJoin()
    {
        //�^�C�g���Ŋm���������Őڑ�
        PhotonNetwork.JoinOrCreateRoom(ConectServer.RoomProperties.RoomName, new RoomOptions(), TypedLobby.Default);
    }

    /// <summary>
    /// �ڑ����ɌĂ΂��֐�
    /// �}�X�^�[�̏ꍇ�A�e��ݒ���s��
    /// </summary>
    public override void OnJoinedRoom()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        isInRoom = true;
        if (isMaster)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = (byte)ConectServer.RoomProperties.MaxPlayer;
        }
        Debug.Log("OnJoin");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        MessageText.text = message + " �ɂ���Đڑ��o���܂���";
    }

    /// <summary>
    /// ���[�����̏����X�V���A�\������
    /// </summary>
    void RoomStatusUpDate()
    {
        if (PhotonNetwork.CountOfPlayersInRooms <= 1)
        {
            SceanMoveButton.interactable = false;
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        if (isMaster) SceanMoveButton.interactable = true;

        SceanMoveButton.transform.GetChild(0).gameObject.GetComponent<Text>().text
            = "�J�n(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")";

        //�����o���X�g��\��

    }

    // Start is called before the first frame update
    void Start()
    {
        isMaster = false;
        isInRoom = false;
        TryRoomJoin();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRoom)
        {
            RoomStatusUpDate();
        }
    }
}