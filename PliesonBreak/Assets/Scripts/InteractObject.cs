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
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
    }

    protected InteractObj NowInteract;

    void Start()
    {
        NowInteract = InteractObj.None;
    }

    void Update()
    {
        
    }

    public virtual void CopyProperty(InteractObject newobject)
    {
        //���̃R�s�[
    }
}
