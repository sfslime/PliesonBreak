using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 �T���|�C���g�e�X�g�p�N���X
 */

public class TestItem : MonoBehaviour
{
    //�T���|�C���g������
    public GameObject ItemPrefab;
    //�T���|�C���g�̈ʒu���X�g
    public List<Vector3Int> SearchPointList = new List<Vector3Int>();
    //���������T���|�C���g�̐e
    public GameObject PointLists;

    //�e�X�g�ň�����m��
    public GameObject Point;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �e�X�g�ŒT���|�C���g�𐶐�����{�^��
    /// ���ۂɂ�MapManager�ŃX�^�[�g���ɗ����Ő�������
    /// </summary>
    public void TestItemPush()
    {
        foreach (var pos in SearchPointList)
        {
            var obj = Instantiate(ItemPrefab, pos, Quaternion.identity);
            obj.transform.parent = PointLists.transform;
            Point = obj;
            obj.GetComponent<SearchPoint>().SetDropItem(obj.GetComponent<SearchPoint>().NowInteract);
        }
    }

    /// <summary>
    /// �e�X�g�ŒT�����J�n����
    /// ���ۂɂ̓v���C���[�ŌĂԁB
    /// </summary>
    public void TestUsePush()
    {
        if (!Point.GetComponent<SearchPoint>().GetSearchState())
        {
            StartCoroutine(TestSearch());
        }
        else
        {
            Debug.Log("test:�T�����~");
            Point.GetComponent<SearchPoint>().StopSearch();
        }
    }

    IEnumerator TestSearch()
    {
        if (Point.GetComponent<SearchPoint>().GetSearchState())
        {
            Debug.Log("test:�T���J�n");
            yield return StartCoroutine(Point.GetComponent<SearchPoint>().SearchStart(1));
            Debug.Log("test:�T���I��");
        }
    }
}
