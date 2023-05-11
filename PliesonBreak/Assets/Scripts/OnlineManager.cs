using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class OnlineManager : MonoBehaviourPunCallbacks
{
    GameObject Player;
    [SerializeField] float Speed;
    [SerializeField] string Name;
    bool isJoin;
    [SerializeField] InputAction InputAction;
    //[SerializeField] Inpu

    private void Start()
    {
        PhotonNetwork.NickName = Name;
        isJoin = false;
        InputAction.Enable();  // InputSystem�̑���̎�tON.
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        Player =  PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        isJoin = true;
    }

    void Update()
    {
        if (!isJoin) return;
        var pos = Player.transform.position;

        var MoveVector = InputAction.ReadValue<Vector2>();

        pos.x += MoveVector.x * Speed * Time.deltaTime;
        pos.y += MoveVector.y * Speed * Time.deltaTime;

        Player.transform.position = pos;
    }
}