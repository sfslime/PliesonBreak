using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region �ϐ�.

    [SerializeField] float speed;     // �ړ��̑���.

    public InputAction cInputAction;  // ����ɂǂ��������L�[�����蓖�Ă邩�����߂邽�߂̃N���X.
    public UIManager cUIManager;      // UI���Ǘ�����}�l�[�W���[.

    string ObjName;                   // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    public bool isGetKey;             // ���������Ă��邩.


    enum InteractObjID
    {
        None,
        Key,
        Door,
    }

    #endregion

    /// <summary>
    /// �I�u�W�F�N�g����A�N�e�B�u�ɂȂ����ۂȂǂɌĂ΂��֐�.
    /// </summary>
    private void OnDisable()
    {
        cInputAction.Disable();  // InputSystem�̑���̎�tOFF.
    }

    /// <summary>
    /// �I�u�W�F�N�g���A�N�e�B�u�ɂȂ����ۂȂǂɌĂ΂��֐�.
    /// </summary>
    private void OnEnable()
    {
        cInputAction.Enable();  // InputSystem�̑���̎�tON.
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
    /// InputSystem��p�����v���C���[�̈ړ�.
    /// </summary>
    void PlayerMove()
    {
        var MoveVector = cInputAction.ReadValue<Vector2>();

        transform.position += new Vector3(MoveVector.x * speed, MoveVector.y * speed, 0) * Time.deltaTime;
        // Debug.Log(MoveVector);
    }

    /// <summary>
    /// ���ݐG��Ă���I�u�W�F�N�g�̏����擾.
    /// </summary>
    /// <param name="collision"></param>
    void GetItemInformation(Collider2D collision)
    {
        ObjName = collision.gameObject.name;
        Debug.Log(ObjName);
    }

    /// <summary>
    /// �C���g���N�g�{�^���������ꂽ�ۂɌĂԊ֐�.
    /// </summary>
    public void PushInteractButton()
    {
        if(ObjName == InteractObjID.Key.ToString())
        {
            isGetKey = true;
            Debug.Log("�������");
        }
        else if(ObjName == InteractObjID.Door.ToString())
        {
            PlayerHaveKey();
        }
    }

    /// <summary>
    /// �v���C���[�����������Ă���ꍇ�Ƀh�A���J���鏈��.
    /// </summary>
    void PlayerHaveKey()
    {
        if (isGetKey == false)
        {
            Debug.Log("�����������Ă���");
        }
        else if (isGetKey == true)
        {
            Debug.Log("�h�A���J����");
        }
    }
}
