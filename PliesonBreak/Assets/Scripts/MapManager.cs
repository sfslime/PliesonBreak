using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ConstList;

public class MapManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    class PopSetting
    {
        public int ItemNm;
    }

    [SerializeField,Tooltip("�T�[�`�|�C���g�̃��X�g")]GameObject SearchPointList;

    [SerializeField, Tooltip("�e�G���A�̌��n�_�̐e�܂Ƃ�")] GameObject ListRoot;
    [SerializeField, Tooltip("(�G���A�P)�o������|�C���g���̐e")] GameObject PointListArea1;

    [SerializeField, Header("�A�C�e���̏o���ݒ�")] PopSetting PopSettings;

    GameManager GameManager;

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
        for(int i=0;i< ListRoot.transform.childCount; i++)
        {
            //�e�G���A���Ƃɓn���Č��n����ϊ�����
            var AreaList = ListRoot.transform.GetChild(i).gameObject;
            var PopList = SelectPoint(AreaList);

            //���肵���|�C���g���ɐ�������
            foreach(var point in PopList)
            {
                //���肵���|�C���g�ƃG���A���Ƃ̌��n����|�W�V������n���A����
                var SeachPoint = PopSeachPoint(AreaList.transform.GetChild(point).position);
                //���������A�C�e�������X�g�ɕϊ�

                //�A�C�e���̒��I
                //�A�C�e���̊��蓖��
            }
        }

        yield break;
    }

    /// <summary>
    /// �����Ŏ󂯎������₩��|�C���g�����肵�A���X�g�ŕԂ�
    /// </summary>
    /// <param name="PointList"></param>
    /// <returns></returns>
    List<int> SelectPoint(GameObject PointList)
    {
        List<int> SelectedPoint = new List<int>();
        for(int i = 0; i < PopSettings.ItemNm; i++)
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
                int Nm = Random.Range(0, PointList.transform.childCount);
                if (SelectedPoint.Contains(Nm)) continue;

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
        obj.transform.parent = SearchPointList.transform;
        return obj;
    }

    /// <summary>
    /// ���Ȃ��Ƃ�������ɗ^����ꂽID���܂ނ悤�Ƀ����_���Ō��肵���A�C�e�����A
    /// �����ŗ^����ꂽ���X�g���ŕԂ�
    /// </summary>
    /// <returns></returns>
    //List<InteractObjs> ItemSelect(int Index,InteractObjs KeyID)
    //{
    //    List<InteractObjs> IDList = new List<InteractObjs>();
    //    bool isKeyPop = false;
    //}

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
