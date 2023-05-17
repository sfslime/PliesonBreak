using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    // ��邱��
    //Player > ����oldId.
    //GameManager����oldID�̉摜���擾.
    //oldId�ŉ摜�̍����ւ�������.
    //�߂�l�ŐV����ID��Player�ɕԂ�.


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
    /// �V�����������ꂽ�I�u�W�F�N�g�ɓn���֐�.
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
