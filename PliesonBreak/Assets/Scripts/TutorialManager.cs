using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using ConstList;

public class TutorialManager : MonoBehaviour
{
    #region �`���[�g���A���w����`

    List<string> TutorialTexts = new List<string>() { "�����J���悤","����T����","�S�����J���悤","�T�������悤","������낤","�����J���悤","��֐i����","������Ȃ��悤�ɐi����","�E�o�̃p�[�c��T����>�c��2��","�C�����悤", "�E�o�̃p�[�c��T����>�c��1��","�E�o���I" };

    List<string> TutorialMessage = new List<string>() { "��������E�o����ׂ���", "�����K�v��...", "���̘S���A�O����ȊJ�������", "�������ɂȂɂ����肻����", "���̔��Ɠ����F�B���ꂾ��", "����}����", "�N�������...", "����������I��肾��", "���̑D�ɂ̓p�[�c������Ȃ�", "��������ĂȂ��B��x�����Ă�����", "����ōŌゾ��", "�ǂ���O�ɓ����悤" };

    [SerializeField, Tooltip("���̈ʒu")] List<GameObject> TutorialArrow = new List<GameObject>();
    #endregion

    [SerializeField,Tooltip("���݂̃`���[�g���A���i�K")] int Faze;

    [SerializeField, Tooltip("�ړI�\���e�L�X�g�̕\����")] Text TutorialTextObject;
    [SerializeField, Tooltip("�����o���̕\����")] Text TutorialMessageObject;
    // Start is called before the first frame update
    void Start()
    {
        Faze = -1;

        foreach(var Arrow in TutorialArrow)
        {
            Arrow.SetActive(false);
        }

        TutorialTrriger(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �v���C���[���`���[�g���A���g���K�[�𓥂񂾂Ƃ�
    /// �`���[�g���A����i�s������
    /// </summary>
    /// <param name="Trrigerfaze"></param>
    public void TutorialTrriger(int Trrigerfaze)
    {
        if(Faze + 1 == Trrigerfaze)
        {
            Faze++;
            TutorialMessageObject.text = TutorialTexts[Faze];
            TutorialMessageObject.text = TutorialMessage[Faze];
            TutorialArrow[Faze].SetActive(true);
        }

        if(Faze == TutorialTexts.Count-1)
        {

        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            //��ʂ��Â��Ȃ鉉�o
            break;
        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceanNames.TITLE.ToString());
    }

}