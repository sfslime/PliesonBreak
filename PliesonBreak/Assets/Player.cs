using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region 変数.

    [SerializeField] float speed;     // 移動の速さ.

    public InputAction cInputAction;  // 操作にどういったキーを割り当てるかを決めるためのクラス.

    #endregion

    /// <summary>
    /// オブジェクトが非アクティブになった際などに呼ばれる関数.
    /// </summary>
    private void OnDisable()
    {
        cInputAction.Disable();
    }

    /// <summary>
    /// オブジェクトがアクティブになった際などに呼ばれる関数.
    /// </summary>
    private void OnEnable()
    {
        cInputAction.Enable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerMove();
    }

    /// <summary>
    /// InputSystemを用いたプレイヤーの移動.
    /// </summary>
    void PlayerMove()
    {
        var MoveVector = cInputAction.ReadValue<Vector2>();

        transform.position += new Vector3(MoveVector.x * speed, MoveVector.y * speed, 0) * Time.deltaTime;
        Debug.Log(MoveVector);
    }
}
