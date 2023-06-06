using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ConstList;

/*
�S�̂̐i�s���Ǘ�����}�l�[�W���[
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

    [System.Serializable]
    class ReleaseEffectSetting
    {
        [Range(1,5)] public float ActiveTime;
        public float FontSize;
        public AudioClip EffectSE;
        public float SEvolume;
        public GameObject EffectPanel;
    }

    [System.Serializable]
    class EscapeSetting
    {
        public bool isGoal;
        public List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    }

    #endregion

    #region �ϐ��錾

    #region �Q�[���i�s�ϐ�

    [SerializeField, Tooltip("���݂̃Q�[���̏��")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("�v���C���[�N���X(test�ŃC���X�y�N�^�[����)")] GameObject Player;

    [SerializeField, Tooltip("�Q�[���̐i�s��ԁi�G���A�̉����ԁj")] int ReleaseErea;

    [SerializeField, Header("�G���A������ݒ�"), Tooltip("�E�o�A�C�e���E�\�����ԁE���ʁESE�Ȃǂ̐ݒ�")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField, Header("�S�[�������֌W"), Tooltip("�K�v�ȃA�C�e���E�I�����ɌĂ΂��֐��Ȃ�")] EscapeSetting EscapeSettings;

    [SerializeField, Header("�A�C�e���֌W"), Tooltip("�A�C�e���ύX�p�摜")] List<Sprite> InteractSprits = new List<Sprite>();

    [SerializeField, Tooltip("�A�C�e���o���p�v���t�@�u")] List<GameObject> interactObjectPrefabs = new List<GameObject>();

    #endregion

    #region �}�l�[�W���[�ϐ�

    //�Q�[���}�l�[�W���[�̃C���X�^���X
    public static GameManager GameManagerInstance;

    //�}�b�v���Ǘ�����}�l�[�W���[
    //private MapManager MapManager;

    //SE���Ǘ�����}�l�[�W���[
    private AudioManager AudioManager;

    #endregion

    #endregion

    #region �֐�

    bool Init()
    {
        GameManagerInstance = this;

        AudioManager = GetComponent<AudioManager>();
        if(AudioManager == null)
        {
            Debug.Log("AudioManager not found");
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
        //�v���C���[�̎��̐錾
        //Player = �v���C���[�̒T��

        //�e�}�l�[�W���[�̋N���A�G���[�`�F�b�N
        //MapManager = �}�b�v�}�l�[�W���[�̌���
        //if(MapManager == null)
        //{
        //    Debug.Log("MapManager Not Find");
        //}else{
        //�}�b�v�̐���
        //�v���C���[�̃��X�|�[���ʒu�̎擾
        //}

        //�e�X�g
        GameStatus = GAMESTATUS.INGAME;
        ReleaseErea = 0;

        Debug.Log("Start OK");
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

    #endregion

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
        //�e�X�g
        GameStart();
    }

    
    void Update()
    {
        
    }
}
