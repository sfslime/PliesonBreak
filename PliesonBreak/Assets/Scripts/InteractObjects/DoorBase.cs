using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : InteractObjectBase
{
    Collider2D Collider2D;

    //飛田追加分
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
    /// 扉を開けるスクリプト.
    /// PlayerBase.csがこの関数を呼び出す.
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