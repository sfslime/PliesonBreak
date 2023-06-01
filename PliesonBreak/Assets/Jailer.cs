using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D; //NavMeshAgent2D���g�p���邽�߂̕ϐ�
    [SerializeField] Transform Target; //�ǐՂ���^�[�Q�b�g

    [SerializeField] List<Transform> PatrolPointList = new List<Transform>();
    int PatrolNumIndex;

    [SerializeField] bool isDiscover; // �v���C���[�������Ă��邩�ǂ���.

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agent��NavMeshAgent2D���擾
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
        int   RayNum = 5;     // Ray�̖{��.
        float Angle = 60f;    // �p�x.
        float Distance = 10f; // Ray�̋���.
        float StartAngle = -Angle / 2;     // ��̊J�n�p�x.
        float AngleIncrement = Angle / (RayNum - 1); // Ray�̊p�x����
        Vector2 Velocity = GetComponent<Rigidbody2D>().velocity; // �I�u�W�F�N�g�̑��x���擾
        Vector2 ForwardDir = Velocity.normalized; // �I�u�W�F�N�g�������Ă�������̎擾.

        for (int num = 0; num < RayNum; num++)
        {
            // ���݂̌���.
            float CurrentAngle = StartAngle + num * AngleIncrement;

            Quaternion RayRotation = Quaternion.Euler(0, 0, CurrentAngle);
            Vector2 RayDir = RayRotation * ForwardDir;

            // Ray�̔���.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDir, Distance);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isDiscover = true;
            }

            Debug.DrawRay(transform.position, RayDir * Distance, Color.red);
        }
    }
}