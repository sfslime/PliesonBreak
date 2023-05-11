using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
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
        InputAction.Enable();  // InputSystemの操作の受付ON.
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
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