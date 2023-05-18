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
    [SerializeField] Goal Goal;
    int ObjID;                           // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    [SerializeField] int PlayerHaveItem; // �v���C���[����x�Ɏ��Ă�A�C�e���̌�.
    public bool isGetKey;                // ���������Ă��邩.
    [SerializeField] bool isSearch;      // �C���^���N�g���Ă��邩�ǂ���

    [SerializeField] List<bool> isGetEscapeItem;  // �E�o�A�C�e���������Ă��邩.
    [SerializeField] List<bool> isEscapeItem;     // �E�o�A�C�e���������Ă���Ƃ��ɒE�o�I�u�W�F�N�g�ɐG�ꂽ��true��Ԃ�.



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

        isGetKey = false;

        for (int i = 0; i < isGetEscapeItem.Count; i++)
        {
            isGetEscapeItem[i] = false;
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
        Door = collision.gameObject.GetComponent<DoorBase>();
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
        if (ObjID == (int)InteractObjectBase.InteractObjs.Search)
        {
            StartCoroutine("Search");
        }
        else if ((ObjID == (int)InteractObjectBase.InteractObjs.Key) && PlayerHaveItem > 0)
        {
            isGetKey = true;
            PlayerHaveItem--;
            Debug.Log("�������");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.Door)
        {
            PlayerHaveKey();
        }
        else if ((ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem1) && PlayerHaveItem > 0)
        {
            isGetEscapeItem[0] = true;
            PlayerHaveItem--;
            Debug.Log("�E�o�A�C�e��1�����");
        }
        else if ((ObjID == (int)InteractObjectBase.InteractObjs.EscapeItem2) && PlayerHaveItem > 0)
        {
            isGetEscapeItem[1] = true;
            PlayerHaveItem--;
            Debug.Log("�E�o�A�C�e��2�����");
        }
        else if (ObjID == (int)InteractObjectBase.InteractObjs.EscapeObj)
        {

            if (isGetEscapeItem[0] == true && isEscapeItem[0] == false)
            {
                isEscapeItem[0] = true;
                PlayerHaveItem++;
            }
            if (isGetEscapeItem[1] == true && isEscapeItem[1] == false)
            {
                isEscapeItem[1] = true;
                PlayerHaveItem++;
            }
            if (isGetEscapeItem[0] == true && isGetEscapeItem[1] == true)
            {
                Goal.PlayerGoal();
            }
        }
        if (PlayerHaveItem <= 0)
        {
            Debug.Log("����ȏ�A�C�e�������Ă܂���");
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
            PlayerHaveItem++;
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
}
