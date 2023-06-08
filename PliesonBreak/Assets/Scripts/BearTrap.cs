using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : InteractObjectBase
{
    GameObject RestraintObj;  // �S�����Ă���I�u�W�F�N�g.

    bool isRestraint;         // �S�����Ă��邩�ǂ���.

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
    /// �w�肵���b�������Ȃ����鏈��.
    /// </summary>
    IEnumerator RestraintTime(float time)
    {
        isRestraint = true;
         yield return new WaitForSeconds(time);
        isRestraint = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// �I�u�W�F�N�g�𓮂��Ȃ����鏈��.
    /// </summary>
    void Restraint()
    {
        if(isRestraint == true)
        {
            RestraintObj.transform.position = transform.position;
        }
    }
}
