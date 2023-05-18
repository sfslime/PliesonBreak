using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    protected PlayerBase PlayerBase;
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
    protected void SetUp()
    {
      
    }
    void Start()
    {
        SetUp();
    }

    void Update()
    {
        
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
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
}
