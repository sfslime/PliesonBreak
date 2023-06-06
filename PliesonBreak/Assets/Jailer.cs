using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D;      //NavMeshAgent2Dを使用するための変数.
    [SerializeField] Transform Target;  //追跡するターゲット.

    [SerializeField] List<Transform> PatrolPointList = new List<Transform>();
    int PatrolNumIndex;

    [SerializeField] bool isDiscover;    // プレイヤーを見つけているかどうか.
    [SerializeField] bool isLostTarget;  // ターゲットを見失った時.
    [SerializeField] float LostTime;     // ターゲットを見失ってから巡回に戻るまでの時間.
    [SerializeField] float SetTime;      // LostTimeにセットする時間.
    Vector3 ThisSavePos;                 // 自身のポジションを保存.
    Vector3 SavePlayerPos;               // プレイヤーのポジションを保存.

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
        isDiscover = false;
        ThisSavePos = transform.position;
    }

    void Update()
    {
        SetNextPatrolPoint();
        LostPlayer();
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
        int   RayNum = 10;              // Rayの本数.
        float Angle = 15f;              // 角度.
        float Distance = 5f;           // Rayの距離.
        float StartAngle = -Angle / 2;  // 扇の開始角度.
        float AngleIncrement = Angle / (RayNum - 1);                     // Rayの角度増分
        Vector3 RelativeVector = transform.position - ThisSavePos;       // 相対ベクター.
        Vector3 ForwardDir = RelativeVector.normalized;                  // オブジェクトが向いている方向の取得.
        ThisSavePos = transform.position;

        for (int num = 0; num < RayNum; num++)
        {
            // 現在の向き.
            float CurrentAngle = StartAngle + num * AngleIncrement;

            Quaternion RayRotation = Quaternion.Euler(0, 0, CurrentAngle);
            Vector3 RayDir = RayRotation * ForwardDir;

            // Rayの発射.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDir, Distance);

            // プレイヤーを発見.
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isDiscover = true;
                LostTime = SetTime;
                SavePlayerPos = Target.position;
                Debug.Log("SavePlayerPos" + SavePlayerPos);
            }
            else if (hit.collider == null && SavePlayerPos != Vector3.zero)
            {
                isLostTarget = true;
                Debug.Log("プレイヤーを見失った");
            }
            else if(hit.collider == null)
            {
                //isDiscover = false;
                SavePlayerPos = Vector3.zero;
            }

            Debug.DrawRay(transform.position, RayDir * Distance, Color.red);
        }
    }

    /// <summary>
    /// プレイヤーを見失った時の処理.
    /// </summary>
    /// <returns></returns>
    void LostPlayer()
    {
        if (isLostTarget == true)
        {
            LostTime -= Time.deltaTime;

            // プレイヤーを見つけられなかった場合巡回に戻る
            if(LostTime < 0)
            {
                isDiscover = isLostTarget = false;

                LostTime = 0;
            }
        }
    }
}