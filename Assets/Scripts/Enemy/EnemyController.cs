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
    public LayerMask obstacleLayer; // 用于射线检测的障碍物层
    private GameObject player;
    private Rigidbody2D rb;
    // private Vector2 perpendicularDirection; // 垂直方向
    //private bool isAvoidingObstacle = false; // 是否正在避开障碍物
    // private float avoidTime = 3f; // 避障时间
    private int maxHealth=2;
    private int currentHealth;
    Animator ani;
     AIDestinationSetter destinationSetter;
    //范围攻击
    public GameObject attackEffectPrefab;
    private Collider2D[] colliders;
    public float attackRange = 1.5f; // 攻击范围的一半，3x3范围即范围的“半径”是1.5
    public LayerMask playerLayer; // 用于检测玩家的层
    public float checkInterval = 1.0f; // 检测间隔时间，单位秒
    private Vector2 boxSize = new Vector2(3f, 3f); // 3x3范围的大小
    private bool isEffectCreated = false;
    //得分
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

            // 确保找到了玩家对象。
            if (player != null)
            {
                // 将玩家对象的Transform组件分配给AIDestinationSetter。
                destinationSetter.target = player.transform;
            }
        }
    }

    void FixedUpdate()
    {
        Action();
        //MeleeAttackAnim();
        LookAtPlayer();
        // 没有障碍物时向玩家移动
    }

    void Action()
    {

        // 以角色为中心设置3x3单位的攻击区域
        Vector2 attackPosition = transform.position; // 角色当前位置即为攻击区域中心
        colliders = Physics2D.OverlapBoxAll(attackPosition, new Vector2(3f, 3f), 0);

        foreach (var collider in colliders)
        {
            // 输出控制台信
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
                    // 调用TakeDamage方法
                    playerHealth.TakeDamage(1);
                }*/
                isEffectCreated = true; // 标记特效已创建
                StartCoroutine(AttackCoroutine());
            }
            Debug.Log("攻击到了玩家：" + collider.name);
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
            // 计算动画时长
            float animationLength = effect_ani.GetCurrentAnimatorStateInfo(0).length;

            // 动画播放完毕后销毁特效
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
        // 确保目标玩家仍然存在
        if (player.transform != null)
        {
            // 检查玩家是否在敌人的左侧或右侧
            if (player.transform.position.x < transform.position.x)
            {
                // 玩家在左侧，敌人朝左翻转
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                // 玩家在右侧，敌人朝右翻转
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ani.SetTrigger("Hit");
        Debug.Log("Meele enemy，剩余血量：" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Meele enemy死亡。");

            EnemyManager.EnemyKilled(EnemyType.Melee);
            StartCoroutine(die_ani());

            // 此处添加玩家死亡的处理逻辑
           
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
   
}
