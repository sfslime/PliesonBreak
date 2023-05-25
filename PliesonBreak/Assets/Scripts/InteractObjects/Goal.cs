using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class Goal : InteractObjectBase
{
    [SerializeField,Tooltip("�Z�b�g�����E�o�A�C�e���̃��X�g")]
    Dictionary<InteractObjs, bool> EscapeItemList = new Dictionary<InteractObjs, bool>();
    [SerializeField,Tooltip("�E�o�ɕK�v�ȃA�C�e���̃��X�g")]
    List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    GoalLink GoalLink;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        SetNeedItemList();
        GoalLink = GetComponent<GoalLink>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGoal();
    }

    /// <summary>
    /// �S�[�������Ƃ��̏���.
    /// </summary>
    public void PlayerGoal()
    {
        Debug.Log("�S�[�����܂����I�I");
    }

    /// <summary>
    /// �E�o�A�C�e���̃Z�b�g
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public bool SetEscapeItem(InteractObjs Item)
    {
        foreach(var needitem in NeedEscapeList)
        {
            if (needitem != Item) continue;
            //���ɓ���ς݂̏ꍇ
            if (EscapeItemList[Item]) return false;

            //�E�o�A�C�e�����Z�b�g
            EscapeItemList[Item] = true;
            GoalLink.StateLink(Item);
            return true;
        }

        //�s�v�ȃA�C�e���̏ꍇ
        return false;
    }

    void CheckGoal()
    {
        if (NeedEscapeList.Count == 0) return;
        foreach(var checkitem in NeedEscapeList)
        {
            if (EscapeItemList[checkitem] == false) return;
        }

        PlayerGoal();
    }

    void SetNeedItemList()
    {
        NeedEscapeList = GameManager.GetNeedItemList();
        foreach(var item in NeedEscapeList)
        {
            EscapeItemList.Add(item, false);
        }
        
    }

    public void testSetItem(int obj)
    {
        SetEscapeItem((InteractObjs)obj);
    }
}
