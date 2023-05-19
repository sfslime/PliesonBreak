using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    #endregion

    #region �ϐ��錾

    #region �Q�[���i�s�ϐ�

    [SerializeField, Tooltip("���݂̃Q�[���̏��")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("�v���C���[�N���X")] PlayerBase Player;

    [SerializeField, Tooltip("�Q�[���̐i�s��ԁi�G���A�̉����ԁj")] int ReleaseErea;

    [SerializeField, Header("�G���A������ݒ�"), Tooltip("�\�����ԁE���ʁESE�Ȃǂ̐ݒ�")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField,Tooltip("�A�C�e���ύX�p�摜")] List<Sprite> InteractSprits = new List<Sprite>();

    #endregion

    #region �}�l�[�W���[�ϐ�

    //�Q�[���}�l�[�W���[�̃C���X�^���X
    public static GameManager GameManagerInstance;

    //�}�b�v���Ǘ�����}�l�[�W���[
    //private MapManager MapManager;

    #endregion

    #endregion

    #region �֐�

    bool Init()
    {
        GameManagerInstance = this;

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

    public Sprite ReturnSprite(InteractObjectBase.InteractObjs ObjID)
    {
        return InteractSprits[(int)ObjID];
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
        //Init();
        //�e�X�g
        GameStart();
    }

    
    void Update()
    {
        
    }
}
