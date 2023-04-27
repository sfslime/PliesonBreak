using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:PliesonBreak/Assets/Scripts/cSearchPoint.cs
public class cSearchPoint : InteractObject
{
    //保持している、探索後に出現させるアイテム
    private InteractObject DropItem;
    //生成するアイテムプレファブのリスト
    [SerializeField] List<GameObject> InteractObjectList = new List<GameObject>();
    //標準探索時間。秒単位で記述
    [SerializeField,Range(1f,10f)] float DefaltSearchTime;
    //アイテムを保持していない状態なら消えるかどうか
=======
public class SearchPoint : InteractObjectBase
{
    private InteractObjectBase DropItem;
    [SerializeField] float DefaltSearchTime;
>>>>>>> c5fdca62245b2074e985cbe2dbdc8545a78b2268:PliesonBreak/Assets/Scripts/SearchPoint.cs
    [SerializeField] bool isDestroy;

    // Start is called before the first frame update
    void Start()
    {
        NowInteract = InteractObjs.Search;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="addsearchtime"></param>
    /// <returns></returns>
    public IEnumerator SearchStart(float addsearchtime)
    { 
        float SearchTime = DefaltSearchTime * addsearchtime;

        yield return new WaitForSeconds(SearchTime);

        if(DropItem != null)
        {

            //var Obj = Instantiate(InteractObjectList[/*オブジェクトのID*/],transform.position,transform.rotation);
            //Obj.GetComponent<InteractObject>().CopyProperty(DropItem);
            //あたり演出
        }
        else
        {
            //はずれ演出
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public void StopSearch()
    {
        StopCoroutine("SearchStart");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    public void SetDropItem(InteractObject interactobj)
    {
        
    }
}
