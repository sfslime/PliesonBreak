using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConstList;

public class ItemImage : MonoBehaviour
{
    [SerializeField] PlayerBase Player;
    [SerializeField] GameManager GameManager;
    Sprite sprite;
    public Image image;     
    int HaveId;
    

    void Start()
    {
       
        image = GetComponent<Image>();
        
        GameManager = GameManager.GameManagerInstance;

        Player = GameManager.GetPlayer().GetComponent<PlayerBase>();
    }

    void Update()
    {
        ItemImageChange();
    }

    /// <summary>
    /// アイテムボタンの画像を切り替える.
    /// </summary>
    void ItemImageChange()
    {
        HaveId = Player.HaveId;

        // Debug.Log("HaveId > " +HaveId);
        if(HaveId == (int)InteractObjs.Key)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjs.Key);
        }
        else if(HaveId == (int)InteractObjs.EscapeItem1)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem1);
        }
        else if (HaveId == (int)InteractObjs.EscapeItem2)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem2);
        }

        if (HaveId != (int)InteractObjs.Key &&
            HaveId != (int)InteractObjs.EscapeItem1 &&
            HaveId != (int)InteractObjs.EscapeItem2)
        {
            Debug.Log("None");
            image.sprite = GameManager.ReturnSprite(InteractObjs.None);
        }
    }
}
