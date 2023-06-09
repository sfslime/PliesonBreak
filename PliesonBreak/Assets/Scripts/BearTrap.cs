using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class BearTrap : InteractObjectBase
{
    GameObject RestraintObj;  // 拘束しているオブジェクト.

    [SerializeField] bool isOpen;              // トラバサミが開いているかどうか.
    [SerializeField] bool isRestraint;         // 拘束しているかどうか.

    void Start()
    {
        SetUp();
        isOpen = true;
    }

    
    void Update()
    {
        Restraint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RestraintObj = collision.gameObject;

        switch (collision.gameObject.tag)
        {
            case "Player":
                PlayerBase = collision.gameObject.GetComponent<PlayerBase>();

                StartCoroutine(RestraintTime(3));
                break;

            case "Jailer":
                Jailer = collision.gameObject.GetComponent<Jailer>();
                StartCoroutine(RestraintTime(3));
                break;
        }
    }

    /// <summary>
    /// 指定した秒数動けなくする処理.
    /// </summary>
    IEnumerator RestraintTime(float time)
    {
        if(isOpen == true)
        {
            isRestraint = true;
            yield return new WaitForSeconds(time);
            isRestraint = false;
            isOpen = false;
            // Destroy(gameObject);
        }
    }

    /// <summary>
    /// オブジェクトを動けなくする処理.
    /// </summary>
    void Restraint()
    {
        if (isRestraint == true && isOpen == true)
        {
            RestraintObj.transform.position = transform.position;
        }

        if(isOpen == true)
        {
            SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.OpenBearTrap);
            NowInteract = InteractObjs.OpenBearTrap;
        }
        else if (isOpen == false)
        {
            NowInteract = InteractObjs.CloseBearTrap;
        }

        if (isOpen == false || isRestraint == true)
        {
            SpriteRenderer.sprite = GameManager.ReturnSprite(InteractObjs.CloseBearTrap);
        }
        
        
    }

    /// <summary>
    /// トラバサミを開けるための処理.
    /// プレイヤーがこの関数を呼ぶ.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator BearTrapOpen(float time)
    {
        if(isOpen == false)
        {
            yield return new WaitForSeconds(time);
            isOpen = true;
        }
    }
}
