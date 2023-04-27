using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    protected Player cPlayer;

    protected enum InteractObj
    {
        None,
        Door,
        Key,
<<<<<<< HEAD
        Search
=======
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
>>>>>>> 23b2aa81cb4cd3f99624771852734103b3c29768
    }

    protected InteractObj NowInteract;

    void Start()
    {
        cPlayer = GameObject.Find("Player").GetComponent<Player>();
        NowInteract = InteractObj.None;
    }

    void Update()
    {
        
    }
}
