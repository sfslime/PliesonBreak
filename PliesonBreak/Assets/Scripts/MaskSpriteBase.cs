using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class MaskSpriteBase : MonoBehaviour
{

    [SerializeField, Range(0.5f, 100f)] float ViewScale;  // �v���C���[�̎��E�̑傫��.

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
        transform.localScale = new Vector3(ViewScale, ViewScale, ViewScale);
    }
}
