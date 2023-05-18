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
            stream.SendNext(mesText.text);
        }
        else
        {
            mesText.text = (string)stream.ReceiveNext();
        }
    }
}
