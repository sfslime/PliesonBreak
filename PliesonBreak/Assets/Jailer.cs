using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D; //NavMeshAgent2Dを使用するための変数
    [SerializeField] Transform target; //追跡するターゲット

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TargetHunt(collision);
    }

    /// <summary>
    /// 自分の目的地をtargetの座標にする
    /// </summary>
    void TargetHunt(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            NavMeshAgent2D.SetDestination(target.position);
        }
    }
}