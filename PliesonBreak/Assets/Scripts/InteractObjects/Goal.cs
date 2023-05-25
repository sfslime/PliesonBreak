using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstList;

public class Goal : InteractObjectBase
{
    [SerializeField,Tooltip("セットした脱出アイテムのリスト")]
    Dictionary<InteractObjs, bool> EscapeItemList = new Dictionary<InteractObjs, bool>();
    [SerializeField,Tooltip("脱出に必要なアイテムのリスト")]
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
    /// ゴールしたときの処理.
    /// </summary>
    public void PlayerGoal()
    {
        Debug.Log("ゴールしました！！");
    }

    /// <summary>
    /// 脱出アイテムのセット
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public bool SetEscapeItem(InteractObjs Item)
    {
        foreach(var needitem in NeedEscapeList)
        {
            if (needitem != Item) continue;
            //既に入手済みの場合
            if (EscapeItemList[Item]) return false;

            //脱出アイテムをセット
            EscapeItemList[Item] = true;
            GoalLink.StateLink(Item);
            return true;
        }

        //不要なアイテムの場合
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
