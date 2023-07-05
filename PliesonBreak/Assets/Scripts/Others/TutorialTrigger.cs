using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    TutorialManager TutorialManager;
    public int NextTrigger;
    // Start is called before the first frame update
    void Start()
    {
        TutorialManager = TutorialManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        TutorialManager.TutorialTrriger(NextTrigger);
    }
}
