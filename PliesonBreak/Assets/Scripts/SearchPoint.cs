using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SearchPoint : InteractObjectBase
{
    //�ێ����Ă���A�T����ɏo��������A�C�e��
    private InteractObjectBase DropItem;
    //�e�X�g�ŁA�o�������A�C�e��������ꍇ�Z�b�g����
    [SerializeField,Tooltip("�e�X�g�ŏo�������A�C�e��������ꍇ")] InteractObjectBase TestDropItem;
    //��������A�C�e���v���t�@�u�̃��X�g
    [SerializeField] List<GameObject> InteractObjectPrefabList = new List<GameObject>();
    //�W���T�����ԁB�b�P�ʂŋL�q
    [SerializeField,Range(1f,10f)] float DefaltSearchTime;
    //�R���[�`���̃X�g�b�v�t���O
    bool isCoroutineStop;
    //�T�������ǂ���
    bool isNowSearch;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        NowInteract = InteractObjs.Search;
        isNowSearch = false;
        isCoroutineStop = false;

        if(TestDropItem != null)
        {
            SetDropItem(TestDropItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���̒T���|�C���g��T���J�n����B�Ăяo���O�ɕK��GetSearchState�ŒT���m�F���s��
    /// �Ăяo���ۂ�StartCroutine�ŌĂяo���A���^�[���ŏI�����Ԃ��Ă���B
    /// ����:addsearchtime,1����ɒT�����Ԃ�ω�������B��@0.9�ő����A1.5�Œx���Ȃ�
    /// </summary>
    /// <param name="addsearchtime"></param>
    /// <returns></returns>
    public IEnumerator SearchStart(float addsearchtime)
    {
        isCoroutineStop = false;
        isNowSearch = true;
        float Timer = 0;
        Debug.Log("point:�T���J�n");
        float SearchTime = DefaltSearchTime * addsearchtime;

        while (true)
        {
            if (isCoroutineStop) yield break;

            if(Timer >= SearchTime)
            {
                break;
            }
            Timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("point:�T�����ԏI��");
        if(DropItem != null)
        {

            //var Obj = Instantiate(InteractObjectList[/*�I�u�W�F�N�g��ID*/],transform.position,transform.rotation);
            //Obj.GetComponent<InteractObject>().CopyProperty(DropItem);
            //�����艉�o
            Debug.Log("point:�A�C�e���h���b�v>"+DropItem.GetComponent<InteractObjectBase>().name);
        }
        else
        {
            //�͂��ꉉ�o
            Debug.Log("point:�A�C�e���Ȃ�");
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// �T�����ɉ��炩�̌`�Œ��f����ۂɌĂ΂��
    /// </summary>
    public void StopSearch()
    {
        isCoroutineStop = true;
        isNowSearch = false;
        Debug.Log("point:�T�����~");
    }

    /// <summary>
    /// ���ݒT�������𔻒肷��
    /// </summary>
    /// <returns></returns>
    public bool GetSearchState()
    {
        return isNowSearch;
    }

    /// <summary>
    /// MapManager�ŃA�C�e���|�C���g�������ɒ��ɓ����A�C�e����ݒ肷��
    /// ����:intearactobj,�h���b�v������e���N���X�Ƃ��ē����BNone���Ǝ��g������
    /// </summary>
    /// <param name="interactobj"></param>
    public void SetDropItem(InteractObjectBase interactobj)
    {
        DropItem = interactobj;

        //if(DropItem/*����ID*/ == InteractObjs.None)
        //{
        //    Destroy(gameobject);
        //}
    }
}
