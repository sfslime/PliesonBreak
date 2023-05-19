using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    protected PlayerBase PlayerBase;
    [SerializeField] protected GameManager GameManager;
    [SerializeField] protected KeysLink KeysLink;
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
    int SaveId;  // ID��ۑ����Ă����ϐ�.

    protected void SetUp()
    {
        GameManager = GameManager.GameManagerInstance;
        if (GameManager == null) Debug.Log("GameManagerInstance not found");
        KeysLink = GetComponent<KeysLink>();
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
        if(collision.gameObject.tag == "Player")
        {
            PlayerBase = collision.gameObject.GetComponent<PlayerBase>();
            PlayerBase.GetItemInformation((int)NowInteract);
        }
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
        // �Q�[���}�l�[�W���[����X�v���C�g�̎擾.
        var Sprite = GameManager.GetComponent<GameManager>().ReturnSprite(NowInteract);
        
        // �����̃X�v���C�g�̕ύX.


        // ������ID��ۑ�.
        SaveId = (int)NowInteract;

        // �v���C���[��������ID�Ɏ�����ID��ύX.
        NowInteract = (InteractObjs)PlayerBase.HaveId;
        

        // �v���C���[�Ɏ�����ID��Ԃ�.
        PlayerBase.ChangeHaveItem(SaveId);

        KeysLink.StateLink(NowInteract);
        Debug.Log("Save"+SaveId);

    }

    public KeysLink PostKeyLink()
    {

        Debug.Log("InteractObject");
        return GetComponent<KeysLink>();
    }
}
