using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class TitleManager : MonoBehaviourPunCallbacks
{
    const string RoomHead = "Room ";
    const string RoomMidle =  "\n�Q���l��\n";
    const string RoomOK = "�Q���\";
    const string RoomNotOK = "�Q���s��";

    [SerializeField, Tooltip("1���[���̎Q���l��")] int MaxPlayer;
    [SerializeField, Tooltip("�Q���{�^�����X�g")] CanvasGroup ButtonRoot;
    //���r�[�Q���ς݂�
    private bool isInLobby;

    private List<RoomInfo> RoomInfos;

    /// <summary>
    /// �R�l�N�g�T�[�o�[����Ă΂�A
    /// ���r�[�ւ̎Q�������s����
    /// </summary>
    public void TryRobyJoin()
    {
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// ���r�[�ڑ����ɌĂ΂��
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        isInLobby = true;

        ButtonRoot.interactable = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        
        foreach(var room in roomList)
        {
            if (int.Parse(room.Name) < 0 || int.Parse(room.Name) > ButtonRoot.transform.childCount) continue;

            //���[���ԍ��ɉ����������X�V
            var button = ButtonRoot.transform.GetChild(int.Parse(room.Name));
            button.transform.GetChild(0).GetComponent<Text>().text = RoomHead+room.Name+RoomMidle + room.PlayerCount + "/" + room.MaxPlayers 
                                                                     + "\n"+ (room.IsOpen ? RoomOK : RoomNotOK);
            if (room.IsOpen)
            {
                button.GetComponent<Button>().interactable = true;
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>
    /// �{�^������������Ăׂ�悤�ɂ���
    /// </summary>
    public void JoinRoom(int RoomNm)
    {
        ButtonRoot.interactable = false;
        ConectServer.RoomProperties.RoomName = RoomNm.ToString();
        ConectServer.RoomProperties.MaxPlayer = MaxPlayer;
        SceneManager.LoadScene("WaitRoom");
    }

    // Start is called before the first frame update
    void Start()
    {
        isInLobby = false;
        //�Q�������������r�[�Q���O�͉����Ȃ�����
        ButtonRoot.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
