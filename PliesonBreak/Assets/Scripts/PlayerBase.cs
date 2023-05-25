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
    public int ObjID;                         // 現在重なっているオブジェクトの情報を取得.
    public int HaveId;                        // 現在持っているアイテムのID.
    [SerializeField] bool isPlayerHaveItem;      // プレイヤーが一度に持てるアイテムの個数.

    [SerializeField] bool isSearch;           // インタラクトしているかどうか

    [SerializeField] List<bool> isEscapeItem; // 脱出アイテムを持っているときに脱出オブジェクトに触れたらtrueを返す.

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
        }

        if ((ObjID == (int)InteractObjectBase.InteractObjs.Key ||
                 ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem1 ||
                 ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem2) &&
                 isPlayerHaveItem == false)
        {
            KeysLink.StateLink(false);
        }

        if (ObjID == (int)InteractObjectBase.InteractObjs.Search)
        {
            StartCoroutine("Search");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.Key)
        {
            isPlayerHaveItem = true;
            Debug.Log("鍵を入手");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.Door)
        {
            PlayerHaveKey();
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem1)
        {
            isPlayerHaveItem = true;
            // Debug.Log("脱出アイテム1を入手");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem2)
        {
            isPlayerHaveItem = true;
            // Debug.Log("脱出アイテム2を入手");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeObj)
        {
            if (HaveId == (int)InteractObjectBase.InteractObjs.EscapeItem1)
            {
                isPlayerHaveItem = isEscapeItem[0] = true;
                HaveId = (int)InteractObjectBase.InteractObjs.None;
                Debug.Log("脱出アイテム1をセット");
            }
            if (HaveId == (int)InteractObjectBase.InteractObjs.EscapeItem2)
            {
                isPlayerHaveItem = isEscapeItem[1] = true;
                HaveId = (int)InteractObjectBase.InteractObjs.None;
                Debug.Log("脱出アイテム2をセット");
            }
            if (isEscapeItem[0] == true && isEscapeItem[1] == true)
            {
                //Goal.PlayerGoal();
                Debug.Log("ゴールしました");
            }
        }
        if (isPlayerHaveItem == true)
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
        if (HaveId != (int)InteractObjectBase.InteractObjs.Key)
        {
            Debug.Log("鍵がかかっている");
        }
        else if (HaveId == (int)InteractObjectBase.InteractObjs.Key)
        {
            Debug.Log("ドアが開いた");
            Door.DoorOpen(true);
            isPlayerHaveItem = false;
            HaveId = (int)InteractObjectBase.InteractObjs.None;
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

