using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooter : MonoBehaviour, IDamageable
{
    public GameObject bulletPrefab; // 子弹的Prefab
    public Transform bulletSpawnPoint; // 子弹发射的位置
    public float fireRate = 1f; // 发射速率，每秒发射的子弹数
    public float firespeed;
    private Transform player; // 玩家的Transform
    public float moveSpeed = 3f; // 敌人的移动速度
    public float minDistance = 5f; // 与玩家保持的最小距离
    public float maxDistance = 10f; // 与玩家保持的最大距离
    private Rigidbody2D rb; // 敌人的Rigidbody2D

    private Vector2 movementDirection; // 当前的移动方向
    private float decisionDelay = 2f; // 决定新移动方向的时间间隔
    private float timeSinceLastDecision; // 距离上次决定移动方向的时间
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
        // 如果当前时间超过了下一次发射时间
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
        // 创建子弹实例
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.SetParent(transform);

        // 计算朝向玩家的方向
        Vector2 direction = (player.position - bulletSpawnPoint.position).normalized;

        // 设置子弹的初始方向朝向玩家
        bullet.transform.up = direction;
        // 给子弹添加速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * 2f;
    }
    void DecideMovementDirection()
    {
        // 随机选择一个新的方向来移动
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void MaintainDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < minDistance)
        {
            // 如果距离玩家太近，后退
            movementDirection = (transform.position - player.position).normalized;
        }
        else if (distanceToPlayer > maxDistance)
        {
            // 如果距离玩家太远，向玩家靠近
            movementDirection = (player.position - transform.position).normalized;
        }

        // 根据当前方向和速度移动敌人
        Vector2 newMovePosition = rb.position + (movementDirection * moveSpeed * Time.deltaTime);
        rb.MovePosition(newMovePosition);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        Debug.Log("Ranged enemy受到伤害，剩余血量：" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Ranged enemy死亡 ");
            EnemyManager.EnemyKilled(EnemyType.Ranged);
            DestroyAllChildren();
            StartCoroutine(die_ani());

            // 此处添加玩家死亡的处理逻辑
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
        // 禁用或销毁影响敌人行动的组件
        // 例如，禁用敌人的AI脚本
        this.enabled = false; // 假设当前脚本负责敌人的移动和攻击

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // 停止所有运动
            rb.isKinematic = true; // 将Rigidbody设置为Kinematic停止物理反应
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

