using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooter : MonoBehaviour, IDamageable
{
    public GameObject bulletPrefab; // �ӵ���Prefab
    public Transform bulletSpawnPoint; // �ӵ������λ��
    public float fireRate = 1f; // �������ʣ�ÿ�뷢����ӵ���
    public float firespeed;
    private Transform player; // ��ҵ�Transform
    public float moveSpeed = 3f; // ���˵��ƶ��ٶ�
    public float minDistance = 5f; // ����ұ��ֵ���С����
    public float maxDistance = 10f; // ����ұ��ֵ�������
    private Rigidbody2D rb; // ���˵�Rigidbody2D

    private Vector2 movementDirection; // ��ǰ���ƶ�����
    private float decisionDelay = 2f; // �������ƶ������ʱ����
    private float timeSinceLastDecision; // �����ϴξ����ƶ������ʱ��
    private int maxHealth = 2;
    private int currentHealth;
    Animator animator;
    //AIDestinationSetter destinationSetter;
    public float attackCooldown = 1f;
    private float attackTimer;

    private int point = 20;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
       
    }
    void Update()
    {
        // �����ǰʱ�䳬������һ�η���ʱ��
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }
        if (timeSinceLastDecision > decisionDelay)
        {
            DecideMovementDirection();
            timeSinceLastDecision = 0;
        }
        else
        {
            timeSinceLastDecision += Time.deltaTime;
        }

        MaintainDistance();
    }

    void Attack()
    {
        // �����ӵ�ʵ��
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.SetParent(transform);

        // ���㳯����ҵķ���
        Vector2 direction = (player.position - bulletSpawnPoint.position).normalized;

        // �����ӵ��ĳ�ʼ���������
        bullet.transform.up = direction;
        // ���ӵ�����ٶ�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * 2f;
    }
    void DecideMovementDirection()
    {
        // ���ѡ��һ���µķ������ƶ�
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void MaintainDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < minDistance)
        {
            // ����������̫��������
            movementDirection = (transform.position - player.position).normalized;
        }
        else if (distanceToPlayer > maxDistance)
        {
            // ����������̫Զ������ҿ���
            movementDirection = (player.position - transform.position).normalized;
        }

        // ���ݵ�ǰ������ٶ��ƶ�����
        Vector2 newMovePosition = rb.position + (movementDirection * moveSpeed * Time.deltaTime);
        rb.MovePosition(newMovePosition);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        Debug.Log("Ranged enemy�ܵ��˺���ʣ��Ѫ����" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Ranged enemy���� ");
            EnemyManager.EnemyKilled(EnemyType.Ranged);
            DestroyAllChildren();
            StartCoroutine(die_ani());

            // �˴������������Ĵ����߼�
        }
    }
    IEnumerator die_ani()
    {
        animator.SetTrigger("Dead");
        ScoreManager.instance.AddScore(point);
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
    void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


}

