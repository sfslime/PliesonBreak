using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:PliesonBreak/Assets/Scripts/cSearchPoint.cs
public class cSearchPoint : InteractObject
{
    //�ێ����Ă���A�T����ɏo��������A�C�e��
    private InteractObject DropItem;
    //��������A�C�e���v���t�@�u�̃��X�g
    [SerializeField] List<GameObject> InteractObjectList = new List<GameObject>();
    //�W���T�����ԁB�b�P�ʂŋL�q
    [SerializeField,Range(1f,10f)] float DefaltSearchTime;
    //�A�C�e����ێ����Ă��Ȃ���ԂȂ�����邩�ǂ���
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

            //var Obj = Instantiate(InteractObjectList[/*�I�u�W�F�N�g��ID*/],transform.position,transform.rotation);
            //Obj.GetComponent<InteractObject>().CopyProperty(DropItem);
            //�����艉�o
        }
        else
        {
            //�͂��ꉉ�o
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