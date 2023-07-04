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
        [SerializeField,Tooltip("各エリアの出現するポイント数")] public List<int> PointNmList;
        [SerializeField,Tooltip("各エリアに出現させるキーアイテムのリスト")]public List<InteractObjs> KeyIDList;
        [SerializeField,Tooltip("各エリアに出現するはずれポイントの確率")] public List<float> NullDropRate = new List<float>();
        [SerializeField, Tooltip("各エリアに出現するアイテムの確立(ItemID順で、パーセント)"), Range(0f, 1.0f)] public List<List<float>> ItemRate = new List<List<float>>();
    }

    [System.Serializable]
    class AreaPopSetting
    {
        [SerializeField, Tooltip("このエリアに出現するポイント数"),Range(1,20)] public int PointNm;
        [SerializeField, Tooltip("このエリアに必ず出現するキーアイテムのリスト")] public List<InteractObjs> KeyList;
        [SerializeField, Tooltip("このエリアのはずれアイテムの確立(値は%)"), Range(0f, 1f)] public float NullDropRate;
        [SerializeField, Tooltip("このエリアの各アイテムの出現確立(ItemID基準)"),Range(0f,1f)] public List<float> ItemRateList = new List<float>();
    }

    [SerializeField,Tooltip("サーチポイントのリストの親まとめ")]GameObject SearchPointListRoot;

    [SerializeField, Tooltip("各エリアの候補地点の親まとめ")] GameObject ListRoot;

    [SerializeField, Header("アイテムの出現設定。各エリアごとに管理")] List<AreaPopSetting> AreaPopSettings = new List<AreaPopSetting>();

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
    /// 各エリアの探索ポイントの生成を開始する
    /// </summary>
    public IEnumerator StartPop()
    {
        //各エリアの候補地の親からエリアごとに取り出す
        for(int AreaNm=0; AreaNm < ListRoot.transform.childCount; AreaNm++)
        {
            Debug.Log(AreaNm + "Area生成開始");
            //各エリアごとに渡して候補地から変換する
            var AreaList = ListRoot.transform.GetChild(AreaNm).gameObject;
            var PopList = SelectPoint(AreaList, AreaNm);

            //各エリアごとのポイント追加先取得
            var SearchPointList = SearchPointListRoot.transform.GetChild(AreaNm).gameObject;

            //アイテムの抽選
            var ItemIDList = ItemSelect(PopList.Count, AreaPopSettings[AreaNm].KeyList,AreaNm);

            //決定したポイント毎に生成処理
            for (int cnt = 0;cnt < PopList.Count;cnt++)
            {
                Debug.Log(cnt + "回目のポイント生成開始");
                //決定したポイントとエリアごとの候補地からポジションを渡し、生成
                var Point = PopSeachPoint(AreaList.transform.GetChild(PopList[cnt]).position);
                
                //生成したアイテムをリストに追加
                Point.transform.parent = SearchPointList.transform;
                //アイテム割り当て
                Point.GetComponent<SearchPoint>().SetDropItem(ItemIDList[cnt]);
            }

            SelectedList.Add(PopList);
            AreaList.SetActive(false);
        }

        yield break;
    }

    /// <summary>
    /// 引数で受け取った候補からポイントを決定し、リストで返す
    /// </summary>
    /// <param name="PointList"></param>
    /// <returns></returns>
    List<int> SelectPoint(GameObject PointList,int Area)
    {
        List<int> SelectedPoint = new List<int>();
        for(int Cnt = 0; Cnt < AreaPopSettings[Area].PointNm; Cnt++)
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

                //候補地から選択
                int Nm = UnityEngine.Random.Range(0, PointList.transform.childCount);
                //選択済みならもう一度
                if (SelectedPoint.Contains(Nm)) continue;

                //選択済みに追加
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
        Debug.Log("ポイント生成");
        return obj;
    }

    /// <summary>
    /// 少なくとも一つ引数に与えられたIDを含むようにランダムで決定したアイテムを、
    /// 引数で与えられたリスト数で返す
    /// </summary>
    /// <returns></returns>
    List<InteractObjs> ItemSelect(int Index, List<InteractObjs> KeyIDList,int Area)
    {
        //追加していくアイテムのリスト
        List<InteractObjs> IDList = new List<InteractObjs>();
        bool isKeyPop = false;

        int errorcnt = 0;
        //キーアイテムが出現していない間ループ
        while (!isKeyPop)
        {
            //無限ループ回避
            errorcnt++;
            if (errorcnt > 1000)
            {
                Debug.Log("ItemSelect error!!!");
                break;
            }

            //値を初期化
            IDList.Clear();

            //アイテムをインデックスの数まで抽選
            for (int Cnt=0;Cnt<Index; Cnt++)
            {
                InteractObjs item;
                //はずれ確率によってはずれ生成
                if (RandomPar(AreaPopSettings[Area].NullDropRate))
                {
                    item = InteractObjs.NullDrop;
                }
                else
                {
                    //アイテムID内でランダムに生成し、その文字列からオブジェクトIDに変換
                    //item = (InteractObjs)Enum.Parse(typeof(InteractObjs), ((ItemID)UnityEngine.Random.Range((int)ItemID.None + 1, (int)ItemID.Count)).ToString());
                    item = (InteractObjs)Enum.Parse(typeof(InteractObjs), RandomItem(AreaPopSettings[Area].ItemRateList).ToString());
                }
                //アイテムリストに追加
                IDList.Add(item);
                Debug.Log("select item"+item);
            }
            //キーアイテムが出現しているかの確認
            isKeyPop = true;
            foreach (var key in KeyIDList)
            {
                //含まれていない時点でもう一度抽選
                if (!IDList.Contains(key))
                {
                    isKeyPop = false;
                    break;
                }
            }
            
        }

        return IDList;
    }

    /// <summary>
    /// 与えられたアイテム出現確立から一つを返す
    /// </summary>
    /// <param name="ItemRate"></param>
    /// <returns></returns>
    ItemID RandomItem(List<float> ItemRate)
    {
        ItemID item = ItemID.None;
        int cnt = 0;
        while(item == ItemID.None)
        {
            //無限ループ回避
            cnt++;
            if(cnt > 10000)
            {
                Debug.Log("RandomItemError");
                break;
            }

            for(int i = 0; i < ItemRate.Count; i++)
            {
                if (RandomPar(ItemRate[i]))
                {
                    item = (ItemID)i;
                }
            }
        }
        return item;
    }

    /// <summary>
    /// 乱数をパーセント(0~0.1)でできる関数
    /// 戻り値でその確立になったかを返す
    /// </summary>
    /// <param name="Probability"></param>
    /// <returns></returns>
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
