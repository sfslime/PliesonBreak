using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region �ϐ��錾

    #region �Q�[���i�s�ϐ�

    [SerializeField, Tooltip("���݂̃Q�[���̏��")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("�v���C���[�N���X")] Player Player;

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

        Debug.Log("Start Ok");
    }


    #endregion

    void Start()
    {
        Init();
        //�e�X�g
        GameStart();
    }

    
    void Update()
    {
        
    }
}
