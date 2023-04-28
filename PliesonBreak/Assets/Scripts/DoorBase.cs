using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : InteractObjectBase
{
    Collider2D Collider2D;

    void Start()
    {
        Collider2D = GetComponent<Collider2D>();
        NowInteract = InteractObjs.Door;
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
        if(isopendoor == true)
        {
            Collider2D.enabled = false;
            GetComponent<cDoorSpriteChange>().ChangeTile(isopendoor);
        }
        else
        {
            Collider2D.enabled = true;
        }
    }
}
