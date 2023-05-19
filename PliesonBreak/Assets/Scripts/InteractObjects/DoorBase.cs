using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : InteractObjectBase
{
    Collider2D Collider2D;

    //��c�ǉ���
    private DoorLink DoorLink;
    private bool isOpen;

    void Start()
    {
        SetUp();
        Collider2D = GetComponent<Collider2D>();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �����J����X�N���v�g.
    /// PlayerBase.cs�����̊֐����Ăяo��.
    /// </summary>
    public void DoorOpen(bool isopendoor)
    {
        if (isopendoor == isOpen) return;
        if(isopendoor == true)
        {
            Collider2D.enabled = false;
            GetComponent<cDoorSpriteChange>().ChangeTile(isopendoor);
        }
        else
        {
            Collider2D.enabled = true;
        }

        isOpen = isopendoor;
        DoorLink.DoorStateLink(isOpen);
    }
}