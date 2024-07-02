using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer; // �������߼����ϰ����
    private GameObject player;
    private Rigidbody2D rb;
    // private Vector2 perpendicularDirection; // ��ֱ����
    //private bool isAvoidingObstacle = false; // �Ƿ����ڱܿ��ϰ���
    // private float avoidTime = 3f; // ����ʱ��
    private int maxHealth=2;
    private int currentHealth;
    Animator ani;
     AIDestinationSetter destinationSetter;
    //��Χ����
    public GameObject attackEffectPrefab;
    private Collider2D[] colliders;
    public float attackRange = 1.5f; // ������Χ��һ�룬3x3��Χ����Χ�ġ��뾶����1.5
    public LayerMask playerLayer; // ���ڼ����ҵĲ�
    public float checkInterval = 1.0f; // �����ʱ�䣬��λ��
    private Vector2 boxSize = new Vector2(3f, 3f); // 3x3��Χ�Ĵ�С
    private bool isEffectCreated = false;
    //�÷�
    private int points = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        ani = GetComponent<Animator>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        if (destinationSetter != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // ȷ���ҵ�����Ҷ���
            if (player != null)
            {
                // ����Ҷ����Transform��������AIDestinationSetter��
                destinationSetter.target = player.transform;
            }
        }
    }

    void FixedUpdate()
    {
        Action();
        //MeleeAttackAnim();
        LookAtPlayer();
        // û���ϰ���ʱ������ƶ�
    }

    void Action()
    {

        // �Խ�ɫΪ��������3x3��λ�Ĺ�������
        Vector2 attackPosition = transform.position; // ��ɫ��ǰλ�ü�Ϊ������������
        colliders = Physics2D.OverlapBoxAll(attackPosition, new Vector2(3f, 3f), 0);

        foreach (var collider in colliders)
        {
            // �������̨��
            OnColliderEnter2D(collider);
        }

    }

    private void OnColliderEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (!isEffectCreated)
            {
                MeleeAttackAnim();
               /* PlayerController playerHealth = collider.gameObject.GetComponent<PlayerController>();
                if (playerHealth != null)
                {
                    // ����TakeDamage����
                    playerHealth.TakeDamage(1);
                }*/
                isEffectCreated = true; // �����Ч�Ѵ���
                StartCoroutine(AttackCoroutine());
            }
            Debug.Log("����������ң�" + collider.name);
        }
    }

    void MeleeAttackAnim()
    {
        GameObject effect = Instantiate(attackEffectPrefab, gameObject.transform.position, Quaternion.identity);
        effect.transform.SetParent(transform);
        Animator effect_ani = effect.GetComponent<Animator>( );
        MeleeAttack attack = effect.GetComponent<MeleeAttack>();
        if (attack != null)
        {
            attack.Initiator = this;
        }
        if (effect_ani != null)
        {
            // ���㶯��ʱ��
            float animationLength = effect_ani.GetCurrentAnimatorStateInfo(0).length;

            // ����������Ϻ�������Ч
            Destroy(effect, animationLength);
        }

    }
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1);
        isEffectCreated = false;
    }

    void LookAtPlayer()
    {
        // ȷ��Ŀ�������Ȼ����
        if (player.transform != null)
        {
            // �������Ƿ��ڵ��˵������Ҳ�
            if (player.transform.position.x < transform.position.x)
            {
                // �������࣬���˳���ת
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                // ������Ҳ࣬���˳��ҷ�ת
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ani.SetTrigger("Hit");
        Debug.Log("Meele enemy��ʣ��Ѫ����" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Meele enemy������");

            EnemyManager.EnemyKilled(EnemyType.Melee);
            StartCoroutine(die_ani());

            // �˴������������Ĵ����߼�
           
        }
    }
    IEnumerator die_ani()
    {
        ani.SetTrigger("Dead");
        ScoreManager.instance.AddScore(points);
        DisableEnemy();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    void DisableEnemy()
    {
        // ���û�����Ӱ������ж������
        // ���磬���õ��˵�AI�ű�
        this.enabled = false; // ���赱ǰ�ű�������˵��ƶ��͹���

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // ֹͣ�����˶�
            rb.isKinematic = true; // ��Rigidbody����ΪKinematicֹͣ����Ӧ
        }
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
   
}
