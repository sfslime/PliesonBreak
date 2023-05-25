using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if(HaveId == (int)InteractObjectBase.InteractObjs.Key)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjectBase.InteractObjs.Key);
        }
        else if(HaveId == (int)InteractObjectBase.InteractObjs.EscapeItem1)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjectBase.InteractObjs.EscapeItem1);
        }
        else if (HaveId == (int)InteractObjectBase.InteractObjs.EscapeItem2)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjectBase.InteractObjs.EscapeItem2);
        }

        if (HaveId != (int)InteractObjectBase.InteractObjs.Key &&
            HaveId != (int)InteractObjectBase.InteractObjs.EscapeItem1 &&
            HaveId != (int)InteractObjectBase.InteractObjs.EscapeItem2)
        {
            Debug.Log("None");
            image.sprite = GameManager.ReturnSprite(InteractObjectBase.InteractObjs.None);
        }
    }
}
