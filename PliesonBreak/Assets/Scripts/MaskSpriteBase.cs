using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class MaskSpriteBase : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField, Range(0.5f, 100f)] float Scale;  // �v���C���[�̎��E�̑傫��.
=======
    [SerializeField, Range(0.5f, 1.25f)] float ViewScale;  // �v���C���[�̎��E�̑傫��.
>>>>>>> 419166b7b746de499991cf9d8b9dc22edb7ca33e

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
