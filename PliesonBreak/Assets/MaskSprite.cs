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
    /// �v���C���[�̎��E�ύX.
    /// </summary>
    public void ChangeView() {
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
