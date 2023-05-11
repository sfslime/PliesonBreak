using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    #region �ϐ�.
    Rigidbody2D Rb;

    [SerializeField] float Speed;     // �ړ��̑���.

    public InputAction InputAction;  // ����ɂǂ��������L�[�����蓖�Ă邩�����߂邽�߂̃N���X.
    public UIManagerBase UIManager;  // UI���Ǘ�����}�l�[�W���[.
    [SerializeField] DoorBase Door;
    [SerializeField] InteractObjectBase InteractObjectBase;
    [SerializeField] SearchPoint SearchPoint;
    int ObjID;                      // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    public bool isGetKey;           // ���������Ă��邩.
    [SerializeField]bool isSearch;                  // �C���^���N�g���Ă��邩�ǂ���

    [SerializeField] List<bool> isEscapeItem;


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
        Rb = GetComponent<Rigidbody2D>();
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

    }

    private void FixedUpdate()
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
        if(isSearch != true)
        {
            var MoveVector = InputAction.ReadValue<Vector2>();

            Rb.velocity = new Vector3(MoveVector.x * Speed, MoveVector.y * Speed, 0) * Time.deltaTime;


            // Debug.Log(MoveVector);
        }
    }

    /// <summary>
    /// ���ݐG��Ă���I�u�W�F�N�g�̏����擾.
    /// </summary>
    /// <param name="collision"></param>
    public void GetItemInformation(int InteractObjID)
    {
        ObjID = InteractObjID;
        Debug.Log(InteractObjID);
    }

    /// <summary>
    /// �C���g���N�g�{�^���������ꂽ�ۂɌĂԊ֐�.
    /// </summary>
    public void PushInteractButton()
    {
        StartCoroutine("Search");

        if (ObjID == (int)InteractObjectBase.InteractObjs.Key)
        {
            isGetKey = true;
            Debug.Log("�������");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.Door)
        {
            PlayerHaveKey();
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem1)
        {
            isEscapeItem[0] = true;
            Debug.Log("�E�o�A�C�e��1�����");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem2)
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
            else if (isEscapeItem[0] == true)
            {

            }
            else if (isEscapeItem[1] == true)
            {

            }
        }

        /// <summary>
        /// �C���g���N�g�\�ȃI�u�W�F�N�g�ɐG�ꂽ�Ƃ��ɌĂ�.
        /// </summary>
        /// <param name="collision"></param>
        void EnterInteractObj(Collider2D collision)
        {
            if (collision.gameObject.tag == "InteractObject")
            {
                UIManager.IsInteractButton(true);
            }
        }

    IEnumerator Search()
    {
        isSearch = true;
        yield return StartCoroutine(SearchPoint.SearchStart(1));
        isSearch = false;
    }
}
