using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
�S���ɕt����X�N���v�g

�S����ɂ���t����
�����ł̓h�A�̓����蔻��̐ݒ�݂̂��s��
 */

public class Prison : InteractObjectBase
{
    //�����蔻��Ǘ�
    Collider2D Collider2D;
    //�S����Ԃ����L����
    PrisonLink PrisonLink;
    //���݊J���Ă��邩�ǂ���
    bool isOpen;
    //��̏�ɗ����Ă��邩�ǂ���
    bool isPlayerStand;

    [SerializeField, Tooltip("�ߔ���ɓ]�������ꏊ")] GameObject Prisonpoint;
    [SerializeField, Tooltip("�܂�܂ł̃��~�b�g")] float CloseTimeLimit;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        Collider2D = GetComponent<Collider2D>();
        PrisonLink = GetComponent<PrisonLink>();

        isOpen = false;
        isPlayerStand = false;

        GameManager.Setprisonpoint(Prisonpoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �S�����J����
    /// �v���C���[����Ă΂�A�J�������Ƃ��Q�[���}�l�[�W���[�ɑ��M����
    /// �J�����ۂɔ��������̓Q�[���}�l�[�W���[�ōs���A�����ł͉摜�̕ύX�Ɠ����蔻��̕ύX���s��
    /// </summary>
    /// <param name="isopened"></param>
    public void PrisonOpen(bool isopened)
    {
        //�������M���
        if (isOpen == isopened) return;
        isOpen = isopened;
        if (isopened)
        {
            Collider2D.isTrigger = true;
            GameManager.ReleasePrison();
            StartCoroutine(PrisonCloseLimit());
        }
        else
        {
            Collider2D.isTrigger = false;
        }
        GetComponent<cPrisonSpriteChange>().ChangeTile(isopened);
        PrisonLink.StateLink(isopened);
    }

    /// <summary>
    /// �J������A���̃J�E���g�����Ă������
    /// </summary>
    /// <returns></returns>
    IEnumerator PrisonCloseLimit()
    {
        float timer = 0;
        while (true)
        {
            //�J���Ă鎞�ɂ̂݌Ă΂��͂��Ȃ̂ŕ܂��Ă���Ȃ�~�߂�
            if (isOpen == false) yield break;
            timer += Time.deltaTime;
            if(timer > CloseTimeLimit)
            {
                if (!isPlayerStand)
                {
                    PrisonOpen(false);
                    yield break;
                }
            }
            yield return null;
        }
    }

    ////�e�X�g
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    PrisonOpen(true);
    //}

    /// <summary>
    /// �J���Ă�����ʂ������ɉ���������s��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(isOpen) GameManager.ReleasePlayer(collision.gameObject);
            isPlayerStand = true;
        }
    }

    /// <summary>
    /// �v���C���[�����������Ƃ��m�F���Ă������
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerStand = false;
        }
    }
}
