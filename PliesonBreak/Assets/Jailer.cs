using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D; //NavMeshAgent2D���g�p���邽�߂̕ϐ�
    //[SerializeField] Transform Target; //�ǐՂ���^�[�Q�b�g

    [SerializeField] List<Transform> PatrolPointList = new List<Transform>();
    int PatrolNumIndex;

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agent��NavMeshAgent2D���擾
        // SetNextPatrolPoint();
    }

    void Update()
    {
        SetNextPatrolPoint();
    }

    /// <summary>
    /// ���̏���|�C���g��ݒ肵�Ď��g���ړI�n�Ɍ��������߂̏���
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