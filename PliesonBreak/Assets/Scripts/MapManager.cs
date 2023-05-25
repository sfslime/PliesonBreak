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

    [SerializeField,Tooltip("サーチポイントのリスト")]GameObject SearchPointList;

    [SerializeField, Tooltip("各エリアの候補地点の親まとめ")] GameObject ListRoot;
    [SerializeField, Tooltip("(エリア１)出現するポイント候補の親")] GameObject PointListArea1;

    [SerializeField, Header("アイテムの出現設定")] PopSetting PopSettings;

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
    /// 各エリアの探索ポイントの生成を開始する
    /// </summary>
    public IEnumerator StartPop()
    {
        //各エリアの候補地の親からエリアごとに取り出す
        for(int i=0;i< ListRoot.transform.childCount; i++)
        {
            //各エリアごとに渡して候補地から変換する
            var AreaList = ListRoot.transform.GetChild(i).gameObject;
            var PopList = SelectPoint(AreaList);

            //決定したポイント毎に生成処理
            foreach(var point in PopList)
            {
                //決定したポイントとエリアごとの候補地からポジションを渡し、生成
                var SeachPoint = PopSeachPoint(AreaList.transform.GetChild(point).position);
                //生成したアイテムをリストに変換

                //アイテムの抽選
                //アイテムの割り当て
            }
        }

        yield break;
    }

    /// <summary>
    /// 引数で受け取った候補からポイントを決定し、リストで返す
    /// </summary>
    /// <param name="PointList"></param>
    /// <returns></returns>
    List<int> SelectPoint(GameObject PointList)
    {
        List<int> SelectedPoint = new List<int>();
        for(int i = 0; i < PopSettings.ItemNm; i++)
        {
            //念のため上限設定
            int errorcount = 0;
            while (true)
            {
                //無限ロード回避
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
    /// 引数で受け取った場所に探索ポイントを生成する
    /// </summary>
    /// <param name="pos"></param>
    GameObject PopSeachPoint(Vector2 pos)
    {
        var obj = PhotonNetwork.Instantiate(InteractObjs.Search.ToString(), pos, Quaternion.identity);
        obj.transform.parent = SearchPointList.transform;
        return obj;
    }

    /// <summary>
    /// 少なくとも一つ引数に与えられたIDを含むようにランダムで決定したアイテムを、
    /// 引数で与えられたリスト数で返す
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
