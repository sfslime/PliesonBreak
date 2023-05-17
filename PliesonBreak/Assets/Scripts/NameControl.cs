using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NameControl : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMesh>().text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
