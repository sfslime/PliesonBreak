using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConstList;

public class Goal : InteractObjectBase
{
    [SerializeField,Tooltip("セットした脱出アイテムのリスト")]
    Dictionary<InteractObjs, bool> EscapeItemList = new Dictionary<InteractObjs, bool>();
    [SerializeField,Tooltip("脱出に必要なアイテムのリスト")]
    List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    [SerializeField, Tooltip("目標アイテムの表示先の親")] 
    GameObject TargetItemImageRoot;
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
        TagetItemDisplay();
    }

    /// <summary>
    /// ゴールしたときの処理.
    /// </summary>
    public void PlayerGoal()
    {
        Debug.Log("ゴールしました！！");
        GameManager.GameClear();
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
            GameManager.PlaySE(SEid.EscapeItemSet, transform.position);
            return true;
        }

        //不要なアイテムの場合
        return false;
    }

    /// <summary>
    /// 既にセットされた脱出アイテムのリストを返す
    /// </summary>
    /// <returns></returns>
    public Dictionary<InteractObjs,bool> GetSetList()
    {
        return EscapeItemList;
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

    void TagetItemDisplay()
    {
        if((GameManager)GameManager.GetGameStatus() == GAMESTATUS.INGAME)
        {
            if(TargetItemImageRoot != null)
            {
                int cnt = 0;
                foreach(var item in EscapeItemList)
                {
                    if (item.Value)
                    {
                        ItemChecked(TargetItemImageRoot.transform.GetChild(cnt).gameObject);
                    }
                    cnt++;
                }
            }
        }
    }

    /// <summary>
    /// セット済みアイテムにチェックマークを付ける
    /// </summary>
    void ItemChecked(GameObject image)
    {
        var item = image.transform.GetChild(0);
        item.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
        item.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void testSetItem(int obj)
    {
        SetEscapeItem((InteractObjs)obj);
    }
}
