using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    #region �ϐ�.

    [SerializeField] float speed;     // �ړ��̑���.

    public InputAction InputAction;  // ����ɂǂ��������L�[�����蓖�Ă邩�����߂邽�߂̃N���X.
    public UIManagerBase UIManager;      // UI���Ǘ�����}�l�[�W���[.
    [SerializeField] DoorBase Door;
    string ObjID;                   // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    public bool isGetKey;             // ���������Ă��邩.

    [SerializeField] List<bool> isEscapeItem;


    enum InteractObjIDs
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
        InputAction.Disable();  // InputSystem�̑���̎�tOFF.
    }

    /// <summary>
    /// �I�u�W�F�N�g���A�N�e�B�u�ɂȂ����ۂȂǂɌĂ΂��֐�.
    /// </summary>
    private void OnEnable()
    {
        InputAction.Enable();  // InputSystem�̑���̎�tON.
    }

    void Start()
    {
        //Door = GameObject.Find("Door").GetComponent<Door>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagerBase>();
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
        EnterInteractObj(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.IsInteractButton(false);
    }

    /// <summary>
    /// InputSystem��p�����v���C���[�̈ړ�.
    /// </summary>
    void PlayerMove()
    {
        var MoveVector = InputAction.ReadValue<Vector2>();

        transform.position += new Vector3(MoveVector.x * speed, MoveVector.y * speed, 0) * Time.deltaTime;
        // Debug.Log(MoveVector);
    }

    /// <summary>
    /// ���ݐG��Ă���I�u�W�F�N�g�̏����擾.
    /// </summary>
    /// <param name="collision"></param>
    void GetItemInformation(Collider2D collision)
    {
       //ObjID  = ;
        Debug.Log(ObjID);
    }

    /// <summary>
    /// �C���g���N�g�{�^���������ꂽ�ۂɌĂԊ֐�.
    /// </summary>
    public void PushInteractButton()
    {
        if(ObjID == InteractObjIDs.Key.ToString())
        {
            isGetKey = true;
            Debug.Log("�������");
        }
        else if(ObjID == InteractObjIDs.Door.ToString())
        {
            PlayerHaveKey();
        }
        else if (ObjID == InteractObjIDs.EscapeItem1.ToString())
        {
            isEscapeItem[0] = true;
            Debug.Log("�E�o�A�C�e��1�����");
        }
        else if (ObjID == InteractObjIDs.EscapeItem2.ToString())
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

    void EnterInteractObj(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractObject")
        {
            UIManager.IsInteractButton(true);
            GetItemInformation(collision);
        }
    }
}
