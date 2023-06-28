using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConstList;

public class Goal : InteractObjectBase
{
    [SerializeField,Tooltip("�Z�b�g�����E�o�A�C�e���̃��X�g")]
    Dictionary<InteractObjs, bool> EscapeItemList = new Dictionary<InteractObjs, bool>();
    [SerializeField,Tooltip("�E�o�ɕK�v�ȃA�C�e���̃��X�g")]
    List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    [SerializeField, Tooltip("�ڕW�A�C�e���̕\����̐e")] 
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
    /// �S�[�������Ƃ��̏���.
    /// </summary>
    public void PlayerGoal()
    {
        Debug.Log("�S�[�����܂����I�I");
        GameManager.GameClear();
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
            GameManager.PlaySE(SEid.EscapeItemSet, transform.position);
            return true;
        }

        //�s�v�ȃA�C�e���̏ꍇ
        return false;
    }

    /// <summary>
    /// ���ɃZ�b�g���ꂽ�E�o�A�C�e���̃��X�g��Ԃ�
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
    /// �Z�b�g�ς݃A�C�e���Ƀ`�F�b�N�}�[�N��t����
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
