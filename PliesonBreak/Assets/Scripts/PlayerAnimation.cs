using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;
using UnityEngine.Animations;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAnimation : MonoBehaviourPunCallbacks
{

    [SerializeField,Tooltip("アニメーションさせるアニメーター")] Animator SpriteAnimator;
    [SerializeField,Tooltip("アニメーションのステータス(関数で変更)")] AnimCode AnimState;
    [SerializeField,Tooltip("現在アニメーションのさせるかどうか(関数で変更)")] bool isAnim;
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
    /// アニメーションを変更する
    /// </summary>
    /// <param name="anim"></param>
    public void SetAnim(AnimCode anim)
    {
        if (isAnim) AnimState = anim;
    }

    /// <summary>
    /// Triggerが使えないため、boolのオンオフで代用
    /// trueにしたあと1フレーム後にfalseにする
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
