using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D; //NavMeshAgent2Dを使用するための変数
    [SerializeField] Transform Target; //追跡するターゲット

    [SerializeField] List<Transform> PatrolPointList = new List<Transform>();
    int PatrolNumIndex;

    [SerializeField] bool isDiscover; // プレイヤーを見つけているかどうか.

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
        isDiscover = false;
    }

    void Update()
    {
        SetNextPatrolPoint();
        
    }

    private void FixedUpdate()
    {
        JailerSight();
    }

    /// <summary>
    /// 次の巡回ポイントを設定して自身が目的地に向かうための処理
    /// </summary>
    void SetNextPatrolPoint()
    {
        if(isDiscover == false)
        {
            if (NavMeshAgent2D.isArrival == true)
            {
                PatrolNumIndex = (PatrolNumIndex + 1) % PatrolPointList.Count;
                NavMeshAgent2D.isArrival = false;
            }
            NavMeshAgent2D.SetDestination(PatrolPointList[PatrolNumIndex].position);
        }
        else if (isDiscover == true)
        {
            // プレイヤーを追いかける処理.
            NavMeshAgent2D.SetDestination(Target.position);
        }
        
    }

    /// <summary>
    /// 敵の視界 扇形
    /// </summary>
    void JailerSight()
    {
        int   RayNum = 5;     // Rayの本数.
        float Angle = 60f;    // 角度.
        float Distance = 10f; // Rayの距離.
        float StartAngle = -Angle / 2;     // 扇の開始角度.
        float AngleIncrement = Angle / (RayNum - 1); // Rayの角度増分
        Vector2 Velocity = GetComponent<Rigidbody2D>().velocity; // オブジェクトの速度を取得
        Vector2 ForwardDir = Velocity.normalized; // オブジェクトが向いている方向の取得.

        for (int num = 0; num < RayNum; num++)
        {
            // 現在の向き.
            float CurrentAngle = StartAngle + num * AngleIncrement;

            Quaternion RayRotation = Quaternion.Euler(0, 0, CurrentAngle);
            Vector2 RayDir = RayRotation * ForwardDir;

            // Rayの発射.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDir, Distance);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isDiscover = true;
            }

            Debug.DrawRay(transform.position, RayDir * Distance, Color.red);
        }
    }
}