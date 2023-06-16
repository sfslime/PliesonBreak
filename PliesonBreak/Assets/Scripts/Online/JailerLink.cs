using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JailerLink : MonoBehaviourPunCallbacks
{
    Jailer OriginObject;
    // Start is called before the first frame update
    void Start()
    {
        OriginObject = GetComponent<Jailer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPatrolPoint(Vector3 point)
    {
        photonView.RPC(nameof(RPCAddPatrolPoint), RpcTarget.All, point);
    }

    [PunRPC]
    void RPCAddPatrolPoint(Vector3 point)
    {
        if (OriginObject == null) OriginObject = GetComponent<Jailer>();
        OriginObject.AddPatrolPoint(point);
    }
}
