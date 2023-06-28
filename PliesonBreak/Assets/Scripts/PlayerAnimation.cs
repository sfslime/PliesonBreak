using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using UnityEngine.Animations;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAnimation : MonoBehaviourPunCallbacks
{

    [SerializeField,Tooltip("�A�j���[�V����������A�j���[�^�[")] Animator SpriteAnimator;
    [SerializeField,Tooltip("�A�j���[�V�����̃X�e�[�^�X(�֐��ŕύX)")] AnimCode AnimState;
    [SerializeField,Tooltip("���݃A�j���[�V�����̂����邩�ǂ���(�֐��ŕύX)")] bool isAnim;
    PlayerBase Player;

    // Start is called before the first frame update
    void Start()
    {
        var sprite = transform.GetChild(photonView.Owner.GetPlayerColorStatus()).gameObject;
        sprite.SetActive(true);
        SpriteAnimator = sprite.GetComponent<Animator>();

        if (!photonView.IsMine) return;

        var gamemanager = GameManager.GameManagerInstance;
        if (gamemanager == null) return;

        Player = gamemanager.GetPlayer().GetComponent<PlayerBase>();

        isAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (isAnim)
        {
            SetAnim(Player.GetAnimState());
            StartCoroutine(SetBoolTrigger(AnimState));
            //switch (AnimState)
            //{
            //    case AnimCode.Idel:
            //        StartCoroutine(SetBoolTrigger(AnimCode.Idel));
            //        break;
            //    case AnimCode.Walk:
            //        StartCoroutine(SetBoolTrigger(AnimCode.Walk));
            //        break;
            //    case AnimCode.Run:
            //        StartCoroutine(SetBoolTrigger(AnimCode.Run));
            //        break;
            //    default:
            //        break;
            //}
        }
    }

    /// <summary>
    /// �A�j���[�V������ύX����
    /// </summary>
    /// <param name="anim"></param>
    public void SetAnim(AnimCode anim)
    {
        if (isAnim) AnimState = anim;
    }

    /// <summary>
    /// Trigger���g���Ȃ����߁Abool�̃I���I�t�ő�p
    /// true�ɂ�������1�t���[�����false�ɂ���
    /// </summary>
    /// <param name="anim"></param>
    /// <returns></returns>
    IEnumerator SetBoolTrigger(AnimCode anim)
    {
        SpriteAnimator.SetBool(anim.ToString(), true);
        yield return null;
        SpriteAnimator.SetBool(anim.ToString(), false);
        yield break;
    }
}
