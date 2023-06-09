using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class BearTrap : InteractObjectBase
{
    GameObject RestraintObj;  // �S�����Ă���I�u�W�F�N�g.

    [SerializeField] bool isOpen;              // �g���o�T�~���J���Ă��邩�ǂ���.
    [SerializeField] bool isRestraint;         // �S�����Ă��邩�ǂ���.

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
    /// �w�肵���b�������Ȃ����鏈��.
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
    /// �I�u�W�F�N�g�𓮂��Ȃ����鏈��.
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
    /// �g���o�T�~���J���邽�߂̏���.
    /// �v���C���[�����̊֐����Ă�.
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
