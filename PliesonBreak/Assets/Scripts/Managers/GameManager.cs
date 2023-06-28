using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
6,�Q�[���J�n�֐�

(�����o�[)
1,�ڑ��m�F�A�ҋ@
2,�v���C���[����
3,�A�C�e����M�ҋ@
4,�Ŏ��M�ҋ@
5,�����������A�ҋ@
6,�Q�[���J�n�֐�

�쐬�ҁF��c
 */

[AddComponentMenu("Managers/GAME/GameManager",10)]

public class GameManager : MonoBehaviour
{

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

    //���������v���C���[�̌����ځi�I�����C���I�u�W�F�N�g�j
    private GameObject PlayerSprite;

    [SerializeField, Tooltip("�o��������v���C���[�̃v���t�@�u��")] string PlayerPrefabName;

    [SerializeField, Tooltip("�S���̏ꏊ")] GameObject PrisonPoint;

    [SerializeField, Tooltip("�Ŏ�̏o���ꏊ�̌����o����")] GameObject JailersRoot;

    [SerializeField, Tooltip("�Ŏ�̏����")] GameObject PatrolRoot;

    [SerializeField, Tooltip("�Q�[���̐i�s��ԁi�G���A�̉����ԁj")] int ReleaseErea;

    [SerializeField, Header("�G���A������ݒ�"), Tooltip("�E�o�A�C�e���E�\�����ԁE���ʁESE�Ȃǂ̐ݒ�")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField, Header("�S�[�������֌W"), Tooltip("�K�v�ȃA�C�e���E�I�����ɌĂ΂��֐��Ȃ�")] EscapeSetting EscapeSettings;

    [SerializeField, Header("�A�C�e���֌W"), Tooltip("�A�C�e���ύX�p�摜")] List<Sprite> InteractSprits = new List<Sprite>();

    [SerializeField, Tooltip("�A�C�e���o���p�v���t�@�u")] List<GameObject> interactObjectPrefabs = new List<GameObject>();

    private InitList InitLists = new InitList();

    //���݂̓e�X�g�Ō��ʂ��L�^
    public static bool GameResult;

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

        //�`���[�g���A�����ŕ߂܂����������Z�b�g
        PhotonNetwork.LocalPlayer.SetArrestStatus(false);

        AudioManager = GameObject.Find("AudioManager").gameObject.GetComponent<AudioManager>();
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

        BGMManager.Instance.SetBGM(BGMid.DEFALTGAME);

        Debug.Log("Start OK");
    }

    /// <summary>
    /// �e�평�������I����������`�F�b�N����
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
       AudioManager.PlaySE(id, pos);
    }

    /// <summary>
    /// �Ŏ�𐶐�����
    /// </summary>
    void PopJailer()
    {
        for(int i=0;i< JailersRoot.transform.childCount;i++)
        {
            var obj = PhotonNetwork.Instantiate("Jailer", JailersRoot.transform.GetChild(i).transform.position, Quaternion.identity);
            for(int j = 0; j < PatrolRoot.transform.GetChild(i).childCount; j++)
            {
                obj.GetComponent<JailerLink>().AddPatrolPoint(PatrolRoot.transform.GetChild(i).GetChild(j).position);
            }
            
        }
    }

    /// <summary>
    /// �v���C���[��߂܂����Ƃ��ɊŎ炩��Ă΂��
    /// ���[�J���v���C���[�ł���΃v���C���[�̏ꏊ���ʂɉ��o���N�����A
    /// ���v���C���[�ł���Ή�ʂɃ��b�Z�[�W��\������
    /// </summary>
    /// <param name="player"></param>
    public void ArrestPlayer(GameObject player)
    {
        StartCoroutine(ArrestEffect(player));
    }

    /// <summary>
    /// �S����������ꂽ�Ƃ��Ƀ��b�Z�[�W��\������
    /// �S������Ă΂��
    /// </summary>
    public void ReleasePrison()
    {
        //������b�Z�[�W

        //�e�X�g�őS�������
        foreach(var player in PhotonNetwork.PlayerList)
        {
            player.SetArrestStatus(false);
        }
    }

    /// <summary>
    /// �ߔ���Ԃ���������
    /// �S���̍�ɐG�ꂽ�Ƃ��ɌĂ΂�A�J�E���g�������~����
    /// </summary>
    /// <param name="player"></param>
    public void ReleasePlayer(GameObject player)
    {
        if (PlayerSprite.GetComponent<PlayerLink>().GetOrigin() == player)
        {
            if (PhotonNetwork.LocalPlayer.GetArrestStatus())
            {
                PhotonNetwork.LocalPlayer.SetArrestStatus(false);
            }
        }
    }

    /// <summary>
    /// �Q�[���J�n�O�ɌĂԑҋ@�֐�
    /// UpDate���ŌĂ΂��
    /// </summary>
    void Ready()
    {
        if (InitCheck())
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GameStart();
            //    Debug.Log("GameStart");
            //}
            GameStart();
            Debug.Log("GameStart");
        }
    }

    /// <summary>
    /// ���[�J���v���C���[�̃J�E���g������������
    /// ���v���C���[�͂��ꂼ��̃Q�[���}�l�[�W���[�Ō���������
    /// ���̌��ʂ�\������
    /// </summary>
    void ArrestCountUpDate()
    {
        if (PhotonNetwork.LocalPlayer.GetArrestStatus())
        {
            PhotonNetwork.LocalPlayer.SetArrestCntStatus(PhotonNetwork.LocalPlayer.GetArrestCntStatus() - Time.deltaTime);
        }

        //�S�����܂��Ă��邩���肷��
        bool isAllArrest = true;
        foreach(var player in PhotonNetwork.PlayerList)
        {
            if(player == PhotonNetwork.LocalPlayer)
            {
                if (player.GetArrestStatus())
                {
                    //���g�̃J�E���g��\��
                    player.GetArrestCntStatus();
                }
                else
                {
                    //�ʏ�摜��\��

                    isAllArrest = false;
                }
            }
            else
            {
                if (player.GetArrestStatus())
                {
                    //���v���C���[�̃J�E���g��\��
                    player.GetArrestCntStatus();
                }
                else
                {
                    //�ʏ�摜��\��
                    isAllArrest = false;
                }
            }
        }
        if (isAllArrest)
        {
            GameOver();
        }
    }

    /// <summary>
    /// �S�[���ɂ��ׂẴA�C�e�������������ɃS�[������Ă΂��
    /// �`���[�g���A�������ǂ����ɂ���ď�����h��������
    /// </summary>
    public void GameClear()
    {
        if (EscapeSettings.isGoal) return;
        EscapeSettings.isGoal = true;
        if(SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString())
        {
            TutorialManager.Instance.TutorialTrriger(true);
            return;
        }
        GameResult = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SceanNames.ENDGAME.ToString());
        }
    }

    /// <summary>
    /// �S�����߂܂������ɌĂ΂��
    /// �Q�[����Ԃ��I���ɃZ�b�g���A�s�k��ԂŎ��̃V�[���Ɉڂ�
    /// </summary>
    void GameOver()
    {
        if (SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString() || GameStatus == GAMESTATUS.ENDGAME_LOSE) return;

        GameStatus = GAMESTATUS.ENDGAME_LOSE;
        GameResult = false;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SceanNames.ENDGAME.ToString());
        }
    }

    /// <summary>
    /// �N�����ɘS������Ă΂�A�߂܂����ۂɂ����ɓ]�������
    /// </summary>
    /// <param name="point"></param>
    public void Setprisonpoint(GameObject point)
    {
        PrisonPoint = point;
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

    /// <summary>
    /// �v���C���[�I�u�W�F�N�g��Ԃ�
    /// �Q�[���I�u�W�F�N�g�Ȃ̂ŕK�v�ɉ�����GetComponent����
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return Player;
    }

    /// <summary>
    /// �S�[���ɕK�v�ȃA�C�e������Ԃ�
    /// ��ɃS�[������Ă΂��
    /// </summary>
    /// <returns></returns>
    public List<InteractObjs> GetNeedItemList()
    {
        return EscapeSettings.NeedEscapeList;
    }

    /// <summary>
    /// ���݂̃Q�[����Ԃ�Ԃ�
    /// </summary>
    /// <returns></returns>
    public GAMESTATUS GetGameStatus()
    {
        return GameStatus;
    }


    #endregion

    #region �R���[�`��

    /// <summary>
    /// �߂܂������̉��o�������Ԃɏ�������
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator ArrestEffect(GameObject player)
    {
        if (PlayerSprite.GetComponent<PlayerLink>().GetOrigin() == player)
        {
            //�߂܂艉�o(���̏�ɃG�t�F�N�g�ASE�A��ʂɃ��b�Z�[�W�A�A�j���[�V����)

            player.transform.position = PrisonPoint.transform.position;

            if (SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString())
            {
                yield break;
            }

            //�߂܂菈���i�J�E���g�J�n��߂܂��ԑ��M�j
            PhotonNetwork.LocalPlayer.SetArrestStatus(true);
        }
        else
        {
            //�߂܂艉�o(��ʂɃ��b�Z�[�W)
        }

        yield break;
    }

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
            bool isReady = true;
            //���ׂẴv���C���[�ɑ΂��Ċm�F
            foreach(var player in PhotonNetwork.PlayerListOthers)
            {
                //���������łȂ��Ȃ�ύX
                if(player.GetGameStatus() != (int)GAMESTATUS.READY)
                {
                    isReady = false;
                }
            }

            //���ׂẴv���C���[�����������Ȃ�i��
            if (isReady) break;
            //�t���[�Y�����1�t���[���ҋ@
            yield return null;
        }

        InitLists.ConectInit = true;
        Debug.Log("���ׂẴv���C���[�̐ڑ��m�F");

        //2,�v���C���[����
        var Link = PhotonNetwork.Instantiate(PlayerPrefabName, Player.transform.position, Quaternion.identity);
        Link.GetComponent<PlayerLink>().SetOrigin(Player);
        Link.GetComponent<Collider2D>().enabled = false;
        PlayerSprite = Link;

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
            InitLists.MapInit = true;
            Debug.Log("Map Ok");
        }

        //5,�������ҋ@

        //���g�̏����������𑗐M
        PhotonNetwork.LocalPlayer.SetInitStatus(true);
        while (true)
        {
            //��Ԋm�F�p
            bool isReady = true;
            //���ׂẴv���C���[�ɑ΂��Ċm�F
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                //���������łȂ��Ȃ�ύX
                if (player.GetInitStatus() != true)
                {
                    isReady = false;
                }
            }

            //���ׂẴv���C���[�����������Ȃ�i��
            if (isReady) break;
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
        }else if(SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString())
        {
            //�`���[�g���A���̗�O����
            var Link = PhotonNetwork.Instantiate(PlayerPrefabName, Player.transform.position, Quaternion.identity);
            Link.GetComponent<PlayerLink>().SetOrigin(Player);
            Link.GetComponent<Collider2D>().enabled = false;
            PlayerSprite = Link;
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        switch (GameStatus)
        {
            case GAMESTATUS.READY:
                Ready();
                break;

            case GAMESTATUS.INGAME:
                ArrestCountUpDate();
                break;

            default:
                break;
        }
    }
}
