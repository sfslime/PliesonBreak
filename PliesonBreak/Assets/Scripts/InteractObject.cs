using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{

    public enum InteractObj
    {
        None,
        Door,
        Key,
    }

    public InteractObj NowInteract;

    void Start()
    {
        NowInteract = InteractObj.None;
    }

    void Update()
    {
        
    }
}
