using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D;      //NavMeshAgent2D���g�p���邽�߂̕ϐ�.
    [SerializeField] Transform Target;  //�ǐՂ���^�[�Q�b�g.

    [SerializeField] List<Transform> PatrolPointList = new List<Transform>();
    int PatrolNumIndex;

    [SerializeField] bool isDiscover;    // �v���C���[�������Ă��邩�ǂ���.
    [SerializeField] bool isLostTarget;  // �^�[�Q�b�g������������.
    [SerializeField] bool isArrest;      // �v���C���[��߂܂������ǂ���.
    [SerializeField] float LostTime;     // �^�[�Q�b�g���������Ă��珄��ɖ߂�܂ł̎���.
    [SerializeField] float SetTime;      // LostTime�ɃZ�b�g���鎞��.
    Vector3 ThisSavePos;                 // ���g�̃|�W�V������ۑ�.
    Vector3 SavePlayerPos;               // �v���C���[�̃|�W�V������ۑ�.

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agent��NavMeshAgent2D���擾
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ArrestPlayer(collision);
    }

    /// <summary>
    /// ���̏���|�C���g��ݒ肵�Ď��g���ړI�n�Ɍ��������߂̏���
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
            // �v���C���[��ǂ������鏈��.
            NavMeshAgent2D.SetDestination(Target.position);
        }
        
    }

    /// <summary>
    /// �G�̎��E ��`
    /// </summary>
    void JailerSight()
    {
        int   RayNum = 10;              // Ray�̖{��.
        float Angle = 15f;              // �p�x.
        float Distance = 5f;           // Ray�̋���.
        float StartAngle = -Angle / 2;  // ��̊J�n�p�x.
        float AngleIncrement = Angle / (RayNum - 1);                     // Ray�̊p�x����
        Vector3 RelativeVector = transform.position - ThisSavePos;       // ���΃x�N�^�[.
        Vector3 ForwardDir = RelativeVector.normalized;                  // �I�u�W�F�N�g�������Ă�������̎擾.
        ThisSavePos = transform.position;

        for (int num = 0; num < RayNum; num++)
        {
            // ���݂̌���.
            float CurrentAngle = StartAngle + num * AngleIncrement;

            Quaternion RayRotation = Quaternion.Euler(0, 0, CurrentAngle);
            Vector3 RayDir = RayRotation * ForwardDir;

            // Ray�̔���.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDir, Distance);

            // �v���C���[�𔭌�.
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
                Debug.Log("�v���C���[����������");
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
                isDiscover = isLostTarget = false;

                LostTime = 0;
            }
        }
    }

    void ArrestPlayer(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // TODO �߂܂����Ƃ��̏���.
            Debug.Log("�߂܂��܂���");
        }
    }

    /// <summary>
    /// ����|�C���g�̒ǉ�.
    /// </summary>
    public void AddPatrolPoint(Transform patrolpoint)
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


}