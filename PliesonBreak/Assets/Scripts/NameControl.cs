using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NameControl : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;
        GetComponent<TextMesh>().text = PhotonNetwork.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
