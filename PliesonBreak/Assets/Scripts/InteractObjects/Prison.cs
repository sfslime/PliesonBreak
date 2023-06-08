using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
牢屋に付けるスクリプト

牢屋一つにつき一つ付ける
ここではドアの当たり判定の設定のみを行う
 */

public class Prison : InteractObjectBase
{
    //当たり判定管理
    Collider2D Collider2D;
    //牢屋状態を共有する
    PrisonLink PrisonLink;
    //現在開いているかどうか
    bool isOpen;
    //柵の上に立っているかどうか
    bool isPlayerStand;

    [SerializeField, Tooltip("捕縛後に転送される場所")] GameObject Prisonpoint;
    [SerializeField, Tooltip("閉まるまでのリミット")] float CloseTimeLimit;
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
    /// 牢屋を開ける
    /// プレイヤーから呼ばれ、開けたことをゲームマネージャーに送信する
    /// 開けた際に伴う処理はゲームマネージャーで行い、ここでは画像の変更と当たり判定の変更を行う
    /// </summary>
    /// <param name="isopened"></param>
    public void PrisonOpen(bool isopened)
    {
        //無限送信回避
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
    /// 開けた後、一定のカウントをしてから閉じる
    /// </summary>
    /// <returns></returns>
    IEnumerator PrisonCloseLimit()
    {
        float timer = 0;
        while (true)
        {
            //開いてる時にのみ呼ばれるはずなので閉まっているなら止める
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

    ////テスト
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    PrisonOpen(true);
    //}

    /// <summary>
    /// 開いている柵を通った時に解放処理を行う
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
    /// プレイヤーが去ったことを確認してから閉じる
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
