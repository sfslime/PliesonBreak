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
        NONE,    //�Q�[���V�[���O�A�������̓Z�b�g����Ă��Ȃ�
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

    [SerializeField, Tooltip("�o��������v���C���[�̃v���t�@�u��")] string PlayerPrefabName;

    [SerializeField, Tooltip("�Q�[���̐i�s��ԁi�G���A�̉����ԁj")] int ReleaseErea;

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

        GameStatus = GAMESTATUS.READY;

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
    bool InitCheck()
    {
        if (InitLists.ConectInit && InitLists.MapInit) return true;
        return false;
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
    /// �Ŏ�𐶐�����
    /// ���쐬�̂��ߋ�
    /// </summary>
    void PopJailer()
    {

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
        yield return StartCoroutine(MapManager.StartPop());
        Debug.Log("Map OK");
        InitLists.MapInit = true;
        yield break;
    }

    /// <summary>
    /// �V�[���ړ���Ɋe�평�������s��
    /// ���ꂼ��̏�������ɑ��̃v���C���[�̑ҋ@���s��
    /// </summary>
    /// <returns></returns>
    IEnumerator InitGame()
    {
        //1,�ڑ��m�F���đ��v���C���[��҂�
        //�ڑ��܂őҋ@���Č��݂̏��i���������j��ݒ�
        while(!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LocalPlayer.SetGameStatus((int)GameStatus);
        //���ׂẴv���C���[�̏�Ԃ��m�F
        while (true)
        {
            //��Ԋm�F�p
            bool isNotReady = false;
            //���ׂẴv���C���[�ɑ΂��Ċm�F
            foreach(var player in PhotonNetwork.PlayerListOthers)
            {
                //���������łȂ��Ȃ�ύX
                if(player.GetGameStatus() != (int)GAMESTATUS.READY)
                {
                    isNotReady = true;
                }
            }

            //���ׂẴv���C���[�����������Ȃ�i��
            if (!isNotReady) break;
            //�t���[�Y�����1�t���[���ҋ@
            yield return null;
        }

        InitLists.ConectInit = true;
        Debug.Log("���ׂẴv���C���[�̐ڑ��m�F");

        //2,�v���C���[����
        var Link = PhotonNetwork.Instantiate(PlayerPrefabName, Player.transform.position, Quaternion.identity);
        Link.GetComponent<PlayerLink>().SetOrigin(Player);

        //3,(�}�X�^�[)�A�C�e�������A���M(�����o�[)�A�C�e����M�ҋ@
        if(PhotonNetwork.LocalPlayer.IsMasterClient == true)
        {
            yield return StartCoroutine(WaitMapPop());

            //���������A�C�e���ʒu��K�v�Ƃ���ꍇ�AMapManager����擾�����M
            //(���݂͕K�v�Ȃ����ߐ����̂�)

            //4,�Ŏ琶��
            PopJailer();
        }
        else
        {
            //�s�����Ƃ͂Ȃ��̂ŃX�L�b�v
        }

        //5,�������ҋ@

        //���g�̏����������𑗐M
        PhotonNetwork.LocalPlayer.SetInitStatus(true);
        while (true)
        {
            //��Ԋm�F�p
            bool isNotReady = false;
            //���ׂẴv���C���[�ɑ΂��Ċm�F
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                //���������łȂ��Ȃ�ύX
                if (player.GetInitStatus() == true)
                {
                    isNotReady = true;
                }
            }

            //���ׂẴv���C���[�����������Ȃ�i��
            if (!isNotReady) break;
            //�t���[�Y�����1�t���[���ҋ@
            yield return null;
        }

        Debug.Log("�S�v���C���[����������");
        yield break;
    }


    #endregion

    private void Awake()
    {
        if (Init())
        {
            StartCoroutine(InitGame());
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if(GameStatus == GAMESTATUS.READY)
        {
            if (InitCheck())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameStart();
                    Debug.Log("GameStart");
                }
            }
        }
    }
}
