using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    PlayerBase PlayerBase;
    public  enum InteractObjs
    {
        None,
        Key,
        Door,
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
    }

    public InteractObjs NowInteract;
    protected void SetUp()
    {
        PlayerBase = GameObject.Find("PlayerSprite").GetComponent<PlayerBase>();
    }
    void Start()
    {
        SetUp();
        NowInteract = InteractObjs.None;
    }

    void Update()
    {
        
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        PlayerBase.GetItemInformation((int)NowInteract);
    }

    /// <summary>
    /// �V�����������ꂽ�I�u�W�F�N�g�ɓn���֐�.
    /// </summary>
    /// <param name="oldobject"></param>
    public virtual void CopyProperty(InteractObjectBase oldobject)
    {
        NowInteract = oldobject.GetComponent<InteractObjectBase>().NowInteract;
    }
}
