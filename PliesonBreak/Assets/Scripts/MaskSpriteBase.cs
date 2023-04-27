using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class MaskSpriteBase : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1.25f)] float Scale;  // プレイヤーの視界の大きさ.

    void Start()
    {
        
    }

    void Update()
    {
        ChangeView();
    }

    /// <summary>
    /// プレイヤーの視界変更.
    /// </summary>
    public void ChangeView() {
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
