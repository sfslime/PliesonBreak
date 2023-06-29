using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Photon.Pun;
using ConstList;

public class Jailer : MonoBehaviourPun
{
    NavMeshAgent2D NavMeshAgent2D;      //NavMeshAgent2D���g�p���邽�߂̕ϐ�.
    [SerializeField] Transform Target;  //�ǐՂ���^�[�Q�b�g.
    LineRenderer[] LineRenderer;
    [SerializeField] GameManager GameManager;

    [SerializeField] List<Vector3> PatrolPointList = new List<Vector3>();
    int PatrolNumIndex;

    [SerializeField] bool isDiscover;    // �v���C���[�������Ă��邩�ǂ���.
    [SerializeField] bool isLostTarget;  // �^�[�Q�b�g������������.
    [SerializeField] bool isCapture;     // �v���C���[��߂܂������ǂ���.
    [SerializeField] bool isTime;        // �w�肵�����Ԍo�߂�����.

    public bool isRestraint;             // �����邩�ǂ���
    [SerializeField] float LostTime;     // �^�[�Q�b�g���������Ă��珄��ɖ߂�܂ł̎���.
    [SerializeField] float SetTime;      // LostTime�ɃZ�b�g���鎞��.

    Vector3 ThisSavePos;                 // ���g�̃|�W�V������ۑ�.
    Vector3 SavePlayerPos;               // �v���C���[�̃|�W�V������ۑ�.

    Animator Animator;                   // �A�j���[�^�[
    AnimCode AnimState;                  // �A�j���[�V�������

    // ���E�̃p�����[�^.
    [SerializeField] int RayNum;         // Ray�̖{��.
    [SerializeField] float Angle;        // �p�x.
    [SerializeField] float Distance;     // Ray�̋���.

    void Start()
    {
        // LineRenderer�R���|�[�l���g���擾���Ĕz��Ɋi�[
        LineRenderer = new LineRenderer[RayNum];
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agent��NavMeshAgent2D���擾
        GameManager = GameManager.GameManagerInstance;
        isDiscover = false;
        AnimState = AnimCode.Walk;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        
        ThisSavePos = transform.position;

        for (int i = 0; i < RayNum; i++)
        {
            GameObject lineObj = new GameObject("LineRenderer" + i);
            LineRenderer[i] = lineObj.AddComponent<LineRenderer>();

            // LineRenderer�̐ݒ�
            LineRenderer[i].positionCount = 2;       // ���_�̐���2�ɐݒ�i�n�_�ƏI�_�j
            LineRenderer[i].startWidth = 0.1f;      // ���̊J�n��
            LineRenderer[i].endWidth = 0.1f;        // ���̏I����
            LineRenderer[i].material = new Material(Shader.Find("Sprites/Default"));  // ���̃}�e���A��
            LineRenderer[i].startColor = Color.white; // ���̊J�n�F
            LineRenderer[i].endColor = Color.yellow;   // ���̏I���F
        }

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
    /// ���̏���|�C���g��ݒ肵�Ď��g���ړI�n�Ɍ��������߂̏���
    /// </summary>
    void SetNextPatrolPoint()
    {
        if (isDiscover == false)
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
            // �v���C���[��ǂ������鏈��.
            NavMeshAgent2D.SetDestination(Target.position);
            AnimState = AnimCode.Run;
        }
        
    }

    /// <summary>
    /// �G�̎��E.
    /// </summary>
    void JailerSight()
    {
        float AngleIncrement = Angle / (RayNum - 1);                   // Ray�̊p�x����
        Vector3 RelativeVector = transform.position - ThisSavePos;     // ���΃x�N�^�[.
        Vector3 ForwardDir = RelativeVector.normalized;                // �I�u�W�F�N�g�������Ă�������̎擾.
        ThisSavePos = transform.position;

        for (int num = 0; num < RayNum; num++)
        {
            // ��̊J�n�p�x.
            float StartAngle = -Angle / 2;

            // ���݂̌���.
            float CurrentAngle = StartAngle + num * AngleIncrement;

            Quaternion RayRotation = Quaternion.Euler(0, 0, CurrentAngle);

            Vector3 RayDir = RayRotation * ForwardDir;

            // Ray�̔���.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDir, Distance);

            // ���̎n�_�ƏI�_�̍��W��ݒ�
            LineRenderer[num].SetPosition(0, transform.position);
            LineRenderer[num].SetPosition(1, transform.position + RayDir * Distance);


            // �����I�ɏ���ɖ߂�����.
            if (hit.collider != null && hit.collider.CompareTag("RetreatArea"))
            {
                isDiscover = false;
                isLostTarget = false;
            }

            // �v���C���[�𔭌�.
            if (hit.collider != null && hit.collider.CompareTag("Player") && isDiscover == false)
            {
                if (GameManager.GetPlayer() == hit.collider.gameObject)
                {
                    BGMManager.Instance.SetBGM(BGMid.CHASE);
                    GameManager.PlaySE(SEid.Discover, GameManager.GetPlayer().transform.position);
                }
                WaitTime(2.0f);
                isDiscover = true;
                isCapture = false;
                Target = hit.collider.gameObject.transform;
                LostTime = SetTime;
                SavePlayerPos = Target.position;
            }
            else if (hit.collider == null && SavePlayerPos != Vector3.zero)
            {
                isLostTarget = true;
                // Debug.Log("�v���C���[����������");
            }
            else if (hit.collider == null)
            {
                // �ۑ����Ă����|�W�V�����̏�����.
                SavePlayerPos = Vector3.zero;
            }

            if (!Debug.isDebugBuild)
            {
                Debug.DrawRay(transform.position, RayDir * Distance, Color.red);
            }
        }
    }

    /// <summary>
    /// �v���C���[�������������̏���.
    /// </summary>
    /// <returns></returns>
    void LostPlayer()
    {
        if (isLostTarget == true)
        {
            LostTime -= Time.deltaTime;

            // �v���C���[���������Ȃ������ꍇ����ɖ߂�
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
    /// �v���C���[��߂܂������̏���.
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
            Debug.Log("�߂܂��܂���");
        }
    }

    /// <summary>
    /// ����|�C���g�̒ǉ�.
    /// </summary>
    public void AddPatrolPoint(Vector3 patrolpoint)
    {
        PatrolPointList.Add(patrolpoint);
    }

    /// <summary>
    /// ����|�C���g�̍폜.
    /// </summary>
    public void ClearPatorlPoint()
    {
        PatrolPointList.Clear();
    }

    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        isTime = true;
    }

    #region �A�j���[�V����

    /// <summary>
    /// Trigger���g���Ȃ����߁Abool�̃I���I�t�ő�p
    /// true�ɂ�������1�t���[�����false�ɂ���
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