using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private Vector2 boxSize = new Vector2(3f, 3f); // 3x3��Χ�Ĵ�С
    private Vector2 attackpoi;
    // Start is called before the first frame update
    private Animator animator; // ����Animator���
    public float interval = 3f; // �������ż����������Ϊ3��
    private Collider2D[] colliders;
    public EnemyController Initiator { get; set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void MeleeAttackAnimEvent()
    {
        attackpoi = Initiator.transform.position;
        animator = GetComponent<Animator>();
        colliders = Physics2D.OverlapBoxAll(attackpoi, new Vector2(3f, 3f), 0);
        foreach (var collider in colliders)
        {
            // �������̨��
            OnColliderEnter2D(collider);
        }
    }
    void OnColliderEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player")|| collider.gameObject.CompareTag("DestructibleObstacles"))
        {
            // �������߼�
            Debug.Log("����������ң�" + collider.name);
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // ����TakeDamage����
                damageable.TakeDamage(1);
            }
        }
    }
}
