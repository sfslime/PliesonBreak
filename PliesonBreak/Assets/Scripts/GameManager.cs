using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

/*
�Q�[���̐i�s���Ǘ�����}�l�[�W���[
���ׂẴv���C���[�������A�Q�[���V�[���J�ڌォ��Ǘ�����
�e�}�l�[�W���[�Ȃǂ̏��������n�߁A�S���̏������I��莟��J�n����
�J�n��̓G���A����A�ߔ��A���~�@�̃��b�Z�[�W���󂯎��A�K�؂ɏ�������

��������
(�}�X�^�[)
1,�ڑ��m�F�A�ҋ@
2,�v���C���[����
3,�A�C�e�������A���M
4,�Ŏ琶���A���M
5,�����������A�ҋ@
6,�R�l�N�g�T�[�o�[�N���X�ɂ��Q�[���J�n�֐�

(�����o�[)
1,�ڑ��m�F�A�ҋ@
2,�v���C���[����
3,�A�C�e����M�ҋ@
4,�Ŏ��M�ҋ@
5,�����������A�ҋ@
6,�R�l�N�g�T�[�o�[�N���X�ɂ��Q�[���J�n�֐�

�쐬�ҁF��c
 */

public class GameManager : MonoBehaviour
{
    #region �񋓑�

    /// <summary>
    /// �Q�[���̌��݂̏�Ԃ�\���񋓑�
    /// </summary>
    enum GAMESTATUS
    {
        READY,   //�Q�[���J�n�O
        INGAME,  //�Q�[����
        ENDGAME, //�Q�[���I����
        COUNT    //���̗񋓑̂̐�
    }

    /// <summary>
    /// �Q�[���̐i�s���\���񋓑�
    /// </summary>
    enum GAMEFAZES
    {
        EXPLORE,  //�T�����B�ŏI�����O�܂ł̏��
        LAST,     //�ŏI����
        COUNT     //���̗񋓑̂̐�
    }

    #endregion

    #region �N���X���N���X

    //������Ɋւ���N���X
    [System.Serializable]
    class ReleaseEffectSetting
    {
        [Range(1,5)] public float ActiveTime;
        public float FontSize;
        public AudioClip EffectSE;
        public float SEvolume;
        public GameObject EffectPanel;
    }

    //�S�[���Ɋւ���N���X
    [System.Serializable]
    class EscapeSetting
    {
        public bool isGoal;
        public List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    }

    //�������p�Ǘ��N���X
    //�e�������̊�����Ԃ��Q�Ƃł���
    [System.Serializable]
    class InitList
    {
        public bool MapInit;
        public bool ConectInit;
    }

    #endregion

    #region �ϐ��錾

    #region �Q�[���i�s�ϐ�

    [SerializeField, Tooltip("���݂̃Q�[���̏��")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("�v���C���[�N���X(test�ŃC���X�y�N�^�[����)")] GameObject Player;

    [SerializeField, Tooltip("�Q�[���̐i�s��ԁi�G���A�̉����ԁj")] int ReleaseErea;

    [SerializeField, Tooltip("���b�Z�[�W�\��UI(�C���X�y�N�^�[����)")] Text MessageText;

    [SerializeField, Header("�G���A������ݒ�"), Tooltip("�E�o�A�C�e���E�\�����ԁE���ʁESE�Ȃǂ̐ݒ�")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField, Header("�S�[�������֌W"), Tooltip("�K�v�ȃA�C�e���E�I�����ɌĂ΂��֐��Ȃ�")] EscapeSetting EscapeSettings;

    [SerializeField, Header("�A�C�e���֌W"), Tooltip("�A�C�e���ύX�p�摜")] List<Sprite> InteractSprits = new List<Sprite>();

    [SerializeField, Tooltip("�A�C�e���o���p�v���t�@�u")] List<GameObject> interactObjectPrefabs = new List<GameObject>();

    private InitList InitLists = new InitList();

    #endregion

    #region �}�l�[�W���[�ϐ�

    //�Q�[���}�l�[�W���[�̃C���X�^���X
    public static GameManager GameManagerInstance;

    //�}�b�v���Ǘ�����}�l�[�W���[
    private MapManager MapManager;

    //SE���Ǘ�����}�l�[�W���[
    private AudioManager AudioManager;

    //�I�����C���ڑ��}�l�[�W���[
    private ConectServer ConectServer;

    #endregion

    #endregion

    #region �֐�

    /// <summary>
    /// �������֐�
    /// �߂�l�ŃG���[�`�F�b�N�ɂȂ�Afalse�̏ꍇ�͒����ɏI�����邱��
    /// </summary>
    /// <returns></returns>
    bool Init()
    {
        GameManagerInstance = this;

        InitLists.ConectInit = false;
        InitLists.MapInit = false;

        //�v���C���[�̒T��
        //Player = GameObject.Find("")
        //�v���C���[�𓮂��Ȃ����鏈��

        AudioManager = GetComponent<AudioManager>();
        if(AudioManager == null)
        {
            Debug.Log("AudioManager not found");
            return false;
        }

        MapManager = GetComponent<MapManager>();
        if(MapManager == null)
        {
            Debug.Log("MapManager not found");
            return false;
        }

        ConectServer = GetComponent<ConectServer>();

        return true;

    }



    /// <summary>
    /// �Q�[���N�����ɌĂ΂��֐�
    /// 
    /// </summary>
    public void GameStart()
    {
        //�e�X�g
        GameStatus = GAMESTATUS.INGAME;
        ReleaseErea = 0;

        //MessageText.transform.parent.gameObject.SetActive(false);

        Debug.Log("Start OK");
    }

    /// <summary>
    /// �e�평�������I����������`�F�b�N����
    /// ���݂̓e�X�g�̂��߁A�}�b�v�̐������s���Ă���
    /// </summary>
    public void InitCheck()
    {
        if (!InitLists.ConectInit)
        {
            MessageText.text = "�ڑ���...";
        }
        else
        {
            //������悤�ɂ���
            //Player.GetComponent<PlayerBase>().

            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                MessageText.text = "�Q���l�� : " + PhotonNetwork.PlayerList.Length + "\nSPACE�L�[�ŊJ�n";


                if (InitLists.MapInit)
                {
                    GameStart();
                }
                //�e�X�g
                if (Input.GetKeyDown(KeyCode.Space) && !InitLists.MapInit)
                {
                    Debug.Log("�}�X�^�[�N���C�A���g�m�F�B�A�C�e���̐����J�n");
                    StartCoroutine(WaitMapPop());

                    //�e�X�g
                    InitLists.MapInit = true;
                }
            }
            else
            {
                MessageText.text = "�Q���l�� : " + PhotonNetwork.PlayerList.Length + "\n�ҋ@��";

                //�}�X�^�[�N���C�A���g����A�C�e�������ʒu���󂯎��
                InitLists.MapInit = true;
            }

        }
    }

    /// <summary>
    /// �T�v�F���̃G���A�ɑ����h�A���J�������ɁA�h�A����Ă΂��֐�
    /// �@�@�@�J�����G���A�ԍ����e���[�U�[�̉�ʂɕ\��������
    /// �@�@�@���̎��AEreaReleaseEffect�R���[�`���ŉ��o���s��
    /// �����FEreaNm>�J�����G���A�̔ԍ����󂯎��
    /// </summary>
    /// <param name="EreaNm"></param>
    public void EreaRelease(int EreaNm)
    {
        //����ɋ󂢂Ă��邩�𔻒�
        if (ReleaseErea + 1 != EreaNm)
        {
            Debug.Log("EreaNm Error!"); 
            return;
        }

        ReleaseErea = EreaNm;
        StartCoroutine(EreaReleaseEffect(EreaNm));
    }

    public void PlaySE(SEid id,Vector2 pos)
    {
        AudioManager.SE(id, pos);
    }

    /// <summary>
    /// �ڑ����ɃR�l�N�g�T�[�o�[����Ă΂��
    /// </summary>
    public void RoomJoined()
    {
        InitLists.ConectInit = true;
        ConectServer.PopPlayer();
    }

    /// <summary>
    /// �A�C�e���̉摜�ύX�p�Ɉ����ɉ����ăX�v���C�g��Ԃ�
    /// </summary>
    /// <param name="ObjID"></param>
    /// <returns></returns>
    public Sprite ReturnSprite(InteractObjs ObjID)
    {
        return InteractSprits[(int)ObjID];
    }

    /// <summary>
    /// �A�C�e���o���p�Ɉ����ɉ����ăQ�[���I�u�W�F�N�g��Ԃ�
    /// </summary>
    /// <param name="ObjectID"></param>
    /// <returns></returns>
    public GameObject GetObjectPrefab(InteractObjs ObjectID)
    {
        return interactObjectPrefabs[(int)ObjectID];
    }

    public GameObject GetPlayer()
    {
        return Player;
    }

    public List<InteractObjs> GetNeedItemList()
    {
        return EscapeSettings.NeedEscapeList;
    }


    #endregion

    #region �R���[�`��

    /// <summary>
    /// �G���A������̉��o�R���[�`��
    /// ��莞�Ԍ�ɉ��o�I���������s��
    /// </summary>
    /// <param name="EreaNm"></param>
    /// <returns></returns>
    IEnumerator EreaReleaseEffect(int EreaNm)
    {
        Debug.Log(EreaNm + " Erea Release");
        //ReleaseEffectSettings.EffectPanel.SetActive(true);

        //SE��炷

        while (true)
        {
            //�����ł������ƕ\��
            break;
        }
        Debug.Log("Release Effect Wait");

        yield return new WaitForSeconds(ReleaseEffectSettings.ActiveTime);

        while (true)
        {
            //�������Ə�����
            break;
        }

        Debug.Log("Release Effect End");
        yield break;
    }

    /// <summary>
    /// �}�b�v�̐�����҂�
    /// �S�����ڑ���A�Q�[���V�[���Ɉڂ�����Ƀ}�X�^�[���Ă�
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitMapPop() 
    {

        MessageText.text = "�}�b�v������...(���[�f�B���O���)";
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(MapManager.StartPop());
        Debug.Log("Map OK");
        InitLists.MapInit = true;
        //�e�X�g
        MessageText.transform.parent.gameObject.SetActive(false);
        yield break;
    }

    #endregion

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
        //�e�X�g
    }

    
    void Update()
    {
        if(GameStatus == GAMESTATUS.READY)
        {
            InitCheck();
        }
    }
}
