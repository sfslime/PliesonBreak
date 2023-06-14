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

        switch (HaveId)
        {
            case (int)InteractObjs.Key1:
                image.sprite = GameManager.ReturnSprite(InteractObjs.Key1);
                break;

            case (int)InteractObjs.Key2:
                image.sprite = GameManager.ReturnSprite(InteractObjs.Key2);
                break;

            case (int)InteractObjs.Key3:
                image.sprite = GameManager.ReturnSprite(InteractObjs.Key3);
                break;

            case (int)InteractObjs.EscapeItem1:
                image.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem1);
                break;

            case (int)InteractObjs.EscapeItem2:
                image.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem2);
                break;

            default:
                image.sprite = GameManager.ReturnSprite(InteractObjs.None);
                Debug.Log("None");
                break;
        }
        // Debug.Log("HaveId > " +HaveId);
    }
}
