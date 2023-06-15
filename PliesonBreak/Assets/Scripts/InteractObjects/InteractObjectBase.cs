using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using UnityEngine.UI;

public class InteractObjectBase : MonoBehaviour
{
    protected PlayerBase PlayerBase;
    [SerializeField] protected GameManager GameManager;
    [SerializeField] KeysLink KeysLink;
    protected Jailer Jailer;

    protected SpriteRenderer SpriteRenderer;
    public InteractObjs NowInteract;
    int SaveId;  // IDを保存しておく変数.

    protected void SetUp()
    {
        GameManager = GameManager.GameManagerInstance;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (GameManager == null) Debug.Log("GameManagerInstance not found");
    }

    void Start()
    {
        SetUp();
        KeysLink = GetComponent<KeysLink>();
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
        // 自分のIDを保存.
        SaveId = (int)NowInteract;

        // プレイヤーから貰ったIDに自分のIDを変更.
        NowInteract = (InteractObjs)PlayerBase.HaveId;

        // ゲームマネージャーからスプライトの取得.
        GameManager.GetComponent<GameManager>().ReturnSprite(NowInteract);

        // 自分のスプライトの変更.
        ChangeSprite();

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

    public void ChangeSprite()
    {
        switch (NowInteract)
        {
            case InteractObjs.Key1:
                Debug.Log("Sprite:Key");
                SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.Key1);
                break;

            case InteractObjs.Key2:
                Debug.Log("Sprite:Key");
                SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.Key2);
                break;

            case InteractObjs.Key3:
                Debug.Log("Sprite:Key");
                SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.Key3);
                break;

            case InteractObjs.EscapeItem1:
                Debug.Log("Sprite:EscapeItem1");
                SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem1);
                break;

            case InteractObjs.EscapeItem2:
                Debug.Log("Sprite:EscapeItem2");
                SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem2);
                break;
        }
    }
}
