using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    TutorialManager TutorialManager;

    public int NextTrriger;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TutorialManager.TutorialTrriger(NextTrriger);
    }

}
