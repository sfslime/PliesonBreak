using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

public class PlayerBase : MonoBehaviour
{
    #region �ϐ�.
    Rigidbody2D Rb;

    [SerializeField] float Speed;        // �ړ��̑���.
    [SerializeField] float SaveSpeed;    // �ړ��X�s�[�h��ۑ�����ϐ�.

    public InputAction InputAction;      // ����ɂǂ��������L�[�����蓖�Ă邩�����߂邽�߂̃N���X.
    public UIManagerBase UIManager;      // UI���Ǘ�����}�l�[�W���[.
    
    [SerializeField] DoorBase Door;
    [SerializeField] Prison Prison;
    [SerializeField] InteractObjectBase InteractObjectBase;
    [SerializeField] SearchPoint SearchPoint;
    [SerializeField] Goal Goal;
    [SerializeField] BearTrap BearTrap;
    public KeysLink KeysLink;
    public int ObjID;                         // ���ݏd�Ȃ��Ă���I�u�W�F�N�g�̏����擾.
    public int HaveId;                        // ���ݎ����Ă���A�C�e����ID.
    [SerializeField] bool isPlayerHaveItem;   // �v���C���[����x�Ɏ��Ă�A�C�e���̌�.
    [SerializeField] bool isSearch;           // �C���^���N�g���Ă��邩�ǂ���
    public bool isMove;                       // �����邩�ǂ���.

    [SerializeField] List<bool> isEscapeItem; // �E�o�A�C�e���������Ă���Ƃ��ɒE�o�I�u�W�F�N�g�ɐG�ꂽ��true��Ԃ�.

    [SerializeField, Header("�A�j���[�V�����p�ϐ�"), Tooltip("���݂̃A�j���[�V�������")] AnimCode AnimState;

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
        SaveSpeed = Speed;
        isMove = false;
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
        // �G�ꂽ�C���^���N�g�I�u�W�F�N�g��Component�̎擾.
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
            }
            // Debug.Log("ObjID" +ObjID);
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
        if (isSearch == false && isMove == false)
        {
            var MoveVector = InputAction.ReadValue<Vector2>();

            Speed = SaveSpeed;
            Rb.velocity = new Vector3(MoveVector.x * Speed, MoveVector.y * Speed, 0) * Time.deltaTime;
        }
        if(isSearch == true && isMove == true)
        {
            Speed = 0;
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
        // �E���Ȃ��A�C�e���̔r��.
        Debug.Log("ID>" + ObjID);
        if (ObjID != (int)InteractObjs.Search && 
            ObjID != (int)InteractObjs.Door &&
            ObjID != (int)InteractObjs.Prison &&
            ObjID != (int)InteractObjs.EscapeObj &&
            ObjID != (int)InteractObjs.OpenBearTrap &&
            ObjID != (int)InteractObjs.CloseBearTrap)
        {
            InteractObjectBase.RequestSprite();
            HaveId = ObjID;
        }
        // �I�u�W�F�N�g��j��.
        if ((ObjID == (int)InteractObjs.Key1 ||
             ObjID == (int)InteractObjs.Key2 ||
             ObjID == (int)InteractObjs.Key3 ||
             ObjID == (int)InteractObjs.EscapeItem1 ||
             ObjID == (int)InteractObjs.EscapeItem2) &&
             isPlayerHaveItem == false)
        {
            KeysLink.StateLink(false);
        }

        // �G��Ă���I�u�W�F�N�g�ɑ΂��ăC���^���N�g�{�^�������������ɍs������.
        switch (ObjID)
        {
            case (int)InteractObjs.Search:
                StartCoroutine("Search");
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
        if (HaveId == (int)Door.NeedKeyID)
        {
            Debug.Log("�h�A���J����");
            Door.DoorOpen(true);
            isPlayerHaveItem = false;
            HaveId = (int)InteractObjs.None;
        }
        else
        {
            Debug.Log("�����������Ă���");
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
            if(ObjID != (int)InteractObjs.OpenBearTrap) UIManager.IsInteractButton(true);
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
    /// �C���^���N�g����̂Ɋ|���鎞��.
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
    /// �v���C���[�����łɃA�C�e���������Ă���Ƃ��ɃA�C�e����؂�ւ��鏈��.
    /// </summary>
    public void ChangeHaveItem(int olditem)
    {
        HaveId = olditem;
    }

    public AnimCode GetAnimState()
    {
        if (isSearch) return AnimCode.Search;

        if (Rb.velocity.magnitude > 0) return AnimCode.Run;

        return AnimCode.Idel;
    }
}

