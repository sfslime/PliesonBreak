using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 探索ポイントテスト用クラス
 */

public class TestItem : MonoBehaviour
{
    //探索ポイントを入れる
    public GameObject ItemPrefab;
    //探索ポイントの位置リスト
    public List<Vector3Int> SearchPointList = new List<Vector3Int>();
    //生成した探索ポイントの親
    public GameObject PointLists;

    //テストで一個だけ確保
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
    /// テストで探索ポイントを生成するボタン
    /// 実際にはMapManagerでスタート時に乱数で生成する
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
    /// テストで探索を開始する
    /// 実際にはプレイヤーで呼ぶ。
    /// </summary>
    public void TestUsePush()
    {
        if (!Point.GetComponent<SearchPoint>().GetSearchState())
        {
            StartCoroutine(TestSearch());
        }
        else
        {
            Debug.Log("test:探索中止");
            Point.GetComponent<SearchPoint>().StopSearch();
        }
    }

    IEnumerator TestSearch()
    {
        if (Point.GetComponent<SearchPoint>().GetSearchState())
        {
            Debug.Log("test:探索開始");
            yield return StartCoroutine(Point.GetComponent<SearchPoint>().SearchStart(1));
            Debug.Log("test:探索終了");
        }
    }
}
