using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPoint : InteractObjectBase
{
    //保持している、探索後に出現させるアイテム
    private InteractObjectBase DropItem;
    //テストで、出したいアイテムがある場合セットする
    [SerializeField,Tooltip("テストで出したいアイテムがある場合")] InteractObjectBase TestDropItem;
    //生成するアイテムプレファブのリスト
    [SerializeField] List<GameObject> InteractObjectPrefabList = new List<GameObject>();
    //標準探索時間。秒単位で記述
    [SerializeField,Range(1f,10f)] float DefaltSearchTime;
    //コルーチンのストップフラグ
    bool isCoroutineStop;
    //探索中かどうか
    bool isNowSearch;

    // Start is called before the first frame update
    void Start()
    {
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
    /// この探索ポイントを探索開始する。呼び出し前に必ずGetSearchStateで探索確認を行う
    /// 呼び出す際はStartCroutineで呼び出し、リターンで終了が返ってくる。
    /// 引数:addsearchtime,1を基準に探索時間を変化させる。例　0.9で早く、1.5で遅くなる
    /// </summary>
    /// <param name="addsearchtime"></param>
    /// <returns></returns>
    public IEnumerator SearchStart(float addsearchtime)
    {
        isCoroutineStop = false;
        isNowSearch = true;
        float Timer = 0;
        Debug.Log("point:探索開始");
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
        Debug.Log("point:探索時間終了");
        if(DropItem != null)
        {

            //var Obj = Instantiate(InteractObjectList[/*オブジェクトのID*/],transform.position,transform.rotation);
            //Obj.GetComponent<InteractObject>().CopyProperty(DropItem);
            //あたり演出
            Debug.Log("point:アイテムドロップ>"+DropItem.GetComponent<InteractObjectBase>().name);
        }
        else
        {
            //はずれ演出
            Debug.Log("point:アイテムなし");
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 探索中に何らかの形で中断する際に呼ばれる
    /// </summary>
    public void StopSearch()
    {
        isCoroutineStop = true;
        isNowSearch = false;
        Debug.Log("point:探索中止");
    }

    /// <summary>
    /// 現在探索中かを判定する
    /// </summary>
    /// <returns></returns>
    public bool GetSearchState()
    {
        return isNowSearch;
    }

    /// <summary>
    /// MapManagerでアイテムポイント生成時に中に入れるアイテムを設定する
    /// 引数:intearactobj,ドロップする内容をクラスとして入れる。Noneだと自身を消す
    /// </summary>
    /// <param name="interactobj"></param>
    public void SetDropItem(InteractObjectBase interactobj)
    {
        DropItem = interactobj;

        //if(DropItem/*所持ID*/ == InteractObjs.None)
        //{
        //    Destroy(gameobject);
        //}
    }
}
