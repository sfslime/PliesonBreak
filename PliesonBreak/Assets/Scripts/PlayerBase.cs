using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    #region �ϐ�.
    Rigidbody2D Rb;

    [SerializeField] float Speed;     // �ړ��̑���.

    public InputAction InputAction;      // ����ɂǂ��������L�[�����蓖�Ă邩�����߂邽�߂̃N���X.
    public UIManagerBase UIManager;      // UI���Ǘ�����}�l�[�W���[.
    [SerializeField] DoorBase Door;
    [SerializeField] InteractObjectBase InteractObjectBase;
    [SerializeField] SearchPoint SearchPoint;
    [SerializeField] Goal Goal;
    public KeysLink KeysLink;
    public int ObjID;                         // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    public int HaveId;                        // ���ݎ����Ă���A�C�e����ID.
    [SerializeField] bool isPlayerHaveItem;      // �v���C���[����x�Ɏ��Ă�A�C�e���̌�.

    [SerializeField] bool isSearch;           // �C���^���N�g���Ă��邩�ǂ���

    [SerializeField] List<bool> isEscapeItem; // �E�o�A�C�e���������Ă���Ƃ��ɒE�o�I�u�W�F�N�g�ɐG�ꂽ��true��Ԃ�.

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
    /// InputSystem��p�����v���C���[�̈ړ�.
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
    /// ���ݐG��Ă���I�u�W�F�N�g�̏����擾.
    /// </summary>
    public void GetItemInformation(int InteractObjID)
    {
        ObjID = InteractObjID;
        // Debug.Log(InteractObjID);
    }

    /// <summary>
    /// �C���g���N�g�{�^���������ꂽ�ۂɌĂԊ֐�.
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
            Debug.Log("�������");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.Door)
        {
            PlayerHaveKey();
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem1)
        {
            isPlayerHaveItem = true;
            // Debug.Log("�E�o�A�C�e��1�����");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem2)
        {
            isPlayerHaveItem = true;
            // Debug.Log("�E�o�A�C�e��2�����");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeObj)
        {
            if (HaveId == (int)InteractObjectBase.InteractObjs.EscapeItem1)
            {
                isPlayerHaveItem = isEscapeItem[0] = true;
                HaveId = (int)InteractObjectBase.InteractObjs.None;
                Debug.Log("�E�o�A�C�e��1���Z�b�g");
            }
            if (HaveId == (int)InteractObjectBase.InteractObjs.EscapeItem2)
            {
                isPlayerHaveItem = isEscapeItem[1] = true;
                HaveId = (int)InteractObjectBase.InteractObjs.None;
                Debug.Log("�E�o�A�C�e��2���Z�b�g");
            }
            if (isEscapeItem[0] == true && isEscapeItem[1] == true)
            {
                //Goal.PlayerGoal();
                Debug.Log("�S�[�����܂���");
            }
        }
        if (isPlayerHaveItem == true)
        {
            Debug.Log("����ȏ�A�C�e�������Ă܂���");
            ChangeHaveItem(HaveId);
        }
    }

    /// <summary>
    /// �v���C���[�����������Ă���ꍇ�Ƀh�A���J���鏈��.
    /// </summary>
    void PlayerHaveKey()
    {
        if (HaveId != (int)InteractObjectBase.InteractObjs.Key)
        {
            Debug.Log("�����������Ă���");
        }
        else if (HaveId == (int)InteractObjectBase.InteractObjs.Key)
        {
            Debug.Log("�h�A���J����");
            Door.DoorOpen(true);
            isPlayerHaveItem = false;
            HaveId = (int)InteractObjectBase.InteractObjs.None;
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
            InteractObjectBase = collision.gameObject.GetComponent<InteractObjectBase>();
            UIManager.IsInteractButton(true);
            KeysLink = InteractObjectBase.PostKeyLink();
        }
    }

    /// <summary>
    /// �T�[�`���Ă��鎞�ԂƃT�[�`���Ă��邩�ǂ����𔻒肷�鏈��.
    /// </summary>
    /// <returns></returns>
    IEnumerator Search()
    {
        isSearch = true;
        yield return StartCoroutine(SearchPoint.SearchStart(1));
        isSearch = false;
    }

    /// <summary>
    /// �v���C���[�����łɃA�C�e���������Ă���Ƃ��ɃA�C�e����؂�ւ��鏈��.
    /// </summary>
    public void ChangeHaveItem(int olditem)
    {
        HaveId = olditem;
        //Debug.Log(olditem);
    }
}

