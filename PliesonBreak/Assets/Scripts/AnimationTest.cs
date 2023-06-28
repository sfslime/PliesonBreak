using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using Photon.Pun;

public class AnimationTest : MonoBehaviour
{
    [SerializeField, Tooltip("�A�j���[�V����������A�j���[�^�[")] Animator SpriteAnimator;

    PlayerBase player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        SpriteChange((PlayerColors)PhotonNetwork.LocalPlayer.GetPlayerColorStatus());
        StartCoroutine(SetBoolTrigger(player.GetAnimState()));

    }

    void SpriteChange(PlayerColors color)
    {
        for(int cnt =0;cnt < transform.childCount; cnt++)
        {
            var sprite = transform.GetChild(cnt).gameObject;

            if (cnt == (int)color)
            {
                sprite.SetActive(true);
                SpriteAnimator = sprite.GetComponent<Animator>();
            }
            else sprite.SetActive(false);

            
        }
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
