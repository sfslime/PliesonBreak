using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
アイテムを出現させる探索ポイント
ゲームマネージャーとの併用必須
探索は SearchStart をスタートコルーチンで呼ぶと探索終了時にリターンする
探索前に必ず GetSearchState で他プレイヤーの探索状態を確認し、
false の場合にのみ探索すること
何らかの理由で探索中止する場合、StopSearch で中止する
作成者：飛田
 */

public class SearchPoint : InteractObjectBase
{
    //保持している、探索後に出現させるアイテム
    private InteractObjectBase DropItem;
    private InteractObjs DropItemID;
    //テストで、出したいアイテムがある場合セットする
    [SerializeField,Tooltip("テストで出したいアイテムの設定済みスクリプト(必要な場合のみSet)")] InteractObjectBase TestDropItem;
    //標準探索時間。秒単位で記述
    [SerializeField,Range(1f,10f),Tooltip("デフォルトの探索時間(Set必須)")] float DefaltSearchTime;
    //コルーチンのストップフラグ
    bool isCoroutineStop;
    //探索中かどうか
    bool isNowSearch;
    //他ユーザーとのリンクスクリプト
    private SearchPointLink SearchPointLink;
    //空のドロップの場合、他のプレイヤーでも破壊するか
    [SerializeField, Tooltip("空ドロップの際、他プレイヤーでの破壊設定")] bool isDestroy;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        NowInteract = InteractObjs.Search;
        isNowSearch = false;
        isCoroutineStop = false;

        SearchPointLink = GetComponent<SearchPointLink>();
        if (SearchPointLink == null) Debug.Log("point:SearchPointLink not found");

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
    /// 引数:addsearchtime,1を基準に探索時間を変化させる。例　0.9で10%早く、1.5で50%遅くなる
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

        //アイテムの出現
        InstantiateItem();

        //他プレイヤーに探索終了を送信
        SearchPointLink.EndInteract((DropItem != null && DropItemID != InteractObjs.NullDrop) || isDestroy);

        Debug.Log("point:探索時間終了");
        
        Destroy(gameObject);
    }

    /// <summary>
    /// テスト用サーチ
    /// </summary>
    public void TestSearch()
    {
        StartCoroutine(SearchStart(1));
    }

    /// <summary>
    /// アイテムを出現させる
    /// </summary>
    public void InstantiateItem()
    {
        if (DropItem != null && DropItemID != InteractObjs.NullDrop)
        {
            //ゲームマネージャーからプレファブを取得し、情報を新しいオブジェクトに反映する
            //オフライン時はこの生成
            //var Obj = Instantiate(GameManager.GetObjectPrefab(DropItemID), transform.position, transform.rotation);
            var Obj = PhotonNetwork.Instantiate(DropItemID.ToString(), transform.position, transform.rotation);
            Obj.GetComponent<InteractObjectBase>().CopyProperty(DropItem);
            //あたり演出
            Debug.Log("point:アイテムドロップ>" + DropItem.GetComponent<InteractObjectBase>().name);
        }
        else
        {
            //はずれ演出
            Debug.Log("point:アイテムなし");
        }
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
        DropItemID = interactobj.NowInteract;

        //if(DropItem/*所持ID*/ == InteractObjs.None)
        //{
        //    Destroy(gameobject);
        //}
    }

    /// <summary>
    /// 他ユーザーの探索終了などの理由で中のアイテムを変更、または消去する
    /// </summary>
    /// <param name="interactObject"></param>
    public void ChangeDropItem(InteractObjectBase interactObject)
    {
        DropItem = interactObject;
    }
}
