using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MesTest : MonoBehaviourPunCallbacks, IPunObservable
{
    string mes;
    [SerializeField] TextMesh mesText;
    [SerializeField] LinkDoor LinkDoor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            LinkDoor.CallRPC();
            stream.SendNext(mesText.text);
        }
        else
        {
            LinkDoor.CallRPC();
            mesText.text = (string)stream.ReceiveNext();
        }
    }
}
