using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractObject
{
    Collider2D Collider2D;

    void Start()
    {
        Collider2D = GetComponent<Collider2D>();
        NowInteract = InteractObj.Door;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �����J����X�N���v�g.
    /// </summary>
    public void DoorOpen(bool isopendoor)
    {
        if(isopendoor == true)
        {
            Collider2D.enabled = false;
        }
        else
        {
            Collider2D.enabled = true;
        }
    }
}
