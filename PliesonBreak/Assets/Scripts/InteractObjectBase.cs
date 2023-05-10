using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectBase : MonoBehaviour
{
    public  enum InteractObjs
    {
        None,
        NullDrop,
        Door,
        Key,
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
    }

    public InteractObjs NowInteract;

    void Start()
    {
        NowInteract = InteractObjs.None;
    }

    void Update()
    {
        
    }

    public virtual void CopyProperty(InteractObjectBase oldobject)
    {
        //èÓïÒÇÃÉRÉsÅ[
        NowInteract =  oldobject.GetComponent<InteractObjectBase>().NowInteract;
    }
}
