using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : InteractObjectBase
{
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �S�[�������Ƃ��̏���.
    /// </summary>
    void PlayerGoal()
    {
        Debug.Log("�S�[�����܂����I�I");
    }
}
