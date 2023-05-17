using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    // やること
    //Player > 引数oldId.
    //GameManagerからoldIDの画像を取得.
    //oldIdで画像の差し替えをする.
    //戻り値で新しいIDをPlayerに返す.


    protected PlayerBase PlayerBase;
    protected GameManager GameManager;
    public  enum InteractObjs
    {
        None,
        Key,
        Door,
        NullDrop,
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
    }

    public InteractObjs NowInteract;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        PlayerBase = collision.gameObject.GetComponent<PlayerBase>();
        PlayerBase.GetItemInformation((int)NowInteract);
    }

    /// <summary>
    /// 新しく生成されたオブジェクトに渡す関数.
    /// </summary>
    /// <param name="oldobject"></param>
    public virtual void CopyProperty(InteractObjectBase oldobject)
    {
        NowInteract = oldobject.GetComponent<InteractObjectBase>().NowInteract;
    }

    public void RequestSprite()
    {
        GameManager.GetComponent<GameManager>().ReturnSprite(NowInteract);
    }
}
