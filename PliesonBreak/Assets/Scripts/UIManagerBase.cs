using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerBase : MonoBehaviour
{
    public static UIManagerBase instance;
    [SerializeField]GameObject InteractButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        InteractButton = GameObject.Find("InteractButton");
        IsInteractButton(false);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// インタラクトボタンのON・OFF.
    /// </summary>
    public void IsInteractButton(bool isInteractButton)
    {
        if(isInteractButton == false)
        {
            InteractButton.SetActive(false);
        }
        else if(InteractButton == true)
        {
            InteractButton.SetActive(true);
        }
    }
}
