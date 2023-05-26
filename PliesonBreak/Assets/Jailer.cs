using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jailer : MonoBehaviour
{
    NavMeshAgent2D NavMeshAgent2D; //NavMeshAgent2D���g�p���邽�߂̕ϐ�
    [SerializeField] Transform target; //�ǐՂ���^�[�Q�b�g

    void Start()
    {
        NavMeshAgent2D = GetComponent<NavMeshAgent2D>(); //agent��NavMeshAgent2D���擾
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TargetHunt(collision);
    }

    /// <summary>
    /// �����̖ړI�n��target�̍��W�ɂ���
    /// </summary>
    void TargetHunt(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            NavMeshAgent2D.SetDestination(target.position);
        }
    }
}