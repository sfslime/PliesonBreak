using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class BGMManager : MonoBehaviour
{
    AudioSource AudioSource;  //再生のためのオーディオソース
    [SerializeField,Tooltip("再生するBGMのリスト")] List<AudioClip> BGMList = new List<AudioClip>();
    public static BGMManager Instance;  //重複回避のインスタンス
    private void Awake()
    {
        //シングルトン化
        if (Instance == null)
        {
            DontDestroyOnLoad(this); 
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        AudioSource = GetComponent<AudioSource>();
    }

    //BGMを再生する
    public void SetBGM(BGMid id)
    {
        AudioSource.clip = BGMList[(int)id];
        AudioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
