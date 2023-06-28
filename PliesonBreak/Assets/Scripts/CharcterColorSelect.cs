using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ConstList;

public class CharcterColorSelect : MonoBehaviour
{
    [SerializeField, Tooltip("PlayerÇ…ê›íËÇ∑ÇÈÉJÉâÅ[")] PlayerColors color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonNetwork.LocalPlayer.SetPlayerColorStatus((int)color);
    }
}
