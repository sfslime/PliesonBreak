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

    [SerializeField, Tooltip("���̈ʒu�̐e")] GameObject ArrowRoot;
    [SerializeField, Tooltip("���̃`���[�g���A���g���K�[�̐e")] GameObject TrrigerPointRoot;
    #endregion

    [SerializeField,Tooltip("���݂̃`���[�g���A���i�K")] int Faze;

    [SerializeField, Tooltip("�ړI�\���e�L�X�g�̕\����")] Text TutorialTextObject;
    [SerializeField, Tooltip("�����o���̕\����")] Text TutorialMessageObject;

    [SerializeField, Tooltip("�I�����̃}�X�N")] Image EndMask;
    [SerializeField, Tooltip("�^���ÂɂȂ�܂ł̃��[�g"),Range(0.01f,0.2f)] float MaskRate;

    public static TutorialManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Faze = -1;

        for(int i = 0; i < ArrowRoot.transform.childCount; i++)
        {
            ArrowRoot.transform.GetChild(i).gameObject.SetActive(false);
            TrrigerPointRoot.transform.GetChild(i).gameObject.SetActive(false);
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
            TutorialTextObject.text = TutorialTexts[Faze];
            TutorialMessageObject.text = TutorialMessage[Faze];
            ArrowRoot.transform.GetChild(Faze).gameObject.SetActive(true);
            TrrigerPointRoot.transform.GetChild(Faze).gameObject.SetActive(true);
            if(Faze != 0)
            {
                ArrowRoot.transform.GetChild(Faze-1).gameObject.SetActive(true);
                TrrigerPointRoot.transform.GetChild(Faze-1).gameObject.SetActive(true);
            }
        }

        if(Faze == TutorialTexts.Count-1)
        {
            StartCoroutine(EndTutorial());
        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(3f);

        float ClearLance = 0;
        while (true)
        {
            //��ʂ��Â��Ȃ鉉�o
            if(EndMask.color.a >= 0.8f)
            {
                break;
            }
            ClearLance += MaskRate;
            EndMask.color = new Color32(1, 1, 1, (byte)ClearLance);
            yield return null;
        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceanNames.LOBBY.ToString());
    }

}
