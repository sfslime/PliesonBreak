using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class MaskSprite : MonoBehaviour
{
    [SerializeField] float Scale;

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
