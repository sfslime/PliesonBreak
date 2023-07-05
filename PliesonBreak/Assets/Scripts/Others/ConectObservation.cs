using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

/*
Photon�Ɋւ���A�ڑ������Ď�����X�N���v�g
 */

public class ConectObservation : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("�N�����ޏo�����Ƃ��ɌĂ΂�鏈��")] UnityEvent LeftPlayer;
    [SerializeField, Tooltip("�T�[�o�[����ؒf���ꂽ�Ƃ��ɌĂ΂�鏈��")] UnityEvent DiscnnectServer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"�T�[�o�[�Ƃ̐ڑ����ؒf����܂���: {cause.ToString()}");
        DiscnnectServer.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName}���ޏo���܂���");
        LeftPlayer.Invoke();
    }
}
