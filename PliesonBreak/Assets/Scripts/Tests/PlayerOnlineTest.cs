using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PlayerOnlineTest : MonoBehaviour
{
    [SerializeField] string ObjName;
    bool isJoin = false;
    GameObject obj;
    [SerializeField] float Speed;
    [SerializeField] InputAction InputAction;

    // Start is called before the first frame update
    void Start()
    {
        InputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isJoin) return;
        var pos = obj.transform.position;

        var MoveVector = InputAction.ReadValue<Vector2>();

        pos.x += MoveVector.x * Speed * Time.deltaTime;
        pos.y += MoveVector.y * Speed * Time.deltaTime;

        obj.transform.position = pos;

        Debug.Log(MoveVector.x);
    }

    public void PopPlayer()
    {
        obj = PhotonNetwork.Instantiate(ObjName, new Vector3(0,0,0), Quaternion.identity);
        isJoin = true;
    }

}
