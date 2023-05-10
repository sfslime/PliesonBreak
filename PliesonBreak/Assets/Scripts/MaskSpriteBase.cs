using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class MaskSpriteBase : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField, Range(0.5f, 100f)] float Scale;  // プレイヤーの視界の大きさ.
=======
    [SerializeField, Range(0.5f, 1.25f)] float ViewScale;  // プレイヤーの視界の大きさ.
>>>>>>> 419166b7b746de499991cf9d8b9dc22edb7ca33e

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
        transform.localScale = new Vector3(ViewScale, ViewScale, ViewScale);
    }
}
