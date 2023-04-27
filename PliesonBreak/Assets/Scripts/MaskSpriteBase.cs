using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class MaskSpriteBase : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1.25f)] float Scale;  // �v���C���[�̎��E�̑傫��.

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
