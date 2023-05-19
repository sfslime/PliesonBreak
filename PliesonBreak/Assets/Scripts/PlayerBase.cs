using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    #region 変数.
    Rigidbody2D Rb;

    [SerializeField] float Speed;     // 移動の速さ.

    public InputAction InputAction;      // 操作にどういったキーを割り当てるかを決めるためのクラス.
    public UIManagerBase UIManager;      // UIを管理するマネージャー.
    [SerializeField] DoorBase Door;
    [SerializeField] InteractObjectBase InteractObjectBase;
    [SerializeField] SearchPoint SearchPoint;
    [SerializeField] Goal Goal;
    public KeysLink KeysLink;
    [SerializeField] int ObjID;          // 現在重なっているオブジェクトの情報を取得.
    public int HaveId;                   // 現在持っているアイテムのID.
    [SerializeField] int PlayerHaveItem; // プレイヤーが一度に持てるアイテムの個数.

    public bool isGetKey;                // 鍵を持っているか.
    [SerializeField] bool isSearch;      // インタラクトしているかどうか

    [SerializeField] List<bool> isGetEscapeItem;  // 脱出アイテムを持っているか.
    [SerializeField] List<bool> isEscapeItem;     // 脱出アイテムを持っているときに脱出オブジェクトに触れたらtrueを返す.

    #endregion

    /// <summary>
    /// オブジェクトが非アクティブになった際などに呼ばれる関数.
    /// </summary>
    private void OnDisable()
    {
        InputAction.Disable();  // InputSystemの操作の受付OFF.
    }

    /// <summary>
    /// オブジェクトがアクティブになった際などに呼ばれる関数.
    /// </summary>
    private void OnEnable()
    {
        InputAction.Enable();  // InputSystemの操作の受付ON.
    }

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagerBase>();
        isGetKey = false;

        for (int i = 0; i < isGetEscapeItem.Count; i++)
        {
            isGetEscapeItem[i] = false;
        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "InteractObject" && ObjID == (int)InteractObjectBase.InteractObjs.Door)
        {
            Door = collision.gameObject.GetComponent<DoorBase>();
        }
        if (collision.gameObject.tag == "InteractObject" && ObjID == (int)InteractObjectBase.InteractObjs.Search)
        {
            SearchPoint = collision.gameObject.GetComponent<SearchPoint>();
        }
        
        EnterInteractObj(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.IsInteractButton(false);
    }

    /// <summary>
    /// InputSystemを用いたプレイヤーの移動.
    /// </summary>
    void PlayerMove()
    {
        if (isSearch != true)
        {
            var MoveVector = InputAction.ReadValue<Vector2>();

            Rb.velocity = new Vector3(MoveVector.x * Speed, MoveVector.y * Speed, 0) * Time.deltaTime;


            // Debug.Log(MoveVector);
        }
    }

    /// <summary>
    /// 現在触れているオブジェクトの情報を取得.
    /// </summary>
    public void GetItemInformation(int InteractObjID)
    {
        ObjID = InteractObjID;
        // Debug.Log(InteractObjID);
    }

    /// <summary>
    /// イントラクトボタンが押された際に呼ぶ関数.
    /// </summary>
    public void PushInteractButton()
    {
        Debug.Log("ID>" + ObjID);
        if (ObjID != (int)InteractObjectBase.InteractObjs.Search && 
            ObjID != (int)InteractObjectBase.InteractObjs.Door && 
            ObjID != (int)InteractObjectBase.InteractObjs.EscapeObj)
        {
            
            InteractObjectBase.RequestSprite();
            HaveId = ObjID;
            Debug.Log("おけ");
        }
        
        if (ObjID == (int)InteractObjectBase.InteractObjs.Search)
        {
            StartCoroutine("Search");
        }
        else if ((ObjID == (int)InteractObjectBase.InteractObjs.Key) && PlayerHaveItem > 0)
        {
            isGetKey = true;
            PlayerHaveItem--;
            Debug.Log("鍵を入手");
            KeysLink.StateLink(false);
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.Door)
        {
            PlayerHaveKey();
        }
        else if ((ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem1) && PlayerHaveItem > 0)
        {
            isGetEscapeItem[0] = true;
            PlayerHaveItem--;
            Debug.Log("脱出アイテム1を入手");
            KeysLink.StateLink(false);
        }
        else if ((ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem2) && PlayerHaveItem > 0)
        {
            isGetEscapeItem[1] = true;
            PlayerHaveItem--;
            Debug.Log("脱出アイテム2を入手");
            KeysLink.StateLink(false);
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeObj)
        {

            if (isGetEscapeItem[0] == true && isEscapeItem[0] == false)
            {
                isEscapeItem[0] = true;
                PlayerHaveItem++;
            }
            if (isGetEscapeItem[1] == true && isEscapeItem[1] == false)
            {
                isEscapeItem[1] = true;
                PlayerHaveItem++;
            }
            if (isGetEscapeItem[0] == true && isGetEscapeItem[1] == true)
            {
                Goal.PlayerGoal();
            }
        }
        if (PlayerHaveItem <= 0)
        {
            Debug.Log("これ以上アイテムを持てません");
            ChangeHaveItem(HaveId);
        }
    }

    /// <summary>
    /// プレイヤーが鍵を持っている場合にドアを開ける処理.
    /// </summary>
    void PlayerHaveKey()
    {
        if (isGetKey == false)
        {
            Debug.Log("鍵がかかっている");
        }
        else if (isGetKey == true)
        {
            Debug.Log("ドアが開いた");
            Door.DoorOpen(true);
            PlayerHaveItem++;
        }

    }

    /// <summary>
    /// イントラクト可能なオブジェクトに触れたときに呼ぶ.
    /// </summary>
    /// <param name="collision"></param>
    void EnterInteractObj(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractObject")
        {
            InteractObjectBase = collision.gameObject.GetComponent<InteractObjectBase>();
            UIManager.IsInteractButton(true);
            KeysLink = InteractObjectBase.PostKeyLink();
        }
    }

    /// <summary>
    /// サーチしている時間とサーチしているかどうかを判定する処理.
    /// </summary>
    /// <returns></returns>
    IEnumerator Search()
    {
        isSearch = true;
        yield return StartCoroutine(SearchPoint.SearchStart(1));
        isSearch = false;
    }

    /// <summary>
    /// プレイヤーがすでにアイテムを持っているときにアイテムを切り替える処理.
    /// </summary>
    public void ChangeHaveItem(int olditem)
    {
        HaveId = olditem;
        //Debug.Log(olditem);
    }
}

