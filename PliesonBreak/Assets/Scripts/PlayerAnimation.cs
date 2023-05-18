using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using UnityEngine.Animations;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField,Tooltip("�A�j���[�V����������A�j���[�^�[(Set�K�{)")] Animator SpriteAnimator;
    [SerializeField,Tooltip("�A�j���[�V�����̃X�e�[�^�X(�֐��ŕύX)")] AnimCode AnimState;
    [SerializeField,Tooltip("���݃A�j���[�V�����̂����邩�ǂ���(�֐��ŕύX)")] bool isAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnim)
        {
            switch (AnimState)
            {
                case AnimCode.Idel:
                    StartCoroutine(SetBoolTrigger(AnimCode.Idel));
                    break;
                case AnimCode.Walk:
                    StartCoroutine(SetBoolTrigger(AnimCode.Walk));
                    break;
                case AnimCode.Run:
                    StartCoroutine(SetBoolTrigger(AnimCode.Run));
                    break;
                default:
                    break;
            }
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
