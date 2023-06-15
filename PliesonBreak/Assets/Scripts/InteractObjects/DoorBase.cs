using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class DoorBase : InteractObjectBase
{
    Collider2D Collider2D;

    public InteractObjs NeedKeyID;

    GameObject Padlock;   // �싞���q�I�u�W�F�N�g.

    //��c�ǉ���
    private DoorLink DoorLink;
    [SerializeField] bool isOpen;

    void Start()
    {
        SetUp();
        Collider2D = GetComponent<Collider2D>();
        isOpen = false;
        DoorLink = GetComponent<DoorLink>();
        Padlock = transform.GetChild(0).gameObject;  // �����̎q�I�u�W�F�N�g�̎擾.
    }

    // Update is called once per frame
    void Update()
    {
        PadlockImage();
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
            Padlock.SetActive(false);
        }
        else
        {
            Collider2D.enabled = true;
        }

        isOpen = isopendoor;
        DoorLink.DoorStateLink(isOpen);
    }

    /// <summary>
    /// �싞���̉摜�\��..
    /// </summary>
    void PadlockImage()
    {
        Renderer PablockColor = Padlock.GetComponent<Renderer>();

        switch (NeedKeyID)
        {
            case InteractObjs.Key1:
                PablockColor.material.color = new Color32(255, 240, 0, 255);
                break;

            case InteractObjs.Key2:
                PablockColor.material.color = new Color32(190, 190, 190, 255);
                break;

            case InteractObjs.Key3:
                PablockColor.material.color = new Color32(160, 65, 40, 255);
                break;
        }
    }
}