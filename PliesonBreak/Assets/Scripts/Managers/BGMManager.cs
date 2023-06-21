using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class BGMManager : MonoBehaviour
{
    AudioSource AudioSource;  //�Đ��̂��߂̃I�[�f�B�I�\�[�X
    [SerializeField,Tooltip("�Đ�����BGM�̃��X�g")] List<AudioClip> BGMList = new List<AudioClip>();
    public static BGMManager Instance;  //�d������̃C���X�^���X
    private void Awake()
    {
        //�V���O���g����
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

    //BGM���Đ�����
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
