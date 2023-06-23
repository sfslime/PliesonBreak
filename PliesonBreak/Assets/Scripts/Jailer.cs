using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Photon.Pun;
using ConstList;

public class Jailer : MonoBehaviourPun
{
    NavMeshAgent2D NavMeshAgent2D;      //NavMeshAgent2Dを使用するための変数.
    [SerializeField] Transform Target;  //追跡するターゲット.
    [SerializeField] GameManager GameManager;

    [SerializeField] List<Vector3> PatrolPointList = new List<Vector3>();
    int PatrolNumIndex;

    [SerializeField] bool isDiscover;    // プレイヤーを見つけているかどうか.
    [SerializeField] bool isLostTarget;  // ターゲットを見失った時.
    [SerializeField] bool isCapture;     // プレイヤーを捕まえたかどうか.
    [SerializeField] bool ishunt;        // プレイヤーを追跡しているかどうか.


    public bool isRestraint;             // 動けるかどうか
    [SerializeField] float LostTime;     // ターゲットを見失ってから巡回に戻るまでの時間.
    [SerializeField] float SetTime;      // LostTimeにセットする時間.

    Vector3 ThisSavePos;                 // 自身のポジションを保存.
    Vector3 SavePlayerPos;               // プレイヤーのポジションを保存.

    Animator Animator;                   // アニメーター
    AnimCode AnimState;                  // アニメーション状態

    // 視界のパラメータ.
    [SerializeField] int RayNum;         // Rayの本数.
    [SerializeField] float Angle;        // 角度.
    [SerializeField] float Distance;     // Rayの距離.

    void Start()
    {
        
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
        GameManager = GameManager.GameManagerInstance;
        isDiscover = false;
        AnimState = AnimCode.Walk;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        
        ThisSavePos = transform.position;
        
    }

    void Update()
    {
        SetNextPatrolPoint();
        LostPlayer();
        StartCoroutine(SetBoolTrigger(AnimState));
        Debug.Log(SEid.Discover);
    }

    private void FixedUpdate()
    {
        JailerSight();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ArrestPlayer(collision);
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

            NavMeshAgent2D.SetDestination(PatrolPointList[PatrolNumIndex]);
            // GameManager.PlaySE(SEid.JailerWalk, transform.position);
            AnimState = AnimCode.Run;
        }
        else if (isDiscover == true && isCapture == false)
        {
            // プレイヤーを追いかける処理.

            NavMeshAgent2D.SetDestination(Target.position);
            AnimState = AnimCode.Run;
        }
        
    }

    /// <summary>
    /// 敵の視界.
    /// </summary>
    void JailerSight()
    {
        float AngleIncrement = Angle / (RayNum - 1);                   // Rayの角度増分
        Vector3 RelativeVector = transform.position - ThisSavePos;     // 相対ベクター.
        Vector3 ForwardDir = RelativeVector.normalized;                // オブジェクトが向いている方向の取得.
        ThisSavePos = transform.position;

        for (int num = 0; num < RayNum; num++)
        {
            // 扇の開始角度.
            float StartAngle = -Angle / 2;

            // 現在の向き.
            float CurrentAngle = StartAngle + num * AngleIncrement;

            Quaternion RayRotation = Quaternion.Euler(0, 0, CurrentAngle);

            Vector3 RayDir = RayRotation * ForwardDir;

            // Rayの発射.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDir, Distance);

            // 強制的に巡回に戻す処理.
            if (hit.collider != null && hit.collider.CompareTag("RetreatArea"))
            {
                isDiscover = false;
                isLostTarget = false;
            }

            // プレイヤーを発見.
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                if(isDiscover == false)
                {
                    if (GameManager.GetPlayer() == hit.collider.gameObject)
                    {
                        BGMManager.Instance.SetBGM(BGMid.CHASE);
                        GameManager.PlaySE(SEid.Discover, GameManager.GetPlayer().transform.position);
                    }
                    
                    isDiscover = true;
                }
                isCapture = false;
                Target = hit.collider.gameObject.transform;
                LostTime = SetTime;
                SavePlayerPos = Target.position;
                // Debug.Log("SavePlayerPos" + SavePlayerPos);
            }
            else if (hit.collider == null && SavePlayerPos != Vector3.zero)
            {
                isLostTarget = true;
                // Debug.Log("プレイヤーを見失った");
            }
            else if(hit.collider == null)
            {
                // 保存していたポジションの初期化.
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
                if(isDiscover == true)
                {
                    BGMManager.Instance.SetBGM(BGMid.DEFALTGAME);
                }
                isDiscover = isLostTarget = false;
                LostTime = 0;
            }
        }
    }

    /// <summary>
    /// プレイヤーを捕まえた時の処理.
    /// </summary>
    /// <param name="collision"></param>
    public void ArrestPlayer(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isCapture = true;
            GameObject HitPlayer = collision.gameObject;
            GameManager.ArrestPlayer(HitPlayer);
            if(collision.gameObject == GameManager.GetPlayer())
            {
                GameManager.PlaySE(SEid.Arrest, GameManager.GameManagerInstance.GetPlayer().transform.position);
            }
            AnimState = AnimCode.Search;
            Debug.Log("捕まえました");
        }
    }

    /// <summary>
    /// 巡回ポイントの追加.
    /// </summary>
    public void AddPatrolPoint(Vector3 patrolpoint)
    {
        PatrolPointList.Add(patrolpoint);
    }

    /// <summary>
    /// 巡回ポイントの削除.
    /// </summary>
    public void ClearPatorlPoint()
    {
        PatrolPointList.Clear();
    }

    #region アニメーション

    /// <summary>
    /// Triggerが使えないため、boolのオンオフで代用
    /// trueにしたあと1フレーム後にfalseにする
    /// </summary>
    /// <param name="anim"></param>
    /// <returns></returns>
    IEnumerator SetBoolTrigger(AnimCode anim)
    {
        Animator.SetBool(anim.ToString(), true);
        yield return null;
        Animator.SetBool(anim.ToString(), false);
        yield break;
    }

    #endregion

}