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
    }

    void Update()
    {
        ItemImageChange();
    }

    /// <summary>
    /// ƒAƒCƒeƒ€‚Ì‰æ‘œ‚ğØ‚è‘Ö‚¦‚é.
    /// </summary>
    void ItemImageChange()
    {
        HaveId = Player.HaveId;

        Debug.Log("HaveId > " +HaveId);
        if(HaveId == (int)InteractObjs.Key)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjs.Key);
        }
        else if(HaveId == (int)InteractObjs.EscapeItem1)
        {
            image.sprite = GameManager.ReturnSprite(InteractObjs.EscapeItem1);
        }

    }
}
