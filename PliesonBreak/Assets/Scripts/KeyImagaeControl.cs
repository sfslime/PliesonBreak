using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyImagaeControl : MonoBehaviour
{
    enum KeyCodeNm
    {
        W,A,S,D
    }

    List<Image> KeyImages = new List<Image>();
    [SerializeField,Tooltip("ó£ÇµÇΩèÛë‘ÇÃKeyâÊëú")] List<Sprite> SpriteKeyUP = new List<Sprite>();
    [SerializeField,Tooltip("âüÇµÇΩèÛë‘ÇÃKeyâÊëú")] List<Sprite> SpriteKeyDOWN = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            KeyImages.Add(transform.GetChild(i).GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) KeyImages[(int)KeyCodeNm.W].sprite = SpriteKeyDOWN[(int)KeyCodeNm.W];
        else KeyImages[(int)KeyCodeNm.W].sprite = SpriteKeyUP[(int)KeyCodeNm.W];

        if (Input.GetKey(KeyCode.A)) KeyImages[(int)KeyCodeNm.A].sprite = SpriteKeyDOWN[(int)KeyCodeNm.A];
        else KeyImages[(int)KeyCodeNm.A].sprite = SpriteKeyUP[(int)KeyCodeNm.A];

        if (Input.GetKey(KeyCode.S)) KeyImages[(int)KeyCodeNm.S].sprite = SpriteKeyDOWN[(int)KeyCodeNm.S];
        else KeyImages[(int)KeyCodeNm.S].sprite = SpriteKeyUP[(int)KeyCodeNm.S];

        if (Input.GetKey(KeyCode.D)) KeyImages[(int)KeyCodeNm.D].sprite = SpriteKeyDOWN[(int)KeyCodeNm.D];
        else KeyImages[(int)KeyCodeNm.D].sprite = SpriteKeyUP[(int)KeyCodeNm.D];
    }
}
