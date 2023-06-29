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
        BGMManager.Instance.SetBGM(BGMid.TITLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            if(TestSceanName != "") SceneManager.LoadScene(TestSceanName);
            else SceneManager.LoadScene(SceanNames.TUTORIAL.ToString());
        }
    }
}
