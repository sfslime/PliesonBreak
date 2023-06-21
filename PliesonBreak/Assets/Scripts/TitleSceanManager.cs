using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ConstList;

public class TitleSceanManager : MonoBehaviour
{
    [SerializeField, Tooltip("�J���p�V�[���Ɉړ�����ꍇ�A���̃V�[����������")] string TestSceanName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(TestSceanName != null) SceneManager.LoadScene(TestSceanName);
            else SceneManager.LoadScene(SceanNames.TUTORIAL.ToString());
        }
    }
}
