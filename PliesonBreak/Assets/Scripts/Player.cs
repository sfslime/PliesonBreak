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
    [SerializeField] Door Door;
    string ObjName;                   // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    public bool isGetKey;             // ���������Ă��邩.

    [SerializeField] List<bool> isEscapeItem;


    enum InteractObjID
    {
        None,
        Key,
        EscapeItem1,
        EscapeItem2,
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
        //Door = GameObject.Find("Door").GetComponent<Door>();
        isGetKey = false;

        for (int i = 0; i < isEscapeItem.Count; i++)
        {
            isEscapeItem[i] = false;
        }
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
        ObjName = collision.gameObject.tag;
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
        else if (ObjName == InteractObjID.EscapeItem1.ToString())
        {
            isEscapeItem[0] = true;
            Debug.Log("�E�o�A�C�e��1�����");
        }
        else if (ObjName == InteractObjID.EscapeItem2.ToString())
        {
            isEscapeItem[1] = true;
            Debug.Log("�E�o�A�C�e��2�����");
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
            Door.DoorOpen(true);
        }
        else if(isEscapeItem[0] == true)
        {

        }
        else if (isEscapeItem[1] == true)
        {

        }
    }
}
