using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LinkDoor : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallRPC()
    {
        photonView.RPC(nameof(RPCtest), RpcTarget.All, "���b�Z�[�W�m�F");
    }

    [PunRPC]
    void RPCtest(string debugmes)
    {
        Debug.Log(debugmes);
    }
}
