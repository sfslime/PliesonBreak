using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D; //NavMeshAgent2Dを使用するための変数
    //[SerializeField] Transform Target; //追跡するターゲット

    [SerializeField] List<Transform> PatrolPointList = new List<Transform>();
    int PatrolNumIndex;

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
        // SetNextPatrolPoint();
    }

    void Update()
    {
        SetNextPatrolPoint();
    }

    /// <summary>
    /// 次の巡回ポイントを設定して自身が目的地に向かうための処理
    /// </summary>
    void SetNextPatrolPoint()
    {
        if(NavMeshAgent2D.isArrival == true)
        {
            PatrolNumIndex = (PatrolNumIndex + 1) % PatrolPointList.Count;
            NavMeshAgent2D.isArrival = false;
        }
        NavMeshAgent2D.SetDestination(PatrolPointList[PatrolNumIndex].position);
    }
}