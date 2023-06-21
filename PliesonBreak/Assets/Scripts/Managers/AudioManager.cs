using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ConstList;

public class AudioManager : MonoBehaviourPun
{
    [SerializeField,Header("SE�ݒ�"), Tooltip("SE���X�g")] List<AudioClip> SEList = new List<AudioClip>();
    [SerializeField, Tooltip("���̕�������͈�")] float Distance;
    [SerializeField, Tooltip("�ŏ�����")] float MinVolume;
    AudioSource AudioSource;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        var gamemanager = GameManager.GameManagerInstance;
        if (gamemanager != null) Player = gamemanager.GetPlayer();

        AudioSource.maxDistance = Distance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE(SEid id, Vector2 pos)
    {
        photonView.RPC(nameof(SE), RpcTarget.All, (int)id, pos);
    }

    [PunRPC]
    public void SE(int id,Vector2 pos)
    {
        float distance = Vector3.Distance(pos, Player.transform.position);
        float volume = 1f - Mathf.Clamp01((distance - AudioSource.minDistance) / (AudioSource.maxDistance - AudioSource.minDistance));
        volume *= (1f - MinVolume) + MinVolume;  // �ŏ����ʂ�K�p

        AudioSource.volume = volume;
        AudioSource.PlayOneShot(SEList[id]);
    }
}
