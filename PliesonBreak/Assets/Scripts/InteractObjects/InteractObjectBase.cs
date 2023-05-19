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
    int SaveId;  // IDを保存しておく変数.

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
    /// 新しく生成されたオブジェクトに渡す関数.
    /// </summary>
    /// <param name="oldobject"></param>
    public virtual void CopyProperty(InteractObjectBase oldobject)
    {
        NowInteract = oldobject.GetComponent<InteractObjectBase>().NowInteract;
    }

    public void RequestSprite()
    {
        // ゲームマネージャーからスプライトの取得.
        var Sprite = GameManager.GetComponent<GameManager>().ReturnSprite(NowInteract);
        
        // 自分のスプライトの変更.


        // 自分のIDを保存.
        SaveId = (int)NowInteract;

        // プレイヤーから貰ったIDに自分のIDを変更.
        NowInteract = (InteractObjs)PlayerBase.HaveId;
        

        // プレイヤーに自分のIDを返す.
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
