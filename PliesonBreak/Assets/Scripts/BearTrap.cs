using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : InteractObjectBase
{
    GameObject RestraintObj;  // 拘束しているオブジェクト.

    bool isRestraint;         // 拘束しているかどうか.

    void Start()
    {
        
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
        isRestraint = true;
         yield return new WaitForSeconds(time);
        isRestraint = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// オブジェクトを動けなくする処理.
    /// </summary>
    void Restraint()
    {
        if(isRestraint == true)
        {
            RestraintObj.transform.position = transform.position;
        }
    }
}
