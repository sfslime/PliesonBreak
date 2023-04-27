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
<<<<<<< HEAD:PliesonBreak/Assets/Scripts/InteractObject.cs
        NowInteract = InteractObj.None;
=======
        NowInteract = InteractObjs.None;
>>>>>>> c5fdca62245b2074e985cbe2dbdc8545a78b2268:PliesonBreak/Assets/Scripts/InteractObjectBase.cs
    }

    void Update()
    {
        
    }

    public virtual void CopyProperty(InteractObject newobject)
    {
        //èÓïÒÇÃÉRÉsÅ[
    }
}
