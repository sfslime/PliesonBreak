using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ConstList;
using System;

public class MapManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    class PopSetting
    {
        [SerializeField,Tooltip("�e�G���A�̏o������|�C���g��")] public List<int> PointNmList;
        [SerializeField,Tooltip("�e�G���A�ɏo��������L�[�A�C�e���̃��X�g")]public List<InteractObjs> KeyIDList;
        [SerializeField, Tooltip("�e�G���A�ɏo������͂���|�C���g�̊m��")] public List<float> NullDropRate = new List<float>();
    }

    [SerializeField,Tooltip("�T�[�`�|�C���g�̃��X�g�̐e�܂Ƃ�")]GameObject SearchPointListRoot;

    [SerializeField, Tooltip("�e�G���A�̌��n�_�̐e�܂Ƃ�")] GameObject ListRoot;

    [SerializeField, Header("�A�C�e���̏o���ݒ�")] PopSetting PopSettings;

    GameManager GameManager;

    private List<List<int>> SelectedList = new List<List<int>>();

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameManager.GameManagerInstance;
        if (GameManager == null) Debug.Log("GameManager not found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �e�G���A�̒T���|�C���g�̐������J�n����
    /// </summary>
    public IEnumerator StartPop()
    {
        //�e�G���A�̌��n�̐e����G���A���ƂɎ��o��
        for(int AreaNm=0; AreaNm < ListRoot.transform.childCount; AreaNm++)
        {
            Debug.Log(AreaNm + "Area�����J�n");
            //�e�G���A���Ƃɓn���Č��n����ϊ�����
            var AreaList = ListRoot.transform.GetChild(AreaNm).gameObject;
            var PopList = SelectPoint(AreaList, AreaNm);

            //�e�G���A���Ƃ̃|�C���g�ǉ���擾
            var SearchPointList = SearchPointListRoot.transform.GetChild(AreaNm).gameObject;

            //�A�C�e���̒��I
            var ItemIDList = ItemSelect(PopList.Count, PopSettings.KeyIDList[AreaNm],AreaNm);

            //���肵���|�C���g���ɐ�������
            for (int cnt = 0;cnt < PopList.Count;cnt++)
            {
                Debug.Log(cnt + "��ڂ̃|�C���g�����J�n");
                //���肵���|�C���g�ƃG���A���Ƃ̌��n����|�W�V������n���A����
                var Point = PopSeachPoint(AreaList.transform.GetChild(PopList[cnt]).position);
                
                //���������A�C�e�������X�g�ɒǉ�
                Point.transform.parent = SearchPointList.transform;
                //�A�C�e�����蓖��
                Point.GetComponent<SearchPoint>().SetDropItem(ItemIDList[cnt]);
                Point.GetComponent<SearchPointLink>().SetDropItem(ItemIDList[cnt]);
            }

            SelectedList.Add(PopList);
            AreaList.SetActive(false);
        }

        yield break;
    }

    /// <summary>
    /// �����Ŏ󂯎������₩��|�C���g�����肵�A���X�g�ŕԂ�
    /// </summary>
    /// <param name="PointList"></param>
    /// <returns></returns>
    List<int> SelectPoint(GameObject PointList,int Area)
    {
        List<int> SelectedPoint = new List<int>();
        for(int Cnt = 0; Cnt < PopSettings.PointNmList[Area]; Cnt++)
        {
            //�O�̂��ߏ���ݒ�
            int errorcount = 0;
            while (true)
            {
                //�������[�h���
                errorcount++;
                if (errorcount > 1000)
                {
                    Debug.Log("Slect Error!!!!");
                    break;
                }

                //���n����I��
                int Nm = UnityEngine.Random.Range(0, PointList.transform.childCount);
                //�I���ς݂Ȃ������x
                if (SelectedPoint.Contains(Nm)) continue;

                //�I���ς݂ɒǉ�
                SelectedPoint.Add(Nm);
                break;
            }
        }
        return SelectedPoint;
    }

    /// <summary>
    /// �����Ŏ󂯎�����ꏊ�ɒT���|�C���g�𐶐�����
    /// </summary>
    /// <param name="pos"></param>
    GameObject PopSeachPoint(Vector2 pos)
    {
        var obj = PhotonNetwork.Instantiate(InteractObjs.Search.ToString(), pos, Quaternion.identity);
        Debug.Log("�|�C���g����");
        return obj;
    }

    /// <summary>
    /// ���Ȃ��Ƃ�������ɗ^����ꂽID���܂ނ悤�Ƀ����_���Ō��肵���A�C�e�����A
    /// �����ŗ^����ꂽ���X�g���ŕԂ�
    /// </summary>
    /// <returns></returns>
    List<InteractObjs> ItemSelect(int Index, InteractObjs KeyID,int Area)
    {
        List<InteractObjs> IDList = new List<InteractObjs>();
        bool isKeyPop = false;

        int errorcnt = 0;
        while (!isKeyPop)
        {
            errorcnt++;
            if (errorcnt > 1000)
            {
                Debug.Log("ItemSelect error!!!");
                break;
            }

            isKeyPop = false;
            for(int Cnt=0;Cnt<Index; Cnt++)
            {
                InteractObjs item;
                //�͂���m���ɂ���Ă͂��ꐶ��
                if (RandomPar(PopSettings.NullDropRate[Area]))
                {
                    item = InteractObjs.NullDrop;
                }
                else
                {
                    //�A�C�e��ID���Ń����_���ɐ������A���̕����񂩂�I�u�W�F�N�gID�ɕϊ�
                    item = (InteractObjs)Enum.Parse(typeof(InteractObjs), ((ItemID)UnityEngine.Random.Range((int)ItemID.None + 1, (int)ItemID.Count - 1)).ToString());
                }
                if (KeyID == item) isKeyPop = true;
                IDList.Add(item);
                Debug.Log("select item"+item);
            }
        }

        return IDList;
    }

    bool RandomPar(float Probability)
    {
        float nm = UnityEngine.Random.Range(0f, 1f);
        return Probability >= nm ? true : false;
    }

    //public void PopSearchPoint()
    //{
    //    for(int i = PointListArea1.transform.childCount-1; i >= 0; i--)
    //    {
    //        var obj = PhotonNetwork.Instantiate(InteractObjs.Search.ToString(), PointListArea1.transform.GetChild(i).transform.position, Quaternion.identity);
    //        obj.transform.parent = SearchPointList.transform;
    //        Destroy(PointListArea1.transform.GetChild(i).gameObject);
    //    } 
    //}
}
