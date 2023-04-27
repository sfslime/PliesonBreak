using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region 変数.

    [SerializeField] float speed;     // 移動の速さ.

    public InputAction cInputAction;  // 操作にどういったキーを割り当てるかを決めるためのクラス.
    public UIManager cUIManager;      // UIを管理するマネージャー.

    string ObjName;                   // 現在重なっているオブジェクトの情報を取得.
    public bool isGetKey;             // 鍵を持っているか.


    enum InteractObjID
    {
        None,
        Key,
        Door,
    }

    #endregion

    /// <summary>
    /// オブジェクトが非アクティブになった際などに呼ばれる関数.
    /// </summary>
    private void OnDisable()
    {
        cInputAction.Disable();  // InputSystemの操作の受付OFF.
    }

    /// <summary>
    /// オブジェクトがアクティブになった際などに呼ばれる関数.
    /// </summary>
    private void OnEnable()
    {
        cInputAction.Enable();  // InputSystemの操作の受付ON.
    }

    void Start()
    {
        isGetKey = false;
    }

    void Update()
    {
        PlayerMove();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        cUIManager.IsInteractButton(true);
        GetItemInformation(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cUIManager.IsInteractButton(false);
    }

    /// <summary>
    /// InputSystemを用いたプレイヤーの移動.
    /// </summary>
    void PlayerMove()
    {
        var MoveVector = cInputAction.ReadValue<Vector2>();

        transform.position += new Vector3(MoveVector.x * speed, MoveVector.y * speed, 0) * Time.deltaTime;
        // Debug.Log(MoveVector);
    }

    /// <summary>
    /// 現在触れているオブジェクトの情報を取得.
    /// </summary>
    /// <param name="collision"></param>
    void GetItemInformation(Collider2D collision)
    {
        ObjName = collision.gameObject.name;
        Debug.Log(ObjName);
    }

    /// <summary>
    /// イントラクトボタンが押された際に呼ぶ関数.
    /// </summary>
    public void PushInteractButton()
    {
        if(ObjName == InteractObjID.Key.ToString())
        {
            isGetKey = true;
            Debug.Log("鍵を入手");
        }
        else if(ObjName == InteractObjID.Door.ToString())
        {
            PlayerHaveKey();
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
        }
    }
}
