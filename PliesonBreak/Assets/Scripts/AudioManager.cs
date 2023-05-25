using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class AudioManager : MonoBehaviour
{
    [SerializeField,Header("SE設定"), Tooltip("SEリスト")] List<AudioClip> SEList = new List<AudioClip>();
    [SerializeField, Tooltip("音の聞こえる範囲")] float Distance;
    [SerializeField, Tooltip("最小音量")] float MinVolume;
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

    public void SE(SEid id,Vector2 pos)
    {
        float distance = Vector3.Distance(pos, Player.transform.position);
        float volume = 1f - Mathf.Clamp01((distance - AudioSource.minDistance) / (AudioSource.maxDistance - AudioSource.minDistance));
        volume *= (1f - MinVolume) + MinVolume;  // 最小音量を適用

        AudioSource.volume = volume;
        AudioSource.PlayOneShot(SEList[(int)id]);
    }
}
