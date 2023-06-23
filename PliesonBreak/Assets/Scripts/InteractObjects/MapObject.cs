using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : InteractObjectBase
{

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookMap(bool isdisplay)
    {
        if (isdisplay == true)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
