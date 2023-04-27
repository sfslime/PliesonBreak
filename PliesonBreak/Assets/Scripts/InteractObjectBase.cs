using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    protected PlayerBase Player;

    protected enum InteractObjs
    {
        None,
        Door,
        Key,
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
    }

    protected InteractObjs NowInteract;

    void Start()
    {
        NowInteract = InteractObjs.None;
    }

    void Update()
    {
        
    }
}
