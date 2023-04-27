using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPoint : InteractObject
{
    private InteractObject DropItem;
    [SerializeField] float DefaltSearchTime;
    [SerializeField] bool isDestroy;

    // Start is called before the first frame update
    void Start()
    {
        NowInteract = InteractObj.Search;
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

            Instantiate(DropItem,transform.position,transform.rotation);
            //Ç†ÇΩÇËââèo
        }
        else
        {
            //ÇÕÇ∏ÇÍââèo
        }
    }

    public void StopSearch()
    {
        StopCoroutine("SearchStart");
    }
}
