using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

public class PlayerBase : MonoBehaviour
{
    #region 変数.
    Rigidbody2D Rb;

    [SerializeField] float Speed;        // 移動の速さ.
    [SerializeField] float SaveSpeed;    // 移動スピードを保存する変数.

    public InputAction InputAction;      // 操作にどういったキーを割り当てるかを決めるためのクラス.
    public UIManagerBase UIManager;      // UIを管理するマネージャー.
    
    [SerializeField] DoorBase Door;
    [SerializeField] Prison Prison;
    [SerializeField] InteractObjectBase InteractObjectBase;
    [SerializeField] SearchPoint SearchPoint;
    [SerializeField] Goal Goal;
    [SerializeField] BearTrap BearTrap;
    public KeysLink KeysLink;
    public int ObjID;                         // 現在重なっているオブジェクトの情報を取得.
    public int HaveId;                        // 現在持っているアイテムのID.
    [SerializeField] bool isPlayerHaveItem;   // プレイヤーが一度に持てるアイテムの個数.
    [SerializeField] bool isSearch;           // インタラクトしているかどうか
    public bool isMove;                       // 動けるかどうか.

    [SerializeField] List<bool> isEscapeItem; // 脱出アイテムを持っているときに脱出オブジェクトに触れたらtrueを返す.

    [SerializeField, Header("アニメーション用変数"), Tooltip("現在のアニメーション状態")] AnimCode AnimState;

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
        SaveSpeed = Speed;
        isMove = false;
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
        // 触れたインタラクトオブジェクトのComponentの取得.
        if (collision.gameObject.tag == "InteractObject")
        {
            switch (ObjID)
            {
                case (int)InteractObjs.Door:
                    Door = collision.gameObject.GetComponent<DoorBase>();
                    break;

                case (int)InteractObjs.Prison:
                    Prison = collision.gameObject.GetComponent<Prison>();
                    break;

                case (int)InteractObjs.Search:
                    SearchPoint = collision.gameObject.GetComponent<SearchPoint>();
                    break;

                case (int)InteractObjs.EscapeObj:
                    Goal = collision.gameObject.GetComponent<Goal>();
                    break;

                case (int)InteractObjs.CloseBearTrap:
                    BearTrap = collision.gameObject.GetComponent<BearTrap>();
                    break;
            }
            // Debug.Log("ObjID" +ObjID);
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
        if (isSearch == false && isMove == false)
        {
            var MoveVector = InputAction.ReadValue<Vector2>();

            Speed = SaveSpeed;
            Rb.velocity = new Vector3(MoveVector.x * Speed, MoveVector.y * Speed, 0) * Time.deltaTime;
        }
        if(isSearch == true && isMove == true)
        {
            Speed = 0;
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
        // 拾えないアイテムの排除.
        Debug.Log("ID>" + ObjID);
        if (ObjID != (int)InteractObjs.Search && 
            ObjID != (int)InteractObjs.Door &&
            ObjID != (int)InteractObjs.Prison &&
            ObjID != (int)InteractObjs.EscapeObj &&
            ObjID != (int)InteractObjs.OpenBearTrap &&
            ObjID != (int)InteractObjs.CloseBearTrap)
        {
            InteractObjectBase.RequestSprite();
            HaveId = ObjID;
        }
        // オブジェクトを破棄.
        if ((ObjID == (int)InteractObjs.Key1 ||
             ObjID == (int)InteractObjs.Key2 ||
             ObjID == (int)InteractObjs.Key3 ||
             ObjID == (int)InteractObjs.EscapeItem1 ||
             ObjID == (int)InteractObjs.EscapeItem2) &&
             isPlayerHaveItem == false)
        {
            KeysLink.StateLink(false);
        }

        // 触れているオブジェクトに対してインタラクトボタンを押した時に行う処理.
        switch (ObjID)
        {
            case (int)InteractObjs.Search:
                StartCoroutine("Search");
                break;

            case (int)InteractObjs.Key1:
            case (int)InteractObjs.Key2:
            case (int)InteractObjs.Key3:
                isPlayerHaveItem = true;
                break;

            case (int)InteractObjs.Door:
                PlayerHaveKey();
                break;

            case (int)InteractObjs.Prison:
                if (!PhotonNetwork.LocalPlayer.GetArrestStatus())
                {
                    Prison.PrisonOpen(true);
                }
                break;

            case (int)InteractObjs.EscapeItem1:
            case (int)InteractObjs.EscapeItem2:
                isPlayerHaveItem = true;
                break;

            case (int)InteractObjs.EscapeObj:
                if (Goal.SetEscapeItem((InteractObjs)HaveId))
                {
                    isPlayerHaveItem = false;
                    HaveId = (int)InteractObjs.None;
                }
                break;

            case (int)InteractObjs.CloseBearTrap:
                StartCoroutine(BearTrap.BearTrapOpen(3));
                StartCoroutine(InteractTime(3));
                break;
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
        if (HaveId == (int)Door.NeedKeyID)
        {
            Debug.Log("ドアが開いた");
            Door.DoorOpen(true);
            isPlayerHaveItem = false;
            HaveId = (int)InteractObjs.None;
        }
        else
        {
            Debug.Log("鍵がかかっている");
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
            if(ObjID != (int)InteractObjs.OpenBearTrap) UIManager.IsInteractButton(true);
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
    /// インタラクトするのに掛かる時間.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator InteractTime(float time)
    {
        isMove = true;
        yield return new WaitForSeconds(time);
        isMove = false;
    }
    

    /// <summary>
    /// プレイヤーがすでにアイテムを持っているときにアイテムを切り替える処理.
    /// </summary>
    public void ChangeHaveItem(int olditem)
    {
        HaveId = olditem;
    }

    public AnimCode GetAnimState()
    {
        if (isSearch) return AnimCode.Search;

        if (Rb.velocity.magnitude > 0) return AnimCode.Run;

        return AnimCode.Idel;
    }
}

