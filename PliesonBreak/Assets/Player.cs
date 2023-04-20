using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region �ϐ�.

    [SerializeField] float speed;     // �ړ��̑���.

    public InputAction cInputAction;  // ����ɂǂ��������L�[�����蓖�Ă邩�����߂邽�߂̃N���X.

    #endregion

    /// <summary>
    /// �I�u�W�F�N�g����A�N�e�B�u�ɂȂ����ۂȂǂɌĂ΂��֐�.
    /// </summary>
    private void OnDisable()
    {
        cInputAction.Disable();
    }

    /// <summary>
    /// �I�u�W�F�N�g���A�N�e�B�u�ɂȂ����ۂȂǂɌĂ΂��֐�.
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
    /// InputSystem��p�����v���C���[�̈ړ�.
    /// </summary>
    void PlayerMove()
    {
        var MoveVector = cInputAction.ReadValue<Vector2>();

        transform.position += new Vector3(MoveVector.x * speed, MoveVector.y * speed, 0) * Time.deltaTime;
        Debug.Log(MoveVector);
    }
}
