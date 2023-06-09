using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    [SerializeField] MapObject MapObject;
    public KeysLink KeysLink;
    public int ObjID;                         // 現在重なっているオブジェクトの情報を取得.
    public int HaveId;                        // 現在持っているアイテムのID.
    [SerializeField] bool isPlayerHaveItem;   // プレイヤーが一度に持てるアイテムの個数.
    [SerializeField] bool isSearch;           // インタラクトしているかどうか.
    [SerializeField] bool isLookMap;          // マップを開いているかどうか.
    
    public bool isMove;                       // 動けるかどうか.
    [SerializeField, Tooltip("探索進行度を表示するスライダー")] Slider ProgressSlider;

    [SerializeField] List<bool> isEscapeItem; // 脱出アイテムを持っているときに脱出オブジェクトに触れたらtrueを返す.

    [SerializeField, Header("アニメーション用変数"), Tooltip("現在のアニメーション状態")] AnimCode AnimState;
    private AudioSource AudioSource;          // 足音再生用

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
        UIManager = UIManagerBase.instance;
        SaveSpeed = Speed;
        isMove = false;
        AudioSource = GetComponent<AudioSource>();
        StartCoroutine(WalkSE());
    }

    void Update()
    {
        if (isSearch && Input.anyKeyDown)
        {
            SearchPoint.StopSearch();
        }
        if (Input.GetKeyDown(KeyCode.Space) && ObjID != (int)InteractObjs.None && UIManager.GetInteractButtonActive())
        {
            PushInteractButton();
        }
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

                case (int)InteractObjs.Map:
                    MapObject = collision.gameObject.GetComponent<MapObject>();
                    break;
            }
            EnterInteractObj(collision);
        }
        else
        {
            if (ObjID != (int)InteractObjs.OpenBearTrap) UIManager.IsInteractButton(false);
        }
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
        var MoveVector = InputAction.ReadValue<Vector2>();

        if (isSearch == false && isMove == false)
        {
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
        // Debug.Log("ID>" + ObjID);
        if (ObjID != (int)InteractObjs.Search && 
            ObjID != (int)InteractObjs.Door &&
            ObjID != (int)InteractObjs.Prison &&
            ObjID != (int)InteractObjs.EscapeObj &&
            ObjID != (int)InteractObjs.OpenBearTrap &&
            ObjID != (int)InteractObjs.CloseBearTrap &&
            ObjID != (int)InteractObjs.Map)
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
                
                if (isSearch == false)
                {
                    StartCoroutine(Search());
                }
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

            case (int)InteractObjs.Map:
                Map();
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
            GameManager.GameManagerInstance.PlaySE(SEid.DoorClose, transform.position);
            Debug.Log("鍵がかかっている");
        }
    }

    /// <summary>
    /// イントラクト可能なオブジェクトに触れたときに呼ぶ.
    /// </summary>
    /// <param name="collision"></param>
    void EnterInteractObj(Collider2D collision)
    {
        InteractObjectBase = collision.gameObject.GetComponent<InteractObjectBase>();
        if (ObjID != (int)InteractObjs.OpenBearTrap) UIManager.IsInteractButton(true);
        KeysLink = InteractObjectBase.PostKeyLink();
    }

    /// <summary>
    /// サーチしている時間とサーチしているかどうかを判定する処理.
    /// </summary>
    /// <returns></returns>
    IEnumerator Search()
    {
        isSearch = true;
        Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return StartCoroutine(SearchPoint.SearchStart(1,ProgressSlider));
        isSearch = false;
        Rb.constraints = RigidbodyConstraints2D.None;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Map()
    {
        if(isLookMap == true)
        {
            MapObject.LookMap(true);
        }
        else
        {
            MapObject.LookMap(false);
        }
        
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
    /// 足音を鳴らすコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkSE()
    {
        Vector3 pos = transform.position;
        while (true)
        {
            if(pos != transform.position)
            {
                if (!AudioSource.isPlaying) AudioSource.Play();
            }
            else
            {
                AudioSource.Stop();
            }
            pos = transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }
    

    /// <summary>
    /// プレイヤーがすでにアイテムを持っているときにアイテムを切り替える処理.
    /// </summary>
    public void ChangeHaveItem(int olditem)
    {
        if(isSearch == false)
        {
            StartCoroutine(ItemSwitchingWaitTime(1.0f));
            HaveId = olditem;
        }
    }

    public AnimCode GetAnimState()
    {
        if (isSearch) return AnimCode.Search;

        if (Rb.velocity.magnitude > 0) return AnimCode.Run;

        return AnimCode.Idel;
    }

    /// <summary>
    /// アイテムの切り替えを連続でできないようにする為の関数.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator ItemSwitchingWaitTime(float time)
    {
        UIManager.IsInteractButton(false);
        yield return new WaitForSeconds(time);
    }
}

