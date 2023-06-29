using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    [SerializeField] TutorialManager TutorialManager;

    public int NextTrriger;

    void Start()
    {
        TutorialManager = TutorialManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TutorialManager.TutorialTrriger(NextTrriger);
        }   
    }

}
